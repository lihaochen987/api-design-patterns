// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using backend.Shared.QueryHandler;

namespace backend.Shared.QueryProcessor;

internal sealed class QueryProcessor(IServiceProvider serviceProvider) : IQueryProcessor
{
    [DebuggerStepThrough]
    public Task<TResult> Process<TResult>(IQuery<TResult> query)
    {
        var handlerType = typeof(IAsyncQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResult));

        dynamic? handler = serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"Handler for {handlerType.FullName} not found.");
        }

        return handler.Handle((dynamic)query);
    }
}
