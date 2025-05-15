// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Address.ApplicationLayer.Queries.GetAddressView;
using backend.Address.Controllers;
using backend.Address.DomainModels;
using backend.Address.Services;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Address.Tests.ControllerTests;

public abstract class GetAddressControllerTestBase
{
    protected readonly IAsyncQueryHandler<GetAddressViewQuery, AddressView?> GetAddressViewHandler =
        Mock.Of<IAsyncQueryHandler<GetAddressViewQuery, AddressView?>>();

    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory =
        new FieldMaskConverterFactory(new AddressFieldPaths().ValidPaths);

    protected readonly IMapper Mapper;

    protected Fixture Fixture = new();

    protected GetAddressControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterAddressMappings();
        Mapper = new Mapper(config);
    }

    protected GetAddressController GetAddressController()
    {
        return new GetAddressController(
            GetAddressViewHandler,
            _fieldMaskConverterFactory,
            Mapper);
    }
}
