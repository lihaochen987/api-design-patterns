// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.InfrastructureLayer;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.CreateProduct;

public class CreateProductHandler(IProductRepository repository) : ICommandHandler<CreateProductQuery>
{
    public async Task Handle(CreateProductQuery command)
    {
        long id = await repository.CreateProductAsync(command.Product);
        command.Product.Id = id;
        switch (command.Product)
        {
            case PetFood petFood:
                await repository.CreatePetFoodProductAsync(petFood);
                break;
            case GroomingAndHygiene groomingAndHygiene:
                await repository.CreateGroomingAndHygieneProductAsync(groomingAndHygiene);
                break;
        }
    }
}
