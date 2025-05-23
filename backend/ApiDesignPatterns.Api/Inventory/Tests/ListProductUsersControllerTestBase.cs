// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetUsersByIds;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Inventory.Services;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Queries.ListUsers;
using backend.User.DomainModels;
using backend.User.Services;
using backend.User.Tests.TestHelpers.Fakes;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Inventory.Tests;

public abstract class ListProductUsersControllerTestBase
{
    protected readonly InventoryViewRepositoryFake InventoryViewRepository = new(new PaginateService<InventoryView>());
    protected readonly UserViewRepositoryFake UserViewRepository = new(new PaginateService<UserView>());

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
        var listInventory = new ListInventoryHandler(InventoryViewRepository);
        var getUsersByIds = new GetUsersByIdsHandler(UserViewRepository);
        return new ListProductUsersController(listInventory, getUsersByIds, Mapper);
    }
}
