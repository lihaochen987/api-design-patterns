// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;

namespace backend.Product.ApplicationLayer;

public class ProductApplicationService(
    IProductRepository repository)
    : IProductApplicationService
{
    public async Task<DomainModels.Product?> GetProductAsync(long id)
    {
        // Prepare
        DomainModels.Product? product = await repository.GetProductAsync(id);

        // Apply
        return product ?? null;
    }

    public async Task CreateProductAsync(DomainModels.Product product) =>
        // Apply
        await repository.CreateProductAsync(product);

    public async Task DeleteProductAsync(DomainModels.Product product) =>
        // Apply
        await repository.DeleteProductAsync(product);
}
