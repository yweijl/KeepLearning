using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Web;

public class AppAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _authenticationState = new (new ClaimsPrincipal());
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(_authenticationState);
    }
}