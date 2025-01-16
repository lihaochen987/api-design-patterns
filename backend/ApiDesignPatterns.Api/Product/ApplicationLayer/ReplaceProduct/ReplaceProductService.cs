// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;
using backend.Shared;
using backend.Shared.CommandService;

namespace backend.Product.ApplicationLayer.ReplaceProduct;

public class ReplaceProductService(IProductRepository repository) : ICommandService<ReplaceProduct>
{
    public async Task Execute(ReplaceProduct command)
    {
        await repository.UpdateProductAsync(command.Product);
    }
}
