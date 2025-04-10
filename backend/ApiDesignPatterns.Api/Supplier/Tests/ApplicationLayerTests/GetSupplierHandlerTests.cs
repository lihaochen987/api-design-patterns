// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public class GetSupplierHandlerTests : GetSupplierHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsSupplier_WhenSupplierExists()
    {
        DomainModels.Supplier expectedSupplier = new SupplierTestDataBuilder().Build();
        Repository.Add(expectedSupplier);
        IQueryHandler<GetSupplierQuery, DomainModels.Supplier?> sut = GetSupplierHandler();

        DomainModels.Supplier? result = await sut.Handle(new GetSupplierQuery { Id = expectedSupplier.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedSupplier);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenSupplierDoesNotExist()
    {
        DomainModels.Supplier nonExistentSupplier = new SupplierTestDataBuilder().Build();
        IQueryHandler<GetSupplierQuery, DomainModels.Supplier?> sut = GetSupplierHandler();

        DomainModels.Supplier? result = await sut.Handle(new GetSupplierQuery { Id = nonExistentSupplier.Id });

        result.Should().BeNull();
    }
}
