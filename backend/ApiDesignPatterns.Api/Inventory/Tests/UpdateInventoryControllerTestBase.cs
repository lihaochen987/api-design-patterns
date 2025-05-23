// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Commands.UpdateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.Controllers;
using backend.Inventory.Services;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Inventory.Tests;

public abstract class UpdateInventoryControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly InventoryRepositoryFake Repository = [];
    protected readonly Fixture Fixture = new();

    protected UpdateInventoryControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterInventoryMappings();
        Mapper = new Mapper(config);
    }

    protected UpdateInventoryController UpdateInventoryController()
    {
        var getInventoryByIdHandler = new GetInventoryByIdByIdHandler(Repository);
        var updateInventoryHandler = new UpdateInventoryHandler(Repository);
        return new UpdateInventoryController(
            getInventoryByIdHandler,
            updateInventoryHandler,
            Mapper);
    }
}
