using groupwork71.DAL.Models;

namespace groupwork71.DAL.Interfaces;

public interface IDishRepository
{
    Task<List<Dish>> GetByRestaurantIdAsync(int restaurantId);
    Task<Dish?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Dish dish);
    Task<bool> DeleteAsync(int id);
}