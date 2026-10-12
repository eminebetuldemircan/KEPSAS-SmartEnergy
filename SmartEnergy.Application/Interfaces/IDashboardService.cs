using SmartEnergy.Application.DTOs;

namespace SmartEnergy.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto?> GetSummaryAsync(
        Guid meterId,
        DateTime startDate,
        DateTime endDate);

    Task<object?> GetConsumptionChartAsync(
        Guid meterId,
        DateTime startDate,
        DateTime endDate);
}