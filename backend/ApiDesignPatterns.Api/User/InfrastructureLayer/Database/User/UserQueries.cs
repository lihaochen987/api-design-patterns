// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.User.InfrastructureLayer.Database.User;

public static class UserQueries
{
    public const string GetUser = """
                                  SELECT
                                      user_id AS Id,
                                      user_firstname AS FirstName,
                                      user_lastname AS LastName,
                                      user_email AS Email,
                                      user_created_at AS CreatedAt
                                  FROM users
                                  WHERE user_id = @Id;
                                  """;

    public const string GetUserPhoneNumberIds = """
                                                SELECT
                                                    phone_number_id
                                                FROM phone_numbers
                                                WHERE user_id = @UserId;
                                                """;

    public const string GetUserAddressIds = """
                                            SELECT
                                                address_id
                                            FROM addresses
                                            WHERE user_id = @UserId;
                                            """;

    public const string DeleteUser = """
                                             DELETE FROM users
                                             WHERE user_id = @Id;
                                     """;

    public const string CreateUser = """
                                     INSERT INTO users (
                                         user_firstname,
                                         user_lastname,
                                         user_email,
                                         user_created_at,
                                         user_password_hash,
                                         user_username)
                                     VALUES (
                                         @FirstName,
                                         @LastName,
                                         @Email,
                                         @CreatedAt,
                                         @PasswordHash,
                                         @UserName)
                                     RETURNING user_id;
                                     """;

    public const string UpdateUser = """
                                     UPDATE users
                                     SET
                                         user_firstname = @FirstName,
                                         user_lastname = @LastName,
                                         user_email = @Email
                                     WHERE user_id = @Id
                                     RETURNING user_id;
                                     """;

    public const string UpdateUserPhoneNumberId = """
                                                  UPDATE phone_numbers
                                                  SET
                                                      user_id = @UserId
                                                  WHERE phone_number_id = @PhoneNumberId;
                                                  """;

    public const string UpdateOldUserPhoneNumberId = """
                                                     UPDATE phone_numbers
                                                     SET
                                                         user_id = NULL
                                                     WHERE phone_number_id = ANY(@PhoneNumberIds);
                                                     """;

    public const string UpdateUserAddressId = """
                                              UPDATE addresses
                                              SET
                                                  user_id = @UserId
                                              WHERE address_id = @AddressId;
                                              """;

    public const string UpdateOldUserAddressId = """
                                                 UPDATE addresses
                                                 SET
                                                     user_id = NULL
                                                 WHERE address_id = ANY(@AddressIds);
                                                 """;
}
