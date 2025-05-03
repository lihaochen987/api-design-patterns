// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.Caching;

public record CachedItem<T>
{
    public T? Item { get; init; }
    public DateTime Timestamp { get; init; }
    public string? Hash { get; init; }
}
