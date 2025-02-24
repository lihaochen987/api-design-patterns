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
            LogCommandExecution(operation, commandDetails);

            await commandHandler.Handle(command);

            stopwatch.Stop();
            LogSuccessfulExecution(operation, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            LogFailedExecution(ex, operation, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    private void LogCommandExecution(string operation, string commandDetails)
    {
        logger.LogInformation(
            "Executing command: {Operation} with data: {CommandDetails}",
            operation,
            commandDetails);
    }

    private void LogSuccessfulExecution(string operation, long elapsedMilliseconds)
    {
        logger.LogInformation(
            "Successfully executed command: {Operation} in {ElapsedMilliseconds}ms",
            operation,
            elapsedMilliseconds);
    }

    private void LogFailedExecution(Exception ex, string operation, long elapsedMilliseconds)
    {
        logger.LogError(
            ex,
            "Error while executing command: {Operation} after {ElapsedMilliseconds}ms",
            operation,
            elapsedMilliseconds);
    }
}
