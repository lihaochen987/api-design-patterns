// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.Controllers;

namespace backend.Address.ApplicationLayer.Commands.UpdateAddress;

public record UpdateAddressCommand
{
    public required UpdateAddressRequest Request { get; set; }
    public required DomainModels.Address Address { get; set; }
}
