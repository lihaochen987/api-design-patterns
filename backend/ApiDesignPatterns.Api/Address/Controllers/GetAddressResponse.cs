// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Address.Controllers;

public class GetAddressResponse
{
    [Required] public required string Id { get; init; }
    [Required] public required string UserId { get; init; }
    [Required] public required string FullAddress { get; init; }
}
