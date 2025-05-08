// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.PhoneNumber.Controllers;

public class GetPhoneNumberRequest
{
    public List<string> FieldMask { get; set; } = ["*"];
}
