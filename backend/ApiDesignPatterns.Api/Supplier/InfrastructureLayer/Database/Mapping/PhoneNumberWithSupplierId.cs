using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.InfrastructureLayer.Database.Mapping;

public record PhoneNumberWithSupplierId : PhoneNumber
{
    public long SupplierId { get; init; }
}
