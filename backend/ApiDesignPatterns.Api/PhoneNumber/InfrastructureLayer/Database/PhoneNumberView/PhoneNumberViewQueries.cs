// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumberView;

public class PhoneNumberViewQueries
{
    public const string GetPhoneNumberView = """
                                          SELECT
                                              supplier_id AS Id,
                                              supplier_fullname AS FullName,
                                              supplier_email AS Email,
                                              supplier_created_at AS CreatedAt
                                          FROM phone_numbers
                                          WHERE supplier_id = @Id;
                                          """;
}
