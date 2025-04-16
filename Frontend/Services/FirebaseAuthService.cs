using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;
using Frontend.Models;

namespace Frontend.Services;

public class FirebaseAuthService(FirebaseAuthClient firebaseAuthClient, HttpClient httpClient, ICookiesTokenService cookiesTokenService) : IFirebaseAuthService
{
    public async Task<string?> RegisterUser(string email, string password)
    {
        var userCred = await firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);
        var userRole = new RoleDto()
        {
            Uid = userCred.User.Uid,
            Role = "user"
        };
        
        await httpClient.PostAsJsonAsync("https://localhost:44313/api/Auth/assign-role", userRole);

        return await userCred.User.GetIdTokenAsync(forceRefresh: true);
    }

    public async Task<string?> LoginUser(string email, string password)
    {
        var userCred = await firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
        // await cookiesTokenService.SetTokenAsync(await userCred.User.GetIdTokenAsync());
        Console.WriteLine($"User {userCred.User.Uid} logged in");
        return await userCred.User.GetIdTokenAsync(forceRefresh: true);
    }

    public void Logout()
    {
        firebaseAuthClient.SignOut();
    }
    
    public async Task AssignRoleAsync(string uid, string role = "user")
    {
        var claims = new Dictionary<string, object> { { "role", role } };
        // await firebaseAuth.SetCustomUserClaimsAsync(uid, claims);
    }

    public async Task<bool> VerifyTokenAsync(string token)
    {
        var response = await httpClient.PostAsJsonAsync("https://localhost:44313/api/Auth/verify-token", token);
        // localStorageService.SetItemAsync("token", token);
        
        // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return response.IsSuccessStatusCode;
        
    }
}