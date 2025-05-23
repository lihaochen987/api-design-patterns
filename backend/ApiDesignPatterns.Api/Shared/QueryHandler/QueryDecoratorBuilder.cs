// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Polly.Bulkhead;

namespace backend.Shared.QueryHandler;

public class QueryDecoratorBuilder<TQuery, TResult>(
    IAsyncQueryHandler<TQuery, TResult> handler,
    ILoggerFactory loggerFactory,
    IDbConnection? dbConnection)
    where TQuery : IQuery<TResult>
{
    private IAsyncQueryHandler<TQuery, TResult> _handler = handler;
    private bool _useTransaction;
    private bool _useValidation;
    private bool _useLogging;
    private bool _useCircuitBreaker;
    private bool _useHandshaking;
    private TimeSpan _durationOfBreak;
    private int _exceptionsAllowedBeforeBreaking;
    private bool _useTimeout;
    private TimeSpan _timeout;
    private bool _useBulkhead;
    private AsyncBulkheadPolicy? _bulkheadPolicy;

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

    public QueryDecoratorBuilder<TQuery, TResult> WithCircuitBreaker(
        TimeSpan durationOfBreak,
        int exceptionsAllowedBeforeBreaking)
    {
        _useCircuitBreaker = true;
        _durationOfBreak = durationOfBreak;
        _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
        return this;
    }

    public QueryDecoratorBuilder<TQuery, TResult> WithHandshaking()
    {
        _useHandshaking = true;
        return this;
    }

    public QueryDecoratorBuilder<TQuery, TResult> WithTimeout(TimeSpan timeout)
    {
        _useTimeout = true;
        _timeout = timeout;
        return this;
    }

    public QueryDecoratorBuilder<TQuery, TResult> WithBulkhead(AsyncBulkheadPolicy policy)
    {
        _useBulkhead = true;
        _bulkheadPolicy = policy;
        return this;
    }

    public IAsyncQueryHandler<TQuery, TResult> Build()
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

        if (_useCircuitBreaker)
        {
            _handler = new CircuitBreakerQueryHandlerDecorator<TQuery, TResult>(
                _handler,
                loggerFactory.CreateLogger<CircuitBreakerQueryHandlerDecorator<TQuery, TResult>>(),
                _durationOfBreak,
                _exceptionsAllowedBeforeBreaking);
        }

        if (_useTimeout)
        {
            _handler = new TimeoutQueryHandlerDecorator<TQuery, TResult>(
                _handler,
                loggerFactory.CreateLogger<TimeoutQueryHandlerDecorator<TQuery, TResult>>(),
                _timeout);
        }

        if (_useHandshaking)
        {
            if (dbConnection == null)
            {
                throw new InvalidOperationException("Database connection is required for handshaking decorator");
            }

            _handler = new HandshakingQueryHandlerDecorator<TQuery, TResult>(
                _handler,
                dbConnection,
                loggerFactory.CreateLogger<HandshakingQueryHandlerDecorator<TQuery, TResult>>());
        }

        if (_useBulkhead)
        {
            if (_bulkheadPolicy == null)
            {
                throw new InvalidOperationException(
                    "Bulkhead policy is required for BulkheadQuery decorator");
            }

            _handler = new BulkheadQueryHandlerDecorator<TQuery, TResult>(
                _handler,
                _bulkheadPolicy);
        }

        return _handler;
    }
}
