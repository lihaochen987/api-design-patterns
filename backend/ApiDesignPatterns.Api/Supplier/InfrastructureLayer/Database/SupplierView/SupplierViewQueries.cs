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

    public const string GetPhoneNumbers = """
                                          SELECT phone_number_id
                                          FROM phone_numbers_view spn
                                          WHERE spn.supplier_id = @Id;
                                          """;

    public const string GetAddresses = """
                                       SELECT address_id
                                       FROM addresses_view av
                                       WHERE av.supplier_id = @Id;
                                       """;

    public const string ListSuppliersBase = """
                                            SELECT
                                                supplier_id AS Id,
                                                supplier_fullname AS FullName,
                                                supplier_email AS Email,
                                                supplier_created_at AS CreatedAt
                                            FROM suppliers_view
                                            WHERE 1=1
                                            """;

    public const string GetPhoneNumbersForMultipleSuppliers = """
                                                              SELECT
                                                                  supplier_id,
                                                                  phone_number_id
                                                              FROM phone_numbers_view
                                                              WHERE supplier_id = ANY(@SupplierIds)
                                                              """;

    public const string GetAddressesForMultipleSuppliers = """
                                                           SELECT
                                                               supplier_id,
                                                               address_id
                                                           FROM addresses_view
                                                           WHERE supplier_id = ANY(@SupplierIds)
                                                           """;
}
