// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.Utility;

public static class JitterUtility
{
    public static TimeSpan AddJitter(TimeSpan original, double jitterPercentage = 0.5)
    {
        double jitterMultiplier = 1 + (Random.Shared.NextDouble() * 2 - 1) * jitterPercentage;

        return TimeSpan.FromMilliseconds(original.TotalMilliseconds * jitterMultiplier);
    }
}
