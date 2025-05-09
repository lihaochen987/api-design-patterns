// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Address.DomainModels;

public class AddressView
{
    public long Id { get; init; }
    public long SupplierId { get; init; }
    public required string FullAddress { get; init; }
}
