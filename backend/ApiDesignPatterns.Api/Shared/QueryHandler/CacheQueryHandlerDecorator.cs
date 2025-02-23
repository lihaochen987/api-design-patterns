// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Caching.Memory;

namespace backend.Shared.QueryHandler;

public class CachingQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    IMemoryCache cache,
    TimeSpan hardTtl,
    TimeSpan softTtl)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<TResult?> Handle(TQuery query)
    {
        string cacheKey = $"{typeof(TQuery).Name}:{query.GetHashCode()}";

        if (cache.TryGetValue<(TResult?, DateTime)>(cacheKey, out var cached))
        {
            (TResult? result, DateTime expiry) = cached;
            DateTime now = DateTime.UtcNow;

            // If within soft TTL, return cached result
            if (now <= expiry)
            {
                return result;
            }

            // If past soft TTL but before hard TTL, refresh the TTL and return result
            if (now <= expiry.Add(hardTtl))
            {
                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(hardTtl)
                    .SetSize(1);

                cache.Set(cacheKey, (result, DateTime.UtcNow.Add(softTtl)), options);
                return result;
            }
        }

        var newResult = await queryHandler.Handle(query);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(hardTtl)
            .SetSize(1);

        cache.Set(cacheKey, (newResult, DateTime.UtcNow.Add(softTtl)), cacheOptions);

        return newResult;
    }
}
