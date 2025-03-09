// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryView;
using backend.Inventory.DomainModels;
using backend.Inventory.InventoryControllers;
using backend.Inventory.Services;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class GetInventoryControllerTestBase
{
    protected readonly Fixture Fixture = new();

    protected readonly IQueryHandler<GetInventoryViewQuery, InventoryView?> MockGetInventoryView =
        Mock.Of<IQueryHandler<GetInventoryViewQuery, InventoryView?>>();

    protected readonly IMapper Mapper;

    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory =
        new FieldMaskConverterFactory(new InventoryFieldPaths().ValidPaths);

    protected GetInventoryControllerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected GetInventoryController GetInventoryController()
    {
        return new GetInventoryController(
            MockGetInventoryView,
            _fieldMaskConverterFactory,
            Mapper);
    }
}
