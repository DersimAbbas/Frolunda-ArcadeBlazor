using FirebaseAdmin.Auth;

namespace Frontend.Services;

public interface IFirebaseAuthService
{
    Task<string?> RegisterUser(string email, string password);
    Task<string?> LoginUser(string email, string password);
    void Logout();
    Task<bool> VerifyTokenAsync(string token);
}