namespace KL.Shared.Resources;

public interface IResourceService
{
    public Task<List<Resource>> ListAsync();
    public Task<Resource> AddAsync(AddResourceRequest request);
    public Task RemoveAsync(Guid id);
    Task<Resource> GetAsync(Guid id);
}