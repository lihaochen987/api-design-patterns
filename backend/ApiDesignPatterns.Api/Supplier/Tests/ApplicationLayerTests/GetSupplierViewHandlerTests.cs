// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.GetSupplierView;
using backend.Supplier.DomainModels;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public class GetSupplierViewHandlerTests : GetSupplierViewHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsNull_WhenSupplierDoesNotExist()
    {
        SupplierView expectedSupplier = new SupplierViewTestDataBuilder().Build();
        var sut = GetSupplierViewHandler();

        SupplierView? result = await sut.Handle(new GetSupplierViewQuery { Id = expectedSupplier.Id });

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ReturnsSupplier_WhenSupplierExists()
    {
        SupplierView expectedSupplier = new SupplierViewTestDataBuilder().Build();
        Repository.Add(expectedSupplier);
        var sut = GetSupplierViewHandler();

        SupplierView? result = await sut.Handle(new GetSupplierViewQuery { Id = expectedSupplier.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedSupplier);
    }
}
