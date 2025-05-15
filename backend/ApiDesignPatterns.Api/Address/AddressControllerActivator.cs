// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Commands.DeleteAddress;
using backend.Address.ApplicationLayer.Commands.ReplaceAddress;
using backend.Address.ApplicationLayer.Queries.GetAddress;
using backend.Address.ApplicationLayer.Queries.GetAddressView;
using backend.Address.ApplicationLayer.Queries.ListAddress;
using backend.Address.Controllers;
using backend.Address.DomainModels;
using backend.Address.InfrastructureLayer.Database.Address;
using backend.Address.InfrastructureLayer.Database.AddressView;
using backend.Address.Services;
using backend.Shared;
using backend.Shared.CommandHandler;
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
    private readonly PaginateService<AddressView> _paginateService;
    private readonly SqlFilterBuilder _sqlFilterBuilder;

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

        _paginateService = new PaginateService<AddressView>();

        AddressColumnMapper addressColumnMapper = new();
        _sqlFilterBuilder = new SqlFilterBuilder(addressColumnMapper);
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(GetAddressController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new AddressViewRepository(dbConnection, _sqlFilterBuilder, _paginateService);

            // GetAddressView handler
            var getAddressViewHandler = new QueryDecoratorBuilder<GetAddressViewQuery, AddressView?>(
                    new GetAddressViewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.AddressRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new GetAddressController(
                getAddressViewHandler,
                _fieldMaskConverterFactory,
                _mapper);
        }

        if (type == typeof(DeleteAddressController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new AddressRepository(dbConnection);

            // GetAddress handler
            var getAddressHandler = new QueryDecoratorBuilder<GetAddressQuery, DomainModels.Address?>(
                    new GetAddressHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.AddressRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // DeletePhoneNumber handler
            var deleteAddressHandler = new CommandDecoratorBuilder<DeleteAddressCommand>(
                    new DeleteAddressHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.AddressWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new DeleteAddressController(
                getAddressHandler,
                deleteAddressHandler);
        }

        if (type == typeof(ListAddressController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new AddressViewRepository(
                dbConnection,
                _sqlFilterBuilder,
                _paginateService);

            // ListAddressHandler
            var listAddressHandler = new QueryDecoratorBuilder<ListAddressQuery, PagedAddress>(
                    new ListAddressHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.AddressRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new ListAddressController(
                listAddressHandler,
                _mapper);
        }

        if (type == typeof(ReplaceAddressController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new AddressRepository(dbConnection);

            // GetAddress handler
            var getAddressHandler = new QueryDecoratorBuilder<GetAddressQuery, DomainModels.Address?>(
                    new GetAddressHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.AddressRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // ReplaceAddress handler
            var replaceAddressHandler = new CommandDecoratorBuilder<ReplaceAddressCommand>(
                    new ReplaceAddressHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.AddressWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new ReplaceAddressController(
                getAddressHandler,
                replaceAddressHandler,
                _mapper);
        }

        return null;
    }
}
