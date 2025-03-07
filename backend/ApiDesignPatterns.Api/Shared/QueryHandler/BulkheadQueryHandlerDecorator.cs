// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly.Bulkhead;

namespace backend.Shared.QueryHandler;

public class BulkheadQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    AsyncBulkheadPolicy bulkheadPolicy)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<TResult?> Handle(TQuery query)
    {
        return await bulkheadPolicy.ExecuteAsync(async () =>
            await queryHandler.Handle(query));
    }
}
