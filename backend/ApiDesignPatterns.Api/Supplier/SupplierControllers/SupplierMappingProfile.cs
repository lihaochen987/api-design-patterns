// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Supplier.DomainModels;

namespace backend.Supplier.SupplierControllers;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
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
    }
}
