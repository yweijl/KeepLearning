using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace KL.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Dictionary<string,string>>))]
    [HttpGet]
    public IActionResult Get()
    {
        var claims = new Dictionary<string, string>();
        ClaimsPrincipal.Current?.Claims.ToList().ForEach(x => claims.Add(x.Type, x.Value));
        return Ok(claims);
    }
    
    
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