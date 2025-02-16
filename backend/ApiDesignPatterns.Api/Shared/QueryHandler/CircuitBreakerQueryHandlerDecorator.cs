// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CircuitBreaker;

namespace backend.Shared.QueryHandler;

public class CircuitBreakerQueryHandlerDecorator<TQuery, TResult>(
    ICircuitBreaker breaker,
    IQueryHandler<TQuery, TResult> queryHandler)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<TResult?> Handle(TQuery query)
    {
        breaker.Guard();
        try
        {
            TResult? result = await queryHandler.Handle(query);
            breaker.Succeed();
            return result;
        }
        catch (Exception)
        {
            breaker.Trip();
            throw;
        }
    }
}
