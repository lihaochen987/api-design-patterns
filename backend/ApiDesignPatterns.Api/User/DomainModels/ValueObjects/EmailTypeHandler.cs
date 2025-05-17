// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.SqlMappers;

namespace backend.User.DomainModels.ValueObjects;

public class EmailTypeHandler : StringValueObjectTypeHandler<Email>
{
    protected override Email Create(string value) => new(value);
    protected override string GetStringValue(Email value) => value.ToString();
}
