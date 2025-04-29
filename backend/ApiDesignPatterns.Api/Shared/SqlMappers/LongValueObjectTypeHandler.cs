// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Shared.SqlMappers;

public abstract class LongValueObjectTypeHandler<T> : SqlMapper.TypeHandler<T> where T : class
{
    protected abstract T Create(long value);
    protected abstract long GetDecimalValue(T value);

    public override T Parse(object value)
    {
        if (value is long decimalValue)
            return Create(decimalValue);

        if (long.TryParse(value.ToString(), out long parsedValue))
            return Create(parsedValue);

        throw new InvalidCastException($"Cannot convert {value.GetType()} to {typeof(T)}");
    }

    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.Value = value == null ? DBNull.Value : GetDecimalValue(value);
    }
}

