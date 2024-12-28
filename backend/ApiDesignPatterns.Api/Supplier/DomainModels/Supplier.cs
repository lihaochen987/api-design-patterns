// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels;

public class Supplier
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required Address Address { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public required PhoneNumber PhoneNumber { get; set; }
}
