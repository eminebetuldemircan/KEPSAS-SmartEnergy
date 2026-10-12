namespace SmartEnergy.Application.DTOs.Meters;

public class CreateMeterRequest
{
    public string MeterNumber { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public int Type { get; set; }
    public DateTime InstallationDate { get; set; }
}