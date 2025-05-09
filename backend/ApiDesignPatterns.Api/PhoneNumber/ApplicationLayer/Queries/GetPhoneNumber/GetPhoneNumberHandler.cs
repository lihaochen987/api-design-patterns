// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumber;
using backend.Shared.QueryHandler;

namespace backend.PhoneNumber.ApplicationLayer.Queries.GetPhoneNumber;

public class GetPhoneNumberHandler(IPhoneNumberRepository repository)
    : IAsyncQueryHandler<GetPhoneNumberQuery, DomainModels.PhoneNumber?>
{
    public async Task<DomainModels.PhoneNumber?> Handle(GetPhoneNumberQuery query)
    {
        var phoneNumber = await repository.GetPhoneNumber(query.Id);
        return phoneNumber;
    }
}
