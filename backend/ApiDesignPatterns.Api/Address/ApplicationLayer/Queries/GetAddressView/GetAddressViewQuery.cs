using backend.Address.DomainModels;
using backend.Shared.QueryHandler;

namespace backend.Address.ApplicationLayer.Queries.GetAddressView;

public class GetAddressViewQuery : IQuery<AddressView?>
{
    public long Id { get; init; }
}
