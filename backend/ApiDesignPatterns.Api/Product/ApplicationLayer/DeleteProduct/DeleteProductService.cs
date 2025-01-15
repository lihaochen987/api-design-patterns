// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;
using backend.Shared;

namespace backend.Product.ApplicationLayer.DeleteProduct;

public class DeleteProductService(IProductRepository repository) : ICommandService<DeleteProduct>
{
    public async Task Execute(DeleteProduct command)
    {
        await repository.DeleteProductAsync(command.Product);
    }
}
