using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Abstractions.Repositories;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<Address>> GetByUserIdAsync(Guid userId);

    Task AddAsync(Address address);

    void Update(Address address);

    void Remove(Address address);
}