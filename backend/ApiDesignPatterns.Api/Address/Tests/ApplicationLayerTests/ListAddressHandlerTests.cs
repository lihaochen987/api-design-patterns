// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Queries.ListAddress;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.ApplicationLayerTests;

public class ListAddressHandlerTests : ListAddressHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnAddressesAndNextPageToken()
    {
        const long supplierId = 123;
        var query = new ListAddressQuery { PageToken = null, Filter = "SupplierId == 123", MaxPageSize = 5 };
        Repository.AddAddressView(supplierId, "123 Main St, New York, NY 10001");
        Repository.AddAddressView(supplierId, "456 Oak Ave, New York, NY 10002");
        var sut = ListAddressHandler();

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Address.Should().HaveCount(2);
        result.NextPageToken.Should().BeNull();
        result.Address.Should().OnlyContain(a => a.SupplierId == supplierId);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAddressesExist()
    {
        var query = new ListAddressQuery { PageToken = null, Filter = null, MaxPageSize = 10 };
        var sut = ListAddressHandler();

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Address.Should().BeEmpty();
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnOnlyAddressesMatchingFilter()
    {
        var query = new ListAddressQuery
        {
            PageToken = null, Filter = "FullAddress.Contains('New York')", MaxPageSize = 5
        };
        Repository.AddAddressView(101, "123 Main St, New York, NY 10001");
        Repository.AddAddressView(102, "456 Oak Ave, Los Angeles, CA 90001");
        Repository.AddAddressView(103, "789 Pine Blvd, New York, NY 10002");
        var sut = ListAddressHandler();

        var result = await sut.Handle(query);

        result.Address.Should().HaveCount(2);
        result.Address.Should().OnlyContain(a => a.FullAddress.Contains("New York"));
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldLimitResultsByMaxPageSize()
    {
        var query = new ListAddressQuery { PageToken = null, Filter = null, MaxPageSize = 2 };
        Repository.AddAddressView(201, "Address 1");
        Repository.AddAddressView(202, "Address 2");
        Repository.AddAddressView(203, "Address 3");
        Repository.AddAddressView(204, "Address 4");
        var sut = ListAddressHandler();

        var result = await sut.Handle(query);

        result.Address.Should().HaveCount(2);
        result.NextPageToken.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenRepositoryFails()
    {
        var query = new ListAddressQuery
        {
            PageToken = "invalid-token", Filter = "InvalidFilter == 'SomeValue'", MaxPageSize = 5
        };
        var sut = ListAddressHandler();

        await FluentActions.Invoking(() => sut.Handle(query))
            .Should().ThrowAsync<ArgumentException>();
    }
}
