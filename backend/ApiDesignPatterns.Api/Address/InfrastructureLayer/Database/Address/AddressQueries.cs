// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Address.InfrastructureLayer.Database.Address;

public class AddressQueries
{
    public const string GetAddress = """
                                     SELECT
                                         address_id as Id,
                                         supplier_id as SupplierId,
                                         address_street as Street,
                                         address_city as City,
                                         address_postal_code as PostalCode,
                                         address_country AS Country
                                     FROM addresses
                                     WHERE address_id = @id
                                     """;

    public const string DeleteAddress = """
                                        DELETE FROM addresses
                                        WHERE address_id = @id
                                        """;
}
