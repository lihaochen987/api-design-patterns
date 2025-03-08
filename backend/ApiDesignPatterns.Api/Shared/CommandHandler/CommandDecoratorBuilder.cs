// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Polly.Bulkhead;

namespace backend.Shared.CommandHandler;

public class CommandDecoratorBuilder<TCommand>(
    ICommandHandler<TCommand> handler,
    IDbConnection? dbConnection,
    ILoggerFactory loggerFactory)
{
    private ICommandHandler<TCommand> _handler = handler;
    private bool _useCircuitBreaker;
    private bool _useTransaction;
    private bool _useAudit;
    private bool _useLogging;
    private bool _useHandshaking;
    private TimeSpan _durationOfBreak;
    private int _exceptionsAllowedBeforeBreaking;
    private bool _useTimeout;
    private TimeSpan _timeout;
    private bool _useBulkhead;
    private AsyncBulkheadPolicy? _bulkheadPolicy;

    public CommandDecoratorBuilder<TCommand> WithCircuitBreaker(
        TimeSpan durationOfBreak,
        int exceptionsAllowedBeforeBreaking)
    {
        _useCircuitBreaker = true;
        _durationOfBreak = durationOfBreak;
        _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
        return this;
    }

    public CommandDecoratorBuilder<TCommand> WithTransaction()
    {
        _useTransaction = true;
        return this;
    }

    public CommandDecoratorBuilder<TCommand> WithAudit()
    {
        _useAudit = true;
        return this;
    }

    public CommandDecoratorBuilder<TCommand> WithLogging()
    {
        _useLogging = true;
        return this;
    }

    public CommandDecoratorBuilder<TCommand> WithHandshaking()
    {
        _useHandshaking = true;
        return this;
    }

    public CommandDecoratorBuilder<TCommand> WithTimeout(TimeSpan timeout)
    {
        _useTimeout = true;
        _timeout = timeout;
        return this;
    }

    public CommandDecoratorBuilder<TCommand> WithBulkhead(AsyncBulkheadPolicy policy)
    {
        _useBulkhead = true;
        _bulkheadPolicy = policy;
        return this;
    }

    public ICommandHandler<TCommand> Build()
    {
        if (_useAudit)
        {
            if (dbConnection == null)
            {
                throw new InvalidOperationException("Database connection is required for audit decorator");
            }

            _handler = new AuditCommandHandlerDecorator<TCommand>(_handler, dbConnection);
        }

        if (_useLogging)
        {
            _handler = new LoggingCommandHandlerDecorator<TCommand>(
                _handler,
                loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<TCommand>>());
        }

        if (_useTransaction)
        {
            if (dbConnection == null)
            {
                throw new InvalidOperationException("Database connection is required for transaction decorator");
            }

            _handler = new TransactionCommandHandlerDecorator<TCommand>(_handler, dbConnection);
        }

        if (_useHandshaking)
        {
            if (dbConnection == null)
            {
                throw new InvalidOperationException("Database connection is required for handshaking decorator");
            }

            _handler = new HandshakingCommandHandlerDecorator<TCommand>(
                _handler,
                dbConnection,
                loggerFactory.CreateLogger<HandshakingCommandHandlerDecorator<TCommand>>());
        }

        if (_useCircuitBreaker)
        {
            _handler = new CircuitBreakerCommandHandlerDecorator<TCommand>(
                _handler,
                loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<TCommand>>(),
                _durationOfBreak,
                _exceptionsAllowedBeforeBreaking);
        }

        if (_useTimeout)
        {
            _handler = new TimeoutCommandHandlerDecorator<TCommand>(
                _handler,
                loggerFactory.CreateLogger<TimeoutCommandHandlerDecorator<TCommand>>(),
                _timeout);
        }

        if (_useBulkhead)
        {
            if (_bulkheadPolicy == null)
            {
                throw new InvalidOperationException(
                    "Bulkhead policy is required for BulkheadCommand decorator");
            }

            _handler = new BulkheadCommandHandlerDecorator<TCommand>(
                _handler,
                _bulkheadPolicy);
        }

        return _handler;
    }
}
