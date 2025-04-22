using FirebaseAdmin.Auth;

namespace Frontend.Services;

public interface IFirebaseAuthService
{
    Task<string?> RegisterUser(string email, string password);
    Task<string?> LoginUser(string email, string password);
    public Task<bool> VerifyTokenAsync(string token);
    public Task<bool> AssignRole(string uid, string role);
    public Task Logout();

}