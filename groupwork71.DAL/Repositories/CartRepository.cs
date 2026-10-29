using groupwork71.DAL.Data;
using groupwork71.DAL.Interfaces;
using groupwork71.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace groupwork71.DAL.Repositories;

public class CartRepository : ICartRepository
{
    private readonly DeliveryMobileContext _context;

    public CartRepository(DeliveryMobileContext context)
    {
        _context = context;
    }

    public async Task<List<CartItem>> GetByUserAndRestaurantAsync(int userId, int restaurantId)
    {
        return await _context.CartItems
            .Include(c => c.Dish)
            .Where(c => c.UserId == userId && c.Dish!.RestaurantId == restaurantId)
            .OrderBy(c => c.Id)
            .ToListAsync();
    }

    public async Task<CartItem?> GetItemAsync(int userId, int dishId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.DishId == dishId);
    }

    public async Task<bool> DishBelongsToRestaurantAsync(int dishId, int restaurantId)
    {
        return await _context.Dishes.AnyAsync(d => d.Id == dishId && d.RestaurantId == restaurantId);
    }

    public async Task AddAsync(CartItem item)
    {
        _context.CartItems.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CartItem item)
    {
        _context.CartItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CartItem item)
    {
        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task ClearAsync(int userId, int restaurantId)
    {
        var items = await _context.CartItems
            .Where(c => c.UserId == userId && c.Dish!.RestaurantId == restaurantId)
            .ToListAsync();

        _context.CartItems.RemoveRange(items);
        await _context.SaveChangesAsync();
    }
}