// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.InfrastructureLayer.Queries;

public static class SupplierQueries
{
    public const string GetSupplier = """
                                      SELECT
                                          suppliers.supplier_id AS Id,
                                          supplier_firstname AS FirstName,
                                          supplier_lastname AS LastName,
                                          supplier_email AS Email,
                                          supplier_created_at AS CreatedAt,
                                          sa.supplier_id AS Address_SupplierId,
                                          sa.supplier_address_street AS Address_Street,
                                          sa.supplier_address_city AS Address_City,
                                          sa.supplier_address_postal_code AS Address_PostalCode,
                                          sa.supplier_address_country AS Address_Country,
                                          spn.supplier_id AS PhoneNumber_SupplierId,
                                          spn.supplier_phone_country_code AS PhoneNumber_CountryCode,
                                          spn.supplier_phone_area_code AS PhoneNumber_AreaCode,
                                          spn.supplier_phone_number AS PhoneNumber_Number
                                      FROM suppliers
                                      LEFT JOIN supplier_addresses sa ON suppliers.supplier_id = sa.supplier_id
                                      LEFT JOIN supplier_phone_numbers spn ON suppliers.supplier_id = spn.supplier_id
                                      WHERE suppliers.supplier_id = @Id;
                                      """;

    public const string DeleteSupplier = """

                                                 DELETE FROM suppliers
                                                 WHERE supplier_id = @Id;

                                         """;
}
