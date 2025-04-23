using Frontend.Models;

namespace Frontend.Services;

public interface IOrderService
{
    Task<Order?> GetOrderByIdAsync(string id);
    Task<List<Order>?> GetAllOrderAsync();
    Task<List<Order>?> GetOrdersByUserIdAsync(string id);
    Task<bool> AddOrder(Order order);
    Task<bool> UpdateOrder(string id,Order order);
    Task<bool> DeleteOrder(string id);
}