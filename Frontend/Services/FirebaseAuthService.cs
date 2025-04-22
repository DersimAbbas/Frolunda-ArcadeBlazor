using Firebase.Auth;
using Frontend.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using User = Frontend.Models.User;

namespace Frontend.Services;

public class FirebaseAuthService(
    FirebaseAuthClient firebaseAuthClient,
    HttpClient httpClient,
    IHttpContextAccessor httpContextAccessor,
    IUserService userService,
    IJSRuntime JSRuntime,
    NavigationManager navigationManager)
    : IFirebaseAuthService
{
    public async Task<string?> RegisterUser(string email, string password)
        {
            var userCred = await firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);
            userService.AddUser(new User()
            {
                Email = email,
                Role = "user"
            });
            
            var userRole = new RoleDto()
            {
                Uid = userCred.User.Uid,
                Role = "user"
            };
            
            await httpClient.PostAsJsonAsync("https://localhost:44313/api/Auth/assign-role", userRole);
            
            var token = await userCred.User.GetIdTokenAsync(forceRefresh: true);
            navigationManager.NavigateTo("/login", forceLoad: true);
            return token;

        }

        public async Task<string?> LoginUser(string email, string password)
        {
            
            
                var userCred = await firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
                var token = await userCred.User.GetIdTokenAsync(forceRefresh: true);
                
                // httpContextAccessor.HttpContext!.Response.Cookies.Append("token", token, new CookieOptions
                // {
                //     HttpOnly = true,
                //     Secure = true,    
                //     SameSite = SameSiteMode.Lax,
                //     Expires = DateTimeOffset.UtcNow.AddDays(7)
                // });

                JSRuntime.InvokeVoidAsync("setCookie", "token", token);

                // await JSRuntime.InvokeVoidAsync("setCookie", "token", ExtractTokenValue(cookie.ToString()));
                navigationManager.NavigateTo("/", forceLoad: true);
                return token;
        }

        public async Task<bool> VerifyTokenAsync(string token)
        {
            var response = await httpClient.PostAsJsonAsync("/api/Auth/verify-token", token);
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
            Console.WriteLine("User logged out and cookies cleared.");
            navigationManager.NavigateTo("/", forceLoad: true);
        }
        
        private string ExtractTokenValue(string cookie)
        {
            if (cookie != null)
            {
                var tokenValue = cookie.Split('=')[1];
                return tokenValue.Trim();
            }

            return null;
        }
    }