// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Cache;
using backend.Shared;
using backend.Shared.Caching;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;

public class GetListProductsFromCacheHandler(
    IListProductsCache cache)
    : IAsyncQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult>
{
    public async Task<CacheQueryResult> Handle(GetListProductsFromCacheQuery query)
    {
        string cacheKey = GenerateCacheKey(query.Request);
        try
        {
            CachedItem<ListProductsResponse>? cachedData = await cache.GetAsync(cacheKey);

            if (ShouldCheckStaleness(query.CheckRate))
            {
                return new CacheQueryResult
                {
                    ProductsResponse = cachedData?.Item, CacheKey = cacheKey, SelectedForStalenessCheck = true
                };
            }

            return new CacheQueryResult
            {
                ProductsResponse = cachedData?.Item, CacheKey = cacheKey, SelectedForStalenessCheck = false
            };
        }
        catch
        {
            return new CacheQueryResult { ProductsResponse = null, CacheKey = cacheKey };
        }
    }

    private static string GenerateCacheKey(ListProductsRequest request)
    {
        var keyParts = new List<string> { "products", $"maxsize:{request.MaxPageSize}" };

        if (!string.IsNullOrEmpty(request.PageToken))
        {
            keyParts.Add($"page-token:{request.PageToken}");
        }

        if (!string.IsNullOrEmpty(request.Filter))
        {
            string normalizedFilter = request.Filter.Trim().ToLowerInvariant();
            keyParts.Add($"filter:{normalizedFilter}");
        }

        return string.Join(":", keyParts);
    }

    private static bool ShouldCheckStaleness(double checkRate)
    {
        return RandomUtility.CheckProbability(checkRate);
    }
}
