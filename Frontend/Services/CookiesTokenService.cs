namespace Frontend.Services;

public class CookiesTokenService(IHttpContextAccessor httpContextAccessor) : ICookiesTokenService
{
    public Task SetTokenAsync(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };
        
        httpContextAccessor.HttpContext.Response.Cookies.Append("token", token, cookieOptions);
        return Task.CompletedTask;
    }

    public Task<string> GetTokenAsync()
    {
        var token = string.Empty;
        if (httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("token", out var cookieToken) == true)
        {
            token = cookieToken;
        }

        return Task.FromResult(token);
    }

    public Task ClearTokenAsync()
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete("token");
        return Task.CompletedTask;
    }
}