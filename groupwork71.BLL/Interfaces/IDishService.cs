using groupwork71.BLL.DTOs;
using groupwork71.DAL.Models;

namespace groupwork71.BLL.Interfaces;

public interface IDishService
{
    Task<List<Dish>> GetByRestaurantIdAsync(int restaurantId);
    Task<bool> CreateAsync(CreateDishDto dto);
    Task<bool> DeleteAsync(int id);
}