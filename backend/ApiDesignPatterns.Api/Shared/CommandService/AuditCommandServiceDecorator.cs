// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Shared.CommandService;

public class AuditCommandServiceDecorator<TCommand>(
    ICommandService<TCommand> commandService,
    IDbConnection dbConnection)
    : ICommandService<TCommand>
{
    public async Task Execute(TCommand command)
    {
        await commandService.Execute(command);
        await AppendToAuditTrail(command);
    }

    private async Task AppendToAuditTrail(TCommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        string operation = command.GetType().Name;
        string data = Newtonsoft.Json.JsonConvert.SerializeObject(command);
        var timestamp = DateTime.UtcNow;

        const string query =
            "INSERT INTO audit (audit_timestamp, operation, data) VALUES (@Timestamp, @Operation, @Data);";

        await dbConnection.ExecuteAsync(query, new { Timestamp = timestamp, Operation = operation, Data = data });
    }
}
