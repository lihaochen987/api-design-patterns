// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.Caching;

public record CacheStalenessOptions(
    TimeSpan Ttl,
    double CheckRate,
    double MinAcceptableRate,
    double MaxAcceptableRate);
