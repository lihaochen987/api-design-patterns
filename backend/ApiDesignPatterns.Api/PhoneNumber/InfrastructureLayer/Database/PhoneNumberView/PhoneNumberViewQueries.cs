// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumberView;

public class PhoneNumberViewQueries
{
    public const string GetPhoneNumberView = """
                                             SELECT
                                                 phone_number_id AS Id,
                                                 supplier_id AS SupplierId,
                                                 phone_number AS PhoneNumber
                                             FROM phone_numbers_view
                                             WHERE phone_number_id = @Id;
                                             """;
}
