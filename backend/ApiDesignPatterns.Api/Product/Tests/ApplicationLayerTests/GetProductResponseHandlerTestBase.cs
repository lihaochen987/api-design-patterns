// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.GetProductResponse;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Views;
using backend.Product.Services;
using backend.Product.Services.Mappers;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class GetProductResponseHandlerTestBase
{
    protected readonly ProductViewRepositoryFake Repository = new(new PaginateService<ProductView>());
    protected readonly IMapper Mapper;
    protected readonly IProductTypeMapper ProductTypeMapper;

    protected GetProductResponseHandlerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterProductMappings();
        Mapper = new Mapper(config);
        ProductTypeMapper = new ProductTypeMapper(Mapper);
    }

    protected IAsyncQueryHandler<GetProductResponseQuery, GetProductResponse?> GetProductResponseHandler()
    {
        return new GetProductResponseHandler(Repository, ProductTypeMapper);
    }
}
