using KL.Server.Settings;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;
using Azure.Identity;
using KL.Shared.Resources;
using Microsoft.Azure.Cosmos.Linq;

namespace KL.Server.Resources;

public class ResourceRepository
{
    private readonly Container _container;
    private readonly ILogger<ResourceRepository> _logger;

    public ResourceRepository(IOptions<CosmosSettings> settings, ILogger<ResourceRepository> logger)
    {
        _logger = logger;
        _container = new CosmosClientBuilder(settings.Value.Endpoint, new DefaultAzureCredential())
            .WithSerializerOptions(new CosmosSerializationOptions()
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
            })
            .Build()
            .GetContainer(settings.Value.DbName, settings.Value.Container);
    }

    public async Task<List<Resource>> ListAsync()
    {
        var resources = new List<Resource>();
        using var iterator = _container.GetItemLinqQueryable<Resource>().ToFeedIterator();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();

            // Iterate query results
            resources.AddRange(response);
        }
        return resources;
    }

    public async Task<Resource> GetById(Guid id)
    {
        var response =
            await _container.ReadItemAsync<Resource>(id.ToString(),
                new PartitionKey(id.ToString()));

        return response.Resource;
    }

    public async Task AddAsync(Resource resource)
    {
        try
        {
            var result = await _container.CreateItemAsync(resource);
        }
        catch (CosmosException e)
        {
            _logger.LogError(e, "Failed to add resource: {resource}", resource);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
       await _container.DeleteItemAsync<Resource>(id.ToString(), new PartitionKey(id.ToString()));
    }
}