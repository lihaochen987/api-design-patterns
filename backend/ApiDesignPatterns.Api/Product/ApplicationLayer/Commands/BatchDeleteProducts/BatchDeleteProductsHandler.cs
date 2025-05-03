// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.BatchDeleteProducts;

public class BatchDeleteProductsHandler(IProductRepository repository) : ICommandHandler<BatchDeleteProductsCommand>
{
    public async Task Handle(BatchDeleteProductsCommand command)
    {
        await repository.DeleteProductsAsync(command.ProductIds);
    }
}
