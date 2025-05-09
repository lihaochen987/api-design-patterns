// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Address.Services;

public class AddressFieldPaths
{
    public HashSet<string> ValidPaths { get; } =
    [
        "*",
        "id",
        "supplierid",
        "fulladdress"
    ];
}
