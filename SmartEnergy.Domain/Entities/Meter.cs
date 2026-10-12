using SmartEnergy.Domain.Common;
using SmartEnergy.Domain.Enums;

namespace SmartEnergy.Domain.Entities;

public class Meter : BaseEntity
{
    public string MeterNumber { get; private set; } = string.Empty;

    public string SerialNumber { get; private set; } = string.Empty;

    public MeterType Type { get; private set; }

    public MeterStatus Status { get; private set; }

    public DateTime InstallationDate { get; private set; }

    public DateTime? LastReadingDate { get; private set; }

    public decimal LastIndex { get; private set; }

    public Subscription? Subscription { get; private set; }

    public ICollection<ConsumptionRecord> ConsumptionRecords
    { get; private set; } = new List<ConsumptionRecord>();

    private Meter()
    {
    }

    public Meter(
        string meterNumber,
        string serialNumber,
        MeterType type,
        DateTime installationDate)
    {
        MeterNumber = meterNumber;
        SerialNumber = serialNumber;
        Type = type;
        InstallationDate = installationDate;
        Status = MeterStatus.Active;
        LastIndex = 0;
    }

    public void UpdateReading(decimal index, DateTime readingDate)
    {
        LastIndex = index;
        LastReadingDate = readingDate;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        Status = MeterStatus.Inactive;
        SetUpdatedAt();
    }

    public void Activate()
    {
        Status = MeterStatus.Active;
        SetUpdatedAt();
    }
}