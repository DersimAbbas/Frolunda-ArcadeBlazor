using Frontend.Models;

namespace Frontend.Services;

public class UserService(HttpClient httpClient) : IUserService
{
    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await httpClient.GetFromJsonAsync<User>($"api/users/{id}");
    }

    public async Task<List<User>?> GetAllUsersAsync()
    {
        return await httpClient.GetFromJsonAsync<List<User>>("api/users");
    }

    public async Task<bool> AddUser(User user)
    {
        var response = await httpClient.PostAsJsonAsync("api/users", user);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUser(string id, User user)
    {
        var response = await httpClient.PutAsJsonAsync($"api/users/{id}", user);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUser(string id)
    {
        var response = await httpClient.DeleteAsync($"api/users/{id}");
        return response.IsSuccessStatusCode;
    }
}