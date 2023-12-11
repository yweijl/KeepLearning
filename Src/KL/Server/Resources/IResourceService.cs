using KL.Shared.Resources;

namespace KL.Server.Resources;

public interface IResourceService
{
    public Task<List<Resource>> ListAsync();
    public Task<Resource> AddAsync(AddResource request);
    public Task RemoveAsync(Guid id);
    Task<Resource> GetAsync(Guid id);
}