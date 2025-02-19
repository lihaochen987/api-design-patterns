// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.CircuitBreaker;

namespace backend.Shared.CommandHandler;

public class CircuitBreakerCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    ILogger<LoggingCommandHandlerDecorator<TCommand>> logger,
    TimeSpan durationOfBreak,
    int exceptionsAllowedBeforeBreaking)
    : ICommandHandler<TCommand>
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

    public async Task Handle(TCommand command)
    {
        await _circuitBreakerPolicy.ExecuteAsync(async () =>
            await commandHandler.Handle(command));
    }
}
