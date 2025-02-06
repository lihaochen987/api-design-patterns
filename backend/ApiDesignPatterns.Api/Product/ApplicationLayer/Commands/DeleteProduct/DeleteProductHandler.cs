// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.DeleteProduct;

public class DeleteProductHandler(IProductRepository repository) : ICommandHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand command)
    {
        await repository.DeleteProductAsync(command.Id);
    }
}
