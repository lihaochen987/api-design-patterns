// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.SqlMappers;

namespace backend.Inventory.DomainModels.ValueObjects;

public class QuantityTypeHandler : DecimalValueObjectTypeHandler<Quantity>
{
    protected override Quantity Create(decimal value) => new(value);
    protected override decimal GetDecimalValue(Quantity value) => value.Value;
}
