// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Inventory.Services;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class ListInventoryControllerTestBase
{
    protected readonly IAsyncQueryHandler<ListInventoryQuery, PagedInventory> MockListInventory;
    private readonly IMapper _mapper;
    protected const int DefaultMaxPageSize = 10;

    protected ListInventoryControllerTestBase()
    {
        MockListInventory = Mock.Of<IAsyncQueryHandler<ListInventoryQuery, PagedInventory>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
    }

    protected ListInventoryController ListInventoryController()
    {
        return new ListInventoryController(MockListInventory, _mapper);
    }
}
