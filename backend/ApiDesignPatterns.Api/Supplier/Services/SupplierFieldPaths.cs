// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.Services;

public class SupplierFieldPaths
{
    public readonly HashSet<string> ValidPaths =
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
