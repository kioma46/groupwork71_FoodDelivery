using groupwork71.DAL.Data;
using groupwork71.DAL.Interfaces;
using groupwork71.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace groupwork71.DAL.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DeliveryMobileContext _context;

    public OrderRepository(DeliveryMobileContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Dish)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders.Include(o => o.OrderItems)
            .Include(o => o.Restaurant)
            .Include(o => o.User).OrderBy(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task DeleteAsync(Order order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var o = await GetByIdAsync(id);
        if (o != null)
        {
            _context.Orders.Remove(o);
            await _context.SaveChangesAsync();
        }
        else
            throw new KeyNotFoundException();
    }
}