// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Enums;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetProduct;

public class GetProductHandler(IProductRepository repository) : IAsyncQueryHandler<GetProductQuery, DomainModels.Product?>
{
    public async Task<DomainModels.Product?> Handle(GetProductQuery query)
    {
        var product = await repository.GetProductAsync(query.Id);

        return product?.Category switch
        {
            Category.PetFood => await repository.GetPetFoodProductAsync(query.Id),
            Category.GroomingAndHygiene => await repository.GetGroomingAndHygieneProductAsync(query.Id),
            _ => product
        };
    }
}
