using System.Net;
using KL.Server.Resources;
using KL.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace KL.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly IResourceService _resourceService;

    public ResourcesController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [OutputCache(PolicyName = nameof(CachePolicy))]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Resource>))]
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
    public async Task<IActionResult> Post([FromBody] AddResource request, IOutputCacheStore store, CancellationToken ctx)
    {
        var resource = await _resourceService.AddAsync(request);
        await store.EvictByTagAsync(nameof(CachePolicy), ctx);
        return CreatedAtAction(nameof(Get), new { resource.Id }, resource);
    }

    [Authorize(Policy = "RequireInternalUserRole")]
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteById(Guid id, IOutputCacheStore store, CancellationToken ctx)
    {
        await _resourceService.RemoveAsync(id);
        await store.EvictByTagAsync(nameof(CachePolicy), ctx);
        return NoContent();
    }
}