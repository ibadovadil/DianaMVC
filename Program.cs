using Diana.Mvc.Contexts;
using Diana.Mvc.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Diana.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DianaDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration.GetConnectionString("MSSql"));
                options.UseSqlServer(builder.Configuration["ConnectionStrings:MSSql"]);
            });
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            PathConstants.RootPath = builder.Environment.WebRootPath; // fileextensionde isletmek ucun rootpathini burdan goturduk

            app.Run();
        }
    }
}