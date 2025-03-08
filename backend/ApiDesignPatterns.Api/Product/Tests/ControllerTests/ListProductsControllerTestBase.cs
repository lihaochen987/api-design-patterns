// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.ApplicationLayer.Queries.MapListProductsResponse;
using backend.Product.ProductControllers;
using backend.Product.Services.Mappers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class ListProductsControllerTestBase
{
    protected readonly IQueryHandler<ListProductsQuery, PagedProducts> MockListProducts;
    private readonly IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> _mockGetListProductsFromCache;
    private readonly IQueryHandler<MapListProductsResponseQuery, ListProductsResponse> _mapListProducts;
    private readonly ICommandHandler<PersistListProductsToCacheCommand> _mockPersistListProductsToCache;
    protected const int DefaultMaxPageSize = 10;

    protected ListProductsControllerTestBase()
    {
        MockListProducts = Mock.Of<IQueryHandler<ListProductsQuery, PagedProducts>>();
        _mockGetListProductsFromCache = Mock.Of<IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult>>();
        _mockPersistListProductsToCache = Mock.Of<ICommandHandler<PersistListProductsToCacheCommand>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        IMapper mapper = mapperConfiguration.CreateMapper();
        _mapListProducts = new MapListProductsResponseHandler(mapper);
    }

    protected ListProductsController ListProductsController()
    {
        return new ListProductsController(
            MockListProducts,
            _mockGetListProductsFromCache,
            _mapListProducts,
            _mockPersistListProductsToCache);
    }
}
