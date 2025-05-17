// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.User.ApplicationLayer.Commands.CreateUser;

public record CreateUserCommand
{
    public required DomainModels.User User { get; init; }
}
