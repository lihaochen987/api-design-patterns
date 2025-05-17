// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.Address.Services;

public class AddressColumnMapper : IColumnMapper
{
    public string MapToColumnName(string propertyName)
    {
        return propertyName switch
        {
            "Id" => "address_id",
            "UserId" => "user_id",
            "FullAddress" => "full_address",
            _ => throw new ArgumentException($"Invalid property name: {propertyName}")
        };
    }
}
