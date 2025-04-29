// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Supplier.DomainModels.ValueObjects;

public class AddressResponse
{
    [Required] public required string Street { get; init; }
    [Required] public required string City { get; init; }
    [Required] public required string PostalCode { get; init; }
    [Required] public required string Country { get; init; }
}
