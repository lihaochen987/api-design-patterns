// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared;

internal static class RandomUtility
{
    private static readonly Random s_random = new();

    public static bool CheckProbability(double probability)
    {
        lock (s_random)
        {
            return s_random.NextDouble() < probability;
        }
    }
}
