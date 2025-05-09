// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.SqlMappers;

namespace backend.Address.DomainModels.ValueObjects;

public class StreetTypeHandler : StringValueObjectTypeHandler<Street>
{
    protected override Street Create(string value) => new(value);
    protected override string GetStringValue(Street value) => value.ToString();
}
