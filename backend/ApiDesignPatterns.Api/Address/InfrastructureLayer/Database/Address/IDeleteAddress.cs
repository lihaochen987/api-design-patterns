// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Address.InfrastructureLayer.Database.Address;

public interface IDeleteAddress
{
    Task DeleteAddress(long id);
}
