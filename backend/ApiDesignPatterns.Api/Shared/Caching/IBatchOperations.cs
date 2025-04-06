// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.Caching;

public interface IBatchOperations
{
    void HashIncrementAsync(string key, string field);
}
