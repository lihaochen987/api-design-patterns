// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Supplier.SupplierControllers;

public record GetSupplierResponse
{
    [Required] public long Id { get; init; }
    [Required] public required string FullName { get; init; }
    [Required] public required string Email { get; init; }
    [Required] public required string CreatedAt { get; init; }
    [Required] public required string Street { get; init; }
    [Required] public required string City { get; init; }
    [Required] public required string PostalCode { get; init; }
    [Required] public required string Country { get; init; }
    [Required] public required string PhoneNumber { get; init; }
}
