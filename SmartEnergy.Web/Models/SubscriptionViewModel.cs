namespace SmartEnergy.Web.Models;

public class SubscriptionViewModel
{
    public Guid Id { get; set; }
    public string SubscriptionNumber { get; set; } = string.Empty;
    public string ContractNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string MeterNumber { get; set; } = string.Empty;
    public string TariffName { get; set; } = string.Empty;
    public string Status { get; set; } = "Aktif";
    public DateTime StartDate { get; set; }
}
