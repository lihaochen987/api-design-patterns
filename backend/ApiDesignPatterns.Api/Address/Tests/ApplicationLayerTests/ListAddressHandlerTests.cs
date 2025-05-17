// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Address.ApplicationLayer.Queries.ListAddress;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.ApplicationLayerTests;

public class ListAddressHandlerTests : ListAddressHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnAddressesAndNextPageToken()
    {
        const long userId = 123;
        var query = new ListAddressQuery { PageToken = null, Filter = "UserId == 123", MaxPageSize = 5 };
        Repository.AddAddressView(userId, Fixture.Create<string>());
        Repository.AddAddressView(userId, Fixture.Create<string>());
        var sut = ListAddressHandler();

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Address.Should().HaveCount(2);
        result.NextPageToken.Should().BeNull();
        result.Address.Should().OnlyContain(a => a.UserId == userId);
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
        Repository.AddAddressView(102, Fixture.Create<string>());
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
        Repository.AddAddressView(Fixture.Create<long>(), Fixture.Create<string>());
        Repository.AddAddressView(Fixture.Create<long>(), Fixture.Create<string>());
        Repository.AddAddressView(Fixture.Create<long>(), Fixture.Create<string>());
        Repository.AddAddressView(Fixture.Create<long>(), Fixture.Create<string>());
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
