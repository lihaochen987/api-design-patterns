// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;

namespace backend.Shared.QueryHandler;

public class TransactionQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    IDbConnection dbConnection)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<TResult> Handle(TQuery query)
    {
        dbConnection.Open();
        using var transaction = dbConnection.BeginTransaction();
        try
        {
            TResult result = await queryHandler.Handle(query);
            transaction.Commit();
            return result;
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
