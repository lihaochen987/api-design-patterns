// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;

namespace backend.Shared.CircuitBreaker;

public class CircuitBreaker(TimeSpan timeout, int maxFailures = 3) : ICircuitBreaker
{
    private readonly object _syncLock = new();
    private volatile CircuitState _state = CircuitState.Closed;
    private int _failureCount;
    private DateTime? _lastFailureTime;

    public void Guard()
    {
        lock (_syncLock)
        {
            switch (_state)
            {
                case CircuitState.Open:
                    if (ShouldAttemptReset())
                    {
                        _state = CircuitState.HalfOpen;
                    }
                    else
                    {
                        throw new CircuitBreakerOpenException(
                            "Circuit breaker is open - requests are not permitted at this time");
                    }

                    break;

                case CircuitState.HalfOpen:
                    // Only allow one request through in half-open state
                    _state = CircuitState.Open;
                    break;

                case CircuitState.Closed:
                    // Normal operation, allow request through
                    break;
            }
        }
    }

    public void Succeed()
    {
        lock (_syncLock)
        {
            _state = CircuitState.Closed;
            _failureCount = 0;
            _lastFailureTime = null;
        }
    }

    public void Trip()
    {
        lock (_syncLock)
        {
            _failureCount++;
            _lastFailureTime = DateTime.UtcNow;

            if (_failureCount >= maxFailures)
            {
                _state = CircuitState.Open;
            }
        }
    }

    private bool ShouldAttemptReset()
    {
        return _lastFailureTime.HasValue &&
               DateTime.UtcNow - _lastFailureTime.Value >= timeout;
    }
}
