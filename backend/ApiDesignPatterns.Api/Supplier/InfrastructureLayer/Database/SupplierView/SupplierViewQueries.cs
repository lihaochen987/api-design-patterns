// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.InfrastructureLayer.Database.SupplierView;

public static class SupplierViewQueries
{
    public const string GetSupplierView = """
                                          SELECT
                                              supplier_id AS Id,
                                              supplier_fullname AS FullName,
                                              supplier_email AS Email,
                                              supplier_created_at AS CreatedAt
                                          FROM suppliers_view
                                          WHERE supplier_id = @Id;
                                          """;

    public const string GetSupplierAddresses = """
                                               SELECT
                                                   supplier_id AS SupplierId,
                                                   supplier_address_street AS Street,
                                                   supplier_address_city AS City,
                                                   supplier_address_postal_code AS PostalCode,
                                                   supplier_address_country AS Country
                                               FROM supplier_addresses
                                               WHERE supplier_id = @Id;
                                               """;

    public const string GetSupplierPhoneNumbers = """
                                                  SELECT
                                                      supplier_id AS SupplierId,
                                                      supplier_phone_country_code AS CountryCode,
                                                      supplier_phone_area_code AS AreaCode,
                                                      supplier_phone_number AS Number
                                                  FROM supplier_phone_numbers
                                                  WHERE supplier_id = @Id;
                                                  """;

    public const string ListSuppliersBase = """
                                            SELECT
                                                supplier_id AS Id,
                                                supplier_fullname AS FullName,
                                                supplier_email AS Email,
                                                supplier_created_at AS CreatedAt,
                                                supplier_address_street AS Street,
                                                supplier_address_city AS City,
                                                supplier_address_postal_code AS PostalCode,
                                                supplier_address_country AS Country,
                                                supplier_phone_number AS PhoneNumber
                                            FROM suppliers_view
                                            WHERE 1=1
                                            """;
}
