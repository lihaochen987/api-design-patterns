// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Queries.GetAddressView;
using backend.Address.Controllers;
using backend.Address.DomainModels;
using backend.Address.InfrastructureLayer.Database.AddressView;
using backend.Address.Services;
using backend.Shared;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using IMapper = MapsterMapper.IMapper;
using Mapper = MapsterMapper.Mapper;

namespace backend.Address;

public class AddressControllerActivator : BaseControllerActivator
{
    private readonly IMapper _mapper;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly ILoggerFactory _loggerFactory;

    public AddressControllerActivator(
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
        : base(configuration)
    {
        var config = new TypeAdapterConfig();
        config.RegisterAddressMappings();
        _mapper = new Mapper(config);

        var addressFieldPaths = new AddressFieldPaths();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(addressFieldPaths.ValidPaths);

        _loggerFactory = loggerFactory;
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(GetAddressController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new AddressViewRepository(dbConnection);

            // GetAddressView handler
            var getAddressViewHandler = new QueryDecoratorBuilder<GetAddressViewQuery, AddressView?>(
                    new GetAddressViewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ProductRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new GetAddressController(
                getAddressViewHandler,
                _fieldMaskConverterFactory,
                _mapper);
        }

        return null;
    }
}
