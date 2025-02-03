// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Queries.GetProductView;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.ProductControllers;
using backend.Product.Services;
using backend.Product.Services.ProductServices;
using backend.Shared;
using backend.Shared.FieldMask;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product;

public class ProductControllerActivator : BaseControllerActivator
{
    private readonly QueryService<ProductView> _productQueryService;
    private readonly SqlFilterBuilder _productSqlFilterBuilder;
    private readonly IMapper _mapper;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;

    public ProductControllerActivator(IConfiguration configuration) : base(configuration)
    {
        _productQueryService = new QueryService<ProductView>();

        ProductColumnMapper productColumnMapper = new();
        _productSqlFilterBuilder = new SqlFilterBuilder(productColumnMapper);

        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        _mapper = mapperConfig.CreateMapper();

        ProductFieldPaths productFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(productFieldPaths.ValidPaths);
    }

    public override object Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(GetProductController))
        {
            var dbConnection = CreateDbConnection();
            var repository = new ProductViewRepository(dbConnection, _productQueryService, _productSqlFilterBuilder);
            var queryHandler = new GetProductViewHandler(repository);
            TrackDisposable(context, dbConnection);

            return new GetProductController(
                queryHandler,
                _fieldMaskConverterFactory,
                _mapper);
        }

        throw new Exception($"Unknown product controller type: {type.Name}");
    }
}
