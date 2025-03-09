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
using backend.Product.Services.Mappers;
using backend.Shared;
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
            var getProductPricingHandler = new QueryDecoratorBuilder<GetProductPricingQuery, ProductPricingView?>(
                    new GetProductPricingHandler(repository),
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

            return new GetProductPricingController(
                getProductPricingHandler,
                _mapper,
                _fieldMaskConverterFactory);
        }

        if (type == typeof(UpdateProductPricingController))
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

            // UpdateProductPricing handler
            var updateProductPricingHandler = new CommandDecoratorBuilder<UpdateProductPricingCommand>(
                    new UpdateProductPricingHandler(repository),
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

            return new UpdateProductPricingController(
                getProductHandler,
                updateProductPricingHandler,
                _mapper);
        }

        return null;
    }
}
