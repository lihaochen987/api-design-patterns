using backend.PhoneNumber.DomainModels.ValueObjects;
using backend.Supplier.Controllers;
using backend.Supplier.DomainModels;
using backend.Supplier.DomainModels.ValueObjects;
using Mapster;

namespace backend.Supplier.Services;

public static class SupplierMappingConfig
{
    public static void RegisterSupplierMappings(this TypeAdapterConfig config)
    {
        // Value objects
        config.NewConfig<CountryCode, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, CountryCode>()
            .MapWith(src => new CountryCode(src));

        config.NewConfig<AreaCode, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, AreaCode>()
            .MapWith(src => new AreaCode(src));

        config.NewConfig<PhoneDigits, long>()
            .MapWith(src => src.Value);
        config.NewConfig<long, PhoneDigits>()
            .MapWith(src => new PhoneDigits(src));

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

        // Address
        config.NewConfig<AddressRequest, Address>().TwoWays();
        config.NewConfig<Address, AddressResponse>().TwoWays();

        // Supplier mappings
        config.NewConfig<CreateSupplierRequest, DomainModels.Supplier>();
        config.NewConfig<DomainModels.Supplier, CreateSupplierResponse>();
        config.NewConfig<DomainModels.Supplier, ReplaceSupplierResponse>();
        config.NewConfig<ReplaceSupplierRequest, DomainModels.Supplier>();
        config.NewConfig<DomainModels.Supplier, UpdateSupplierResponse>();
        config.NewConfig<SupplierView, GetSupplierResponse>();
        config.NewConfig<DomainModels.Supplier, CreateSupplierRequest>();
        config.NewConfig<DomainModels.Supplier, ReplaceSupplierRequest>();
    }
}
