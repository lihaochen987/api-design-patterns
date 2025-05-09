// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.PhoneNumber.Controllers;
using backend.PhoneNumber.DomainModels;
using Mapster;

namespace backend.PhoneNumber.Services;

public static class PhoneNumberMappingConfig
{
    public static void RegisterPhoneNumberMappings(this TypeAdapterConfig config)
    {
        // GetPhoneNumberController
        config.NewConfig<PhoneNumberView, GetPhoneNumberResponse>();
    }
}

