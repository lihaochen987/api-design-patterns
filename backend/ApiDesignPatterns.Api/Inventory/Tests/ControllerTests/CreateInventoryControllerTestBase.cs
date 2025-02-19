// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.InventoryControllers;
using backend.Inventory.Services;
using backend.Shared.CommandHandler;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class CreateInventoryControllerTestBase
{
    protected readonly Fixture Fixture = new();

    protected readonly IMapper Mapper;

    protected readonly ICommandHandler<CreateInventoryCommand> CreateInventory =
        Mock.Of<ICommandHandler<CreateInventoryCommand>>();

    protected CreateInventoryControllerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected CreateInventoryController GetCreateInventoryController()
    {
        return new CreateInventoryController(CreateInventory, Mapper);
    }
}
