// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.DeleteProduct;

public class DeleteProductHandler(IProductRepository repository) : ICommandHandler<DeleteProductQuery>
{
    public async Task Handle(DeleteProductQuery command)
    {
        await repository.DeleteProductAsync(command.Id);
    }
}
