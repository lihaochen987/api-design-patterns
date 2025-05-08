// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumberView;

public interface IPhoneNumberViewRepository
{
    Task<DomainModels.PhoneNumberView?> GetPhoneNumberViewAsync(long id);
}
