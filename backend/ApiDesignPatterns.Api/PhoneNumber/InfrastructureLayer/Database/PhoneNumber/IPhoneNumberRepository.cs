// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumber;

public interface IPhoneNumberRepository
{
    Task<DomainModels.PhoneNumber?> GetPhoneNumber(long id);
    Task DeletePhoneNumber(long id);
}
