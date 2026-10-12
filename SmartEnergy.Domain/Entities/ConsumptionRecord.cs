using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class ConsumptionRecord : BaseEntity
{
    public Guid MeterId { get; private set; }

    public DateTime ReadingDate { get; private set; }

    public decimal ConsumptionKwh { get; private set; }

    public decimal MeterIndex { get; private set; }

    public bool IsEstimated { get; private set; }

    public Meter Meter { get; private set; } = null!;

    private ConsumptionRecord()
    {
    }

    public ConsumptionRecord(
        Guid meterId,
        DateTime readingDate,
        decimal consumptionKwh,
        decimal meterIndex,
        bool isEstimated = false)
    {
        MeterId = meterId;
        ReadingDate = readingDate;
        ConsumptionKwh = consumptionKwh;
        MeterIndex = meterIndex;
        IsEstimated = isEstimated;
    }

    public void UpdateReading(
        decimal consumptionKwh,
        decimal meterIndex)
    {
        ConsumptionKwh = consumptionKwh;
        MeterIndex = meterIndex;

        SetUpdatedAt();
    }

    public void MarkAsEstimated()
    {
        IsEstimated = true;
        SetUpdatedAt();
    }
}