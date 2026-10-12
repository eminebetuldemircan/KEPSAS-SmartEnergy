using Microsoft.EntityFrameworkCore;
using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly SmartEnergyDbContext _context;

    public AddressRepository(SmartEnergyDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetByIdAsync(Guid id)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Address>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Addresses
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Address address)
    {
        await _context.Addresses.AddAsync(address);
    }

    public void Update(Address address)
    {
        _context.Addresses.Update(address);
    }

    public void Remove(Address address)
    {
        _context.Addresses.Remove(address);
    }
}