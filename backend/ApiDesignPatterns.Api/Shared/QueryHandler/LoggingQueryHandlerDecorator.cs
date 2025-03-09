// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using Newtonsoft.Json;

namespace backend.Shared.QueryHandler;

public class LoggingQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<TResult> Handle(TQuery query)
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        string operation = query.GetType().Name;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            string commandDetails = JsonConvert.SerializeObject(query, Formatting.Indented);
            LogQueryExecution(operation, commandDetails);

            TResult result = await queryHandler.Handle(query);

            stopwatch.Stop();
            LogSuccessfulExecution(operation, result, stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            LogFailedExecution(ex, operation, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    private void LogQueryExecution(string operation, string commandDetails)
    {
        logger.LogInformation(
            "Executing query: {Operation} with data: {CommandDetails}",
            operation,
            commandDetails);
    }

    private void LogSuccessfulExecution(string operation, TResult? result, long elapsedMilliseconds)
    {
        string queryResult = JsonConvert.SerializeObject(result, Formatting.Indented);
        logger.LogInformation(
            "Successfully executed query: {Operation} with data: {commandResult} in {ElapsedMilliseconds}ms",
            operation,
            queryResult,
            elapsedMilliseconds);
    }

    private void LogFailedExecution(Exception ex, string operation, long elapsedMilliseconds)
    {
        logger.LogError(
            ex,
            "Error while executing query: {Operation} after {ElapsedMilliseconds}ms",
            operation,
            elapsedMilliseconds);
    }
}
