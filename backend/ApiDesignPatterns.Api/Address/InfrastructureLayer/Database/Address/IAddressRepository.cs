namespace backend.Address.InfrastructureLayer.Database.Address;

public interface IAddressRepository
{
    Task<DomainModels.Address?> GetAddress(long id);
    Task DeleteAddress(long id);
}
