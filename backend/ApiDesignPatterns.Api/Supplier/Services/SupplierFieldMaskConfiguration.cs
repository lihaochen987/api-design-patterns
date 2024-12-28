// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.Services;

public class SupplierFieldMaskConfiguration
{
    public readonly HashSet<string> SupplierFieldPaths =
    [
        "*",
        "id",
        "fullname",
        "email",
        "address.street",
        "address.city",
        "address.postalcode",
        "address.country",
        "createdat",
        "phonenumber"
    ];
}
