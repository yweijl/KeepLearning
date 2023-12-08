using System.Net;
using Core.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourcesController(
    IResourceService resourceService
    ) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Resource>))]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> List()
    {
        var resources = await resourceService.ListAsync();
        return Ok(resources);
    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Resource))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Get(Guid id)
    {
        var resources = await resourceService.GetAsync(id);
        return Ok(resources);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Post([FromBody] AddResourceRequest request)
    {
        var resource = await resourceService.AddAsync(request);
        return CreatedAtAction(nameof(Get), new { resource.Id }, resource);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        await resourceService.RemoveAsync(id);
        return NoContent();
    }
}