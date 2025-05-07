// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndSupplier;
using backend.Inventory.Controllers;
using backend.Inventory.Services;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class CreateInventoryControllerTestBase
{
    protected readonly IMapper Mapper;

    protected readonly ICommandHandler<CreateInventoryCommand> CreateInventory =
        Mock.Of<ICommandHandler<CreateInventoryCommand>>();

    protected readonly IAsyncQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?>
        GetInventoryByProductAndSupplier =
            Mock.Of<IAsyncQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?>>();

    protected CreateInventoryControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterInventoryMappings();
        Mapper = new Mapper(config);
    }

    protected CreateInventoryController GetCreateInventoryController()
    {
        return new CreateInventoryController(CreateInventory, GetInventoryByProductAndSupplier, Mapper);
    }
}
