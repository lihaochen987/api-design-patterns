// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.User.DomainModels;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace backend.Inventory.Tests;

public class ListProductUsersControllerTests : ListProductUsersControllerTestBase
{
    [Fact]
    public async Task ListProductUsers_ShouldReturnAllUsers_WhenNoPageTokenProvided()
    {
        long productId = Fixture.Create<long>();
        List<InventoryView> inventoryViews =
            Fixture.Build<InventoryView>().With(x => x.ProductId, productId).CreateMany(4).ToList();
        List<UserView> userViews = new UserViewTestDataBuilder()
            .CreateMany(4)
            .ToList();
        for (int i = 0; i < inventoryViews.Count; i++)
        {
            inventoryViews[i].UserId = userViews[i].Id;
        }

        ListProductUsersRequest request = new() { MaxPageSize = 4 };
        InventoryViewRepository.AddInventoryViews(inventoryViews);
        UserViewRepository.AddUserViews(userViews);
        ListProductUsersController sut = ListProductUsersController();

        var result = await sut.ListProductUsers(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductUsersResponse = (ListProductUsersResponse)response.Value!;
        listProductUsersResponse.Results.Count.Should().Be(4);
        listProductUsersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListProductUsers_ShouldReturnUsersAfterPageToken_WhenPageTokenProvided()
    {
        long productId = Fixture.Create<long>();
        List<InventoryView> inventoryViews =
            Fixture.Build<InventoryView>().With(x => x.ProductId, productId).CreateMany(2).ToList();
        List<UserView> userViews = new UserViewTestDataBuilder()
            .CreateMany(2)
            .ToList();
        for (int i = 0; i < inventoryViews.Count; i++)
        {
            inventoryViews[i].UserId = userViews[i].Id;
        }
        ListProductUsersRequest request = new() { PageToken = "2", MaxPageSize = 2 };
        InventoryViewRepository.AddInventoryViews(inventoryViews);
        UserViewRepository.AddUserViews(userViews);

        ListProductUsersController sut = ListProductUsersController();

        var result = await sut.ListProductUsers(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductUsersResponse = (ListProductUsersResponse)response.Value!;
        listProductUsersResponse.Results.Count.Should().Be(2);
        listProductUsersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListProductUsers_ShouldReturnNextPageToken_WhenMoreUsersExist()
    {
        long productId = Fixture.Create<long>();
        var inventoryViews = new InventoryViewTestDataBuilder().CreateMany(2).ToList();
        foreach (var inventoryView in inventoryViews)
        {
            inventoryView.ProductId = productId;
        }
        List<UserView> userViews = new UserViewTestDataBuilder()
            .CreateMany(2)
            .ToList();
        for (int i = 0; i < inventoryViews.Count; i++)
        {
            inventoryViews[i].UserId = userViews[i].Id;
        }

        ListProductUsersRequest request = new() { MaxPageSize = 2 };
        InventoryViewRepository.AddInventoryViews(inventoryViews);
        UserViewRepository.AddUserViews(userViews);
        ListProductUsersController sut = ListProductUsersController();

        var result = await sut.ListProductUsers(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductUsersResponse = (ListProductUsersResponse)response.Value!;
        listProductUsersResponse.Results.Count.Should().Be(2);
        listProductUsersResponse.NextPageToken.Should().BeEquivalentTo(null);
    }

    [Fact]
    public async Task ListProductUsers_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        long productId = Fixture.Create<long>();
        List<InventoryView> inventoryViews = [];
        List<UserView> userViews = [];
        ListProductUsersRequest request = new() { MaxPageSize = 2 };
        InventoryViewRepository.AddInventoryViews(inventoryViews);
        UserViewRepository.AddUserViews(userViews);
        ListProductUsersController sut = ListProductUsersController();

        var result = await sut.ListProductUsers(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductUsersResponse = (ListProductUsersResponse)response.Value!;
        listProductUsersResponse.Results.Should().BeEmpty();
        listProductUsersResponse.NextPageToken.Should().BeNull();
    }
}
