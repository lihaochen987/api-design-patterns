// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Queries.GetAddressView;
using backend.Address.DomainModels;
using backend.Address.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.ApplicationLayerTests;

public class GetAddressViewHandlerTests : GetAddressViewHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsNull_WhenAddressDoesNotExist()
    {
        AddressView expectedAddress = new AddressViewTestDataBuilder().Build();
        var sut = GetAddressViewHandler();

        AddressView? result = await sut.Handle(new GetAddressViewQuery { Id = expectedAddress.Id });

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ReturnsAddress_WhenAddressExists()
    {
        AddressView expectedAddress = new AddressViewTestDataBuilder().Build();
        Repository.Add(expectedAddress);
        var sut = GetAddressViewHandler();

        AddressView? result = await sut.Handle(new GetAddressViewQuery { Id = expectedAddress.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedAddress);
    }
}
