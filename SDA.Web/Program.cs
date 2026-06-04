using Microsoft.AspNetCore.Identity;
using SDA.Db;
using SDA.Db.Models;
namespace SDA.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddControllersWithViews();

            builder.Services.AddRepositories();
            builder.Services.AddDataBase();

            builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<Db.AppContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(8); 
                options.LoginPath = "/Account/Authorization"; //куда логиниться, если нет доступа
                options.LogoutPath = "/Account/Logout"; //что вызывается при выходе пользователя
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true 
                };
            });


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Db.AppContext>();

                //context.Database.Migrate();  // Применить все миграции

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                IdentityInitializer.Initialize(userManager, roleManager);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
