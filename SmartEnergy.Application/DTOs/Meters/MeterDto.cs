namespace SmartEnergy.Application.DTOs.Meters;

public class MeterDto
{
    public Guid Id { get; set; }

    public string MeterNumber { get; set; } = string.Empty;

    public string SerialNumber { get; set; } = string.Empty;

    public int Type { get; set; }

    public int Status { get; set; }

    public DateTime InstallationDate { get; set; }

    public DateTime? LastReadingDate { get; set; }

    public decimal LastIndex { get; set; }
}