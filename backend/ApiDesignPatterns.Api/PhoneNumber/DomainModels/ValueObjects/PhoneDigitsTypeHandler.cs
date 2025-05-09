// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.SqlMappers;

namespace backend.PhoneNumber.DomainModels.ValueObjects;

public class PhoneDigitsTypeHandler : LongValueObjectTypeHandler<PhoneDigits>
{
    protected override PhoneDigits Create(long value) => new(value);
    protected override long GetDecimalValue(PhoneDigits value) => value.Value;
}
