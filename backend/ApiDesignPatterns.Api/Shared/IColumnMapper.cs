// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared;

public interface IColumnMapper
{
    string MapToColumnName(string propertyName);
}
