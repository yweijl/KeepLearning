using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet( "login/{isCommunityUser}")]
    public IResult Login(bool isCommunityUser)
    {
        var principal = new ClaimsPrincipal(new List<ClaimsIdentity>()
        {
            new(new List<Claim>()
                {
                    new (ClaimTypes.Name,"Professor X"),
                    new (ClaimTypes.Role, isCommunityUser ? "CommunityUser" : "InternalUser"),
                }, 
                CookieAuthenticationDefaults.AuthenticationScheme),
        });
        
        return Results.SignIn(principal, new AuthenticationProperties()
        {
            IsPersistent = true,
        });
    }

    [HttpGet("logout")]
    public IResult Logout()
    {
        return Results.SignOut();
    }
}