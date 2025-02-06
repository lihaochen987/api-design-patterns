// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.ReplaceProduct;

public class ReplaceProductHandler(IProductRepository repository) : ICommandHandler<ReplaceProductCommand>
{
    public async Task Handle(ReplaceProductCommand command)
    {
        await repository.UpdateProductAsync(command.Product);
    }
}
