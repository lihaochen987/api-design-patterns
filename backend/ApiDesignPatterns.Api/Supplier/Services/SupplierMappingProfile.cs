// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Supplier.Controllers;
using backend.Supplier.DomainModels;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.Services;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        // Value objects
        CreateMap<CountryCode, string>().ConvertUsing(src => src.Value);
        CreateMap<string, CountryCode>().ConvertUsing(src => new CountryCode(src));

        CreateMap<AreaCode, string>().ConvertUsing(src => src.Value);
        CreateMap<string, AreaCode>().ConvertUsing(src => new AreaCode(src));

        CreateMap<PhoneDigits, long>().ConvertUsing(src => src.Value);
        CreateMap<long, PhoneDigits>().ConvertUsing(src => new PhoneDigits(src));

        // PhoneNumber
        CreateMap<PhoneNumberRequest, PhoneNumber>().ReverseMap();
        CreateMap<PhoneNumber, PhoneNumberResponse>().ReverseMap();

        // Address
        CreateMap<AddressRequest, Address>().ReverseMap();
        CreateMap<Address, AddressResponse>().ReverseMap();

        CreateMap<CreateSupplierRequest, DomainModels.Supplier>();
        CreateMap<DomainModels.Supplier, CreateSupplierResponse>();
        CreateMap<DomainModels.Supplier, ReplaceSupplierResponse>();
        CreateMap<ReplaceSupplierRequest, DomainModels.Supplier>();
        CreateMap<DomainModels.Supplier, UpdateSupplierResponse>();
        CreateMap<SupplierView, GetSupplierResponse>();

        CreateMap<DomainModels.Supplier, CreateSupplierRequest>();
        CreateMap<DomainModels.Supplier, ReplaceSupplierRequest>();
    }
}
