using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Firebase.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Frontend.Provider;

public class FirebaseAuthProvider(IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = httpContextAccessor.HttpContext?.Request.Cookies["token"];

        if (string.IsNullOrEmpty(token))
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = new List<Claim>();

            foreach (var claim in jwtToken.Claims)
            {
                claims.Add(new Claim(claim.Type, claim.Value));
            }

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var roles = jwtToken.Claims.Where(c =>
                c.Type == ClaimTypes.Role || c.Type == "role" || c.Type == "roles");

            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId ?? string.Empty));
            claims.Add(new Claim(ClaimTypes.Name, email ?? string.Empty));
            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Value));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "user"));
            }


            claims.Add(new Claim(ClaimTypes.Role, "user"));

            var identity = new ClaimsIdentity(claims, "Firebase");
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }
        catch
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }
    }

}