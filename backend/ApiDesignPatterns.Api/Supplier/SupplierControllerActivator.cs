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
            var createSupplierHandler = new CommandDecoratorBuilder<CreateSupplierCommand>(
                    new CreateSupplierHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .WithHandshaking()
                .WithTimeout(TimeSpan.FromSeconds(5))
                .WithBulkhead(100, 500)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new CreateSupplierController(
                createSupplierHandler,
                _mapper);
        }

        if (type == typeof(DeleteSupplierController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierRepository(dbConnection);

            // GetSupplier handler
            var getSupplierHandler = new QueryDecoratorBuilder<GetSupplierQuery, DomainModels.Supplier>(
                    new GetSupplierHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .WithHandshaking()
                .WithTimeout(TimeSpan.FromSeconds(5))
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // DeleteSupplier handler
            var deleteSupplierHandler = new CommandDecoratorBuilder<DeleteSupplierCommand>(
                    new DeleteSupplierHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .WithHandshaking()
                .WithTimeout(TimeSpan.FromSeconds(5))
                .WithBulkhead(100, 500)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new DeleteSupplierController(
                getSupplierHandler,
                deleteSupplierHandler);
        }

        if (type == typeof(GetSupplierController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierViewRepository(dbConnection, _supplierSqlFilterBuilder, _supplierQueryService);

            // GetSupplierView handler
            var getSupplierViewHandler = new QueryDecoratorBuilder<GetSupplierViewQuery, SupplierView>(
                    new GetSupplierViewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .WithHandshaking()
                .WithTimeout(TimeSpan.FromSeconds(5))
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new GetSupplierController(
                getSupplierViewHandler,
                _fieldMaskConverterFactory,
                _mapper);
        }

        if (type == typeof(ListSuppliersController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierViewRepository(dbConnection, _supplierSqlFilterBuilder, _supplierQueryService);

            // ListSuppliers query handler
            var listSuppliersHandler = new QueryDecoratorBuilder<ListSuppliersQuery, PagedSuppliers>(
                    new ListSuppliersHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .WithHandshaking()
                .WithTimeout(TimeSpan.FromSeconds(5))
                .WithLogging()
                .WithTransaction()
                .Build();

            return new ListSuppliersController(
                listSuppliersHandler,
                _mapper);
        }

        if (type == typeof(ReplaceSupplierController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierRepository(dbConnection);

            // GetSupplier handler
            var getSupplierHandler = new QueryDecoratorBuilder<GetSupplierQuery, DomainModels.Supplier>(
                    new GetSupplierHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .WithHandshaking()
                .WithTimeout(TimeSpan.FromSeconds(5))
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // ReplaceSupplier handler
            var replaceSupplierHandler = new CommandDecoratorBuilder<ReplaceSupplierCommand>(
                    new ReplaceSupplierHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .WithHandshaking()
                .WithTimeout(TimeSpan.FromSeconds(5))
                .WithBulkhead(100, 500)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new ReplaceSupplierController(
                getSupplierHandler,
                replaceSupplierHandler,
                _mapper);
        }

        if (type == typeof(UpdateSupplierController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new SupplierRepository(dbConnection);

            // GetSupplier handler
            var getSupplierHandler = new QueryDecoratorBuilder<GetSupplierQuery, DomainModels.Supplier>(
                    new GetSupplierHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .WithHandshaking()
                .WithTimeout(TimeSpan.FromSeconds(5))
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // UpdateSupplier handler
            var updateSupplierHandler = new CommandDecoratorBuilder<UpdateSupplierCommand>(
                    new UpdateSupplierHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .WithHandshaking()
                .WithTimeout(TimeSpan.FromSeconds(5))
                .WithBulkhead(100, 500)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new UpdateSupplierController(
                getSupplierHandler,
                updateSupplierHandler,
                _mapper);
        }

        return null;
    }
}
