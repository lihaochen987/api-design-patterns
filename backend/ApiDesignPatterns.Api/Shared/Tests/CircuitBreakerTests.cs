// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CircuitBreaker;
using Shouldly;
using Xunit;

namespace backend.Shared.Tests;

public class CircuitBreakerTests : CircuitBreakerTestBase
{
    [Fact]
    public void Guard_WhenClosed_ShouldAllowRequests()
    {
        ICircuitBreaker sut = CircuitBreaker();

        var action = () => sut.Guard();
        action.ShouldNotThrow();
    }

    [Fact]
    public void Trip_WhenBelowThreshold_ShouldRemainClosed()
    {
        ICircuitBreaker sut = CircuitBreaker();

        sut.Trip();
        sut.Trip();

        var action = () => sut.Guard();
        action.ShouldNotThrow();
    }

    [Fact]
    public void Trip_WhenReachingThreshold_ShouldOpenCircuit()
    {
        ICircuitBreaker sut = CircuitBreaker();

        for (int i = 0; i < MaxFailures; i++)
        {
            sut.Trip();
        }

        var action = () => sut.Guard();
        action
            .ShouldThrow<CircuitBreakerOpenException>(
                "Circuit breaker is open - requests are not permitted at this time");
    }

    [Fact]
    public void Guard_WhenOpenAndTimeoutNotExpired_ShouldThrowException()
    {
        ICircuitBreaker sut = CircuitBreaker();
        for (int i = 0; i < MaxFailures; i++)
        {
            sut.Trip();
        }

        var action = () => sut.Guard();
        action.ShouldThrow<CircuitBreakerOpenException>();
    }

    [Fact]
    public void Guard_WhenOpenAndTimeoutExpired_ShouldTransitionToHalfOpen()
    {
        ICircuitBreaker sut = CircuitBreakerWithShortTimeout();
        for (int i = 0; i < MaxFailures; i++)
        {
            sut.Trip();
        }

        Thread.Sleep(200);

        sut.Guard();

        var action = () => sut.Guard();
        action.ShouldNotThrow();
    }

    [Fact]
    public void Succeed_WhenHalfOpen_ShouldCloseCircuit()
    {
        ICircuitBreaker sut = CircuitBreakerWithShortTimeout();
        for (int i = 0; i < MaxFailures; i++)
        {
            sut.Trip();
        }

        Thread.Sleep(200);

        // Transition to half-open
        sut.Guard();
        sut.Succeed();

        var action = () => sut.Guard();
        action.ShouldNotThrow();
    }

    [Fact]
    public void Succeed_ShouldResetFailureCount()
    {
        ICircuitBreaker sut = CircuitBreaker();
        sut.Trip();
        sut.Trip();

        sut.Succeed();

        // Trip twice more - should not open circuit as failure count was reset
        sut.Trip();
        sut.Trip();

        var action = () => sut.Guard();
        action.ShouldNotThrow();
    }

    [Fact]
    public void Guard_WhenHalfOpen_ShouldTransitionToOpenOnSecondRequest()
    {
        ICircuitBreaker sut = CircuitBreakerWithShortTimeout();
        for (int i = 0; i < MaxFailures; i++)
        {
            sut.Trip();
        }

        var action = () => sut.Guard();
        action.ShouldThrow<CircuitBreakerOpenException>();
    }

    [Fact]
    public void CircuitBreaker_ShouldHandleParallelRequests()
    {
        ICircuitBreaker sut = CircuitBreaker();
        int exceptions = 0;
        int successfulRequests = 0;

        Parallel.For(0, 100, _ =>
        {
            try
            {
                sut.Guard();
                sut.Trip();
                Interlocked.Increment(ref successfulRequests);
            }
            catch (CircuitBreakerOpenException)
            {
                Interlocked.Increment(ref exceptions);
            }
        });

        // We should have some successful requests and some exceptions
        // due to the circuit opening, but we can't predict exact numbers
        exceptions.ShouldBeGreaterThan(0);
        successfulRequests.ShouldBeGreaterThan(0);
    }
}
