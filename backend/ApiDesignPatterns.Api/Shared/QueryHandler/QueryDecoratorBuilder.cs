// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Shared.QueryHandler;

public class QueryDecoratorBuilder<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> handler,
    ILoggerFactory loggerFactory,
    IDbConnection? dbConnection,
    IDatabase? redisCache)
    where TQuery : IQuery<TResult> where TResult : class
{
    private IQueryHandler<TQuery, TResult> _handler = handler;
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
    private int _maxParallelization;
    private int _maxQueuingActions;
    private bool _useRedisCache;
    private CacheStalenessOptions? _cacheStalenessOptions;

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

    public QueryDecoratorBuilder<TQuery, TResult> WithBulkhead(
        int maxParallelization,
        int maxQueuingActions)
    {
        _useBulkhead = true;
        _maxParallelization = maxParallelization;
        _maxQueuingActions = maxQueuingActions;
        return this;
    }

    public QueryDecoratorBuilder<TQuery, TResult> WithRedisCache(CacheStalenessOptions cacheStalenessOptions)
    {
        _useRedisCache = true;
        _cacheStalenessOptions = cacheStalenessOptions;
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

        if (_useHandshaking && dbConnection != null)
        {
            _handler = new HandShakingQueryHandlerDecorator<TQuery, TResult>(
                _handler,
                dbConnection,
                loggerFactory.CreateLogger<HandShakingQueryHandlerDecorator<TQuery, TResult>>());
        }

        if (_useBulkhead)
        {
            _handler = new BulkheadQueryHandlerDecorator<TQuery, TResult>(
                _handler,
                loggerFactory.CreateLogger<BulkheadQueryHandlerDecorator<TQuery, TResult>>(),
                _maxParallelization,
                _maxQueuingActions);
        }

        if (_useRedisCache)
        {
            if (redisCache == null)
            {
                throw new InvalidOperationException("Redis connection is required for RedisCachingQuery decorator");
            }

            if (_cacheStalenessOptions == null)
            {
                throw new InvalidOperationException(
                    "Cache staleness options is required for RedisCachingQuery decorator");
            }

            _handler = new RedisCacheQueryHandlerDecorator<TQuery, TResult>(
                _handler,
                redisCache,
                loggerFactory.CreateLogger<RedisCacheQueryHandlerDecorator<TQuery, TResult>>(),
                _cacheStalenessOptions
            );
        }

        return _handler;
    }
}
