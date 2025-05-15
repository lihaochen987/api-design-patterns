// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.PhoneNumber.ApplicationLayer.Commands.DeletePhoneNumber;
using backend.PhoneNumber.ApplicationLayer.Queries.GetPhoneNumber;
using backend.PhoneNumber.ApplicationLayer.Queries.GetPhoneNumberView;
using backend.PhoneNumber.Controllers;
using backend.PhoneNumber.DomainModels;
using backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumber;
using backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumberView;
using backend.PhoneNumber.Services;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using IMapper = MapsterMapper.IMapper;
using Mapper = MapsterMapper.Mapper;

namespace backend.PhoneNumber;

public class PhoneNumberControllerActivator : BaseControllerActivator
{
    private readonly IMapper _mapper;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly ILoggerFactory _loggerFactory;

    public PhoneNumberControllerActivator(
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
        : base(configuration)
    {
        var config = new TypeAdapterConfig();
        config.RegisterPhoneNumberMappings();
        _mapper = new Mapper(config);

        var phoneNumberFieldPaths = new PhoneNumberFieldPaths();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(phoneNumberFieldPaths.ValidPaths);

        _loggerFactory = loggerFactory;
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(GetPhoneNumberController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new PhoneNumberViewRepository(dbConnection);

            // GetPhoneNumberView handler
            var getPhoneNumberViewHandler = new QueryDecoratorBuilder<GetPhoneNumberViewQuery, PhoneNumberView?>(
                    new GetPhoneNumberViewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.PhoneNumberRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new GetPhoneNumberController(
                getPhoneNumberViewHandler,
                _fieldMaskConverterFactory,
                _mapper);
        }

        if (type == typeof(DeletePhoneNumberController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new PhoneNumberRepository(dbConnection);

            // GetPhoneNumber handler
            var getPhoneNumberHandler = new QueryDecoratorBuilder<GetPhoneNumberQuery, DomainModels.PhoneNumber?>(
                    new GetPhoneNumberHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.PhoneNumberRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // DeletePhoneNumber handler
            var deletePhoneNumberHandler = new CommandDecoratorBuilder<DeletePhoneNumberCommand>(
                    new DeletePhoneNumberHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.PhoneNumberWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new DeletePhoneNumberController(
                getPhoneNumberHandler,
                deletePhoneNumberHandler);
        }

        return null;
    }
}
