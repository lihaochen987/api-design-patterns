// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Shared.CommandHandler;

public class HandshakingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    IDbConnection dbConnection,
    ILogger<HandshakingCommandHandlerDecorator<TCommand>> logger)
    : ICommandHandler<TCommand>
{
    private const string SynHandshakeQuery =
        "SELECT pg_backend_pid() as ServerSequence, " +
        "(SELECT COUNT(*) < (setting::int * 0.8) FROM pg_stat_activity, pg_settings " +
        "WHERE pg_settings.name = 'max_connections' AND state = 'active' group by setting) as IsReady";

    private const string AckHandshakeQuery =
        "SELECT @ClientSequence IS NOT NULL AND @ServerSequence = pg_backend_pid() as ConnectionEstablished";

    public async Task Handle(TCommand command)
    {
        string clientSequence = Guid.NewGuid().ToString();
        logger.LogDebug("Initiating database handshake with sequence {ClientSequence}", clientSequence);

        try
        {
            // Step 1 (SYN): Client initiates handshake with sequence identifier (clientSequence)
            // Step 2 (SYN+ACK): Server acknowledges and responds with its own sequence (serverSequence)
            (int serverSequence, bool isReady) =
                await dbConnection.QueryFirstOrDefaultAsync<(int ServerSequence, bool IsReady)>(SynHandshakeQuery);

            if (!isReady)
            {
                logger.LogWarning("Database rejected handshake during SYN - at capacity");
                throw new DatabaseNotAvailableException("Database is at capacity and cannot accept more connections");
            }

            // Step 3 (ACK): Client confirms the handshake by acknowledging server's sequence
            bool ackResult = await dbConnection.QueryFirstOrDefaultAsync<bool>(AckHandshakeQuery,
                new { ClientSequence = clientSequence, ServerSequence = serverSequence });

            if (!ackResult)
            {
                logger.LogWarning("Database handshake failed at ACK phase");
                throw new DatabaseNotAvailableException("Failed to establish reliable connection with database");
            }

            logger.LogDebug("Handshake successful, connection established");

            await commandHandler.Handle(command);
        }
        catch (Exception ex) when (ex is not DatabaseNotAvailableException)
        {
            logger.LogError("Database handshake failed with error: {Error}", ex.Message);
            throw new DatabaseNotAvailableException("Database handshake failed");
        }
    }
}
