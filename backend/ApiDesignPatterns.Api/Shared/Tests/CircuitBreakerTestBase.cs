// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CircuitBreaker;

namespace backend.Shared.Tests;

public abstract class CircuitBreakerTestBase
{
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(30);
    private readonly TimeSpan _shortTimeout = TimeSpan.FromMilliseconds(100);
    protected const int MaxFailures = 3;

    protected ICircuitBreaker CircuitBreaker()
    {
        return new CircuitBreaker.CircuitBreaker(_timeout);
    }

    protected ICircuitBreaker CircuitBreakerWithShortTimeout()
    {
        return new CircuitBreaker.CircuitBreaker(_shortTimeout);
    }
}
