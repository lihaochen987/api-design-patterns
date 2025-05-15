// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Address.Controllers;

public class ListAddressResponse
{
    [Required] public required IEnumerable<GetAddressResponse?> Results { get; init; } = [];
    public string? NextPageToken { get; init; }
}
