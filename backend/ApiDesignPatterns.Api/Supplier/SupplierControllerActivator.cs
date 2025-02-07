// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Review.DomainModels;
using backend.Review.Services;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Supplier.ApplicationLayer.Commands.CreateSupplier;
using backend.Supplier.InfrastructureLayer.Database.Supplier;
using backend.Supplier.Services;
using backend.Supplier.SupplierControllers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Supplier;

public class SupplierControllerActivator : BaseControllerActivator
{
    private readonly QueryService<ReviewView> _reviewQueryService;
    private readonly SqlFilterBuilder _reviewSqlFilterBuilder;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly IMapper _mapper;

    public SupplierControllerActivator(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration)
    {
        _reviewQueryService = new QueryService<ReviewView>();

        ReviewColumnMapper reviewColumnMapper = new();
        _reviewSqlFilterBuilder = new SqlFilterBuilder(reviewColumnMapper);

        _loggerFactory = loggerFactory;

        ReviewFieldPaths reviewFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(reviewFieldPaths.ValidPaths);

        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<SupplierMappingProfile>(); });
        _mapper = mapperConfig.CreateMapper();
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(CreateSupplierController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierRepository(dbConnection);

            // CreateSupplier handler
            var createSupplier = new CreateSupplierHandler(repository);
            var createSupplierWithLogging = new LoggingCommandHandlerDecorator<CreateSupplierCommand>(createSupplier,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<CreateSupplierCommand>>());
            var createSupplierWithAudit =
                new AuditCommandHandlerDecorator<CreateSupplierCommand>(createSupplierWithLogging, dbConnection);
            var createSupplierWithTransaction =
                new TransactionCommandHandlerDecorator<CreateSupplierCommand>(createSupplierWithAudit, dbConnection);

            return new CreateSupplierController(
                createSupplierWithTransaction,
                _mapper);
        }

        return null;
    }
}
