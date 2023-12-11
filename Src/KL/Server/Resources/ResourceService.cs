using KL.Shared.Resources;

namespace KL.Server.Resources;

public class ResourceService : IResourceService
{
    private readonly ResourceRepository _resourceRepository;

    public ResourceService(ResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public Task<List<Resource>> ListAsync()
    {
        return _resourceRepository.ListAsync();
    }

    public async Task<Resource> AddAsync(AddResource request)
    {
        var resource = new Resource()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Source = request.Source,
            Comments = request.Comments,
        };

        await _resourceRepository.AddAsync(resource);
        
        return resource;
    }

    public Task RemoveAsync(Guid id)
    {
        return _resourceRepository.DeleteAsync(id);
    }

    public Task<Resource> GetAsync(Guid id)
    {
        return _resourceRepository.GetById(id);
    }
}