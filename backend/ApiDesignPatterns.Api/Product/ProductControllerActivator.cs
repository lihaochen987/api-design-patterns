// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.BatchCreateProducts;
using backend.Product.ApplicationLayer.Commands.BatchDeleteProducts;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Commands.DeleteProduct;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.ApplicationLayer.Commands.UpdateListProductsStaleness;
using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.ApplicationLayer.Queries.BatchGetProducts;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ApplicationLayer.Queries.GetProductResponse;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;
using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Product.ApplicationLayer.Queries.MapListProductsResponse;
using backend.Product.ApplicationLayer.Queries.MapReplaceProductResponse;
using backend.Product.Controllers.Product;
using backend.Product.Controllers.ProductPricing;
using backend.Product.InfrastructureLayer.Cache;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.Services;
using backend.Product.Services.Mappers;
using backend.Shared;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.Infrastructure;
using backend.Shared.QueryHandler;
using backend.Shared.QueryProcessor;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace backend.Product;

public class ProductControllerActivator : BaseControllerActivator
{
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

            // CreateProductRequest handler
            var createProductRequestHandler = new MapCreateProductRequestHandler(_mapper);

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
            var createProductResponseHandler = new MapCreateProductResponseHandler(_mapper);

            return new CreateProductController(
                createProductHandler,
                createProductResponseHandler,
                createProductRequestHandler);
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
            var getProductHandler = new QueryDecoratorBuilder<GetProductQuery, DomainModels.Product?>(
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
            var repository = new ProductViewRepository(dbConnection, _productSqlFilterBuilder);

            // GetProductResponse handler
            var getProductResponseHandler = new QueryDecoratorBuilder<GetProductResponseQuery, GetProductResponse?>(
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
            var repository = new ProductViewRepository(dbConnection, _productSqlFilterBuilder);
            IDatabase redisDatabase = new RedisService(_configuration).GetDatabase();
            var redisCache = new ListProductsCache(redisDatabase);
            var services = new Dictionary<Type, object>();

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
            services[typeof(IAsyncQueryHandler<ListProductsQuery, PagedProducts>)] = listProductsHandler;

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
            services[typeof(IAsyncQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult>)] =
                getListProductsFromCacheHandler;

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
            var mapListProductsResponseHandler = new MapListProductsResponseHandler(_mapper);
            services[typeof(ISyncQueryHandler<MapListProductsResponseQuery, ListProductsResponse>)] =
                mapListProductsResponseHandler;

            // UpdateListProductsStaleness handler
            var updateListProductStalenessHandler = new CommandDecoratorBuilder<UpdateListProductStalenessCommand>(
                    new UpdateListProductStalenessHandler(redisCache,
                        _loggerFactory.CreateLogger<UpdateListProductStalenessHandler>()),
                    null,
                    _loggerFactory)
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.RedisWrite)
                .Build();

            // Queries Processor
            IServiceProvider serviceProvider = new DictionaryServiceProvider(services);
            var queryProcessor = new QueryProcessor(serviceProvider);

            return new ListProductsController(
                queryProcessor,
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
            var getProductHandler = new QueryDecoratorBuilder<GetProductQuery, DomainModels.Product?>(
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
            var mapReplaceProductResponseHandler = new MapReplaceProductResponseHandler(_mapper);

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
            var getProductHandler = new QueryDecoratorBuilder<GetProductQuery, DomainModels.Product?>(
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
            var repository = new ProductViewRepository(dbConnection, _productSqlFilterBuilder);

            // GetProductResponse handler
            var getProductResponseHandler = new QueryDecoratorBuilder<GetProductResponseQuery, GetProductResponse?>(
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

        if (type == typeof(BatchGetProductsController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductViewRepository(dbConnection, _productSqlFilterBuilder);

            // BatchGetProducts handler
            var batchGetProductsHandler = new QueryDecoratorBuilder<BatchGetProductsQuery, List<GetProductResponse>>(
                    new BatchGetProductsHandler(repository, _mapper),
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

            return new BatchGetProductsController(
                batchGetProductsHandler);
        }

        if (type == typeof(BatchDeleteProductsController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductRepository(dbConnection);
            var viewRepository = new ProductViewRepository(dbConnection, _productSqlFilterBuilder);

            // BatchDeleteProducts handler
            var batchDeleteProductsHandler = new CommandDecoratorBuilder<BatchDeleteProductsCommand>(
                    new BatchDeleteProductsHandler(repository),
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

            // BatchGetProducts handler
            var batchGetProductsHandler = new QueryDecoratorBuilder<BatchGetProductsQuery, List<GetProductResponse>>(
                    new BatchGetProductsHandler(viewRepository, _mapper),
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

            return new BatchDeleteProductsController(
                batchDeleteProductsHandler,
                batchGetProductsHandler
            );
        }

        if (type == typeof(BatchCreateProductsController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductRepository(dbConnection);

            // BatchCreateProducts handler
            var batchCreateProducts = new CommandDecoratorBuilder<BatchCreateProductsCommand>(
                    new BatchCreateProductsHandler(repository),
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

            // CreateProductRequest handler
            var createProductRequestHandler = new MapCreateProductRequestHandler(_mapper);

            // CreateProductResponse handler
            var createProductResponseHandler = new MapCreateProductResponseHandler(_mapper);

            return new BatchCreateProductsController(
                batchCreateProducts,
                createProductRequestHandler,
                createProductResponseHandler);
        }

        return null;
    }
}
