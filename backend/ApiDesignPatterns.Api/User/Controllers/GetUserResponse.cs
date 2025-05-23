// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.User.Controllers;

public record GetUserResponse
{
    [Required] public long Id { get; init; }
    [Required] public required string FullName { get; init; }
    [Required] public required string Email { get; init; }
    [Required] public required string CreatedAt { get; init; }
    [Required] public required List<long> AddressIds { get; init; }
    [Required] public required List<long> PhoneNumberIds { get; init; }
}
