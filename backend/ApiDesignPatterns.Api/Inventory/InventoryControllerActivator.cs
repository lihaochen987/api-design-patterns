// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.ApplicationLayer.Commands.DeleteInventory;
using backend.Inventory.ApplicationLayer.Commands.UpdateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndUser;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryView;
using backend.Inventory.ApplicationLayer.Queries.GetUsersByIds;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Inventory.InfrastructureLayer.Database.InventoryView;
using backend.Inventory.Services;
using backend.Product.ApplicationLayer.Queries.BatchGetProductResponses;
using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.Services;
using backend.Product.Services.Mappers;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using backend.Shared.Utility;
using backend.User.DomainModels;
using backend.User.InfrastructureLayer.Database.UserView;
using backend.User.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using IMapper = MapsterMapper.IMapper;
using Mapper = MapsterMapper.Mapper;

namespace backend.Inventory;

public class InventoryControllerActivator : BaseControllerActivator
{
    private readonly PaginateService<UserView> _userPaginateService;
    private readonly SqlFilterBuilder _userSqlFilterBuilder;
    private readonly PaginateService<InventoryView> _inventoryViewPaginateService;
    private readonly SqlFilterBuilder _inventorySqlFilterBuilder;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly ILoggerFactory _loggerFactory;
    private readonly SqlFilterBuilder _productSqlFilterBuilder;
    private readonly IMapper _mapper;

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

        _userPaginateService = new PaginateService<UserView>();

        UserColumnMapper userColumnMapper = new();
        _userSqlFilterBuilder = new SqlFilterBuilder(userColumnMapper);

        ProductColumnMapper productColumnMapper = new();
        _productSqlFilterBuilder = new SqlFilterBuilder(productColumnMapper);

        var config = new TypeAdapterConfig();
        config.RegisterProductMappings();
        config.RegisterUserMappings();
        config.RegisterInventoryMappings();
        _mapper = new Mapper(config);
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
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.InventoryWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            // GetByProductAndUser handler
            var getInventoryByProductAndUser =
                new QueryDecoratorBuilder<GetInventoryByProductAndUserQuery, DomainModels.Inventory?>(
                        new GetInventoryByProductAndUserHandler(repository),
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
                getInventoryByProductAndUser,
                _mapper);
        }

        if (type == typeof(GetInventoryController))
        {
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
                _mapper);
        }

        if (type == typeof(UpdateInventoryController))
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
                _mapper);
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
                _mapper);
        }

        if (type == typeof(ListProductUsersController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var inventoryViewRepository = new InventoryViewRepository(
                dbConnection,
                _inventorySqlFilterBuilder,
                _inventoryViewPaginateService);

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

            // Transient BatchGetUsersByIdsHandler
            var getUsersByIds =
                new TransientQueryHandler<GetUsersByIdsQuery, List<UserView>>(
                    BatchGetUsersByIdsFactory);

            return new ListProductUsersController(
                listInventoryHandler,
                getUsersByIds,
                _mapper);

            IAsyncQueryHandler<GetUsersByIdsQuery, List<UserView>>
                BatchGetUsersByIdsFactory()
            {
                var batchGetDbConnection = CreateDbConnection();
                TrackDisposable(context, batchGetDbConnection);

                var batchGetRepository =
                    new UserViewRepository(dbConnection, _userSqlFilterBuilder, _userPaginateService);

                return new QueryDecoratorBuilder<GetUsersByIdsQuery, List<UserView>>(
                        new GetUsersByIdsHandler(batchGetRepository),
                        _loggerFactory,
                        batchGetDbConnection)
                    .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                    .WithHandshaking()
                    .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                    .WithBulkhead(BulkheadPolicies.UserRead)
                    .WithLogging()
                    .WithValidation()
                    .WithTransaction()
                    .Build();
            }
        }

        if (type == typeof(ListUserProductsController))
        {
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
            var batchGetProductsHandler =
                new QueryDecoratorBuilder<BatchGetProductResponsesQuery, Result<List<GetProductResponse>>>(
                        new BatchGetProductResponsesHandler(productViewRepository, _mapper),
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

            return new ListUserProductsController(
                listInventoryHandler,
                batchGetProductsHandler,
                _mapper);
        }

        return null;
    }
}
