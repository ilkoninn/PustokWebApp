using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebAppRelation.DAL;

namespace WebAppRelation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // Password settings.
                options.Password.RequiredLength = 8;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.";

            }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            //builder.Services.AddSingleton<>();

            var app = builder.Build();


            app.MapControllerRoute(
                name: "Admin",
                pattern: "{area:exists}/{controller=Admin}/{action=Index}/{Id?}"
                );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Home}/{id?}");

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.Run();
        }
    }
}