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
    public async Task<TResult?> Handle(TQuery query)
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
            logger.LogInformation(
                "Executing query: {Operation} with data: {CommandDetails}",
                operation,
                commandDetails);

            TResult? result = await queryHandler.Handle(query);

            stopwatch.Stop();
            string queryResult = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.LogInformation(
                "Successfully executed query: {Operation} with data: {commandResult} in {ElapsedMilliseconds}ms",
                operation,
                queryResult,
                stopwatch.ElapsedMilliseconds);
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(
                ex,
                "Error while executing query: {Operation} after {ElapsedMilliseconds}ms",
                operation,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
