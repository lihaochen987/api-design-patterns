// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Queries.GetUserView;
using backend.User.Controllers;
using backend.User.DomainModels;
using backend.User.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.User.Tests.ControllerLayerTests;

public abstract class GetUserControllerTestBase
{
    protected readonly Fixture Fixture = new();

    protected readonly IAsyncQueryHandler<GetUserViewQuery, UserView?> MockGetUserView =
        Mock.Of<IAsyncQueryHandler<GetUserViewQuery, UserView?>>();

    protected readonly IMapper Mapper;

    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory =
        new FieldMaskConverterFactory(new UserFieldPaths().ValidPaths);

    protected GetUserControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterUserMappings();
        Mapper = new Mapper(config);
    }

    protected GetUserController GetUserController()
    {
        return new GetUserController(
            MockGetUserView,
            _fieldMaskConverterFactory,
            Mapper);
    }
}
