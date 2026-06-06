using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SDA.Db.Repositories;

namespace SDA.Db
{
    public static class Extension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IThemeRepository, ThemeRepository>();
            services.AddScoped<IQuestionAttemptRepository, QuestionAttemptRepository>();
            services.AddScoped<IExamAttemptRepository, ExamAttemptRepository>();
            services.AddScoped<IUserMistakeQueueRepository, UserMistakeQueueRepository>();
            return services;
        }

        public static IServiceCollection AddDataBase(this IServiceCollection services)
        {
            services.AddDbContext<AppContext>(options =>
    options.UseSqlite("Data Source=database.db"));
            return services;
        }
    }
}
