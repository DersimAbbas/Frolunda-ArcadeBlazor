using Frontend.Models;

namespace Frontend.Services.Interfaces;

public interface IPaymentService
{
    Task<PaymentIntentResponse> ProcessPayment(string paymentMethodId, decimal amount, string userId, Dictionary<string, int> products);
    Task<bool> ConfirmPayment(string clientSecret);
    Task<bool> RegisterOrderAfterPayment(string id, Dictionary<string,int> products, string email);
    
}
