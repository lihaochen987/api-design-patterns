// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Shared.SqlMappers;

public abstract class DecimalValueObjectTypeHandler<T> : SqlMapper.TypeHandler<T> where T : class
{
    protected abstract T Create(decimal value);
    protected abstract decimal GetDecimalValue(T value);

    public override T Parse(object value)
    {
        if (value is decimal decimalValue)
            return Create(decimalValue);

        if (decimal.TryParse(value.ToString(), out decimal parsedValue))
            return Create(parsedValue);

        throw new InvalidCastException($"Cannot convert {value.GetType()} to {typeof(T)}");
    }

    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.Value = value == null ? DBNull.Value : GetDecimalValue(value);
    }
}
