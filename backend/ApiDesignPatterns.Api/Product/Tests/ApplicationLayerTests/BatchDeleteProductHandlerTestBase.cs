// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.BatchDeleteProducts;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class BatchDeleteProductsHandlerTestBase
{
    protected readonly ProductRepositoryFake Repository = [];
    protected readonly Fixture Fixture = new();

    protected ICommandHandler<BatchDeleteProductsCommand> BatchDeleteProductsService()
    {
        return new BatchDeleteProductsHandler(Repository);
    }
}
