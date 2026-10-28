using groupwork71.DAL.Data;
using groupwork71.DAL.Interfaces;
using groupwork71.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace groupwork71.DAL.Repositories;

public class DishRepository : IDishRepository
{
    private readonly DeliveryMobileContext _context;

    public DishRepository(DeliveryMobileContext context)
    {
        _context = context;
    }

    public async Task<List<Dish>> GetByRestaurantIdAsync(int restaurantId)
    {
        return await _context.Dishes.Where(d => d.RestaurantId == restaurantId).ToListAsync();
    }

    public async Task<Dish?> GetByIdAsync(int id)
    {
        return await _context.Dishes.FindAsync(id);
    }

    public async Task<bool> CreateAsync(Dish dish)
    {
        bool restaurantExists = await _context.Restaurants.AnyAsync(r => r.Id == dish.RestaurantId);

        if (!restaurantExists)
            return false;

        _context.Dishes.Add(dish);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Dish? dish = await _context.Dishes.FindAsync(id);

        if (dish == null)
            return false;

        _context.Dishes.Remove(dish);
        await _context.SaveChangesAsync();
        
        return true;
    }
}