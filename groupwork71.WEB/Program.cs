using groupwork71.BLL.Interfaces;
using groupwork71.BLL.Services;
using groupwork71.DAL.Data;
using groupwork71.DAL.Interfaces;
using groupwork71.DAL.Models;
using groupwork71.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace groupwork71;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DeliveryMobileContext>(options => options.UseNpgsql(connection))
            .AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<DeliveryMobileContext>();
        
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

        builder.Services.AddScoped<IRestaurantService, RestaurantService>();
        builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        builder.Services.AddScoped<IDishService, DishService>();
        builder.Services.AddScoped<IDishRepository, DishRepository>();
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Restaurant}/{action=RestaurantList}/{id?}")
            .WithStaticAssets();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var rolesManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            await AdminInitializer.SeedAdminAndRoleData(rolesManager, userManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }

        app.Run();
    }
}