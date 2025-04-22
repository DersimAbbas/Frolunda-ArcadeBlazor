using System.Web;
using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("frontend/auth");

            group.MapPost("/register", async (
                HttpContext context,
                [FromForm] RegisterUserDto model) =>
            {
                var firebaseAuthService = context.RequestServices.GetRequiredService<IFirebaseAuthService>();

                if (model.Password.Length < 6)
                {
                    var encodedError = Uri.EscapeDataString("Password must be at least 6 characters");
                    return Results.Redirect("/register?error=" + HttpUtility.UrlEncode(encodedError));
                }

                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    var encodedError = Uri.EscapeDataString("Email, password, and confirm password are required");
                    return Results.Redirect($"/register?error={encodedError}");
                }


                if (model.Password != model.ConfirmPassword)
                {
                    var encodedError = Uri.EscapeDataString("Passwords dont match");
                    return Results.Redirect($"/register?error={encodedError}");
                }

                try
                {
                    var token = await firebaseAuthService.RegisterUser(model.Email, model.Password);

                    if (token != null)
                    {
                        return Results.Redirect("/login?message=" +
                                                Uri.EscapeDataString("Registration successful! Please login."));
                    }

                    var encodedError = Uri.EscapeDataString("Registration failed. Please try again.");
                    return Results.Redirect($"/register?error={encodedError}");
                }
                catch (Exception ex)
                {
                    var encodedError = Uri.EscapeDataString("Invalid email or passwords");
                    return Results.Redirect($"/register?error={encodedError}");
                }
            });

            group.MapPost("/login", async (
                HttpContext context,
                [FromForm] LoginUserDto model) =>
            {
                var firebaseAuthService = context.RequestServices.GetRequiredService<IFirebaseAuthService>();

                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    return Results.Redirect("/login?error=Email%20and%20password%20are%20required");
                }

                try
                {
                    var token = await firebaseAuthService.LoginUser(model.Email, model.Password);

                    if (token == null)
                    {
                        return Results.Redirect("/login?error=Invalid%20login%20credentials");
                    }

                    return Results.Redirect("/");
                }
                catch (Exception ex)
                {
                    var encodedError = Uri.EscapeDataString("Invalid email or password");
                    return Results.Redirect($"/login?error={encodedError}");
                }
            });

            group.MapPost("/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Redirect("/");
            });
        }
    }
}