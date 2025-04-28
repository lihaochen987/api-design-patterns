// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.SqlMappers;

namespace backend.Review.DomainModels.ValueObjects;

public class RatingTypeHandler : DecimalValueObjectTypeHandler<Rating>
{
    protected override Rating Create(decimal value) => new(value);
    protected override decimal GetDecimalValue(Rating value) => value.Value;
}
