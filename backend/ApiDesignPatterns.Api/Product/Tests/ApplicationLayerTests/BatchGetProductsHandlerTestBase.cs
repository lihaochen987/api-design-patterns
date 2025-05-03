// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Product.ApplicationLayer.Queries.BatchGetProducts;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Views;
using backend.Product.Services.Mappers;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class BatchGetProductsHandlerTestBase
{
    protected readonly ProductViewRepositoryFake Repository = new(new PaginateService<ProductView>());
    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected BatchGetProductsHandlerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = new Mapper(mapperConfiguration);
    }

    protected IAsyncQueryHandler<BatchGetProductsQuery, Result<List<GetProductResponse>>> GetBatchGetProductsHandler()
    {
        return new BatchGetProductsHandler(Repository, Mapper);
    }
}
