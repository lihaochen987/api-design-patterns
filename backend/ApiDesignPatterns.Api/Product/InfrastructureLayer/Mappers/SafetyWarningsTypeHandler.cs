// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.ValueObjects;
using backend.Shared.SqlMappers;

namespace backend.Product.InfrastructureLayer.Mappers;

public class SafetyWarningsTypeHandler : StringValueObjectTypeHandler<SafetyWarnings>
{
    protected override SafetyWarnings Create(string value) => new(value);
    protected override string GetStringValue(SafetyWarnings value) => value.ToString();
}

