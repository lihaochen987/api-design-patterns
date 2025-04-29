// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Shared.CommandHandler;

public class HandshakingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    IDbConnection dbConnection,
    ILogger<HandshakingCommandHandlerDecorator<TCommand>> logger)
    : ICommandHandler<TCommand>
{
    private const string HealthCheckQuery =
        "SELECT " +
        "(SELECT COUNT(*) < (setting::int * 0.8) FROM pg_stat_activity, pg_settings " +
        "WHERE pg_settings.name = 'max_connections' AND state = 'active' group by setting) as IsReady";

    public async Task Handle(TCommand command)
    {
        logger.LogDebug("Performing database health check before executing command");

        bool isReady = await dbConnection.QueryFirstOrDefaultAsync<bool>(HealthCheckQuery);

        if (!isReady)
        {
            logger.LogWarning("Database rejected command - server at capacity");
            throw new DatabaseNotAvailableException("Database is at capacity and cannot accept more connections");
        }

        logger.LogDebug("Health check successful, proceeding with command");

        await commandHandler.Handle(command);
    }
}
