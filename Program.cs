using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;

namespace OnlineShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/Login";
                   options.AccessDeniedPath = "/Login";
               });

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<OnlineShopContext>(options => options.UseSqlServer(connection));

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Products}/{action=Index}"
           );
            app.MapControllerRoute(
                name: "Login",
                pattern: "Login",
                defaults: new { controller = "Login", action = "Login" }

           );

            app.Run();
        }
    }
}
