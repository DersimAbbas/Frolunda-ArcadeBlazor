namespace Frontend.Services;

public interface ICookiesTokenService
{
    Task SetTokenAsync(string token);
    Task<string> GetTokenAsync();
    Task ClearTokenAsync();
}