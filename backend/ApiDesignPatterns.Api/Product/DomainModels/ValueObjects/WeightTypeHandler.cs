// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.SqlMappers;

namespace backend.Product.DomainModels.ValueObjects;

public class WeightTypeHandler : DecimalValueObjectTypeHandler<Weight>
{
    protected override Weight Create(decimal value) => new(value);
    protected override decimal GetDecimalValue(Weight value) => value.Value;
}

