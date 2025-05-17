// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.User.Controllers;

namespace backend.User.ApplicationLayer.Commands.UpdateUser;

public record UpdateUserCommand
{
    public required UpdateUserRequest Request { get; init; }
    public required DomainModels.User User { get; init; }
}
