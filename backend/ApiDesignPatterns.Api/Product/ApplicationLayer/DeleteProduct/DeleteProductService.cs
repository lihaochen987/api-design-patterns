// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;
using backend.Shared;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.DeleteProduct;

public class DeleteProductHandler(IProductRepository repository) : ICommandHandler<DeleteProduct>
{
    public async Task Handle(DeleteProduct command)
    {
        await repository.DeleteProductAsync(command.Product);
    }
}
