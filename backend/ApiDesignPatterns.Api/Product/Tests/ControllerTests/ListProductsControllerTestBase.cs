// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ApplicationLayer.Commands.UpdateListProductsStaleness;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.ApplicationLayer.Queries.MapListProductsResponse;
using backend.Product.ProductControllers;
using backend.Product.Services.Mappers;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class ListProductsControllerTestBase
{
    protected readonly IQueryHandler<ListProductsQuery, PagedProducts> MockListProducts;
    protected readonly IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> MockGetListProductsFromCache;
    protected readonly IQueryHandler<MapListProductsResponseQuery, ListProductsResponse> MapListProducts;
    protected readonly ICommandHandler<UpdateListProductStalenessCommand> MockUpdateListProductStaleness;
    protected readonly ICommandHandler<PersistListProductsToCacheCommand> MockPersistListProductsToCache;
    protected readonly CacheStalenessOptions CacheStalenessOptions;
    protected readonly Fixture Fixture = new();
    protected const int DefaultMaxPageSize = 10;

    protected ListProductsControllerTestBase()
    {
        MockListProducts = Mock.Of<IQueryHandler<ListProductsQuery, PagedProducts>>();
        CacheStalenessOptions = Fixture.Create<CacheStalenessOptions>();
        MockGetListProductsFromCache = Mock.Of<IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult>>();
        MockPersistListProductsToCache = Mock.Of<ICommandHandler<PersistListProductsToCacheCommand>>();
        MockUpdateListProductStaleness = Mock.Of<ICommandHandler<UpdateListProductStalenessCommand>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        IMapper mapper = mapperConfiguration.CreateMapper();
        MapListProducts = new MapListProductsResponseHandler(mapper);
    }

    protected ListProductsController ListProductsController()
    {
        return new ListProductsController(
            MockListProducts,
            MockGetListProductsFromCache,
            MapListProducts,
            MockUpdateListProductStaleness,
            MockPersistListProductsToCache,
            CacheStalenessOptions
        );
    }
}
