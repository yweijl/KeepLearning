using System.Net;
using Core;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    [HttpGet()]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Resource))]
    public IActionResult Get()
    {
        return Ok(new Resource()
        {
            Name = "Test",
            Description = "Desc",
            Comments = new List<string>() { "1","2","3" },
            Source = "source"
        });
    }
}