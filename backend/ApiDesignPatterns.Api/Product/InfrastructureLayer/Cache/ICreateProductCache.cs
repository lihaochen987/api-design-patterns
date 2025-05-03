// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Shared.Caching;

namespace backend.Product.InfrastructureLayer.Cache;

public interface ICreateProductCache
{
    Task<CachedItem<CreateProductResponse>?> GetAsync(string key);
}
