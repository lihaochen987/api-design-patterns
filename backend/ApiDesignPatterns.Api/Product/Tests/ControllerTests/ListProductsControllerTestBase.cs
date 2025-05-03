// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ApplicationLayer.Commands.UpdateListProductsStaleness;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.ApplicationLayer.Queries.MapListProductsResponse;
using backend.Product.Controllers.Product;
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
    private readonly ICommandHandler<UpdateListProductStalenessCommand> _mockUpdateListProductStaleness;
    protected readonly ICommandHandler<PersistListProductsToCacheCommand> MockPersistListProductsToCache;
    private readonly CacheStalenessOptions _cacheStalenessOptions;
    protected readonly Fixture Fixture = new();
    protected const int DefaultMaxPageSize = 10;
    protected const string CacheKey = "products-cache-key";

    protected ListProductsControllerTestBase()
    {
        MockQueryProcessor = new Mock<IQueryProcessor>();
        MockPersistListProductsToCache = Mock.Of<ICommandHandler<PersistListProductsToCacheCommand>>();
        _mockUpdateListProductStaleness = Mock.Of<ICommandHandler<UpdateListProductStalenessCommand>>();
        _cacheStalenessOptions = Fixture.Create<CacheStalenessOptions>();

        // ListProductsQuery
        Mock<IAsyncQueryHandler<ListProductsQuery, PagedProducts>> listProducts = new();
        MockQueryProcessor
            .Setup(qp => qp.Process(It.IsAny<ListProductsQuery>()))
            .Returns<ListProductsQuery>(query =>
                Task.FromResult(listProducts.Object.Handle(query).Result));

        // GetListProductsFromCacheQuery
        Mock<IAsyncQueryHandler<GetListProductsFromCacheQuery, GetListProductsFromCacheResult>>
            getListProductsFromCacheHandler = new();
        MockQueryProcessor
            .Setup(qp => qp.Process(It.IsAny<GetListProductsFromCacheQuery>()))
            .Returns<GetListProductsFromCacheQuery>(query =>
                Task.FromResult(getListProductsFromCacheHandler.Object.Handle(query).Result));

        // MapListProductsResponseQuery
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        IMapper mapper = mapperConfiguration.CreateMapper();
        ISyncQueryHandler<MapListProductsResponseQuery, ListProductsResponse> mapListProductsResponseHandler =
            new MapListProductsResponseHandler(mapper);
        MockQueryProcessor
            .Setup(qp => qp.Process(It.IsAny<MapListProductsResponseQuery>()))
            .Returns<MapListProductsResponseQuery>(query =>
                Task.FromResult(mapListProductsResponseHandler.Handle(query)));
    }

    protected ListProductsController ListProductsController()
    {
        return new ListProductsController(
            MockQueryProcessor.Object,
            _mockUpdateListProductStaleness,
            MockPersistListProductsToCache,
            _cacheStalenessOptions
        );
    }
}
