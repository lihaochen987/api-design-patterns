using backend.Address.Controllers;
using backend.Address.DomainModels;
using Mapster;

namespace backend.Address.Services;

public static class AddressMappingConfig
{
    public static void RegisterAddressMappings(this TypeAdapterConfig config)
    {
        // GetAddressController
        config.NewConfig<AddressView, GetAddressResponse>();
    }
}
