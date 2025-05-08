using Frontend.Models;

namespace Frontend.Services.Interfaces;

public interface IOrderService
{
    Task<Order?> GetOrderByIdAsync(string id);
    Task<List<RegisterOrder>?> GetAllOrdersAsync();
    Task<List<RegisterOrder>?> GetOrdersByUserIdAsync(string id);
    Task<bool> AddOrder(Order order);
    Task<bool> UpdateOrder(string id,Order order);
    Task<bool> DeleteOrder(string id);
    Task<bool> DisputeOrder(string email, string name, string orderId);
}