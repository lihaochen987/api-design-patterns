// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using backend.Supplier.DomainModels;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.Controllers;

public record ReplaceSupplierResponse
{
    [Required] public required string Id { get; init; }
    [Required] public required string FirstName { get; init; }
    [Required] public required string LastName { get; init; }
    [Required] public required string Email { get; init; }
    [Required] public required AddressResponse Address { get; init; }
    [Required] public required string CreatedAt { get; init; }
    [Required] public required PhoneNumberResponse PhoneNumber { get; init; }
}
