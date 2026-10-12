namespace SmartEnergy.Application.DTOs.ConsumptionRecords;

public class ConsumptionRecordListItemDto
{
    public Guid Id { get; set; }

    public Guid MeterId { get; set; }

    public string MeterNumber { get; set; } = string.Empty;

    public DateTime ReadingDate { get; set; }

    public decimal ConsumptionKwh { get; set; }

    public decimal MeterIndex { get; set; }

    public bool IsEstimated { get; set; }
}