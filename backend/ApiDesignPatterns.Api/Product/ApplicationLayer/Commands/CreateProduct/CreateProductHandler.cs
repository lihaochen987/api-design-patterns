// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

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

        await (updatedProduct.Category switch
        {
            Category.PetFood =>
                ProcessSpecializedProduct<PetFood>(updatedProduct, repository.CreatePetFoodProductAsync),
            Category.GroomingAndHygiene => ProcessSpecializedProduct<GroomingAndHygiene>(updatedProduct,
                repository.CreateGroomingAndHygieneProductAsync),
            Category.Toys or Category.CollarsAndLeashes or Category.Beds or
                Category.Feeders or Category.TravelAccessories or Category.Clothing => Task.CompletedTask,
            _ => throw new ArgumentOutOfRangeException(updatedProduct.Category.ToString())
        });
    }

    private static async Task ProcessSpecializedProduct<T>(DomainModels.Product product, Func<T, Task> createAction)
        where T : DomainModels.Product
    {
        if (product is T specializedProduct)
        {
            await createAction(specializedProduct);
        }
        else
        {
            throw new InvalidOperationException(
                $"Product with category {product.Category} must be of type {typeof(T).Name}");
        }
    }
}
