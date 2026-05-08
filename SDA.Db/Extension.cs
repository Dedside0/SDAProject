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
            services.AddScoped<IQuestionRepository,QuestionRepository>();
            return services;
        }

        public static IServiceCollection AddDataBase(this IServiceCollection services)
        {
            services.AddDbContext<AppContext>();
            return services;
        }
    }
}
