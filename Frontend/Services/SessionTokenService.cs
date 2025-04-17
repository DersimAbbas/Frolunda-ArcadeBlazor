namespace Frontend.Services;

public class SessionTokenService(IHttpContextAccessor httpContextAccessor) : ISessionTokenService
{
    public string GetToken()
    {
        return httpContextAccessor.HttpContext.Session.GetString("token");
    }

    public void SetToken(string token)
    {
        httpContextAccessor.HttpContext?.Session.SetString("token", token);
    }
}