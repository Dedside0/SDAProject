using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;

namespace SDA.Db
{
    public class AppContext: IdentityDbContext<User>
    {

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketQuestion> TicketQuestions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<QuestionAttempt> QuestionAttempts { get; set; }
        public DbSet<Topic> Themes { get; set; }

        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TicketQuestion>()
                .HasKey(tq => new { tq.TicketId, tq.QuestionId});

            modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)          // У ответа есть один вопрос
            .WithMany(q => q.Answers)         // У вопроса много ответов
            .HasForeignKey(a => a.QuestionId) // Внешний ключ в таблице Answers
            .OnDelete(DeleteBehavior.Cascade); //

            modelBuilder.Entity<Question>()
            .HasOne(q => q.Topic)            // У вопроса может быть одна тема
            .WithMany(t => t.Questions)      // У темы много вопросов
            .HasForeignKey(q => q.TopicId)   // Внешний ключ в таблице Questions
            .IsRequired(false)               // Указываем, что связь НЕ обязательна
            .OnDelete(DeleteBehavior.SetNull);

        }


    }
}
