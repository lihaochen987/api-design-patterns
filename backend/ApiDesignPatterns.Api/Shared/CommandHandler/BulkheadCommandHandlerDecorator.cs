// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.Bulkhead;

namespace backend.Shared.CommandHandler;

public class BulkheadCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    AsyncBulkheadPolicy bulkheadPolicy)
    : ICommandHandler<TCommand>
{
    public async Task Handle(TCommand command)
    {
        await bulkheadPolicy.ExecuteAsync(async () =>
            await commandHandler.Handle(command));
    }
}
