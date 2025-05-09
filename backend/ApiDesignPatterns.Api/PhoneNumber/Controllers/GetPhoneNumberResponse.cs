// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.PhoneNumber.Controllers;

public record GetPhoneNumberResponse
{
    [Required] public required string Id { get; init; }
    [Required] public required string SupplierId { get; init; }
    [Required] public required string PhoneNumber { get; init; }
}
