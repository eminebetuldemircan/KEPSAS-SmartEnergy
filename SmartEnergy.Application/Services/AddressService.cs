using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Application.DTOs.Addresses;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddressService(
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork)
    {
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Address?> GetByIdAsync(Guid id)
    {
        return await _addressRepository.GetByIdAsync(id);
    }

    public async Task<IReadOnlyList<Address>> GetByUserIdAsync(Guid userId)
    {
        return await _addressRepository.GetByUserIdAsync(userId);
    }

    public async Task<Address> CreateAsync(CreateAddressRequest request)
    {
        var address = new Address(
            request.UserId,
            request.City,
            request.District,
            request.Neighborhood,
            request.Street,
            request.BuildingNumber,
            request.ApartmentNumber,
            request.PostalCode);

        await _addressRepository.AddAsync(address);

        await _unitOfWork.SaveChangesAsync();

        return address;
    }
}