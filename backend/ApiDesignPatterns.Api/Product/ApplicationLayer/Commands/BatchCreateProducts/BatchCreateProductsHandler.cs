// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.BatchCreateProducts;

public class BatchCreateProductsHandler(IProductRepository repository) : ICommandHandler<BatchCreateProductsCommand>
{
    public async Task Handle(BatchCreateProductsCommand command)
    {
        if (command.Products.Count == 0)
        {
            return;
        }

        var productIds = await repository.CreateProductsAsync(command.Products);

        var createdProducts = command.Products.Zip(productIds, (product, id) => product with { Id = id }).ToList();

        var petFoodProducts = new List<PetFood>();
        var groomingProducts = new List<GroomingAndHygiene>();

        foreach (var createdProduct in createdProducts)
        {
            switch (createdProduct.Category)
            {
                case Category.PetFood when createdProduct is not PetFood:
                case Category.GroomingAndHygiene when createdProduct is not GroomingAndHygiene:
                    throw new InvalidOperationException(
                        $"Product with ID {createdProduct.Id} has mismatched category and type");
                case Category.Toys:
                case Category.CollarsAndLeashes:
                case Category.Beds:
                case Category.Feeders:
                case Category.TravelAccessories:
                case Category.Clothing:
                    break;
                default:
                    switch (createdProduct)
                    {
                        case PetFood petFood:
                            petFoodProducts.Add(petFood);
                            break;
                        case GroomingAndHygiene groomingAndHygiene:
                            groomingProducts.Add(groomingAndHygiene);
                            break;
                    }

                    break;
            }
        }

        var tasks = new List<Task>();

        if (petFoodProducts.Count > 0)
        {
            tasks.Add(repository.CreatePetFoodProductsAsync(petFoodProducts));
        }

        if (groomingProducts.Count > 0)
        {
            tasks.Add(repository.CreateGroomingAndHygieneProductsAsync(groomingProducts));
        }

        await Task.WhenAll(tasks);
    }
}
