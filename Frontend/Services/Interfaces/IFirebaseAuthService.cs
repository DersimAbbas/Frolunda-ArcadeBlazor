using Frontend.Models;

namespace Frontend.Services.Interfaces;

public interface IFirebaseAuthService
{
    Task<string?> RegisterUser(RegisterUserDto user);
    Task<string?> LoginUser(string email, string password);
    public Task<bool> VerifyTokenAsync(string token);
    public Task<bool> AssignRole(string uid, string role);
    public Task Logout();

}