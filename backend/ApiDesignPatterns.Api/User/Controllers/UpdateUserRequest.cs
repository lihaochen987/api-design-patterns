// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.User.Controllers;

public class UpdateUserRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public List<long>? AddressIds { get; init; }
    public List<long>? PhoneNumberIds { get; init; }
    public List<string> FieldMask { get; init; } = ["*"];
}
