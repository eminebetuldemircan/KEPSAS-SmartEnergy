namespace SmartEnergy.Application.DTOs.ConsumptionRecords;

public class CreateConsumptionRecordRequest
{
    public Guid MeterId { get; set; }

    public DateTime ReadingDate { get; set; }

    public decimal MeterIndex { get; set; }

    public bool IsEstimated { get; set; }
}