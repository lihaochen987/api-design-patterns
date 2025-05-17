// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.User.Controllers;

public record GetUserRequest
{
    public List<string> FieldMask { get; set; } = ["*"];
}
