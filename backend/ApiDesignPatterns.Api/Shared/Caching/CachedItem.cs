// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.Caching;

public class CachedItem<T>
{
    public T? Item { get; set; }
    public DateTime Timestamp { get; set; }

    public CachedItem(T item)
    {
        Item = item;
        Timestamp = DateTime.UtcNow;
    }

    // Required for deserialization
    public CachedItem()
    {
        Timestamp = DateTime.UtcNow;
    }
}
