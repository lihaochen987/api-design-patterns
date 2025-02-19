// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.CircuitBreaker;

namespace backend.Shared.QueryHandler;

public class CircuitBreakerQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy = Policy
        .Handle<Exception>()
        .CircuitBreakerAsync(
            exceptionsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (exception, duration) =>
            {
                logger.LogCritical("Executing command: {Operation} with data: {CommandDetails}",
                    duration.TotalSeconds,
                    exception.Message);
            },
            onReset: () => { logger.LogCritical("Circuit breaker reset"); },
            onHalfOpen: () => { logger.LogCritical("Circuit breaker half-opened"); });

    public async Task<TResult?> Handle(TQuery query)
    {
        return await _circuitBreakerPolicy.ExecuteAsync(async () =>
            await queryHandler.Handle(query));
    }
}
