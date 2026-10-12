namespace SmartEnergy.Web.Models;

public class DashboardSummaryViewModel
{
    public Guid MeterId { get; set; }

    public decimal TotalConsumptionKwh { get; set; }

    public decimal AverageDailyConsumptionKwh { get; set; }

    public decimal HighestDailyConsumptionKwh { get; set; }

    public decimal LowestDailyConsumptionKwh { get; set; }

    public int RecordCount { get; set; }

    public decimal EstimatedBillAmount { get; set; }

    public decimal PreviousPeriodConsumptionKwh { get; set; }

    public decimal ConsumptionChangePercentage { get; set; }
}