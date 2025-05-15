// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Address.ApplicationLayer.Commands.ReplaceAddress;

public record ReplaceAddressCommand
{
    public required DomainModels.Address Address { get; init; }
}
