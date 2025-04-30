using Frontend.Models;
using Frontend.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Frontend.Services;

public class PaymentService(HttpClient httpClient) : IPaymentService
{
    private const string LogicAppUrl = "https://prod-02.swedencentral.logic.azure.com:443/workflows/577863aa15bd438cae6d1124944302c2/triggers/When_a_HTTP_request_is_received/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2FWhen_a_HTTP_request_is_received%2Frun&sv=1.0&sig=E1Ak5YYJnnMjAL2zt7pvUfB-PtSbfFDiNHMwds-iIGs";
    public async Task<PaymentIntentResponse> ProcessPayment(string paymentMethodId, decimal amount, string userId, Dictionary<string, int> products)
    {
        var response = await httpClient.PostAsJsonAsync("/api/payment", new
        {
            PaymentMethodId = paymentMethodId,
            Products = products,
            userId = userId, // Replace with actual user ID
            Amount = amount
        });

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<PaymentIntentResponse>();
            data.IsSuccess = true;
            return data!;
        }

        var errorContent = await response.Content.ReadAsStringAsync();

        return new PaymentIntentResponse
        {
            IsSuccess = false,
            ErrorMessage = errorContent
        };
    }

    public async Task<bool> ConfirmPayment(string clientSecret)
    {
        var response = await httpClient.PostAsJsonAsync("/api/payment/confirm", new { ClientSecret = clientSecret });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ConfirmResponse>();
            return result?.Success == true;
        }
        return false;

    }

    public async Task<bool> RegisterOrderAfterPayment(string userId, Dictionary<string, int> products)
    {
        var createOrder = new RegisterOrder
        {
             
            UserId = userId,
            Products = products
            
        };
        var opts = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(createOrder, opts);
        
        using var client = new HttpClient();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(LogicAppUrl, content);

        return response.IsSuccessStatusCode;

    }
    
}