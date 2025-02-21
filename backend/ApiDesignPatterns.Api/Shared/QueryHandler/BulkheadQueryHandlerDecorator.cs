// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.Bulkhead;

namespace backend.Shared.QueryHandler;

public class BulkheadQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger,
    int maxParallelization,
    int maxQueuingActions)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    private readonly AsyncBulkheadPolicy _bulkheadPolicy = Policy
        .BulkheadAsync(
            maxParallelization: maxParallelization,
            maxQueuingActions: maxQueuingActions,
            onBulkheadRejectedAsync: async _ =>
            {
                logger.LogError("Bulkhead rejected execution - max concurrent operations ({Max}) reached",
                    maxParallelization);
                await Task.CompletedTask;
            });

    public async Task<TResult?> Handle(TQuery query)
    {
        return await _bulkheadPolicy.ExecuteAsync(async () =>
            await queryHandler.Handle(query));
    }
}
