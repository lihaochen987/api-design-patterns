// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Inventory.Services;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Inventory.Tests;

public abstract class ListInventoryControllerTestBase
{
    protected readonly InventoryViewRepositoryFake Repository = new(new PaginateService<InventoryView>());
    private readonly IMapper _mapper;
    protected const int DefaultMaxPageSize = 10;

    protected ListInventoryControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterInventoryMappings();
        _mapper = new Mapper(config);
    }

    protected ListInventoryController ListInventoryController()
    {
        var listInventory = new ListInventoryHandler(Repository);
        return new ListInventoryController(listInventory, _mapper);
    }
}
