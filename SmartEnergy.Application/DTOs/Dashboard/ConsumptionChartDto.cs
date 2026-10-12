namespace SmartEnergy.Application.DTOs.Dashboard;

public class ConsumptionChartDto
{
    public DateTime Date { get; set; }

    public decimal ConsumptionKwh { get; set; }

    public decimal MeterIndex { get; set; }

    public bool IsEstimated { get; set; }
}