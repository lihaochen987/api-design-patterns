// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.ReplaceProduct;

public class ReplaceProductHandler(IProductRepository repository) : ICommandHandler<ReplaceProductQuery>
{
    public async Task Handle(ReplaceProductQuery command)
    {
        await repository.UpdateProductAsync(command.Product);
    }
}
