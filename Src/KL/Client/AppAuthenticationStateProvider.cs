using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace KL.Client;

public class AppAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public AppAuthenticationStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await GetUserClaimsAsync();
        return new AuthenticationState(user);
    }

    public async Task LoginAsync(bool isCommunityUser)
    {
        await _httpClient.GetAsync($"api/User/login/{isCommunityUser}");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async Task<ClaimsPrincipal> GetUserClaimsAsync()
    {
        var claims = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>("api/User");
        
        if (!claims!.Any())
        {
            return new ClaimsPrincipal();
        }
        
        return new ClaimsPrincipal(new ClaimsIdentity(
                claims?.Select(x => new Claim(x.Key, x.Value)), "cookie"
            ));
    }

    public async Task LogoutAsync()
    {
        await _httpClient.GetAsync("api/User/logout");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}