// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;

namespace backend.Shared.CommandHandler;

public class LoggingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    ILogger<LoggingCommandHandlerDecorator<TCommand>> logger)
    : ICommandHandler<TCommand>
{
    public async Task Execute(TCommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        string operation = command.GetType().Name;

        try
        {
            string commandDetails = JsonConvert.SerializeObject(command, Formatting.Indented);
            logger.LogInformation("Executing command: {Operation} with data: {CommandDetails}", operation,
                commandDetails);
            await commandHandler.Execute(command);
            logger.LogInformation("Successfully executed command: {Operation}", operation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while executing command: {Operation}", operation);
            throw;
        }
    }
}
