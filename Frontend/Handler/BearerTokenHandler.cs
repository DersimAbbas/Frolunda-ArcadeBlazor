using System.Net.Http.Headers;
using Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Handler;

public class BearerTokenHandler(IHttpContextAccessor httpContextAccessor, ICookiesTokenService cookiesTokenService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // var token = httpContextAccessor.HttpContext?.Request.Cookies["token"];
        var token = await cookiesTokenService.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}