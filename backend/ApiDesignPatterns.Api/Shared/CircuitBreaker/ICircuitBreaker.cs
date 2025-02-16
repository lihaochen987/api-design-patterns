// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.CircuitBreaker;

public interface ICircuitBreaker
{
    void Guard();
    void Succeed();
    void Trip();
}
