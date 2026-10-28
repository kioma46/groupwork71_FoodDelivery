using groupwork71.BLL.DTOs;
using groupwork71.DAL.DTOs;
using groupwork71.DAL.Models;

namespace groupwork71.BLL.Interfaces;

public interface IRestaurantService
{
    Task CreateRestaurantAsync(CreateRestaurantDto dto);
    Task<List<Restaurant>> GetAllRestaurantsAsync();
    Task<Restaurant?> GetRestaurantDetailsAsync(int id);
    Task<bool> UpdateRestaurantAsync(int id, EditRestaurantDto dto);
    Task<bool> DeleteRestaurantAsync(int id);
}