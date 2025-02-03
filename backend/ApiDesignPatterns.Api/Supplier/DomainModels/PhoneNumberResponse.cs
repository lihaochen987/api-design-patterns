// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Supplier.DomainModels;

public record PhoneNumberResponse
{
    [Required] public required string CountryCode { get; init; }
    [Required] public required string AreaCode { get; init; }
    [Required] public required string Number { get; init; }
}
