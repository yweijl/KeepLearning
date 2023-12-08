namespace Core.Resources;

public class ResourceService : IResourceService
{
    private static readonly Dictionary<Guid, Resource> _resources = new ();
    public Task<List<Resource>> ListAsync()
    {
        return Task.FromResult(_resources.Values.ToList());
    }

    public async Task<Resource> AddAsync(AddResourceRequest request)
    {
        var resource = new Resource()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Source = request.Source,
            Comments = request.Comments,
        };
        
        _resources.Add(resource.Id, resource);
        return resource;
    }

    public async Task RemoveAsync(Guid id)
    {
        _resources.Remove(id);
    }

    public async Task<Resource> GetAsync(Guid id)
    {
        return _resources[id];
    }
}