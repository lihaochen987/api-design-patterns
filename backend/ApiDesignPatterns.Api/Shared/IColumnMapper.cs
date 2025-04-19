// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared;

/// <summary>
/// Maps between class fields and column names. Primary use is for filtering in list queries.
/// </summary>
public interface IColumnMapper
{
    string MapToColumnName(string propertyName);
}
