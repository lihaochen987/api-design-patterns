// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.Address.DomainModels;

public record AddressView : Identifier
{
    public long SupplierId { get; init; }
    public required string FullAddress { get; init; }
}
