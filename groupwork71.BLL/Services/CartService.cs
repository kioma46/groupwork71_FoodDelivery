using groupwork71.BLL.DTOs;
using groupwork71.BLL.Interfaces;
using groupwork71.DAL.Interfaces;
using groupwork71.DAL.Models;

namespace groupwork71.BLL.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _repository;

    public CartService(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task<CartDto> GetCartAsync(int userId, int restaurantId)
    {
        var items = await _repository.GetByUserAndRestaurantAsync(userId, restaurantId);

        var itemDtos = items.Select(i => new CartItemDto
        {
            DishId = i.DishId,
            DishName = i.Dish?.Name ?? string.Empty,
            Price = i.Dish?.Price ?? 0,
            Quantity = i.Quantity
        }).ToList();

        return new CartDto
        {
            RestaurantId = restaurantId,
            Items = itemDtos,
            TotalPrice = itemDtos.Sum(i => i.Total)
        };
    }

    public async Task<bool> AddDishAsync(int userId, int restaurantId, int dishId)
    {
        if (!await _repository.DishBelongsToRestaurantAsync(dishId, restaurantId))
            return false;

        var existing = await _repository.GetItemAsync(userId, dishId);

        if (existing == null)
        {
            await _repository.AddAsync(new CartItem
            {
                UserId = userId,
                DishId = dishId,
                Quantity = 1
            });
        }
        else
        {
            existing.Quantity++;
            await _repository.UpdateAsync(existing);
        }

        return true;
    }

    public async Task DecreaseDishAsync(int userId, int dishId)
    {
        var item = await _repository.GetItemAsync(userId, dishId);
        if (item == null) return;

        if (item.Quantity <= 1)
        {
            await _repository.DeleteAsync(item);
        }
        else
        {
            item.Quantity--;
            await _repository.UpdateAsync(item);
        }
    }

    public async Task RemoveDishAsync(int userId, int dishId)
    {
        var item = await _repository.GetItemAsync(userId, dishId);
        if (item != null)
            await _repository.DeleteAsync(item);
    }

    public Task ClearCartAsync(int userId, int restaurantId)
    {
        return _repository.ClearAsync(userId, restaurantId);
    }
}