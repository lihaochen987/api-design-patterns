// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using backend.Shared.Utility;
using backend.User.ApplicationLayer.Commands.CreateUser;
using backend.User.ApplicationLayer.Commands.DeleteUser;
using backend.User.ApplicationLayer.Commands.ReplaceUser;
using backend.User.ApplicationLayer.Commands.UpdateUser;
using backend.User.ApplicationLayer.Queries.GetUser;
using backend.User.ApplicationLayer.Queries.GetUserView;
using backend.User.ApplicationLayer.Queries.ListUsers;
using backend.User.Controllers;
using backend.User.DomainModels;
using backend.User.InfrastructureLayer.Database.User;
using backend.User.InfrastructureLayer.Database.UserView;
using backend.User.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace backend.User;

public class UserControllerActivator : BaseControllerActivator
{
    private readonly PaginateService<UserView> _userPaginateService;
    private readonly SqlFilterBuilder _userSqlFilterBuilder;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly IMapper _mapper;

    public UserControllerActivator(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration)
    {
        _userPaginateService = new PaginateService<UserView>();

        UserColumnMapper userColumnMapper = new();
        _userSqlFilterBuilder = new SqlFilterBuilder(userColumnMapper);

        _loggerFactory = loggerFactory;

        UserFieldPaths userFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(userFieldPaths.ValidPaths);

        var config = new TypeAdapterConfig();
        config.RegisterUserMappings();
        _mapper = new Mapper(config);
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(CreateUserController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new UserRepository(dbConnection);

            // CreateUser handler
            var createUserHandler = new CommandDecoratorBuilder<CreateUserCommand>(
                    new CreateUserHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.UserWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new CreateUserController(
                createUserHandler,
                _mapper);
        }

        if (type == typeof(DeleteUserController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new UserRepository(dbConnection);

            // GetUser handler
            var getUserHandler = new QueryDecoratorBuilder<GetUserQuery, DomainModels.User?>(
                    new GetUserHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.UserRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // DeleteUser handler
            var deleteUserHandler = new CommandDecoratorBuilder<DeleteUserCommand>(
                    new DeleteUserHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.UserWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new DeleteUserController(
                getUserHandler,
                deleteUserHandler);
        }

        if (type == typeof(GetUserController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new UserViewRepository(dbConnection, _userSqlFilterBuilder, _userPaginateService);

            // GetUserView handler
            var getUserViewHandler = new QueryDecoratorBuilder<GetUserViewQuery, UserView?>(
                    new GetUserViewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.UserRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            return new GetUserController(
                getUserViewHandler,
                _fieldMaskConverterFactory,
                _mapper);
        }

        if (type == typeof(ListUsersController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new UserViewRepository(dbConnection, _userSqlFilterBuilder, _userPaginateService);

            // ListUsers query handler
            var listUsersHandler = new QueryDecoratorBuilder<ListUsersQuery, PagedUsers>(
                    new ListUsersHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.UserRead)
                .WithLogging()
                .WithTransaction()
                .Build();

            return new ListUsersController(
                listUsersHandler,
                _mapper);
        }

        if (type == typeof(ReplaceUserController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new UserRepository(dbConnection);

            // GetUser handler
            var getUserHandler = new QueryDecoratorBuilder<GetUserQuery, DomainModels.User?>(
                    new GetUserHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.UserRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // ReplaceUser handler
            var replaceUserHandler = new CommandDecoratorBuilder<ReplaceUserCommand>(
                    new ReplaceUserHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.UserWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new ReplaceUserController(
                getUserHandler,
                replaceUserHandler,
                _mapper);
        }

        if (type == typeof(UpdateUserController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new UserRepository(dbConnection);

            // GetUser handler
            var getUserHandler = new QueryDecoratorBuilder<GetUserQuery, DomainModels.User?>(
                    new GetUserHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.UserRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // UpdateUser handler
            var updateUserHandler = new CommandDecoratorBuilder<UpdateUserCommand>(
                    new UpdateUserHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.UserWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new UpdateUserController(
                getUserHandler,
                updateUserHandler,
                _mapper);
        }

        return null;
    }
}
