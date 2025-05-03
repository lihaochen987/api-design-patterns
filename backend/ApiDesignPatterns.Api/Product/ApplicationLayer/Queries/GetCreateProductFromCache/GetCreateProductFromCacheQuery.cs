// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetCreateProductFromCache;

public class GetCreateProductFromCacheQuery : IQuery<GetCreateProductFromCacheResult>
{
    public string? RequestId { get; set; }
}
