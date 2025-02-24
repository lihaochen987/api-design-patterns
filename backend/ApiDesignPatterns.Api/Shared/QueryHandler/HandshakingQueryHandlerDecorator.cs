// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Shared.QueryHandler;

public class HandShakingQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    IDbConnection dbConnection,
    ILogger<HandShakingQueryHandlerDecorator<TQuery, TResult>> logger)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{

    public async Task<TResult?> Handle(TQuery query)
    {
        try
        {
            await dbConnection.ExecuteScalarAsync<int>("SELECT 1");
        }
        catch (Exception ex)
        {
            logger.LogError("Database handshake failed: {Error}", ex.Message);
            throw new DatabaseNotAvailableException("Database health check failed", ex);
        }

        TResult? result = await queryHandler.Handle(query);
        return result;
    }
}
