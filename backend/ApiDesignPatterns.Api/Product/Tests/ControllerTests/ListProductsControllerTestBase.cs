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
using backend.Shared.QueryProcessor;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class ListProductsControllerTestBase
{
    protected readonly Mock<IQueryProcessor> MockQueryProcessor;
    protected readonly ICommandHandler<UpdateListProductStalenessCommand> MockUpdateListProductStaleness;
    protected readonly ICommandHandler<PersistListProductsToCacheCommand> MockPersistListProductsToCache;
    protected readonly CacheStalenessOptions CacheStalenessOptions;
    protected readonly Fixture Fixture = new();
    protected const int DefaultMaxPageSize = 10;

    protected readonly Mock<IQueryHandler<ListProductsQuery, PagedProducts>> MockListProducts;

    protected readonly Mock<IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult>>
        MockGetListProductsFromCacheHandler;

    protected readonly IQueryHandler<MapListProductsResponseQuery, ListProductsResponse> MapListProductsResponseHandler;

    protected ListProductsControllerTestBase()
    {
        MockQueryProcessor = new Mock<IQueryProcessor>();
        MockPersistListProductsToCache = Mock.Of<ICommandHandler<PersistListProductsToCacheCommand>>();
        MockUpdateListProductStaleness = Mock.Of<ICommandHandler<UpdateListProductStalenessCommand>>();
        CacheStalenessOptions = Fixture.Create<CacheStalenessOptions>();

        MockListProducts = new Mock<IQueryHandler<ListProductsQuery, PagedProducts>>();
        MockQueryProcessor
            .Setup(qp => qp.Process(It.IsAny<ListProductsQuery>()))
            .Returns<ListProductsQuery>(query =>
                Task.FromResult(MockListProducts.Object.Handle(query).Result));

        MockGetListProductsFromCacheHandler =
            new Mock<IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult>>();
        MockQueryProcessor
            .Setup(qp => qp.Process(It.IsAny<GetListProductsFromCacheQuery>()))
            .Returns<GetListProductsFromCacheQuery>(query =>
                Task.FromResult(MockGetListProductsFromCacheHandler.Object.Handle(query).Result));

        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        IMapper mapper = mapperConfiguration.CreateMapper();
        MapListProductsResponseHandler = new MapListProductsResponseHandler(mapper);
        MockQueryProcessor
            .Setup(qp => qp.Process(It.IsAny<MapListProductsResponseQuery>()))
            .Returns<MapListProductsResponseQuery>(query =>
                Task.FromResult(MapListProductsResponseHandler.Handle(query).Result));
    }

    protected ListProductsController ListProductsController()
    {
        return new ListProductsController(
            MockQueryProcessor.Object,
            MockUpdateListProductStaleness,
            MockPersistListProductsToCache,
            CacheStalenessOptions
        );
    }
}
