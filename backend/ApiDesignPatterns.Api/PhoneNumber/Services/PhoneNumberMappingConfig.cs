// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.PhoneNumber.Controllers;
using backend.PhoneNumber.DomainModels;
using backend.PhoneNumber.DomainModels.ValueObjects;
using Mapster;

namespace backend.PhoneNumber.Services;

public static class PhoneNumberMappingConfig
{
    public static void RegisterPhoneNumberMappings(this TypeAdapterConfig config)
    {
        // Value Objects
        config.NewConfig<CountryCode, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, CountryCode>()
            .MapWith(src => new CountryCode(src));

        config.NewConfig<AreaCode, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, AreaCode>()
            .MapWith(src => new AreaCode(src));

        config.NewConfig<PhoneDigits, long>()
            .MapWith(src => src.Value);
        config.NewConfig<long, PhoneDigits>()
            .MapWith(src => new PhoneDigits(src));

        // GetPhoneNumberController
        config.NewConfig<PhoneNumberView, GetPhoneNumberResponse>();
    }
}
