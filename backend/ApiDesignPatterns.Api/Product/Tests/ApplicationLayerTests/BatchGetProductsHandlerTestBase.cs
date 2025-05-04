// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.BatchGetProducts;
using backend.Shared;
using backend.Shared.QueryHandler;
using backend.Product.Tests.TestHelpers.Fakes;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class BatchGetProductsHandlerTestBase
{
    protected readonly ProductRepositoryFake Repository = [];
    protected readonly Fixture Fixture = new();

    protected IAsyncQueryHandler<BatchGetProductsQuery, Result<List<DomainModels.Product>>> GetBatchGetProductsHandler()
    {
        return new BatchGetProductsHandler(Repository);
    }
}
