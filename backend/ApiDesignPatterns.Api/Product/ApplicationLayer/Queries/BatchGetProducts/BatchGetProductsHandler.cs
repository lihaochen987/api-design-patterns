// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Enums;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.BatchGetProducts;

public class BatchGetProductsHandler(IProductRepository repository)
    : IAsyncQueryHandler<BatchGetProductsQuery, Result<List<DomainModels.Product>>>
{
    public async Task<Result<List<DomainModels.Product>>> Handle(BatchGetProductsQuery query)
    {
        var basicProducts = await repository.GetProductsByIds(query.ProductIds);

        var missingProductIds = query.ProductIds
            .Where(id => basicProducts.All(p => p.Id != id))
            .ToList();

        if (missingProductIds.Count != 0)
        {
            return Result.Failure<List<DomainModels.Product>>(
                $"Products not found: {string.Join(", ", missingProductIds)}");
        }

        throw new NotImplementedException();

        // var productTasks = basicProducts.Select(async product =>
        // {
        //     return product.Category switch
        //     {
        //         Category.PetFood => await repository.GetPetFoodProductAsync(product.Id),
        //         Category.GroomingAndHygiene => await repository.GetGroomingAndHygieneProductAsync(product.Id),
        //         _ => product
        //     };
        // });
        //
        // var detailedProducts = await Task.WhenAll(productTasks);
        //
        // return Result.Success(detailedProducts.Where(x => x != null).ToList())!;
    }
}
