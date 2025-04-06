// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.Caching;

public class BatchResult
{
    public Dictionary<string, Task<long>> HashIncrements { get; set; } = new();
}

