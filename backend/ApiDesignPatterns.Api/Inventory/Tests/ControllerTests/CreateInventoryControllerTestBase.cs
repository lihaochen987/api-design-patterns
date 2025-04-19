// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndSupplier;
using backend.Inventory.Controllers;
using backend.Inventory.Services;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class CreateInventoryControllerTestBase
{
    protected readonly IMapper Mapper;

    protected readonly ICommandHandler<CreateInventoryCommand> CreateInventory =
        Mock.Of<ICommandHandler<CreateInventoryCommand>>();

    protected readonly IQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?>
        GetInventoryByProductAndSupplier =
            Mock.Of<IQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?>>();

    protected CreateInventoryControllerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected CreateInventoryController GetCreateInventoryController()
    {
        return new CreateInventoryController(CreateInventory, GetInventoryByProductAndSupplier, Mapper);
    }
}
