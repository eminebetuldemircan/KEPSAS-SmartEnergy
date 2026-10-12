namespace SmartEnergy.Web.Models;

public class MeterViewModel
{
    public Guid Id { get; set; }

    public string MeterNumber { get; set; } = string.Empty;

    public string SerialNumber { get; set; } = string.Empty;

    public int Type { get; set; } = 1;

    public string MeterType { get; set; } = string.Empty;

    public int Status { get; set; } = 1;

    public string Address { get; set; } = string.Empty;

    public DateTime InstallationDate { get; set; }

    public DateTime? LastReadingDate { get; set; }

    public decimal LastIndex { get; set; }
}

