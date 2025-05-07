// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.GetSupplierView;
using backend.Supplier.Controllers;
using backend.Supplier.DomainModels;
using backend.Supplier.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Supplier.Tests.ControllerLayerTests;

public abstract class GetSupplierControllerTestBase
{
    protected readonly Fixture Fixture = new();

    protected readonly IAsyncQueryHandler<GetSupplierViewQuery, SupplierView?> MockGetSupplierView =
        Mock.Of<IAsyncQueryHandler<GetSupplierViewQuery, SupplierView?>>();

    protected readonly IMapper Mapper;

    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory =
        new FieldMaskConverterFactory(new SupplierFieldPaths().ValidPaths);

    protected GetSupplierControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterSupplierMappings();
        Mapper = new Mapper(config);
    }

    protected GetSupplierController GetSupplierController()
    {
        return new GetSupplierController(
            MockGetSupplierView,
            _fieldMaskConverterFactory,
            Mapper);
    }
}
