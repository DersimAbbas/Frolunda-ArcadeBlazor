using System.Text.Json;
using Blazored.LocalStorage;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Frontend.Components;
using Frontend.Handler;
using Frontend.Services;
using Frontend.Services.Firebase;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents() 
    .AddInteractiveServerComponents();


builder.Services.AddAntiforgery(options =>
{
    // Optionally configure settings here (e.g., set a custom header name)
});

builder.Services.AddBlazoredLocalStorage();



builder.Services.AddScoped<FirebaseAuthClient>(sp =>
{
    var config = new FirebaseAuthConfig
    {
        ***REMOVED***,
        AuthDomain = "frolundaarcade.firebaseapp.com",
        Providers = new FirebaseAuthProvider[]
        {
            new EmailProvider()
        }
    };

    return new FirebaseAuthClient(config);
});


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddHttpContextAccessor(); // Must have this first
builder.Services.AddScoped<ICookiesTokenService, CookiesTokenService>();

builder.Services.AddScoped<BearerTokenHandler>();

builder.Services.AddHttpClient("AuthenticatedClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:44313/");
}).AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"));


builder.Services.AddScoped<IFirebaseAuthService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var namedClient = httpClientFactory.CreateClient("AuthenticatedClient");
    var firebaseClient = sp.GetRequiredService<FirebaseAuthClient>();
    var tokenService = sp.GetRequiredService<ICookiesTokenService>();

    return new FirebaseAuthService(firebaseClient, namedClient, tokenService);
});

builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCookiePolicy();


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
