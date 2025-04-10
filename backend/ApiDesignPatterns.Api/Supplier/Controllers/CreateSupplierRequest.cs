// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using backend.Supplier.DomainModels;

namespace backend.Supplier.Controllers;

public record CreateSupplierRequest
{
    [Required] public required string FirstName { get; init; }
    [Required] public required string LastName { get; init; }
    [Required] public required string Email { get; init; }
    [Required] public required AddressRequest Address { get; init; }
    [Required] public required PhoneNumberRequest PhoneNumber { get; init; }
}
