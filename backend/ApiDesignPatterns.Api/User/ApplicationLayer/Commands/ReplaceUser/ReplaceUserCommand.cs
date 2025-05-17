// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.User.ApplicationLayer.Commands.ReplaceUser;

public record ReplaceUserCommand
{
    public required DomainModels.User User { get; init; }
    public required long UserId { get; init; }
}
