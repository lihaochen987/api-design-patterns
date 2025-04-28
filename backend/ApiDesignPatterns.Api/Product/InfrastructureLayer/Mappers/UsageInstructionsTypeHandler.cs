// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.ValueObjects;
using backend.Shared.SqlMappers;

namespace backend.Product.InfrastructureLayer.Mappers;

public class UsageInstructionsTypeHandler : StringValueObjectTypeHandler<UsageInstructions>
{
    protected override UsageInstructions Create(string value) => new(value);
    protected override string GetStringValue(UsageInstructions value) => value.ToString();
}

