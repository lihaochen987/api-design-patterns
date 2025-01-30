// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Shared.QueryHandler;

public class ValidationQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    RecursiveValidator validator)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<TResult?> Handle(TQuery query)
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        TResult? result = await queryHandler.Handle(query);

        if (result == null)
        {
            return result;
        }

        var validationResults = validator.Validate(result);
        if (validationResults.Count == 0)
        {
            return result;
        }

        var errorMessages = validationResults
            .Select(validationResult =>
                $"{string.Join(", ", validationResult.MemberNames)}: {validationResult.ErrorMessage}");

        throw new ValidationException(
            $"Validation failed for {typeof(TResult).Name}: {string.Join(Environment.NewLine, errorMessages)}");
    }
}
