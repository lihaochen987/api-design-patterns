// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.Timeout;

namespace backend.Shared.QueryHandler;

public class TimeoutQueryHandlerDecorator<TQuery, TResult>(
    IAsyncQueryHandler<TQuery, TResult> queryHandler,
    ILogger<TimeoutQueryHandlerDecorator<TQuery, TResult>> logger,
    TimeSpan timeout)
    : IAsyncQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    private readonly AsyncTimeoutPolicy _timeoutPolicy = Policy
        .TimeoutAsync(timeout, onTimeoutAsync: (_, timespan, _) =>
        {
            logger.LogError("Operation timed out after {Timeout} seconds", timespan.TotalSeconds);
            return Task.CompletedTask;
        });

    public async Task<TResult> Handle(TQuery query)
    {
        return await _timeoutPolicy.ExecuteAsync(async () =>
            await queryHandler.Handle(query));
    }
}

