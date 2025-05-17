// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using backend.User.Controllers;

namespace backend.Inventory.Controllers;

public class ListProductUsersResponse
{
    [Required] public required List<GetUserResponse> Results { get; init; } = [];
    public string? NextPageToken { get; init; }
}
