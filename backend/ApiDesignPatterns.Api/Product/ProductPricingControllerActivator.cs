// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Queries.GetProductResponse;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.ProductControllers;
using backend.Product.ProductPricingControllers;
using backend.Product.Services;
using backend.Shared;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product;

public class ProductPricingControllerActivator : BaseControllerActivator
{
    private readonly QueryService<ProductView> _productQueryService;
    private readonly SqlFilterBuilder _productSqlFilterBuilder;
    private readonly IMapper _mapper;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly ILoggerFactory _loggerFactory;

    public ProductPricingControllerActivator(
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

        _loggerFactory = loggerFactory;
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(GetProductPricingController))
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

        return null;
    }
}
