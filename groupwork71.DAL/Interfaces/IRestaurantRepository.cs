using groupwork71.DAL.DTOs;
using groupwork71.DAL.Models;

namespace groupwork71.DAL.Interfaces;

public interface IRestaurantRepository
{
    Task CreateRestaurantAsync(Restaurant restaurant);
    Task<List<Restaurant>> GetAllRestaurantsAsync();
    Task<Restaurant?> GetRestaurantByIdAsync(int id);
    Task<bool> UpdateRestaurantAsync(int id, EditRestaurantDto dto);
    Task<bool> DeleteRestaurantAsync(int id);
}