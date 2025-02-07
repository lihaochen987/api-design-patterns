// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using backend.Supplier.SupplierControllers;

namespace backend.Supplier.ApplicationLayer;

public interface ISupplierViewApplicationService
{
    Task<(List<SupplierView>, string?)> ListSuppliersAsync(ListSuppliersRequest request);
}
