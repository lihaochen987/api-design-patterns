// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.Supplier.Services;

public class ProductColumnMapper : IColumnMapper
{
    public string MapToColumnName(string propertyName)
    {
        return propertyName switch
        {
            "Id" => "supplier_id",
            "FullName" => "supplier_fullname",
            "Email" => "supplier_email",
            "CreatedAt" => "supplier_created_at",
            "Street" => "supplier_address_street",
            "City" => "supplier_address_city",
            "PostalCode" => "supplier_address_postal_code",
            "Country" => "supplier_address_country",
            "PhoneNumber" => "supplier_phone_number",
            _ => throw new ArgumentException($"Invalid property name: {propertyName}")
        };
    }
}
