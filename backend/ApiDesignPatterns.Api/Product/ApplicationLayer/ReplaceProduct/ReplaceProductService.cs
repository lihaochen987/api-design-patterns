// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;
using backend.Shared;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.ReplaceProduct;

public class ReplaceProductHandler(IProductRepository repository) : ICommandHandler<ReplaceProduct>
{
    public async Task Handle(ReplaceProduct command)
    {
        await repository.UpdateProductAsync(command.Product);
    }
}
