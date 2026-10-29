using groupwork71.BLL.DTOs;

namespace groupwork71.BLL.Interfaces;

public interface ICartService
{
    Task<CartDto> GetCartAsync(int userId, int restaurantId);
    Task<bool> AddDishAsync(int userId, int restaurantId, int dishId);
    Task DecreaseDishAsync(int userId, int dishId);
    Task RemoveDishAsync(int userId, int dishId);
    Task ClearCartAsync(int userId, int restaurantId); 
}