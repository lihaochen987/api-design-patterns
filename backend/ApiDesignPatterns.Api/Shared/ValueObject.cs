// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared;

public abstract class ValueObject<T> where T : ValueObject<T>
{
    protected abstract IEnumerable<object> GetAttributesToIncludeInEqualityCheck();

    public override bool Equals(object? other)
    {
        return Equals(other as T ?? throw new InvalidOperationException());
    }

    public bool Equals(T other)
    {
        return GetAttributesToIncludeInEqualityCheck()
            .SequenceEqual(other.GetAttributesToIncludeInEqualityCheck());
    }

    public static bool operator ==(ValueObject<T> left, ValueObject<T>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return GetAttributesToIncludeInEqualityCheck()
            .Aggregate(17, (current, obj) => current * 31 + obj.GetHashCode());
    }
}
