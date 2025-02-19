// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryView;
using backend.Inventory.DomainModels;
using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Inventory.InfrastructureLayer.Database.InventoryView;
using backend.Inventory.InventoryControllers;
using backend.Inventory.Services;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;

namespace backend.Inventory;

public class InventoryControllerActivator : BaseControllerActivator
{
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly IMapper _mapper;
    private readonly ILoggerFactory _loggerFactory;

    public InventoryControllerActivator(
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
        : base(configuration)
    {
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
        _mapper = mapperConfig.CreateMapper();

        InventoryFieldPaths inventoryFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(inventoryFieldPaths.ValidPaths);

        _loggerFactory = loggerFactory;
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(CreateInventoryController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new InventoryRepository(dbConnection);

            // CreateInventory handler
            var createInventoryHandler = new CommandDecoratorBuilder<CreateInventoryCommand>(
                    new CreateInventoryHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithAudit()
                .WithLogging()
                .WithTransaction()
                .WithCircuitBreaker()
                .Build();

            return new CreateInventoryController(
                createInventoryHandler,
                _mapper);
        }

        if (type == typeof(GetInventoryController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new InventoryViewRepository(dbConnection);

            // GetInventoryView handler
            var getInventoryViewHandler = new QueryDecoratorBuilder<GetInventoryViewQuery, InventoryView>(
                    new GetInventoryViewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .WithCircuitBreaker()
                .Build();

            return new GetInventoryController(
                getInventoryViewHandler,
                _fieldMaskConverterFactory,
                _mapper);
        }

        return null;
    }
}
