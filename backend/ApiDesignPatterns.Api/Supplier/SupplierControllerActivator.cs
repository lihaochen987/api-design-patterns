// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Commands.CreateSupplier;
using backend.Supplier.ApplicationLayer.Commands.DeleteSupplier;
using backend.Supplier.ApplicationLayer.Commands.ReplaceSupplier;
using backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplierView;
using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Database.Supplier;
using backend.Supplier.InfrastructureLayer.Database.SupplierView;
using backend.Supplier.Services;
using backend.Supplier.SupplierControllers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Supplier;

public class SupplierControllerActivator : BaseControllerActivator
{
    private readonly QueryService<SupplierView> _supplierQueryService;
    private readonly SqlFilterBuilder _supplierSqlFilterBuilder;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly IMapper _mapper;

    public SupplierControllerActivator(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration)
    {
        _supplierQueryService = new QueryService<SupplierView>();

        SupplierColumnMapper supplierColumnMapper = new();
        _supplierSqlFilterBuilder = new SqlFilterBuilder(supplierColumnMapper);

        _loggerFactory = loggerFactory;

        SupplierFieldPaths supplierFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(supplierFieldPaths.ValidPaths);

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

        if (type == typeof(DeleteSupplierController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierRepository(dbConnection);

            // GetSupplier handler
            var getSupplier = new GetSupplierHandler(repository);
            var getSupplierWithLogging = new LoggingQueryHandlerDecorator<GetSupplierQuery, DomainModels.Supplier>(
                getSupplier,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetSupplierQuery, DomainModels.Supplier>>());
            var getSupplierWithValidation =
                new ValidationQueryHandlerDecorator<GetSupplierQuery, DomainModels.Supplier>(getSupplierWithLogging);

            // DeleteSupplier handler
            var deleteSupplier = new DeleteSupplierHandler(repository);
            var deleteSupplierWithLogging = new LoggingCommandHandlerDecorator<DeleteSupplierCommand>(deleteSupplier,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<DeleteSupplierCommand>>());
            var deleteSupplierWithAudit =
                new AuditCommandHandlerDecorator<DeleteSupplierCommand>(deleteSupplierWithLogging, dbConnection);
            var deleteSupplierWithTransaction =
                new TransactionCommandHandlerDecorator<DeleteSupplierCommand>(deleteSupplierWithAudit, dbConnection);

            return new DeleteSupplierController(
                getSupplierWithValidation,
                deleteSupplierWithTransaction);
        }

        if (type == typeof(GetSupplierController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierViewRepository(dbConnection, _supplierSqlFilterBuilder, _supplierQueryService);

            // GetSupplierView handler
            var getSupplierView = new GetSupplierViewHandler(repository);
            var getSupplierViewWithLogging = new LoggingQueryHandlerDecorator<GetSupplierViewQuery, SupplierView>(
                getSupplierView,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetSupplierViewQuery, SupplierView>>());
            var getSupplierViewWithValidation =
                new ValidationQueryHandlerDecorator<GetSupplierViewQuery, SupplierView>(getSupplierViewWithLogging);

            return new GetSupplierController(
                getSupplierViewWithValidation,
                _fieldMaskConverterFactory,
                _mapper);
        }

        if (type == typeof(ListSuppliersController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierViewRepository(dbConnection, _supplierSqlFilterBuilder, _supplierQueryService);

            // ListSuppliers query handler
            var listSuppliers = new ListSuppliersHandler(repository);
            var listSuppliersWithLogging =
                new LoggingQueryHandlerDecorator<ListSuppliersQuery, (List<SupplierView>, string?)>(
                    listSuppliers,
                    _loggerFactory
                        .CreateLogger<LoggingQueryHandlerDecorator<ListSuppliersQuery, (List<SupplierView>, string?
                            )>>());

            return new ListSuppliersController(
                listSuppliersWithLogging,
                _mapper);
        }

        if (type == typeof(ReplaceSupplierController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierRepository(dbConnection);

            // GetSupplier handler
            var getSupplier = new GetSupplierHandler(repository);
            var getSupplierWithLogging = new LoggingQueryHandlerDecorator<GetSupplierQuery, DomainModels.Supplier>(
                getSupplier,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetSupplierQuery, DomainModels.Supplier>>());
            var getSupplierWithValidation =
                new ValidationQueryHandlerDecorator<GetSupplierQuery, DomainModels.Supplier>(getSupplierWithLogging);

            // ReplaceSupplier handler
            var replaceSupplier = new ReplaceSupplierHandler(repository);
            var replaceSupplierWithLogging = new LoggingCommandHandlerDecorator<ReplaceSupplierCommand>(replaceSupplier,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<ReplaceSupplierCommand>>());
            var replaceSupplierWithAudit =
                new AuditCommandHandlerDecorator<ReplaceSupplierCommand>(replaceSupplierWithLogging, dbConnection);
            var replaceSupplierWithTransaction =
                new TransactionCommandHandlerDecorator<ReplaceSupplierCommand>(replaceSupplierWithAudit, dbConnection);

            return new ReplaceSupplierController(
                getSupplierWithValidation,
                replaceSupplierWithTransaction,
                _mapper);
        }

        if (type == typeof(UpdateSupplierController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierRepository(dbConnection);

            // GetSupplier handler
            var getSupplier = new GetSupplierHandler(repository);
            var getSupplierWithLogging = new LoggingQueryHandlerDecorator<GetSupplierQuery, DomainModels.Supplier>(
                getSupplier,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetSupplierQuery, DomainModels.Supplier>>());
            var getSupplierWithValidation =
                new ValidationQueryHandlerDecorator<GetSupplierQuery, DomainModels.Supplier>(getSupplierWithLogging);

            // UpdateSupplier handler
            var updateSupplier = new UpdateSupplierHandler(repository);
            var updateSupplierWithLogging = new LoggingCommandHandlerDecorator<UpdateSupplierCommand>(updateSupplier,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<UpdateSupplierCommand>>());
            var updateSupplierWithAudit =
                new AuditCommandHandlerDecorator<UpdateSupplierCommand>(updateSupplierWithLogging, dbConnection);
            var updateSupplierWithTransaction =
                new TransactionCommandHandlerDecorator<UpdateSupplierCommand>(updateSupplierWithAudit, dbConnection);

            return new UpdateSupplierController(
                getSupplierWithValidation,
                updateSupplierWithTransaction,
                _mapper);
        }

        return null;
    }
}
