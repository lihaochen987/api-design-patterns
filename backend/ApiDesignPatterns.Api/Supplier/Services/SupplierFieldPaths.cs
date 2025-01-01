// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.FieldPath;

namespace backend.Supplier.Services;

public class SupplierFieldPaths : IFieldPaths
{
    public HashSet<string> ValidPaths { get; } =
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
