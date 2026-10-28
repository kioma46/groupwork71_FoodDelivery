using groupwork71.DAL.Data;
using groupwork71.DAL.DTOs;
using groupwork71.DAL.Interfaces;
using groupwork71.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace groupwork71.DAL.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly DeliveryMobileContext _context;
    
    public RestaurantRepository(DeliveryMobileContext context)
    {
        _context = context;
    }
    
    public async Task CreateRestaurantAsync(Restaurant restaurant)
    {
        _context.Restaurants.Add(restaurant);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Restaurant>> GetAllRestaurantsAsync()
    {
        return await _context.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
    {
        return await _context.Restaurants.Include(r => r.Dishes).FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> UpdateRestaurantAsync(int id, EditRestaurantDto dto)
    {
        Restaurant? restaurant = await GetRestaurantByIdAsync(id);
        if (restaurant == null)
        {
            return false;
        }
        
        restaurant.Name = dto.Name;
        restaurant.Image = dto.Image;
        restaurant.Description = dto.Description;
        
        _context.Restaurants.Update(restaurant);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteRestaurantAsync(int id)
    {
        Restaurant? restaurant = await GetRestaurantByIdAsync(id);
        if (restaurant == null)
        {
            return false;
        }

        _context.Restaurants.Remove(restaurant);
        await _context.SaveChangesAsync();
        
        return true;
    }
}