// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using backend.Shared.Exceptions;
using Dapper;

namespace backend.Shared.QueryHandler;

public class HandshakingQueryHandlerDecorator<TQuery, TResult>(
    IAsyncQueryHandler<TQuery, TResult> queryHandler,
    IDbConnection dbConnection,
    ILogger<HandshakingQueryHandlerDecorator<TQuery, TResult>> logger)
    : IAsyncQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    private const string HealthCheckQuery =
        "SELECT " +
        "(SELECT COUNT(*) < (setting::int * 0.8) FROM pg_stat_activity, pg_settings " +
        "WHERE pg_settings.name = 'max_connections' AND state = 'active' group by setting) as IsReady";

    public async Task<TResult> Handle(TQuery query)
    {
        logger.LogDebug("Performing database health check before executing query");

        bool isReady = await dbConnection.QueryFirstOrDefaultAsync<bool>(HealthCheckQuery);

        if (!isReady)
        {
            logger.LogWarning("Database rejected query - server at capacity");
            throw new DatabaseNotAvailableException("Database is at capacity and cannot accept more connections");
        }

        logger.LogDebug("Health check successful, proceeding with query");

        TResult result = await queryHandler.Handle(query);
        return result;
    }
}
