using Firebase.Auth;
using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using User = Frontend.Models.User;

namespace Frontend.Services;

public class FirebaseAuthService(
    FirebaseAuthClient firebaseAuthClient,
    HttpClient httpClient,
    IUserService userService,
    IJSRuntime JSRuntime,
    NavigationManager navigationManager)
    : IFirebaseAuthService
{
    public async Task<string?> RegisterUser(string email, string password)
    {
        var userCred = await firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);
        await userService.AddUser(new User()
        {
            Id = userCred.User.Uid,
            Email = email,
            Role = "user",
            //Kanske flytta detta till models? Eller bara göra detta mer "clean:t"
            FirstName = string.Empty,
            LastName = string.Empty,
            // Password = string.Empty,
            PhoneNumber = string.Empty,
            Address = string.Empty,
        });
        
        var userRole = new RoleDto()
        {
            Uid = userCred.User.Uid,
            Role = "user"
        };

        await httpClient.PostAsJsonAsync("api/Auth/assign-role", userRole);

        var token = await userCred.User.GetIdTokenAsync(forceRefresh: true);

        return token;
    }

    public async Task<string?> LoginUser(string email, string password)
    {
        var userCred = await firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
        var token = await userCred.User.GetIdTokenAsync(forceRefresh: true);

        await JSRuntime.InvokeVoidAsync("setCookie", "token", token);

        return token;
    }

    public async Task<bool> VerifyTokenAsync(string token)
    {
        var requestBody = new { token };
        var response = await httpClient.PostAsJsonAsync("/api/Auth/verify-token", requestBody);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AssignRole(string uid, string role)
    {
        var newUserRole = new RoleDto()
        {
            Uid = uid,
            Role = role
        };
        var response = await httpClient.PostAsJsonAsync("/api/Auth/assign-role", newUserRole);

        return response.IsSuccessStatusCode;
    }

    public async Task Logout()
    {
        if (firebaseAuthClient.User != null)
        {
            firebaseAuthClient.SignOut();
        }

        await JSRuntime.InvokeVoidAsync("clearAuthCookies");

        navigationManager.NavigateTo("/", forceLoad: true);
    }

}