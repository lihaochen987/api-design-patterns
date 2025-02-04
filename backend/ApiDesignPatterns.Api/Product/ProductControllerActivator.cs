// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Commands.DeleteProduct;
using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.ApplicationLayer.Queries.CreateProductResponse;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ApplicationLayer.Queries.GetProductResponse;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.ProductControllers;
using backend.Product.Services;
using backend.Product.Services.ProductServices;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product;

public class ProductControllerActivator : BaseControllerActivator
{
    private readonly QueryService<ProductView> _productQueryService;
    private readonly SqlFilterBuilder _productSqlFilterBuilder;
    private readonly IMapper _mapper;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly CreateProductExtensions _createProductExtensions;
    private readonly ILoggerFactory _loggerFactory;

    public ProductControllerActivator(
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
        : base(configuration)
    {
        _productQueryService = new QueryService<ProductView>();

        ProductColumnMapper productColumnMapper = new();
        _productSqlFilterBuilder = new SqlFilterBuilder(productColumnMapper);

        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        _mapper = mapperConfig.CreateMapper();

        ProductFieldPaths productFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(productFieldPaths.ValidPaths);

        TypeParser typeParser = new();
        _createProductExtensions = new CreateProductExtensions(typeParser);

        _loggerFactory = loggerFactory;
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
            var createProduct = new CreateProductHandler(repository);
            var createProductWithAudit =
                new AuditCommandHandlerDecorator<CreateProductQuery>(createProduct, dbConnection);
            var createProductWithLogging = new LoggingCommandHandlerDecorator<CreateProductQuery>(
                createProductWithAudit,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<CreateProductQuery>>());
            var createProductWithTransaction =
                new TransactionCommandHandlerDecorator<CreateProductQuery>(createProductWithLogging, dbConnection);

            // CreateProductResponse handler
            var createProductResponse = new CreateProductResponseHandler(_mapper);
            var createProductResponseWithLogging =
                new LoggingQueryHandlerDecorator<CreateProductResponseQuery, CreateProductResponse>(
                    createProductResponse,
                    _loggerFactory
                        .CreateLogger<
                            LoggingQueryHandlerDecorator<CreateProductResponseQuery, CreateProductResponse>>());
            var createProductResponseWithValidation =
                new ValidationQueryHandlerDecorator<CreateProductResponseQuery, CreateProductResponse>(
                    createProductResponseWithLogging);

            return new CreateProductController(createProductWithTransaction, createProductResponseWithValidation,
                _createProductExtensions);
        }

        if (type == typeof(DeleteProductController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductRepository(dbConnection);

            // DeleteProduct handler
            var deleteProduct = new DeleteProductHandler(repository);
            var deleteProductWithAudit =
                new AuditCommandHandlerDecorator<DeleteProductQuery>(deleteProduct, dbConnection);
            var deleteProductWithLogging = new LoggingCommandHandlerDecorator<DeleteProductQuery>(
                deleteProductWithAudit,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<DeleteProductQuery>>());

            // GetProduct handler
            var getProduct = new GetProductHandler(repository);
            var getProductWithLogging = new LoggingQueryHandlerDecorator<GetProductQuery, DomainModels.Product>(
                getProduct,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetProductQuery, DomainModels.Product>>());

            return new DeleteProductController(deleteProductWithLogging, getProductWithLogging);
        }

        if (type == typeof(GetProductController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductViewRepository(dbConnection, _productQueryService, _productSqlFilterBuilder);

            // GetProductResponse handler
            var getProductResponse = new GetProductResponseHandler(repository, _mapper);
            var getProductResponseWithLogging =
                new LoggingQueryHandlerDecorator<GetProductResponseQuery, GetProductResponse>(
                    getProductResponse,
                    _loggerFactory
                        .CreateLogger<LoggingQueryHandlerDecorator<GetProductResponseQuery, GetProductResponse>>());
            var getProductResponseWithValidation =
                new ValidationQueryHandlerDecorator<GetProductResponseQuery, GetProductResponse>(
                    getProductResponseWithLogging);

            return new GetProductController(
                getProductResponseWithValidation,
                _fieldMaskConverterFactory);
        }

        if (type == typeof(ListProductsController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductViewRepository(dbConnection, _productQueryService, _productSqlFilterBuilder);

            // ListProducts handler
            var listProducts = new ListProductsHandler(repository);
            var listProductsWithLogging =
                new LoggingQueryHandlerDecorator<ListProductsQuery, (List<ProductView>, string?)>(
                    listProducts,
                    _loggerFactory
                        .CreateLogger<LoggingQueryHandlerDecorator<ListProductsQuery, (List<ProductView>, string?)>>());

            return new ListProductsController(listProductsWithLogging, _mapper);
        }

        if (type == typeof(ReplaceProductController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductRepository(dbConnection);

            // GetProduct handler
            var getProduct = new GetProductHandler(repository);
            var getProductWithLogging = new LoggingQueryHandlerDecorator<GetProductQuery, DomainModels.Product>(
                getProduct,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetProductQuery, DomainModels.Product>>());

            // ReplaceProduct handler
            var replaceProduct = new ReplaceProductHandler(repository);
            var replaceProductWithAuditing =
                new AuditCommandHandlerDecorator<ReplaceProductQuery>(replaceProduct, dbConnection);
            var replaceProductWithLogging = new LoggingCommandHandlerDecorator<ReplaceProductQuery>(
                replaceProductWithAuditing,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<ReplaceProductQuery>>());

            return new ReplaceProductController(getProductWithLogging, replaceProductWithLogging, _mapper);
        }

        if (type == typeof(UpdateProductController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductRepository(dbConnection);

            // GetProduct handler
            var getProduct = new GetProductHandler(repository);
            var getProductWithLogging = new LoggingQueryHandlerDecorator<GetProductQuery, DomainModels.Product>(
                getProduct,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetProductQuery, DomainModels.Product>>());

            // UpdateProduct handler
            var updateProduct = new UpdateProductHandler(repository);
            var updateProductWithAuditing =
                new AuditCommandHandlerDecorator<UpdateProductQuery>(updateProduct, dbConnection);
            var updateProductWithLogging = new LoggingCommandHandlerDecorator<UpdateProductQuery>(
                updateProductWithAuditing,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<UpdateProductQuery>>());
            var updateProductWithTransaction =
                new TransactionCommandHandlerDecorator<UpdateProductQuery>(updateProductWithLogging, dbConnection);
            return new UpdateProductController(getProductWithLogging, updateProductWithTransaction, _mapper);
        }

        return null;
    }
}
