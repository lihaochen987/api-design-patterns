// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetUsersByIds;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.Services;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;
using backend.User.DomainModels;
using backend.User.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class ListProductUsersControllerTestBase
{
    protected readonly IAsyncQueryHandler<ListInventoryQuery, PagedInventory> MockListInventory =
        Mock.Of<IAsyncQueryHandler<ListInventoryQuery, PagedInventory>>();

    protected readonly IAsyncQueryHandler<GetUsersByIdsQuery, List<UserView>> MockGetUsersByIds =
        Mock.Of<IAsyncQueryHandler<GetUsersByIdsQuery, List<UserView>>>();

    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected ListProductUsersControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterUserMappings();
        config.RegisterInventoryMappings();
        Mapper = new Mapper(config);
    }

    protected ListProductUsersController ListProductUsersController()
    {
        return new ListProductUsersController(MockListInventory, MockGetUsersByIds, Mapper);
    }
}
