using SmartEnergy.Application.DTOs.Addresses;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public interface IAddressService
{
    Task<Address?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<Address>> GetByUserIdAsync(Guid userId);

    Task<Address> CreateAsync(CreateAddressRequest request);
}