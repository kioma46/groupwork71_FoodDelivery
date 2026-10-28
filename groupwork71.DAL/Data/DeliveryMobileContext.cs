using groupwork71.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace groupwork71.DAL.Data;

public class DeliveryMobileContext : IdentityDbContext<User, IdentityRole<int>, int>
{

    public DeliveryMobileContext(DbContextOptions<DeliveryMobileContext> options) : base(options)
    {}
    
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();

    public DbSet<Dish> Dishes => Set<Dish>();
    
    public DbSet<Order> Orders => Set<Order>();
    
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    
    public DbSet<CartItem> CartItems => Set<CartItem>();
}