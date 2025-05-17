// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.User.DomainModels;

public record UserView : Identifier
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public required List<long> AddressIds { get; init; }
    public required List<long> PhoneNumberIds { get; init; }
}
