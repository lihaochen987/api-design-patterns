// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using StackExchange.Redis;

namespace backend.Shared.Caching;

public interface ICache<T> where T : class
{
    Task<T?> GetAsync(string key);
    Task SetAsync(string key, T value, TimeSpan expiry);
    IBatch CreateBatch();
}
