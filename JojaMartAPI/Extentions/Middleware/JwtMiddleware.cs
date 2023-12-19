using JojaMartAPI.DTOs.JwtDtos;
using JojaMartAPI.Services.Interfaces;
using Microsoft.Identity.Client;

namespace JojaMartAPI.Extentions.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next; IHttpClientFactory _httpClientFactory;

    public JwtMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _httpClientFactory = httpClientFactory;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (!string.IsNullOrEmpty(accessToken) && tokenService.IsTokenExpired(accessToken))
                {
                    var refreshToken = tokenService.GetRefreshTokenDboByAccessToken(accessToken).Result.RefreshToken;

                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        var httpClient = _httpClientFactory.CreateClient();
                        var reauthenticationEndpoint = "https://localhost:7177/User/RefreshJwt";

                        var reauthenticationRequest = new RefreshRequest { RefreshToken = refreshToken };

                        var response = await httpClient.PostAsJsonAsync(reauthenticationEndpoint, reauthenticationRequest);

                        if (response.IsSuccessStatusCode)
                        {
                            var newTokens = await response.Content.ReadFromJsonAsync<AuthenticationResult>();

                            if (newTokens != null)
                            {
                                context.Request.Headers["Authorization"] = "Bearer " + newTokens.AccessToken;
                            }
                        }
                    }
                }
            }
        }

        await _next.Invoke(context);
    }
}