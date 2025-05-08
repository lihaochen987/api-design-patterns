// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.InfrastructureLayer.Database.Supplier;

public static class SupplierQueries
{
    public const string GetSupplier = """
                                      SELECT
                                          supplier_id AS Id,
                                          supplier_firstname AS FirstName,
                                          supplier_lastname AS LastName,
                                          supplier_email AS Email,
                                          supplier_created_at AS CreatedAt
                                      FROM suppliers
                                      WHERE supplier_id = @Id;
                                      """;

    public const string GetSupplierPhoneNumberIds = """
                                                    SELECT
                                                        phone_number_id
                                                    FROM phone_numbers
                                                    WHERE supplier_id = @SupplierId;
                                                    """;

    public const string GetSupplierAddressIds = """
                                                    SELECT
                                                        address_id
                                                    FROM addresses
                                                    WHERE supplier_id = @SupplierId;
                                                    """;

    public const string DeleteSupplier = """
                                                 DELETE FROM suppliers
                                                 WHERE supplier_id = @Id;
                                         """;

    public const string CreateSupplier = """
                                         INSERT INTO suppliers (
                                             supplier_firstname,
                                             supplier_lastname,
                                             supplier_email,
                                             supplier_created_at)
                                         VALUES (
                                             @FirstName,
                                             @LastName,
                                             @Email,
                                             @CreatedAt)
                                         RETURNING supplier_id;
                                         """;

    public const string CreateSupplierAddress = """
                                                INSERT INTO supplier_addresses(
                                                                               supplier_id,
                                                                               supplier_address_street,
                                                                               supplier_address_city,
                                                                               supplier_address_postal_code,
                                                                               supplier_address_country)
                                                VALUES (
                                                        @SupplierId,
                                                        @Street,
                                                        @City,
                                                        @PostalCode,
                                                        @Country)
                                                """;

    public const string CreateSupplierPhoneNumber = """
                                                    INSERT INTO phone_numbers(
                                                                                       supplier_id,
                                                                                       supplier_phone_country_code,
                                                                                       supplier_phone_area_code,
                                                                                       supplier_phone_number)
                                                    VALUES (
                                                            @SupplierId,
                                                            @CountryCode,
                                                            @AreaCode,
                                                            @Number)
                                                    """;

    public const string UpdateSupplier = """
                                         UPDATE suppliers
                                         SET
                                             supplier_firstname = @FirstName,
                                             supplier_lastname = @LastName,
                                             supplier_email = @Email
                                         WHERE supplier_id = @Id
                                         RETURNING supplier_id;
                                         """;

    public const string UpdateSupplierAddress = """
                                                UPDATE supplier_addresses
                                                SET
                                                    supplier_address_street = @Street,
                                                    supplier_address_city = @City,
                                                    supplier_address_postal_code = @PostalCode,
                                                    supplier_address_country = @Country
                                                WHERE supplier_id = @SupplierId;
                                                """;

    public const string UpdateSupplierPhoneNumberId = """
                                                      UPDATE phone_numbers
                                                      SET
                                                          supplier_id = @SupplierId
                                                      WHERE phone_number_id = @PhoneNumberId;
                                                      """;
}
