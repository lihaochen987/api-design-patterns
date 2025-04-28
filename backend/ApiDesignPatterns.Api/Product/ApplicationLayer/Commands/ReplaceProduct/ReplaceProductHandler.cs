// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.ReplaceProduct;

public class ReplaceProductHandler(
    IProductRepository repository,
    IMapper mapper)
    : ICommandHandler<ReplaceProductCommand>
{
    public async Task Handle(ReplaceProductCommand command)
    {
        DomainModels.Product replacedProduct = command.Request.Category switch
        {
            nameof(Category.PetFood) => mapper.Map<PetFood>(command.Request),
            nameof(Category.GroomingAndHygiene) => mapper.Map<GroomingAndHygiene>(command.Request),
            _ => mapper.Map<DomainModels.Product>(command.Request)
        };

        var replacedProductWithId = replacedProduct with { Id = command.ExistingProductId };
        await repository.UpdateProductAsync(replacedProductWithId);
        switch (replacedProductWithId)
        {
            case PetFood petFood:
                await repository.UpdatePetFoodProductAsync(petFood);
                break;
            case GroomingAndHygiene groomingAndHygiene:
                await repository.UpdateGroomingAndHygieneProductAsync(groomingAndHygiene);
                break;
        }
    }
}
