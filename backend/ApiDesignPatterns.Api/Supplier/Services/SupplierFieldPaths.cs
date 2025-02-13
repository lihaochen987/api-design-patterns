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
        "address.*",
        "address.street",
        "address.city",
        "address.postalcode",
        "address.country",
        "createdat",
        "phonenumber.*",
        "phonenumber.countrycode",
        "phonenumber.areacode",
        "phonenumber.number"
    ];
}
