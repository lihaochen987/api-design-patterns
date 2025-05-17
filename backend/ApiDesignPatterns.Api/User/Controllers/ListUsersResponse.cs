// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.User.Controllers;

public record ListUsersResponse
{
    [Required] public required IEnumerable<GetUserResponse?> Results { get; init; } = [];
    public string? NextPageToken { get; init; }
}
