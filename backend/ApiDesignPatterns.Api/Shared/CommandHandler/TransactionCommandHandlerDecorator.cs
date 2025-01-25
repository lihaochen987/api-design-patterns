// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;

namespace backend.Shared.CommandHandler;

public class TransactionCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    IDbConnection dbConnection)
    : ICommandHandler<TCommand>
{
    public async Task Handle(TCommand command)
    {
        dbConnection.Open();
        using var transaction = dbConnection.BeginTransaction();
        try
        {
            await commandHandler.Handle(command);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            dbConnection.Close();
        }
    }
}
