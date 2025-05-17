// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.User.DomainModels.ValueObjects;

namespace backend.User.DomainModels;

public record User
{
    public long Id { get; init; }
    public required FirstName FirstName { get; init; }
    public required LastName LastName { get; init; }
    public required Email Email { get; init; }
    public required ICollection<long> AddressIds { get; init; } = [];
    public DateTimeOffset CreatedAt { get; init; }
    public required ICollection<long> PhoneNumberIds { get; init; } = [];
}
