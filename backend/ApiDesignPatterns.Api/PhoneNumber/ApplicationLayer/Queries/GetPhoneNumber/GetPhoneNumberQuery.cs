// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.PhoneNumber.ApplicationLayer.Queries.GetPhoneNumber;

public class GetPhoneNumberQuery : IQuery<DomainModels.PhoneNumber?>
{
    public long Id { get; init; }
}
