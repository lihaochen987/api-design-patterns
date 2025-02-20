// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.Bulkhead;

namespace backend.Shared.CommandHandler;

public class BulkheadCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    ILogger<BulkheadCommandHandlerDecorator<TCommand>> logger,
    int maxParallelization,
    int maxQueuingActions)
    : ICommandHandler<TCommand>
{
    private readonly AsyncBulkheadPolicy _bulkheadPolicy = Policy
        .BulkheadAsync(
            maxParallelization: maxParallelization,
            maxQueuingActions: maxQueuingActions,
            onBulkheadRejectedAsync: async _ =>
            {
                logger.LogError("Bulkhead rejected execution - max concurrent operations ({Max}) reached", maxParallelization);
                await Task.CompletedTask;
            });

    public async Task Handle(TCommand command)
    {
        await _bulkheadPolicy.ExecuteAsync(async () =>
            await commandHandler.Handle(command));
    }
}
