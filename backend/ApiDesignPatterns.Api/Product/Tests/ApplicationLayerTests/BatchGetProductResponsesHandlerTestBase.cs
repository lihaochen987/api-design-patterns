﻿// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.BatchGetProductResponses;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Views;
using backend.Product.Services.Mappers;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class BatchGetProductResponsesHandlerTestBase
{
    protected readonly ProductViewRepositoryFake Repository = new(new PaginateService<ProductView>());
    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected BatchGetProductResponsesHandlerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterProductMappings();
        Mapper = new Mapper(config);
    }

    protected IAsyncQueryHandler<BatchGetProductResponsesQuery, Result<List<GetProductResponse>>>
        GetBatchGetProductsHandler()
    {
        return new BatchGetProductResponsesHandler(Repository, Mapper);
    }
}
