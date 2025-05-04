// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.CommandHandler;

/// <summary>
/// A command handler decorator that creates a new handler instance for each command execution.
/// This class is useful for implementing transient dependencies in command handlers, particularly
/// for resources that need a fresh instance on each use, such as database connections.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle. Must be a reference type.</typeparam>
public class TransientCommandHandler<TCommand>(
    Func<ICommandHandler<TCommand>> handlerFactory)
    : ICommandHandler<TCommand>
    where TCommand : class
{
    /// <summary>
    /// Handles the specified command by creating a new handler instance and delegating execution to it.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method creates a new handler instance for each invocation, making it suitable for
    /// parallel command execution scenarios where each command needs its own resources,
    /// such as in Task.WhenAll operations.
    /// </remarks>
    public async Task Handle(TCommand command)
    {
        var handler = handlerFactory();
        await handler.Handle(command);
    }
}
