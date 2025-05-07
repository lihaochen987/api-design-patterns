// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryView;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Inventory.Services;
using backend.Product.Services.Mappers;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class GetInventoryControllerTestBase
{
    protected readonly Fixture Fixture = new();

    protected readonly IAsyncQueryHandler<GetInventoryViewQuery, InventoryView?> MockGetInventoryView =
        Mock.Of<IAsyncQueryHandler<GetInventoryViewQuery, InventoryView?>>();

    private readonly IMapper _mapper;

    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory =
        new FieldMaskConverterFactory(new InventoryFieldPaths().ValidPaths);

    protected GetInventoryControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterInventoryMappings();
        _mapper = new Mapper(config);
    }

    protected GetInventoryController GetInventoryController()
    {
        return new GetInventoryController(
            MockGetInventoryView,
            _fieldMaskConverterFactory,
            _mapper);
    }
}
