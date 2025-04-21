// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.CircuitBreaker;

namespace backend.Shared.QueryHandler;

public class CircuitBreakerQueryHandlerDecorator<TQuery, TResult>(
    IAsyncQueryHandler<TQuery, TResult> queryHandler,
    ILogger<CircuitBreakerQueryHandlerDecorator<TQuery, TResult>> logger,
    TimeSpan durationOfBreak,
    int exceptionsAllowedBeforeBreaking)
    : IAsyncQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy = Policy
        .Handle<Exception>()
        .CircuitBreakerAsync(
            exceptionsAllowedBeforeBreaking: exceptionsAllowedBeforeBreaking,
            durationOfBreak: durationOfBreak,
            onBreak: (exception, duration) =>
            {
                logger.LogCritical("Executing command: {Operation} with data: {CommandDetails}",
                    duration.TotalSeconds,
                    exception.Message);
            },
            onReset: () => { logger.LogCritical("Circuit breaker reset"); },
            onHalfOpen: () => { logger.LogCritical("Circuit breaker half-opened"); });

    public async Task<TResult> Handle(TQuery query)
    {
        return await _circuitBreakerPolicy.ExecuteAsync(async () =>
            await queryHandler.Handle(query));
    }
}
