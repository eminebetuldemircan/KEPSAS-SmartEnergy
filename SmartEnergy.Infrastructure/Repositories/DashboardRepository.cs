using Microsoft.EntityFrameworkCore;
using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Application.DTOs;
using SmartEnergy.Application.Interfaces;
using SmartEnergy.Infrastructure.Persistence;

namespace SmartEnergy.Infrastructure.Persistence.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly SmartEnergyDbContext _context;

    public DashboardRepository(SmartEnergyDbContext context)
    {
        _context = context;
    }

    public async Task<List<DashboardConsumptionData>> GetConsumptionDataAsync(
        Guid meterId,
        DateTime startDate,
        DateTime endDate)
    {
        return await _context.ConsumptionRecords
            .AsNoTracking()
            .Where(x =>
                x.MeterId == meterId &&
                x.ReadingDate >= startDate &&
                x.ReadingDate <= endDate)
            .OrderBy(x => x.ReadingDate)
            .Select(x => new DashboardConsumptionData
            {
                ReadingDate = x.ReadingDate,
                ConsumptionKwh = x.ConsumptionKwh,
                MeterIndex = x.MeterIndex,
                IsEstimated = x.IsEstimated
            })
            .ToListAsync();
    }
}