// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.SqlFilter;

public interface ISqlFilterParser
{
    public List<string> Tokenize(string filter);
    public string GenerateWhereClause(List<string> tokens);
}
