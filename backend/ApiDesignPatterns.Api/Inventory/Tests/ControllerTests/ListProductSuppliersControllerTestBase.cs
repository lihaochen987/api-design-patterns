// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetSuppliersByIds;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.Services;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;
using backend.Supplier.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class ListProductSuppliersControllerTestBase
{
    protected readonly IAsyncQueryHandler<ListInventoryQuery, PagedInventory> MockListInventory =
        Mock.Of<IAsyncQueryHandler<ListInventoryQuery, PagedInventory>>();

    protected readonly IAsyncQueryHandler<GetSuppliersByIdsQuery, List<SupplierView>> MockGetSuppliersByIds =
        Mock.Of<IAsyncQueryHandler<GetSuppliersByIdsQuery, List<SupplierView>>>();

    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected ListProductSuppliersControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterSupplierMappings();
        config.RegisterInventoryMappings();
        Mapper = new Mapper(config);
    }

    protected ListProductSuppliersController ListProductSuppliersController()
    {
        return new ListProductSuppliersController(MockListInventory, MockGetSuppliersByIds, Mapper);
    }
}
