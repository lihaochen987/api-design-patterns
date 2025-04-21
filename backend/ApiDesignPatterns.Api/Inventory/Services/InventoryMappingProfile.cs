// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Supplier.Controllers;
using backend.Supplier.DomainModels;

namespace backend.Inventory.Services;

public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        CreateMap<CreateInventoryRequest, DomainModels.Inventory>();
        CreateMap<DomainModels.Inventory, CreateInventoryResponse>();
        CreateMap<InventoryView, GetInventoryResponse>();
        CreateMap<DomainModels.Inventory, CreateInventoryRequest>();
        CreateMap<DomainModels.Inventory, UpdateInventoryResponse>();

        // From other domains
        CreateMap<SupplierView, GetSupplierResponse>();
    }
}
