// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.DomainModels;
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
        switch (command.Product)
        {
            case PetFood petFood:
                mapper.Map(command.Request, petFood);
                break;

            case GroomingAndHygiene groomingAndHygiene:
                mapper.Map(command.Request, groomingAndHygiene);
                break;

            default:
                mapper.Map(command.Request, command.Product);
                break;
        }

        await repository.UpdateProductAsync(command.Product);
    }
}
