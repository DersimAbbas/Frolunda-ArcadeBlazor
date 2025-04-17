using Frontend.Models;

namespace Frontend.Services;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(string id);
    Task<List<User>?> GetAllUsersAsync();
    Task<bool> AddUser(User user);
    Task<bool> UpdateUser(string id,User user);
    Task<bool> DeleteUser(string id);
}