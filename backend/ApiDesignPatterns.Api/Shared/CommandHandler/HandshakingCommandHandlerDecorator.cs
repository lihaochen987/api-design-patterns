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
    public async Task Handle(TCommand command)
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

        await commandHandler.Handle(command);
    }
}
