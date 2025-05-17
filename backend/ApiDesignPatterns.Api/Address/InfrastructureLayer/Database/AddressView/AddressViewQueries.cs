namespace backend.Address.InfrastructureLayer.Database.AddressView;

public class AddressViewQueries
{
    public const string GetAddressView = """
                                         SELECT
                                             address_id AS Id,
                                             user_id AS UserId,
                                             full_address AS FullAddress
                                         FROM addresses_view
                                         WHERE address_id = @Id;
                                         """;

    public const string ListAddressBase = """
                                            SELECT
                                                address_id AS Id,
                                                user_id AS UserId,
                                                full_address AS FullAddress
                                            FROM addresses_view
                                            WHERE 1=1
                                            """;
}
