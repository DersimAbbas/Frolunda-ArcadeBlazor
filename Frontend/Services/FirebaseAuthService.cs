using Firebase.Auth;
using Frontend.Models;

namespace Frontend.Services;

public class FirebaseAuthService(
    FirebaseAuthClient firebaseAuthClient,
    HttpClient httpClient,
    IHttpContextAccessor httpContextAccessor)
    : IFirebaseAuthService
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
            
            var token = await userCred.User.GetIdTokenAsync(forceRefresh: true);

            return token;

        }

        public async Task<string?> LoginUser(string email, string password)
        {
            
            
                var userCred = await firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
                var token = await userCred.User.GetIdTokenAsync(forceRefresh: true);
                
                httpContextAccessor.HttpContext!.Response.Cookies.Append("token", token, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,    
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
                
                return token;
        }

        public async Task<bool> VerifyTokenAsync(string token)
        {
            var response = await httpClient.PostAsJsonAsync("https://localhost:44313/api/Auth/verify-token", token);
            return response.IsSuccessStatusCode;
        }
    }