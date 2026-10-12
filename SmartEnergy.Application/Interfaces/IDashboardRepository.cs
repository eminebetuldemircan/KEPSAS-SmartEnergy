using SmartEnergy.Application.DTOs;

namespace SmartEnergy.Application.Interfaces;

public interface IDashboardRepository
{
    Task<List<DashboardConsumptionData>> GetConsumptionDataAsync(
        Guid meterId,
        DateTime startDate,
        DateTime endDate);
}