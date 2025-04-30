using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Frontend.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    private const string _ordersKey = "orders";
    private const string _timestampKey = "ordersTimestamp";

    public OrderService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this._httpClient = httpClient;
        this._jsRuntime = jsRuntime;
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<Order>($"api/orders/{id}");
    }

    public async Task<List<Order>?> GetAllOrdersAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _ordersKey);
        var timestampString = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _timestampKey);

        if (!string.IsNullOrEmpty(json) && long.TryParse(timestampString, out var ticks))
        {
            var savedTime = new DateTime(ticks);
            if ((DateTime.UtcNow - savedTime).TotalHours < 24)
            {
                try
                {
                    var cachedOrders = JsonSerializer.Deserialize<List<Order>>(json);
                    if (cachedOrders != null)
                        return cachedOrders;
                }
                catch
                {

                }
            }
        }

        var orders = await _httpClient.GetFromJsonAsync<List<Order>>("api/orders") ?? new List<Order>();

        var orderJson = JsonSerializer.Serialize(orders);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _ordersKey, orderJson);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _timestampKey, DateTime.UtcNow.Ticks.ToString());

        return orders;
    }

    public async Task<List<Order>?> GetOrdersByUserIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<List<Order>>("api/orders/user/" + id);
    }

    public async Task<bool> AddOrder(Order order)
    {
        var result = await _httpClient.PostAsJsonAsync("api/orders", order);
        if (result.IsSuccessStatusCode)
        {
            var orderJson = JsonSerializer.Serialize(order);
            await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _ordersKey, orderJson);
        }
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateOrder(string id, Order order)
    {
        var result = await _httpClient.PutAsJsonAsync($"api/orders/{id}", order);
        if (result.IsSuccessStatusCode)
        {
            var orderJson = JsonSerializer.Serialize(order);
            await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _ordersKey, orderJson);
        }
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteOrder(string id)
    {
        var order = await GetOrderByIdAsync(id);
        var result = await _httpClient.DeleteAsync($"api/orders/{id}");

        if (result.IsSuccessStatusCode)
        {
            var orderJson = JsonSerializer.Serialize(order);
            await _jsRuntime.InvokeVoidAsync("myLocalStorage.removeItem", _ordersKey, orderJson);
        }

        return result.IsSuccessStatusCode;
    }

    

    
}