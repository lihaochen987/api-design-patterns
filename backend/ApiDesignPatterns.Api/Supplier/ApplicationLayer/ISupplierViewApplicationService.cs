// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;

namespace backend.Supplier.ApplicationLayer;

public interface ISupplierViewApplicationService
{
    Task<SupplierView?> GetSupplierView(long id);
}
