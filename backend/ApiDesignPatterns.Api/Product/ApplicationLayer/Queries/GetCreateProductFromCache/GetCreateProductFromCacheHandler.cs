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
        if (query.RequestId == null)
        {
            return new GetCreateProductFromCacheResult { CreateProductResponse = null, Hash = null };
        }

        CachedItem<CreateProductResponse>? cachedData = await cache.GetAsync(query.RequestId);
        return cachedData == null
            ? new GetCreateProductFromCacheResult { CreateProductResponse = null, Hash = null }
            : new GetCreateProductFromCacheResult { CreateProductResponse = cachedData.Item, Hash = cachedData.Hash };
    }
}
