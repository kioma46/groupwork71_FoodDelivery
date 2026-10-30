using groupwork71.BLL.DTOs;
using groupwork71.DAL.Models;

namespace groupwork71.BLL.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(int userId, int restaurantId, string recipientName, string phoneNumber, string deliveryAddress, CartDto cart);
    Task AddAsync(Order order);
    Task<Order> GetByIdAsync(int id);
    Task<List<Order>> GetAllAsync();
}