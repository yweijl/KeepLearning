using System.Net;
using KL.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KL.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly IResourceService _resourceService;

    public ResourcesController(IResourceService resourceService)
    {
        this._resourceService = resourceService;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Resource>))]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> List()
    {
        var resources = await _resourceService.ListAsync();
        return Ok(resources);
    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Resource))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Get(Guid id)
    {
        var resources = await _resourceService.GetAsync(id);
        return Ok(resources);
    }

    [Authorize(Policy = "RequireInternalUserRole")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Post([FromBody] AddResourceRequest request)
    {
        var resource = await _resourceService.AddAsync(request);
        return CreatedAtAction(nameof(Get), new { resource.Id }, resource);
    }

    [Authorize(Policy = "RequireInternalUserRole")]
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        await _resourceService.RemoveAsync(id);
        return NoContent();
    }
}