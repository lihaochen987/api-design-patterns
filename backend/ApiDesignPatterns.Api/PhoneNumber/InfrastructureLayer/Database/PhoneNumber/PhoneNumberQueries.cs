// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumber;

public class PhoneNumberQueries
{
    public const string GetPhoneNumber = """
                                             SELECT
                                                 phone_number_id AS Id,
                                                 user_id AS UserId,
                                                 phone_number_country_code AS CountryCode,
                                                 phone_number_area_code AS AreaCode,
                                                 phone_number_digits AS Number
                                             FROM phone_numbers
                                             WHERE phone_number_id = @Id;
                                             """;

    public const string DeletePhoneNumber = """
                                            DELETE FROM phone_numbers
                                            WHERE phone_number_id = @Id;
                                            """;
}
