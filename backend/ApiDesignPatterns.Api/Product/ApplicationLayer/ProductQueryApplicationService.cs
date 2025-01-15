// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;

namespace backend.Product.ApplicationLayer;

public class ProductQueryApplicationService(IProductRepository repository) : IProductQueryApplicationService
{
    public async Task<DomainModels.Product?> GetProductAsync(long id)
    {
        DomainModels.Product? product = await repository.GetProductAsync(id);
        return product ?? null;
    }
}
