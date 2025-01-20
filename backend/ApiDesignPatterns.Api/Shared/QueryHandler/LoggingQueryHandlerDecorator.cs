// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;

namespace backend.Shared.QueryHandler;

public class LoggingQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<TResult?> Handle(TQuery query)
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        string operation = query.GetType().Name;

        try
        {
            string commandDetails = JsonConvert.SerializeObject(query, Formatting.Indented);
            logger.LogInformation(
                "Executing command: {Operation} with data: {CommandDetails}",
                operation,
                commandDetails);
            TResult? result = await queryHandler.Handle(query);
            string commandResult = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.LogInformation(
                "Successfully executed command: {Operation} with data: {commandResult}",
                operation,
                commandResult);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while executing command: {Operation}", operation);
            throw;
        }
    }
}
