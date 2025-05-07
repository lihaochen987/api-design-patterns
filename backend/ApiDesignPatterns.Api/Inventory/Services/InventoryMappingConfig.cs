// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Mapster;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;

namespace backend.Inventory.Services;

public static class InventoryMappingConfig
{
    public static void RegisterInventoryMappings(this TypeAdapterConfig config)
    {
        config.NewConfig<CreateInventoryRequest, DomainModels.Inventory>();
        config.NewConfig<DomainModels.Inventory, CreateInventoryResponse>();
        config.NewConfig<InventoryView, GetInventoryResponse>();
        config.NewConfig<DomainModels.Inventory, CreateInventoryRequest>();
        config.NewConfig<DomainModels.Inventory, UpdateInventoryResponse>();
    }
}
