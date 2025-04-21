// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.GetSuppliersFromInventory;
using backend.Supplier.DomainModels;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class GetSuppliersFromInventoryHandlerTests : GetSuppliersFromInventoryHandlerTestBase
{
    [Fact]
    public void Handle_ReturnsAllSuppliers_WhenAllTasksHaveResults()
    {
        var suppliers = new SupplierViewTestDataBuilder().CreateMany(3).ToList();
        var tasks = suppliers.Select(Task.FromResult<SupplierView?>).ToArray();
        var query = new GetSuppliersFromInventoryQuery { SupplierTasks = tasks };
        var sut = GetSuppliersFromInventoryHandler();

        List<SupplierView?> result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(suppliers);
    }

    [Fact]
    public void Handle_FiltersOutNullSuppliers_WhenSomeTasksHaveNullResults()
    {
        var suppliers = new SupplierViewTestDataBuilder().CreateMany(2).ToList();
        var tasks = new[]
        {
            Task.FromResult<SupplierView?>(suppliers[0]), Task.FromResult<SupplierView?>(null),
            Task.FromResult<SupplierView?>(suppliers[1])
        };
        var query = new GetSuppliersFromInventoryQuery { SupplierTasks = tasks };
        var sut = GetSuppliersFromInventoryHandler();

        List<SupplierView?> result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(suppliers);
    }

    [Fact]
    public void Handle_ReturnsEmptyList_WhenAllTasksHaveNullResults()
    {
        var tasks = new[]
        {
            Task.FromResult<SupplierView?>(null), Task.FromResult<SupplierView?>(null),
            Task.FromResult<SupplierView?>(null)
        };
        var query = new GetSuppliersFromInventoryQuery { SupplierTasks = tasks };
        var sut = GetSuppliersFromInventoryHandler();

        List<SupplierView?> result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void Handle_ReturnsEmptyList_WhenNoTasksProvided()
    {
        var tasks = Array.Empty<Task<SupplierView?>>();
        var query = new GetSuppliersFromInventoryQuery { SupplierTasks = tasks };
        var sut = GetSuppliersFromInventoryHandler();

        List<SupplierView?> result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
