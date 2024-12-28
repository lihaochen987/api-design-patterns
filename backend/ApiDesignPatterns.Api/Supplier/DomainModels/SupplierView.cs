// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels;

public record SupplierView
{
    public long Id { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required Address Address { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public required string PhoneNumber { get; init; }
}
