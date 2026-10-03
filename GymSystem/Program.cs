using GymSystem.BLL;
using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.Date.DbContexts;
using GymSystem.DAL.Reprostries.Classes;
using GymSystem.DAL.Reprostries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //builder.Services.AddScoped<IPlanRepository, PlanRepository>(); // Now Swapping implementations means changing one line in Program.cs, not hunting through every controller.
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IPlanService, PlanService>(); 
            builder.Services.AddScoped<ITrainerService, TrainerSerivce>();
            //builder.Services.AddScoped<DAL.Reprostries.Interfaces.IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

            /* That's the whole point of "killing new": move the responsibility of creating dependencies 
             out of the classes that use them, and centralize it in one place (the DI container).*/

            builder.Services.AddDbContext<GymDbContext>( options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });;

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

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
