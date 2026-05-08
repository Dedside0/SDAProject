using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;

namespace SDA.Db
{
    public class AppContext: DbContext
    {

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketQuestion> TicketQuestions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketQuestion>()
                .HasKey(tq => new { tq.TicketId, tq.QuestionId});
        }


    }
}
