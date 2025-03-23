// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.CreateProduct;

public class CreateProductHandler(IProductRepository repository) : ICommandHandler<CreateProductCommand>
{
    public async Task Handle(CreateProductCommand command)
    {
        long id = await repository.CreateProductAsync(command.Product);
        var updatedProduct = command.Product with { Id = id };

        switch (updatedProduct.Category)
        {
            case Category.PetFood when updatedProduct is not PetFood:
            case Category.GroomingAndHygiene when updatedProduct is not GroomingAndHygiene:
                throw new InvalidOperationException();
            case Category.Toys:
            case Category.CollarsAndLeashes:
            case Category.Beds:
            case Category.Feeders:
            case Category.TravelAccessories:
            case Category.Clothing:
                break;
            default:
                switch (updatedProduct)
                {
                    case PetFood petFood:
                        await repository.CreatePetFoodProductAsync(petFood);
                        break;
                    case GroomingAndHygiene groomingAndHygiene:
                        await repository.CreateGroomingAndHygieneProductAsync(groomingAndHygiene);
                        break;
                }

                break;
        }
    }
}
