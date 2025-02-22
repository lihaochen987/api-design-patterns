// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using Newtonsoft.Json;

namespace backend.Shared.CommandHandler;

public class LoggingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    ILogger<LoggingCommandHandlerDecorator<TCommand>> logger)
    : ICommandHandler<TCommand>
{
    public async Task Handle(TCommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        string operation = command.GetType().Name;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            string commandDetails = JsonConvert.SerializeObject(command, Formatting.Indented);
            logger.LogInformation("Executing command: {Operation} with data: {CommandDetails}", operation,
                commandDetails);

            await commandHandler.Handle(command);

            stopwatch.Stop();
            logger.LogInformation(
                "Successfully executed command: {Operation} in {ElapsedMilliseconds}ms",
                operation,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(
                ex,
                "Error while executing command: {Operation} after {ElapsedMilliseconds}ms",
                operation,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
