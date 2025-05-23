// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndUser;
using backend.Inventory.Controllers;
using backend.Inventory.Services;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Inventory.Tests;

public abstract class CreateInventoryControllerTestBase
{
    protected readonly IMapper Mapper;

    protected readonly InventoryRepositoryFake Repository = [];

    protected CreateInventoryControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterInventoryMappings();
        Mapper = new Mapper(config);
    }

    protected CreateInventoryController GetCreateInventoryController()
    {
        var createInventory = new CreateInventoryHandler(Repository);
        var getInventoryByProductAndUser = new GetInventoryByProductAndUserHandler(Repository);
        return new CreateInventoryController(createInventory, getInventoryByProductAndUser, Mapper);
    }
}
