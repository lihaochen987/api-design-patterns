// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetSuppliersByIds;
using backend.Supplier.DomainModels;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class GetSuppliersByIdsHandlerTests : GetSuppliersByIdsHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsSuppliers_WhenSuppliersExist()
    {
        var supplierOne = new SupplierViewTestDataBuilder().Build();
        var supplierTwo = new SupplierViewTestDataBuilder().Build();
        var expectedSuppliers = new List<SupplierView> { supplierOne, supplierTwo };
        var supplierIds = expectedSuppliers.Select(s => s.Id).ToList();
        Repository.Add(supplierOne);
        Repository.Add(supplierTwo);
        var sut = GetSuppliersByIdsHandler();
        var query = new GetSuppliersByIdsQuery { SupplierIds = supplierIds };

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedSuppliers);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoSuppliersExist()
    {
        var supplierIds = new List<long> { Fixture.Create<long>(), Fixture.Create<long>() };
        var sut = GetSuppliersByIdsHandler();
        var query = new GetSuppliersByIdsQuery { SupplierIds = supplierIds };

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ReturnsOnlyFoundSuppliers_WhenSomeExist()
    {
        var existingSupplier = new SupplierViewTestDataBuilder().Build();
        var existingSuppliers = new List<SupplierView> { existingSupplier };
        var supplierIds = new List<long> { existingSupplier.Id, Fixture.Create<long>() };
        Repository.Add(existingSupplier);
        var sut = GetSuppliersByIdsHandler();
        var query = new GetSuppliersByIdsQuery { SupplierIds = supplierIds };

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(existingSuppliers);
    }
}
