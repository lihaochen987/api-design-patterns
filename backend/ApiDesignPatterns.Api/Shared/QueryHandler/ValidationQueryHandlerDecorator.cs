// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Shared.QueryHandler;

public class ValidationQueryHandlerDecorator<TQuery, TResult>(
    IAsyncQueryHandler<TQuery, TResult> queryHandler)
    : IAsyncQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public async Task<TResult> Handle(TQuery query)
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        TResult result = await queryHandler.Handle(query);
        if (result is object obj && !obj.GetType().IsValueType)
        {
            ValidateModel(obj);
        }

        return result;
    }


    private static void ValidateModel<T>(T model) where T : class
    {
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();

        if (Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true))
        {
            return;
        }

        var errors = validationResults
            .Select(r => r.ErrorMessage)
            .ToList();

        throw new ValidationException($"Validation failed: {string.Join(", ", errors)}");
    }
}
