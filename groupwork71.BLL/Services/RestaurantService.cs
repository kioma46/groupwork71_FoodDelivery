using groupwork71.BLL.DTOs;
using groupwork71.BLL.Interfaces;
using groupwork71.DAL.DTOs;
using groupwork71.DAL.Interfaces;
using groupwork71.DAL.Models;

namespace groupwork71.BLL.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _restaurantRepository;

    public RestaurantService(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    
    public async Task CreateRestaurantAsync(CreateRestaurantDto dto)
    {
        Restaurant restaurant = new Restaurant
        {
            Name = dto.Name,
            Image = dto.Image,
            Description = dto.Description
        };
        
        await _restaurantRepository.CreateRestaurantAsync(restaurant);
    }

    public async Task<List<Restaurant>> GetAllRestaurantsAsync()
    {
        return await _restaurantRepository.GetAllRestaurantsAsync();
    }
    
    public async Task<Restaurant?> GetRestaurantDetailsAsync(int id)
    {
        return await _restaurantRepository.GetRestaurantByIdAsync(id);
    }
    
    public async Task<bool> UpdateRestaurantAsync(int id, EditRestaurantDto dto)
    {
        return await _restaurantRepository.UpdateRestaurantAsync(id, dto);
    }

    public async Task<bool> DeleteRestaurantAsync(int id)
    {
        return await _restaurantRepository.DeleteRestaurantAsync(id);
    }
}