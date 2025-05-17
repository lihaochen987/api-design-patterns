// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.User.Services;

public class UserColumnMapper : IColumnMapper
{
    public string MapToColumnName(string propertyName)
    {
        return propertyName switch
        {
            "Id" => "user_id",
            "FullName" => "user_fullname",
            "Email" => "user_email",
            "CreatedAt" => "user_created_at",
            _ => throw new ArgumentException($"Invalid property name: {propertyName}")
        };
    }
}
