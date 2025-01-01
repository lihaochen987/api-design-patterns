// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.FieldPath;

public interface IFieldPaths
{
    public HashSet<string> ValidPaths { get; }
}
