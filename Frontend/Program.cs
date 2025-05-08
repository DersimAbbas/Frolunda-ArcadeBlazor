using Firebase.Auth;
using Firebase.Auth.Providers;
using Frontend.Components;
using Frontend.Handler;
using Frontend.Services;
using Frontend.Services.Firebase;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;
using Frontend.Services.Interfaces;
using FirebaseAuthProvider = Frontend.Provider.FirebaseAuthProvider;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

builder.Services.AddScoped(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseAdress = config["ApiBaseUrl"];
    return new HttpClient { BaseAddress = new Uri(baseAdress) };
});

builder.Services.AddScoped<BearerTokenHandler>();
builder.Services.AddHttpClient("AuthenticatedClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl); 
}).AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"));



builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAntiforgery();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "token";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.LoginPath = "/login";
        options.LogoutPath = "/";
        options.AccessDeniedPath = "/";
    });
builder.Services.AddAuthorization();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IServicePrincipalProvider, DevFirebaseConnection>();
}
else
{
    builder.Services.AddScoped<IServicePrincipalProvider, ProdFirebaseConnection>();
}



var credentialProvider = builder.Services.BuildServiceProvider().GetRequiredService<IServicePrincipalProvider>();

var keyVaultSecret = await credentialProvider.GetKeyVaultSecretAsync();

builder.Services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();
builder.Services.AddScoped<FirebaseAuthClient>(provider =>
{
    return new FirebaseAuthClient(new FirebaseAuthConfig
    {
        ApiKey = keyVaultSecret.FirebaseApiKey,
        AuthDomain = $"{keyVaultSecret.ProjectId}.firebaseapp.com",
        Providers = new Firebase.Auth.Providers.FirebaseAuthProvider[]
        {
            new EmailProvider()
        }
    });
});

builder.Services.AddScoped<AuthenticationStateProvider, FirebaseAuthProvider>();

builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IHighscoresService, HighscoresService>();
builder.Services.AddScoped<IForumService, ForumService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ILocalCartStorageService, LocalCartStorageService>();
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<ConfirmDialogService>();

builder.Services.AddMudServices();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();