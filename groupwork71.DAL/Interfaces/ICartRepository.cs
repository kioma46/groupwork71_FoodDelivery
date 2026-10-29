using groupwork71.DAL.Models;

namespace groupwork71.DAL.Interfaces;

public interface ICartRepository
{
    Task<List<CartItem>> GetByUserAndRestaurantAsync(int userId, int restaurantId);
    Task<CartItem?> GetItemAsync(int userId, int dishId);
    Task<bool> DishBelongsToRestaurantAsync(int dishId, int restaurantId);
    Task AddAsync(CartItem item);
    Task UpdateAsync(CartItem item);
    Task DeleteAsync(CartItem item);
    Task ClearAsync(int userId, int restaurantId);
}