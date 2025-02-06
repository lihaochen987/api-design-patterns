// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.DeleteProduct;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.CommandHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class DeleteProductHandlerTestBase
{
    protected readonly ProductRepositoryFake Repository = [];

    protected ICommandHandler<DeleteProductCommand> DeleteProductService()
    {
        return new DeleteProductHandler(Repository);
    }
}
