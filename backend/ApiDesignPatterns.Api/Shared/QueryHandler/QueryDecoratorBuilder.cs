// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;

namespace backend.Shared.QueryHandler;

public class QueryDecoratorBuilder<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> handler,
    ILoggerFactory loggerFactory,
    IDbConnection? dbConnection)
    where TQuery : IQuery<TResult> where TResult : class
{
    private IQueryHandler<TQuery, TResult> _handler = handler;
    private bool _useTransaction;
    private bool _useValidation;
    private bool _useLogging;
    private bool _useCircuitBreaker;
    private TimeSpan _durationOfBreak;
    private int _exceptionsAllowedBeforeBreaking;

    public QueryDecoratorBuilder<TQuery, TResult> WithTransaction()
    {
        if (dbConnection == null)
        {
            throw new InvalidOperationException("Database connection is required for transaction decorator");
        }

        _useTransaction = true;
        return this;
    }

    public QueryDecoratorBuilder<TQuery, TResult> WithValidation()
    {
        _useValidation = true;
        return this;
    }

    public QueryDecoratorBuilder<TQuery, TResult> WithLogging()
    {
        _useLogging = true;
        return this;
    }

    public QueryDecoratorBuilder<TQuery, TResult> WithCircuitBreaker(TimeSpan durationOfBreak,
        int exceptionsAllowedBeforeBreaking)
    {
        _useCircuitBreaker = true;
        _durationOfBreak = durationOfBreak;
        _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
        return this;
    }

    public IQueryHandler<TQuery, TResult> Build()
    {
        if (_useLogging)
        {
            _handler = new LoggingQueryHandlerDecorator<TQuery, TResult>(
                _handler,
                loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<TQuery, TResult>>());
        }

        if (_useValidation)
        {
            _handler = new ValidationQueryHandlerDecorator<TQuery, TResult>(_handler);
        }

        if (_useTransaction && dbConnection != null)
        {
            _handler = new TransactionQueryHandlerDecorator<TQuery, TResult>(_handler, dbConnection);
        }

        if (_useCircuitBreaker && _useTransaction)
        {
            _handler = new CircuitBreakerQueryHandlerDecorator<TQuery, TResult>(
                _handler,
                loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<TQuery, TResult>>(),
                _durationOfBreak,
                _exceptionsAllowedBeforeBreaking);
        }

        return _handler;
    }
}
