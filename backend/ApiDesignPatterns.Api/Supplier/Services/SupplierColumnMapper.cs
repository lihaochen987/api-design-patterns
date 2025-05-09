// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.Supplier.Services;

public class SupplierColumnMapper : IColumnMapper
{
    public string MapToColumnName(string propertyName)
    {
        return propertyName switch
        {
            "Id" => "supplier_id",
            "FullName" => "supplier_fullname",
            "Email" => "supplier_email",
            "CreatedAt" => "supplier_created_at",
            _ => throw new ArgumentException($"Invalid property name: {propertyName}")
        };
    }
}
