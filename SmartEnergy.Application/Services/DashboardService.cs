using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Application.DTOs;
using SmartEnergy.Application.Interfaces;

namespace SmartEnergy.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<DashboardSummaryDto?> GetSummaryAsync(
        Guid meterId,
        DateTime startDate,
        DateTime endDate)
    {
        var records = await _dashboardRepository.GetConsumptionDataAsync(
            meterId,
            startDate,
            endDate);

        if (records.Count == 0)
        {
            return null;
        }

        var totalConsumption = records.Sum(x => x.ConsumptionKwh);

        var averageDailyConsumption = records
            .GroupBy(x => x.ReadingDate.Date)
            .Select(g => g.Sum(x => x.ConsumptionKwh))
            .DefaultIfEmpty(0)
            .Average();

        var highestDailyConsumption = records
            .GroupBy(x => x.ReadingDate.Date)
            .Select(g => g.Sum(x => x.ConsumptionKwh))
            .DefaultIfEmpty(0)
            .Max();

        var lowestDailyConsumption = records
            .GroupBy(x => x.ReadingDate.Date)
            .Select(g => g.Sum(x => x.ConsumptionKwh))
            .DefaultIfEmpty(0)
            .Min();

        var recordCount = records.Count;

        var previousPeriodDuration = endDate - startDate;

        var previousPeriodEnd = startDate.AddDays(-1);

        var previousPeriodStart =
            previousPeriodEnd - previousPeriodDuration;

        var previousRecords =
            await _dashboardRepository.GetConsumptionDataAsync(
                meterId,
                previousPeriodStart,
                previousPeriodEnd);

        var previousPeriodConsumption =
            previousRecords.Sum(x => x.ConsumptionKwh);

        decimal consumptionChangePercentage = 0;

        if (previousPeriodConsumption > 0)
        {
            consumptionChangePercentage =
                ((totalConsumption - previousPeriodConsumption)
                / previousPeriodConsumption) * 100;
        }

        var estimatedBillAmount =
            totalConsumption * 2.60m;

        return new DashboardSummaryDto
        {
            MeterId = meterId,

            TotalConsumptionKwh = totalConsumption,

            AverageDailyConsumptionKwh =
                Math.Round(averageDailyConsumption, 2),

            HighestDailyConsumptionKwh =
                Math.Round(highestDailyConsumption, 2),

            LowestDailyConsumptionKwh =
                Math.Round(lowestDailyConsumption, 2),

            RecordCount = recordCount,

            EstimatedBillAmount =
                Math.Round(estimatedBillAmount, 2),

            PreviousPeriodConsumptionKwh =
                Math.Round(previousPeriodConsumption, 2),

            ConsumptionChangePercentage =
                Math.Round(consumptionChangePercentage, 2)
        };
    }

    public async Task<object?> GetConsumptionChartAsync(
        Guid meterId,
        DateTime startDate,
        DateTime endDate)
    {
        var records = await _dashboardRepository.GetConsumptionDataAsync(
            meterId,
            startDate,
            endDate);

        var chartData = records
            .GroupBy(x => x.ReadingDate.Date)
            .OrderBy(g => g.Key)
            .Select(g => new
            {
                Date = g.Key,
                ConsumptionKwh = g.Sum(x => x.ConsumptionKwh)
            })
            .ToList();

        return chartData;
    }
}