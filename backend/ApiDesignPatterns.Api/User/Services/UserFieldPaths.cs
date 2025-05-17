// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.


namespace backend.User.Services;

public class UserFieldPaths
{
    public HashSet<string> ValidPaths { get; } =
    [
        "*",
        "id",
        "fullname",
        "email",
        "createdat",
        "phonenumberids",
        "addressids"
    ];
}
