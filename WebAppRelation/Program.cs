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
            app.Run();
        }
    }
}