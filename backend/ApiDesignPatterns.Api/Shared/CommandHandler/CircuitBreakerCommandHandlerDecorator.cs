// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CircuitBreaker;

namespace backend.Shared.CommandHandler;

public class CircuitBreakerCommandHandlerDecorator<TCommand>(
    ICircuitBreaker breaker,
    ICommandHandler<TCommand> commandHandler)
    : ICommandHandler<TCommand>
{
    public async Task Handle(TCommand command)
    {
        breaker.Guard();
        try
        {
            await commandHandler.Handle(command);
            breaker.Succeed();
        }
        catch (Exception)
        {
            breaker.Trip();
            throw;
        }
    }
}

public enum CircuitState
{
    Closed,
    Open,
    HalfOpen
}

public class CircuitBreakerOpenException(string message) : Exception(message);
