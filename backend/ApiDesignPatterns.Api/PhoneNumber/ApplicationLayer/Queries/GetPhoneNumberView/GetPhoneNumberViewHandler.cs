// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.PhoneNumber.DomainModels;
using backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumberView;
using backend.Shared.QueryHandler;

namespace backend.PhoneNumber.ApplicationLayer.Queries.GetPhoneNumberView;

public class GetPhoneNumberViewHandler(IPhoneNumberViewRepository repository) : IAsyncQueryHandler<GetPhoneNumberViewQuery, PhoneNumberView?>
{
    public async Task<PhoneNumberView?> Handle(GetPhoneNumberViewQuery query)
    {
        var phoneNumber = await repository.GetPhoneNumberViewAsync(query.Id);
        return phoneNumber;
    }
}
