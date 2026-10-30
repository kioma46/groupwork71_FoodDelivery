using groupwork71.DAL.Models;

namespace groupwork71.DAL.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetAllAsync();
    Task DeleteAsync(Order order);
    Task DeleteByIdAsync(int id);
}