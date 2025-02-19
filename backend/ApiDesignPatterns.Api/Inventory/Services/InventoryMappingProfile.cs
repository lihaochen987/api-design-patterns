// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.DomainModels;
using backend.Inventory.InventoryControllers;

namespace backend.Inventory.Services;

public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        CreateMap<CreateInventoryRequest, DomainModels.Inventory>();
        CreateMap<DomainModels.Inventory, CreateInventoryResponse>();
        CreateMap<InventoryView, GetInventoryResponse>();
    }
}
