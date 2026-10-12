using Microsoft.EntityFrameworkCore;
using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence.Repositories;

public class ConsumptionRecordRepository
    : IConsumptionRecordRepository
{
    private readonly SmartEnergyDbContext _context;

    public ConsumptionRecordRepository(
        SmartEnergyDbContext context)
    {
        _context = context;
    }

    public async Task<ConsumptionRecord?> GetByIdAsync(
        Guid id)
    {
        return await _context.ConsumptionRecords
            .Include(x => x.Meter)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<ConsumptionRecord>>
        GetAllAsync()
    {
        return await _context.ConsumptionRecords
            .Include(x => x.Meter)
            .AsNoTracking()
            .OrderByDescending(x => x.ReadingDate)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ConsumptionRecord>>
        GetByMeterIdAsync(Guid meterId)
    {
        return await _context.ConsumptionRecords
            .Include(x => x.Meter)
            .AsNoTracking()
            .Where(x => x.MeterId == meterId)
            .OrderBy(x => x.ReadingDate)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ConsumptionRecord>>
        GetByDateRangeAsync(
            Guid meterId,
            DateTime startDate,
            DateTime endDate)
    {
        return await _context.ConsumptionRecords
            .Include(x => x.Meter)
            .AsNoTracking()
            .Where(x =>
                x.MeterId == meterId &&
                x.ReadingDate >= startDate &&
                x.ReadingDate < endDate.AddDays(1))
            .OrderBy(x => x.ReadingDate)
            .ToListAsync();
    }

    public async Task AddAsync(
        ConsumptionRecord record)
    {
        await _context.ConsumptionRecords
            .AddAsync(record);
    }

    public void Update(
        ConsumptionRecord record)
    {
        _context.ConsumptionRecords.Update(record);
    }

    public void Remove(
        ConsumptionRecord record)
    {
        _context.ConsumptionRecords.Remove(record);
    }
}