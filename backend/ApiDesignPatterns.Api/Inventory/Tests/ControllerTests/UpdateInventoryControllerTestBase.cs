// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Inventory.ApplicationLayer.Commands.UpdateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.Controllers;
using backend.Inventory.Services;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class UpdateInventoryControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly IAsyncQueryHandler<GetInventoryByIdQuery, DomainModels.Inventory?> MockGetInventoryHandler;
    protected readonly ICommandHandler<UpdateInventoryCommand> MockUpdateInventoryHandler;
    protected readonly Fixture Fixture = new();

    protected UpdateInventoryControllerTestBase()
    {
        MockGetInventoryHandler = Mock.Of<IAsyncQueryHandler<GetInventoryByIdQuery, DomainModels.Inventory?>>();
        MockUpdateInventoryHandler = Mock.Of<ICommandHandler<UpdateInventoryCommand>>();
        var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected UpdateInventoryController UpdateInventoryController()
    {
        return new UpdateInventoryController(
            MockGetInventoryHandler,
            MockUpdateInventoryHandler,
            Mapper);
    }
}
