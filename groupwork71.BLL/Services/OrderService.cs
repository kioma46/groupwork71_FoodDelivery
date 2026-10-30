using groupwork71.BLL.DTOs;
using groupwork71.BLL.Interfaces;
using groupwork71.DAL.Interfaces;
using groupwork71.DAL.Models;

namespace groupwork71.BLL.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly ICartService _cartService;
    
    public OrderService(IOrderRepository orderRepository, ICartService cartService)
    {
        _repository = orderRepository;
        _cartService = cartService;
    }

    public async Task<Order> CreateOrderAsync(int userId, int restaurantId, string recipientName, string phoneNumber, string deliveryAddress, CartDto cart)
    {
        if (cart == null || !cart.Items.Any())
            throw new InvalidOperationException("Корзина пуста");

        var order = new Order
        {
            UserId = userId,
            RestaurantId = restaurantId,
            RecipientName = recipientName,
            PhoneNumber = phoneNumber,
            DeliveryAddress = deliveryAddress,
            TotalPrice = cart.TotalPrice,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var item in cart.Items)
        {
            order.OrderItems.Add(new OrderItem
            {
                DishId = item.DishId,
                Quantity = item.Quantity,
                Price = item.Price,
                Order = order
            });
        }

        await _repository.AddAsync(order);

        await _cartService.ClearCartAsync(userId, restaurantId);

        return order;
    }
    
    public async Task AddAsync(Order order)
    {
        await _repository.AddAsync(order);
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        var o = await _repository.GetByIdAsync(id);
        if(o != null)
            return o;
        
        throw new KeyNotFoundException("Order not found");
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task DeleteAsync(Order order)
    {
        await _repository.DeleteAsync(order);
    }

    public async Task DeleteByIdAsync(int id)
    {
        await _repository.DeleteByIdAsync(id);
    }
}