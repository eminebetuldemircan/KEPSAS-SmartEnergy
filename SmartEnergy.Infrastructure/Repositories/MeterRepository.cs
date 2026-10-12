using Microsoft.EntityFrameworkCore;
using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence.Repositories;

public class MeterRepository : IMeterRepository
{
    private readonly SmartEnergyDbContext _context;

    public MeterRepository(SmartEnergyDbContext context)
    {
        _context = context;
    }

    public async Task<Meter?> GetByIdAsync(Guid id)
    {
        return await _context.Meters
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Meter?> GetByMeterNumberAsync(string meterNumber)
    {
        return await _context.Meters
            .FirstOrDefaultAsync(x => x.MeterNumber == meterNumber);
    }

    public async Task<IReadOnlyList<Meter>> GetAllAsync()
    {
        return await _context.Meters
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Meter meter)
    {
        await _context.Meters.AddAsync(meter);
    }

    public void Update(Meter meter)
    {
        _context.Meters.Update(meter);
    }

    public void Remove(Meter meter)
    {
        _context.Meters.Remove(meter);
    }
}