namespace SmartEnergy.Application.DTOs;

public class DashboardConsumptionData
{
    public DateTime ReadingDate { get; set; }

    public decimal ConsumptionKwh { get; set; }

    public decimal MeterIndex { get; set; }

    public bool IsEstimated { get; set; }
}