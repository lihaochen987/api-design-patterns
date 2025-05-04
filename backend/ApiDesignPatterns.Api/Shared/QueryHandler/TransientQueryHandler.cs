// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.QueryHandler;

public class TransientQueryHandler<TQuery, TResult>(
    Func<IAsyncQueryHandler<TQuery, TResult>> handlerFactory)
    : IAsyncQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public async Task<TResult> Handle(TQuery query)
    {
        var handler = handlerFactory();
        var result = await handler.Handle(query);
        return result;
    }
}
