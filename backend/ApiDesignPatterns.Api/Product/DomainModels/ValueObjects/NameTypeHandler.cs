// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.SqlMappers;

namespace backend.Product.DomainModels.ValueObjects;

public class NameTypeHandler : StringValueObjectTypeHandler<Name>
{
    protected override Name Create(string value) => new(value);
    protected override string GetStringValue(Name value) => value.ToString() ?? throw new InvalidOperationException();
}
