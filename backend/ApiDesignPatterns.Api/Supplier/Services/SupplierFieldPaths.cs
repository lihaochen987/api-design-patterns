// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.


namespace backend.Supplier.Services;

public class SupplierFieldPaths
{
    public HashSet<string> ValidPaths { get; } =
    [
        "*",
        "id",
        "fullname",
        "email",
        "createdat",
        "street",
        "city",
        "postalcode",
        "country",
        "phonenumber",
    ];
}
