// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.User.ApplicationLayer.Commands.DeleteUser;

public record DeleteUserCommand
{
    public long Id { get; init; }
}
