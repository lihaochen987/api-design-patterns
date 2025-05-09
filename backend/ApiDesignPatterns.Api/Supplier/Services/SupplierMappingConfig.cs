using backend.Address.DomainModels.ValueObjects;
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
        config.NewConfig<FirstName, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, FirstName>()
            .MapWith(src => new FirstName(src));

        config.NewConfig<LastName, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, LastName>()
            .MapWith(src => new LastName(src));

        config.NewConfig<Email, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, Email>()
            .MapWith(src => new Email(src));

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
