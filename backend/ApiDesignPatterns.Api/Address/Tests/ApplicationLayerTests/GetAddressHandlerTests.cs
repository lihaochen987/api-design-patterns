// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Queries.GetAddress;
using backend.Address.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.ApplicationLayerTests;

public class GetAddressHandlerTests : GetAddressHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsAddress_WhenAddressExists()
    {
        DomainModels.Address expectedAddress = new AddressTestDataBuilder().Build();
        Repository.Add(expectedAddress);
        var sut = GetAddressHandler();

        DomainModels.Address? result = await sut.Handle(new GetAddressQuery { Id = expectedAddress.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedAddress);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenAddressDoesNotExist()
    {
        DomainModels.Address nonExistentAddress = new AddressTestDataBuilder().Build();
        var sut = GetAddressHandler();

        DomainModels.Address? result = await sut.Handle(new GetAddressQuery { Id = nonExistentAddress.Id });

        result.Should().BeNull();
    }
}
