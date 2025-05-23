// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Address.Controllers;
using backend.Address.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace backend.Address.Tests;

public class GetAddressControllerTests : GetAddressControllerTestBase
{
    [Fact]
    public async Task GetAddress_ReturnsAddress_WhenAddressExists()
    {
        var expectedAddressView = new AddressViewTestDataBuilder().Build();
        var expectedResponse = Mapper.Map<GetAddressResponse>(expectedAddressView);
        Repository.Add(expectedAddressView);
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
        var sut = GetAddressController();
        var request = new GetAddressRequest();

        var actionResult = await sut.GetAddress(nonExistentId, request);

        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAddress_AppliesFieldMask_WhenFieldMaskIsProvided()
    {
        var addressView = new AddressViewTestDataBuilder().Build();
        Repository.Add(addressView);
        var sut = GetAddressController();
        var request = new GetAddressRequest { FieldMask = ["street", "city"] };

        var actionResult = await sut.GetAddress(addressView.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
    }
}
