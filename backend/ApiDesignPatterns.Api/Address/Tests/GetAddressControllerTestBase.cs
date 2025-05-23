// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Address.ApplicationLayer.Queries.GetAddressView;
using backend.Address.Controllers;
using backend.Address.DomainModels;
using backend.Address.Services;
using backend.Address.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.FieldMask;
using Mapster;
using MapsterMapper;

namespace backend.Address.Tests;

public abstract class GetAddressControllerTestBase
{
    protected readonly AddressViewRepositoryFake Repository = new(new PaginateService<AddressView>());

    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory =
        new FieldMaskConverterFactory(new AddressFieldPaths().ValidPaths);

    protected readonly IMapper Mapper;

    protected readonly Fixture Fixture = new();

    protected GetAddressControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterAddressMappings();
        Mapper = new Mapper(config);
    }

    protected GetAddressController GetAddressController()
    {
        var getAddressViewHandler = new GetAddressViewHandler(Repository);

        return new GetAddressController(
            getAddressViewHandler,
            _fieldMaskConverterFactory,
            Mapper);
    }
}
