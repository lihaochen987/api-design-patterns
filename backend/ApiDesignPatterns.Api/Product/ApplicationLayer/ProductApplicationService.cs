// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.InfrastructureLayer;
using backend.Product.ProductControllers;
using backend.Product.Services;

namespace backend.Product.ApplicationLayer;

public class ProductApplicationService(
    IProductRepository repository,
    ProductFieldMaskConfiguration maskConfiguration,
    UpdateProductService updateProductService)
    : IProductApplicationService
{
    public async Task<DomainModels.Product?> GetProductAsync(long id)
    {
        DomainModels.Product? product = await repository.GetProductAsync(id);
        return product ?? null;
    }

    public async Task CreateProductAsync(DomainModels.Product product) =>
        await repository.CreateProductAsync(product);

    public async Task DeleteProductAsync(DomainModels.Product product) =>
        await repository.DeleteProductAsync(product);

    public async Task UpdateProductAsync(
        UpdateProductRequest request,
        DomainModels.Product product)
    {
        updateProductService.UpdateBaseProduct(maskConfiguration, request, product);
        switch (product)
        {
            case PetFood petFood:
                updateProductService.UpdatePetFood(maskConfiguration, request, petFood);
                break;
            case GroomingAndHygiene groomingAndHygiene:
                updateProductService.UpdateGroomingAndHygiene(maskConfiguration, request, groomingAndHygiene);
                break;
        }

        await repository.UpdateProductAsync(product);
    }

    public async Task ReplaceProductAsync(DomainModels.Product product)
    {
        await repository.UpdateProductAsync(product);
    }
}
