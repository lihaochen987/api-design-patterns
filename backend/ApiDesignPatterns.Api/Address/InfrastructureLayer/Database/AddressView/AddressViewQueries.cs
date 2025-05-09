namespace backend.Address.InfrastructureLayer.Database.AddressView;

public class AddressViewQueries
{
    public const string GetAddressView = """
                                         SELECT
                                             address_id AS Id,
                                             supplier_id AS SupplierId,
                                             full_address AS FullAddress
                                         FROM addresses_view
                                         WHERE address_id = @Id;
                                         """;
}
