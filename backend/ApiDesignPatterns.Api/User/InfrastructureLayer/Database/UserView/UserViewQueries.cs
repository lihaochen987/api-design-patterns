// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.User.InfrastructureLayer.Database.UserView;

public static class UserViewQueries
{
    public const string GetUserView = """
                                          SELECT
                                              user_id AS Id,
                                              user_fullname AS FullName,
                                              user_email AS Email,
                                              user_created_at AS CreatedAt
                                          FROM users_view
                                          WHERE user_id = @Id;
                                          """;

    public const string GetPhoneNumbers = """
                                          SELECT phone_number_id
                                          FROM phone_numbers_view spn
                                          WHERE spn.user_id = @Id;
                                          """;

    public const string GetAddresses = """
                                       SELECT address_id
                                       FROM addresses_view av
                                       WHERE av.user_id = @Id;
                                       """;

    public const string ListUsersBase = """
                                            SELECT
                                                user_id AS Id,
                                                user_fullname AS FullName,
                                                user_email AS Email,
                                                user_created_at AS CreatedAt
                                            FROM users_view
                                            WHERE 1=1
                                            """;

    public const string GetPhoneNumbersForMultipleUsers = """
                                                              SELECT
                                                                  user_id,
                                                                  phone_number_id
                                                              FROM phone_numbers_view
                                                              WHERE user_id = ANY(@UserIds)
                                                              """;

    public const string GetAddressesForMultipleUsers = """
                                                           SELECT
                                                               user_id,
                                                               address_id
                                                           FROM addresses_view
                                                           WHERE user_id = ANY(@UserIds)
                                                           """;
}
