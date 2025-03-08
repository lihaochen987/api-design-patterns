// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.Timeout;

namespace backend.Shared.CommandHandler;

public class TimeoutCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    ILogger<TimeoutCommandHandlerDecorator<TCommand>> logger,
    TimeSpan timeout)
    : ICommandHandler<TCommand>
{
    private readonly AsyncTimeoutPolicy _timeoutPolicy = Policy
        .TimeoutAsync(timeout, TimeoutStrategy.Pessimistic, onTimeoutAsync: (_, timespan, _) =>
        {
            logger.LogError("Operation timed out after {Timeout} seconds", timespan.TotalSeconds);
            return Task.CompletedTask;
        });

    public async Task Handle(TCommand command)
    {
        await _timeoutPolicy.ExecuteAsync(async () =>
            await commandHandler.Handle(command));
    }
}
