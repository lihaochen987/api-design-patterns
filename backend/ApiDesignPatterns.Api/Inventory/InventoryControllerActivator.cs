// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.ApplicationLayer.Commands.DeleteInventory;
using backend.Inventory.ApplicationLayer.Commands.UpdateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndSupplier;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryView;
using backend.Inventory.ApplicationLayer.Queries.GetSuppliersByIds;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Inventory.InfrastructureLayer.Database.InventoryView;
using backend.Inventory.Services;
using backend.Product.ApplicationLayer.Queries.BatchGetProducts;
using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.Services;
using backend.Product.Services.Mappers;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Database.SupplierView;
using backend.Supplier.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Inventory;

public class InventoryControllerActivator : BaseControllerActivator
{
    private readonly PaginateService<SupplierView> _supplierPaginateService;
    private readonly SqlFilterBuilder _supplierSqlFilterBuilder;
    private readonly PaginateService<InventoryView> _inventoryViewPaginateService;
    private readonly SqlFilterBuilder _inventorySqlFilterBuilder;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly ILoggerFactory _loggerFactory;
    private readonly SqlFilterBuilder _productSqlFilterBuilder;

    public InventoryControllerActivator(
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
        : base(configuration)
    {
        InventoryFieldPaths inventoryFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(inventoryFieldPaths.ValidPaths);

        _loggerFactory = loggerFactory;

        _inventoryViewPaginateService = new PaginateService<InventoryView>();

        InventoryColumnMapper inventoryColumnMapper = new();
        _inventorySqlFilterBuilder = new SqlFilterBuilder(inventoryColumnMapper);

        _supplierPaginateService = new PaginateService<SupplierView>();

        SupplierColumnMapper supplierColumnMapper = new();
        _supplierSqlFilterBuilder = new SqlFilterBuilder(supplierColumnMapper);

        ProductColumnMapper productColumnMapper = new();
        _productSqlFilterBuilder = new SqlFilterBuilder(productColumnMapper);
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(CreateInventoryController))
        {
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
            var mapper = mapperConfig.CreateMapper();

            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new InventoryRepository(dbConnection);

            // CreateInventory handler
            var createInventoryHandler = new CommandDecoratorBuilder<CreateInventoryCommand>(
                    new CreateInventoryHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.InventoryWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            // GetByProductAndSupplier handler
            var getInventoryByProductAndSupplier =
                new QueryDecoratorBuilder<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?>(
                        new GetInventoryByProductAndSupplierHandler(repository),
                        _loggerFactory,
                        dbConnection)
                    .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                    .WithHandshaking()
                    .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                    .WithBulkhead(BulkheadPolicies.InventoryRead)
                    .WithLogging()
                    .WithValidation()
                    .WithTransaction()
                    .Build();

            return new CreateInventoryController(
                createInventoryHandler,
                getInventoryByProductAndSupplier,
                mapper);
        }

        if (type == typeof(GetInventoryController))
        {
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
            var mapper = mapperConfig.CreateMapper();

            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new InventoryViewRepository(
                dbConnection,
                _inventorySqlFilterBuilder,
                _inventoryViewPaginateService);

            // GetInventoryView handler
            var getInventoryViewHandler = new QueryDecoratorBuilder<GetInventoryViewQuery, InventoryView?>(
                    new GetInventoryViewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.InventoryRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new GetInventoryController(
                getInventoryViewHandler,
                _fieldMaskConverterFactory,
                mapper);
        }

        if (type == typeof(UpdateInventoryController))
        {
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
            var mapper = mapperConfig.CreateMapper();

            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new InventoryRepository(dbConnection);

            // GetByIdInventory handler
            var getInventoryByIdHandler = new QueryDecoratorBuilder<GetInventoryByIdQuery, DomainModels.Inventory?>(
                    new GetInventoryByIdByIdHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.InventoryRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // UpdateInventory handler
            var updateInventoryHandler = new CommandDecoratorBuilder<UpdateInventoryCommand>(
                    new UpdateInventoryHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.InventoryWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new UpdateInventoryController(
                getInventoryByIdHandler,
                updateInventoryHandler,
                mapper);
        }

        if (type == typeof(DeleteInventoryController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new InventoryRepository(dbConnection);

            // GetByIdInventory handler
            var getInventoryByIdHandler = new QueryDecoratorBuilder<GetInventoryByIdQuery, DomainModels.Inventory?>(
                    new GetInventoryByIdByIdHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.InventoryRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // DeleteInventory handler
            var deleteInventoryHandler = new CommandDecoratorBuilder<DeleteInventoryCommand>(
                    new DeleteInventoryCommandHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.InventoryWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new DeleteInventoryController(
                getInventoryByIdHandler,
                deleteInventoryHandler);
        }

        if (type == typeof(ListInventoryController))
        {
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<InventoryMappingProfile>(); });
            var mapper = mapperConfig.CreateMapper();

            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new InventoryViewRepository(
                dbConnection,
                _inventorySqlFilterBuilder,
                _inventoryViewPaginateService);

            // ListInventory handler
            var listInventoryHandler = new QueryDecoratorBuilder<ListInventoryQuery, PagedInventory>(
                    new ListInventoryHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.InventoryRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new ListInventoryController(
                listInventoryHandler,
                mapper);
        }

        if (type == typeof(ListProductSuppliersController))
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<InventoryMappingProfile>();
                cfg.AddProfile<SupplierMappingProfile>();
            });
            var mapper = mapperConfig.CreateMapper();

            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var inventoryViewRepository = new InventoryViewRepository(
                dbConnection,
                _inventorySqlFilterBuilder,
                _inventoryViewPaginateService);
            var supplierViewRepository =
                new SupplierViewRepository(dbConnection, _supplierSqlFilterBuilder, _supplierPaginateService);

            // ListInventory handler
            var listInventoryHandler = new QueryDecoratorBuilder<ListInventoryQuery, PagedInventory>(
                    new ListInventoryHandler(inventoryViewRepository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.InventoryRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // GetSuppliersByIds handler
            var getSuppliersByIds = new QueryDecoratorBuilder<GetSuppliersByIdsQuery, List<SupplierView>>(
                    new GetSuppliersByIdsHandler(supplierViewRepository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.SupplierRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new ListProductSuppliersController(
                listInventoryHandler,
                getSuppliersByIds,
                mapper);
        }

        if (type == typeof(ListSupplierProductsController))
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<InventoryMappingProfile>();
                cfg.AddProfile<ProductMappingProfile>();
            });
            var mapper = mapperConfig.CreateMapper();

            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var inventoryViewRepository = new InventoryViewRepository(
                dbConnection,
                _inventorySqlFilterBuilder,
                _inventoryViewPaginateService);
            var productViewRepository = new ProductViewRepository(dbConnection, _productSqlFilterBuilder);

            // ListInventory handler
            var listInventoryHandler = new QueryDecoratorBuilder<ListInventoryQuery, PagedInventory>(
                    new ListInventoryHandler(inventoryViewRepository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.InventoryRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // BatchGetProducts handler
            var batchGetProductsHandler = new QueryDecoratorBuilder<BatchGetProductsQuery, Result<List<GetProductResponse>>>(
                    new BatchGetProductsHandler(productViewRepository, mapper),
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

            return new ListSupplierProductsController(
                listInventoryHandler,
                batchGetProductsHandler,
                mapper);
        }

        return null;
    }
}
