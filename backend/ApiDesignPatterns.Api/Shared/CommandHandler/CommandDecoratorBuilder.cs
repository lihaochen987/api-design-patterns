// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;

namespace backend.Shared.CommandHandler;

public class CommandDecoratorBuilder<TCommand>(
    ICommandHandler<TCommand> handler,
    IDbConnection dbConnection,
    ILoggerFactory loggerFactory)
{
    private ICommandHandler<TCommand> _handler = handler;
    private bool _useCircuitBreaker;
    private bool _useTransaction;
    private bool _useAudit;
    private bool _useLogging;

    public CommandDecoratorBuilder<TCommand> WithCircuitBreaker(TimeSpan? timeout = null)
    {
        _useCircuitBreaker = true;
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

    public ICommandHandler<TCommand> Build()
    {
        if (_useAudit)
        {
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
            _handler = new TransactionCommandHandlerDecorator<TCommand>(_handler, dbConnection);
        }

        if (_useCircuitBreaker && _useTransaction)
        {
            _handler = new CircuitBreakerCommandHandlerDecorator<TCommand>(
                _handler,
                loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<TCommand>>());
        }

        return _handler;
    }
}
