namespace backend.Supplier.InfrastructureLayer.Database.Mapping;

public record PhoneNumberWithSupplierId : DomainModels.ValueObjects.PhoneNumber
{
    public long SupplierId { get; init; }
}
