// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Commands.DeleteProduct;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.ApplicationLayer.Commands.UpdateListProductsStaleness;
using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ApplicationLayer.Queries.GetProductResponse;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Product.ApplicationLayer.Queries.MapListProductsResponse;
using backend.Product.ApplicationLayer.Queries.MapReplaceProductResponse;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Cache;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.ProductControllers;
using backend.Product.ProductPricingControllers;
using backend.Product.Services;
using backend.Product.Services.Mappers;
using backend.Shared;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.Infrastructure;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace backend.Product;

public class ProductControllerActivator : BaseControllerActivator
{
    private readonly PaginateService<ProductView> _productPaginateService;
    private readonly SqlFilterBuilder _productSqlFilterBuilder;
    private readonly IMapper _mapper;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IConfiguration _configuration;

    public ProductControllerActivator(
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
        : base(configuration)
    {
        _productPaginateService = new PaginateService<ProductView>();

        ProductColumnMapper productColumnMapper = new();
        _productSqlFilterBuilder = new SqlFilterBuilder(productColumnMapper);

        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        _mapper = mapperConfig.CreateMapper();

        ProductFieldPaths productFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(productFieldPaths.ValidPaths);

        _loggerFactory = loggerFactory;

        _configuration = configuration;
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(CreateProductController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductRepository(dbConnection);

            // CreateProduct handler
            var createProductHandler = new CommandDecoratorBuilder<CreateProductCommand>(
                    new CreateProductHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            // CreateProductResponse handler
            var createProductResponseHandler =
                new QueryDecoratorBuilder<MapCreateProductResponseQuery, CreateProductResponse>(
                        new MapCreateProductResponseHandler(_mapper),
                        _loggerFactory,
                        null)
                    .WithLogging()
                    .WithValidation()
                    .WithTransaction()
                    .Build();

            return new CreateProductController(
                createProductHandler,
                createProductResponseHandler,
                _mapper);
        }

        if (type == typeof(DeleteProductController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductRepository(dbConnection);

            // DeleteProduct handler
            var deleteProductHandler = new CommandDecoratorBuilder<DeleteProductCommand>(
                    new DeleteProductHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            // GetProduct handler
            var getProductHandler = new QueryDecoratorBuilder<GetProductQuery, DomainModels.Product>(
                    new GetProductHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new DeleteProductController(deleteProductHandler, getProductHandler);
        }

        if (type == typeof(GetProductController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductViewRepository(dbConnection, _productPaginateService, _productSqlFilterBuilder);

            // GetProductResponse handler
            var getProductResponseHandler = new QueryDecoratorBuilder<GetProductResponseQuery, GetProductResponse>(
                    new GetProductResponseHandler(repository, _mapper),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new GetProductController(
                getProductResponseHandler,
                _fieldMaskConverterFactory);
        }

        if (type == typeof(ListProductsController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductViewRepository(dbConnection, _productPaginateService, _productSqlFilterBuilder);
            IDatabase redisDatabase = new RedisService(_configuration).GetDatabase();
            var redisCache = new ListProductsCache(redisDatabase);

            // Todo: Give CacheStalenessOptions legit values
            var cacheStalenessOptions = new CacheStalenessOptions(TimeSpan.FromDays(1), 0.2, 5, 5);

            // ListProducts handler
            var listProductsHandler = new QueryDecoratorBuilder<ListProductsQuery, PagedProducts>(
                    new ListProductsHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductRead)
                .WithLogging()
                .WithTransaction()
                .Build();

            // GetListProductsFromCache handler
            var getListProductsFromCacheHandler =
                new QueryDecoratorBuilder<GetListProductsFromCacheQuery, CacheQueryResult>(
                        new GetListProductsFromCacheHandler(redisCache),
                        _loggerFactory,
                        null)
                    .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                    .WithBulkhead(BulkheadPolicies.RedisRead)
                    .WithLogging()
                    .Build();

            // persistListProductsToCacheHandler handler
            var persistListProductsToCacheHandler = new CommandDecoratorBuilder<PersistListProductsToCacheCommand>(
                    new PersistListProductsToCacheCommandHandler(redisCache),
                    null,
                    _loggerFactory)
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.RedisWrite)
                .WithLogging()
                .Build();

            // MapListProductsResponse handler
            var mapListProductsResponseHandler =
                new QueryDecoratorBuilder<MapListProductsResponseQuery, ListProductsResponse>(
                        new MapListProductsResponseHandler(_mapper),
                        _loggerFactory,
                        null)
                    .WithLogging()
                    .WithValidation()
                    .Build();

            // UpdateListProductsStaleness handler
            var updateListProductStalenessHandler = new CommandDecoratorBuilder<UpdateListProductStalenessCommand>(
                    new UpdateListProductStalenessHandler(redisCache,
                        _loggerFactory.CreateLogger<UpdateListProductStalenessHandler>()),
                    null,
                    _loggerFactory)
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.RedisWrite)
                .Build();

            return new ListProductsController(
                listProductsHandler,
                getListProductsFromCacheHandler,
                mapListProductsResponseHandler,
                updateListProductStalenessHandler,
                persistListProductsToCacheHandler,
                cacheStalenessOptions);
        }

        if (type == typeof(ReplaceProductController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductRepository(dbConnection);

            // GetProduct handler
            var getProductHandler = new QueryDecoratorBuilder<GetProductQuery, DomainModels.Product>(
                    new GetProductHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // ReplaceProduct handler
            var replaceProductHandler = new CommandDecoratorBuilder<ReplaceProductCommand>(
                    new ReplaceProductHandler(repository, _mapper),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            // MapReplaceProductResponse handler
            var mapReplaceProductResponseHandler =
                new QueryDecoratorBuilder<MapReplaceProductResponseQuery, ReplaceProductResponse>(
                        new MapReplaceProductResponseHandler(_mapper),
                        _loggerFactory,
                        null)
                    .WithLogging()
                    .WithValidation()
                    .Build();

            return new ReplaceProductController(
                getProductHandler,
                replaceProductHandler,
                mapReplaceProductResponseHandler);
        }

        if (type == typeof(UpdateProductController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductRepository(dbConnection);

            // GetProduct handler
            var getProductHandler = new QueryDecoratorBuilder<GetProductQuery, DomainModels.Product>(
                    new GetProductHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // UpdateProduct handler
            var updateProductHandler = new CommandDecoratorBuilder<UpdateProductCommand>(
                    new UpdateProductHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new UpdateProductController(getProductHandler, updateProductHandler, _mapper);
        }

        if (type == typeof(GetProductPricingController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductViewRepository(dbConnection, _productPaginateService, _productSqlFilterBuilder);

            // GetProductResponse handler
            var getProductResponseHandler = new QueryDecoratorBuilder<GetProductResponseQuery, GetProductResponse>(
                    new GetProductResponseHandler(repository, _mapper),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new GetProductController(
                getProductResponseHandler,
                _fieldMaskConverterFactory);
        }

        return null;
    }
}
