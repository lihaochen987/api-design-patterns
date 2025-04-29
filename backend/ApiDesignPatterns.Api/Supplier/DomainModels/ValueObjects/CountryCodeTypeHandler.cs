// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.SqlMappers;

namespace backend.Supplier.DomainModels.ValueObjects;

public class CountryCodeTypeHandler : StringValueObjectTypeHandler<CountryCode>
{
    protected override CountryCode Create(string value) => new(value);
    protected override string GetStringValue(CountryCode value) => value.ToString();
}
