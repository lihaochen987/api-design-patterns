// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Shared.SqlMappers;

public abstract class StringValueObjectTypeHandler<T> : SqlMapper.TypeHandler<T> where T : struct
{
    protected abstract T Create(string value);

    protected abstract string GetStringValue(T value);

    public override T Parse(object value)
    {
        return Create((string)value);
    }

    public override void SetValue(IDbDataParameter parameter, T value)
    {
        parameter.Value = GetStringValue(value);
    }
}
