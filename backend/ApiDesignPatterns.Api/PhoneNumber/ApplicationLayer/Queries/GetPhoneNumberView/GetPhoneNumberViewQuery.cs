// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.PhoneNumber.DomainModels;
using backend.Shared.QueryHandler;

namespace backend.PhoneNumber.ApplicationLayer.Queries.GetPhoneNumberView;

public class GetPhoneNumberViewQuery : IQuery<PhoneNumberView?>
{
    public long Id { get; init; }
}
