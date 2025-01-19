// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.InfrastructureLayer;
using backend.Product.Services.ProductServices;
using backend.Shared;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.UpdateProduct;

public class UpdateProductHandler(
    IProductRepository repository,
    UpdateProductTypeService updateProductTypeService)
    : ICommandHandler<UpdateProductQuery>
{
    public async Task Handle(UpdateProductQuery command)
    {
        updateProductTypeService.UpdateBaseProduct(command.Request, command.Product);
        switch (command.Product)
        {
            case PetFood petFood:
                updateProductTypeService.UpdatePetFood(command.Request, petFood);
                break;
            case GroomingAndHygiene groomingAndHygiene:
                updateProductTypeService.UpdateGroomingAndHygiene(command.Request, groomingAndHygiene);
                break;
        }

        await repository.UpdateProductAsync(command.Product);
    }
}
