// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Address.Controllers;

public class UpdateAddressRequest
{
    public long? UserId { get; init; }
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public List<string> FieldMask { get; init; } = ["*"];
}
