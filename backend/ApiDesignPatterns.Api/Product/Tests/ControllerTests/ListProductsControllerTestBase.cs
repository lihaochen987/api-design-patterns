// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.ProductControllers;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class ListProductsControllerTestBase
{
    protected readonly IQueryHandler<ListProductsQuery, PagedProducts> MockListProducts;
    protected readonly IQueryHandler<GetListProductsFromCacheQuery, ListProductsResponse> MockGetListProductsFromCache;
    private readonly IMapper _mapper;
    protected const int DefaultMaxPageSize = 10;

    protected ListProductsControllerTestBase()
    {
        MockListProducts = Mock.Of<IQueryHandler<ListProductsQuery, PagedProducts>>();
        MockGetListProductsFromCache = Mock.Of<IQueryHandler<GetListProductsFromCacheQuery, ListProductsResponse>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
    }

    protected ListProductsController ListProductsController()
    {
        return new ListProductsController(MockListProducts, MockGetListProductsFromCache, _mapper);
    }
}
