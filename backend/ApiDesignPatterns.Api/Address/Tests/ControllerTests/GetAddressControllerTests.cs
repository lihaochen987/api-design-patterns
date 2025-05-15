// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Address.ApplicationLayer.Queries.GetAddressView;
using backend.Address.Controllers;
using backend.Address.DomainModels;
using backend.Address.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace backend.Address.Tests.ControllerTests;

public class GetAddressControllerTests : GetAddressControllerTestBase
{
    [Fact]
    public async Task GetAddress_ReturnsAddress_WhenAddressExists()
    {
        var expectedAddressView = new AddressViewTestDataBuilder().Build();
        var expectedResponse = Mapper.Map<GetAddressResponse>(expectedAddressView);
        Mock.Get(GetAddressViewHandler)
            .Setup(h => h.Handle(It.Is<GetAddressViewQuery>(q => q.Id == expectedAddressView.Id)))
            .ReturnsAsync(expectedAddressView);
        var sut = GetAddressController();
        var request = new GetAddressRequest();

        var actionResult = await sut.GetAddress(expectedAddressView.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        string? jsonResponse = okResult.Value.ToString();
        var deserializedResponse = JsonConvert.DeserializeObject<GetAddressResponse>(jsonResponse!);
        deserializedResponse.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetAddress_ReturnsNotFound_WhenAddressDoesNotExist()
    {
        long nonExistentId = Fixture.Create<long>();
        Mock.Get(GetAddressViewHandler)
            .Setup(h => h.Handle(It.Is<GetAddressViewQuery>(q => q.Id == nonExistentId)))
            .ReturnsAsync((AddressView?)null);
        var sut = GetAddressController();
        var request = new GetAddressRequest();

        var actionResult = await sut.GetAddress(nonExistentId, request);

        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAddress_AppliesFieldMask_WhenFieldMaskIsProvided()
    {
        var addressView = new AddressViewTestDataBuilder().Build();
        Mock.Get(GetAddressViewHandler).Setup(h => h.Handle(It.IsAny<GetAddressViewQuery>()))
            .ReturnsAsync(addressView);
        var sut = GetAddressController();
        var request = new GetAddressRequest { FieldMask = ["street", "city"] };

        var actionResult = await sut.GetAddress(addressView.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
    }
}
