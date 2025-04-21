// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Views;
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

        // From Supplier domain
        CreateMap<SupplierView, GetSupplierResponse>();

        // From Product domain
        CreateMap<ProductView, GetProductResponse>();
        CreateMap<ProductView, GetPetFoodResponse>()
            .IncludeBase<ProductView, GetProductResponse>();
        CreateMap<ProductView, GetGroomingAndHygieneResponse>()
            .IncludeBase<ProductView, GetProductResponse>();
    }
}
