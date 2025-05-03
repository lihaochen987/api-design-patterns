using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Cache;
using backend.Shared.Caching;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetCreateProductFromCache;

public class GetCreateProductFromCacheHandler(ICreateProductCache cache)
    : IAsyncQueryHandler<GetCreateProductFromCacheQuery, GetCreateProductFromCacheResult>
{
    public async Task<GetCreateProductFromCacheResult> Handle(GetCreateProductFromCacheQuery query)
    {
        string hash = GenerateHash(query.CreateProductRequest);

        if (query.RequestId == null)
        {
            return new GetCreateProductFromCacheResult { CreateProductResponse = null, Hash = null };
        }

        CachedItem<CreateProductResponse>? cachedData = await cache.GetAsync(query.RequestId);

        if (cachedData == null || cachedData.Hash != hash)
        {
            return new GetCreateProductFromCacheResult { CreateProductResponse = null, Hash = null };
        }

        return new GetCreateProductFromCacheResult { CreateProductResponse = cachedData.Item, Hash = cachedData.Hash };
    }

    private static string GenerateHash<T>(T obj)
    {
        string json = System.Text.Json.JsonSerializer.Serialize(obj);
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(json));

        return Convert.ToBase64String(hashBytes);
    }
}
