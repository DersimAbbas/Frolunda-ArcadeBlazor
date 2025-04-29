using Frontend.Models;
using Frontend.Services.Interfaces;

namespace Frontend.Services;

public class PaymentService(HttpClient httpClient) : IPaymentService
{
    public async Task<PaymentIntentResponse> ProcessPayment(string paymentMethodId, decimal amount, string userId)
    {
        var response = await httpClient.PostAsJsonAsync("/api/payment", new
        {
            PaymentMethodId = paymentMethodId,
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
}