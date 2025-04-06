// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.QueryProcessor;

internal class DictionaryServiceProvider(Dictionary<Type, object> services) : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        services.TryGetValue(serviceType, out object? service);
        return service;
    }
}
