using groupwork71.BLL.DTOs;
using groupwork71.BLL.Interfaces;
using groupwork71.DAL.Interfaces;
using groupwork71.DAL.Models;

namespace groupwork71.BLL.Services;

public class DishService : IDishService
{
    private readonly IDishRepository _repository;

    public DishService(IDishRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Dish>> GetByRestaurantIdAsync(int restaurantId)
    {
        return _repository.GetByRestaurantIdAsync(restaurantId);
    }

    public async Task<bool> CreateAsync(CreateDishDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0)
            return false;

        var dish = new Dish
        {
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description,
            RestaurantId = dto.RestaurantId
        };

        return await _repository.CreateAsync(dish);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }
}