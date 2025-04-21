// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Inventory.ApplicationLayer.Queries.GetSuppliersFromInventory;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.Services;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.GetSupplierView;
using backend.Supplier.DomainModels;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class ListProductSuppliersControllerTestBase
{
    protected readonly IAsyncQueryHandler<ListInventoryQuery, PagedInventory> MockListInventory;
    protected readonly IAsyncQueryHandler<GetSupplierViewQuery, SupplierView?> MockGetSupplierView;
    protected readonly ISyncQueryHandler<GetSuppliersFromInventoryQuery, List<SupplierView?>> GetSuppliersFromInventory;
    private readonly IMapper _mapper;
    protected const int DefaultMaxPageSize = 10;
    protected readonly Fixture Fixture = new();

    protected ListProductSuppliersControllerTestBase()
    {
        GetSuppliersFromInventory = new GetSuppliersFromInventoryHandler();
        MockListInventory = Mock.Of<IAsyncQueryHandler<ListInventoryQuery, PagedInventory>>();
        MockGetSupplierView = Mock.Of<IAsyncQueryHandler<GetSupplierViewQuery, SupplierView?>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
    }

    protected ListProductSuppliersController ListProductSuppliersController()
    {
        return new ListProductSuppliersController(
            MockListInventory,
            MockGetSupplierView,
            GetSuppliersFromInventory,
            _mapper);
    }
}
