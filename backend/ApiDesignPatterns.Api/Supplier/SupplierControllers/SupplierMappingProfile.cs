// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;

namespace backend.Supplier.SupplierControllers;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<CreateSupplierRequest, DomainModels.Supplier>();
        CreateMap<DomainModels.Supplier, CreateSupplierResponse>();
        CreateMap<DomainModels.Supplier, ReplaceSupplierResponse>();
        CreateMap<ReplaceSupplierRequest, DomainModels.Supplier>();
    }
}
