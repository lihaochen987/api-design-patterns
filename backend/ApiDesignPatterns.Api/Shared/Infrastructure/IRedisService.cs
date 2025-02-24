// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using StackExchange.Redis;

namespace backend.Shared.Infrastructure;

public interface IRedisService
{
    IDatabase GetDatabase();
}
