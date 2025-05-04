using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Cache;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.CacheCreateProductResponses;

public class CacheCreateProductResponsesHandler(IBatchCreateProductsCache cache)
    : ICommandHandler<CacheCreateProductResponsesCommand>
{
    public async Task Handle(CacheCreateProductResponsesCommand command)
    {
        string hash = GenerateHash(command.CreateProductRequests);
        var cachedItem = new CachedItem<IEnumerable<CreateProductResponse>>
        {
            Hash = hash,
            Item = command.CreateProductResponses,
            Timestamp = DateTime.UtcNow + TimeSpan.FromSeconds(5)
        };

        await cache.SetAsync(command.RequestId, cachedItem, TimeSpan.FromSeconds(5));
    }

    private static string GenerateHash<T>(T obj)
    {
        string json = System.Text.Json.JsonSerializer.Serialize(obj);
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(json));

        return Convert.ToBase64String(hashBytes);
    }
}
