// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.UpdateProductPricing;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ApplicationLayer.Queries.GetProductPricing;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.InfrastructureLayer.Database.ProductPricing;
using backend.Product.ProductPricingControllers;
using backend.Product.Services;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product;

public class ProductPricingControllerActivator : BaseControllerActivator
{
    private readonly IMapper _mapper;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly ILoggerFactory _loggerFactory;

    public ProductPricingControllerActivator(
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
        : base(configuration)
    {
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ProductPricingMappingProfile>(); });
        _mapper = mapperConfig.CreateMapper();

        ProductPricingFieldPaths productPricingFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(productPricingFieldPaths.ValidPaths);

        _loggerFactory = loggerFactory;
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(GetProductPricingController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductPricingRepository(dbConnection);

            // GetProductPricing handler
            var getProductPricing = new GetProductPricingHandler(repository);
            var getProductPricingWithLogging =
                new LoggingQueryHandlerDecorator<GetProductPricingQuery, ProductPricingView>(
                    getProductPricing,
                    _loggerFactory
                        .CreateLogger<LoggingQueryHandlerDecorator<GetProductPricingQuery, ProductPricingView>>());
            var getPricingWithValidation =
                new ValidationQueryHandlerDecorator<GetProductPricingQuery, ProductPricingView>(
                    getProductPricingWithLogging);

            return new GetProductPricingController(
                getPricingWithValidation,
                _mapper,
                _fieldMaskConverterFactory);
        }

        if (type == typeof(UpdateProductPricingController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ProductRepository(dbConnection);

            // GetProduct handler
            var getProduct = new GetProductHandler(repository);
            var getProductWithLogging = new LoggingQueryHandlerDecorator<GetProductQuery, DomainModels.Product>(
                getProduct,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetProductQuery, DomainModels.Product>>());

            // UpdateProductPricing handler
            var updateProduct = new UpdateProductPricingHandler(repository);
            var updateProductPricingWithAuditing =
                new AuditCommandHandlerDecorator<UpdateProductPricingCommand>(updateProduct, dbConnection);
            var updateProductPricingWithLogging = new LoggingCommandHandlerDecorator<UpdateProductPricingCommand>(
                updateProductPricingWithAuditing,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<UpdateProductPricingCommand>>());
            var updateProductPricingWithTransaction =
                new TransactionCommandHandlerDecorator<UpdateProductPricingCommand>(updateProductPricingWithLogging,
                    dbConnection);

            return new UpdateProductPricingController(
                getProductWithLogging,
                updateProductPricingWithTransaction,
                _mapper);
        }

        return null;
    }
}
