using Microsoft.EntityFrameworkCore;

namespace SDA.Db
{
    public class AppContext: DbContext
    {


        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }


    }
}
