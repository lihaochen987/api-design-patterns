using backend.Address.Controllers;
using backend.Address.DomainModels;
using backend.Address.DomainModels.ValueObjects;
using Mapster;

namespace backend.Address.Services;

public static class AddressMappingConfig
{
    public static void RegisterAddressMappings(this TypeAdapterConfig config)
    {
        // Value Objects
        config.NewConfig<Street, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, Street>()
            .MapWith(src => new Street(src));

        config.NewConfig<City, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, City>()
            .MapWith(src => new City(src));

        config.NewConfig<PostalCode, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, PostalCode>()
            .MapWith(src => new PostalCode(src));

        config.NewConfig<Country, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, Country>()
            .MapWith(src => new Country(src));

        // GetAddressController
        config.NewConfig<AddressView, GetAddressResponse>();
    }
}
