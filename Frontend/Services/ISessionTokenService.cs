namespace Frontend.Services;

public interface ISessionTokenService
{
    string GetToken();
    void SetToken(string token);
}