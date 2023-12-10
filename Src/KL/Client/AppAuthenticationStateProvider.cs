using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace KL.Client;

public class AppAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private AuthenticationState _authenticationState = new(new ClaimsPrincipal());

    public AppAuthenticationStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var claims = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>("api/User");
        if (claims!.Any())
        {
            _authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                claims?.Select(x => new Claim(x.Key, x.Value))
            )));
        }

        return _authenticationState;
    }

    public async Task LoginAsync(bool isCommunityUser)
    {
        var result = await _httpClient.GetAsync($"api/User/login/{isCommunityUser}");
    }
}