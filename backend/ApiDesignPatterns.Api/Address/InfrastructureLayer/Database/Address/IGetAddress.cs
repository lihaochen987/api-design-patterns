// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Address.InfrastructureLayer.Database.Address;

public interface IGetAddress
{
    Task<DomainModels.Address?> GetAddress(long id);
}
