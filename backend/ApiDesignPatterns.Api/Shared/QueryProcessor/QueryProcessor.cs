// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using backend.Shared.QueryHandler;

namespace backend.Shared.QueryProcessor;

internal sealed class QueryProcessor(IServiceProvider serviceProvider) : IQueryProcessor
{
    [DebuggerStepThrough]
    public async Task<TResult> Process<TResult>(IQuery<TResult> query)
    {
        var asyncHandlerType = typeof(IAsyncQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResult));

        dynamic? asyncHandler = serviceProvider.GetService(asyncHandlerType);

        if (asyncHandler != null)
        {
            return await asyncHandler.Handle((dynamic)query);
        }

        var syncHandlerType = typeof(ISyncQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResult));

        dynamic? syncHandler = serviceProvider.GetService(syncHandlerType);

        if (syncHandler != null)
        {
            TResult result = syncHandler.Handle((dynamic)query);
            return result;
        }

        throw new InvalidOperationException(
            $"No handler found for query type {query.GetType().FullName}. " +
            $"Ensure a handler implementing either IAsyncQueryHandler<{query.GetType().Name}, {typeof(TResult).Name}> " +
            $"or ISyncQueryHandler<{query.GetType().Name}, {typeof(TResult).Name}> is registered.");
    }
}
