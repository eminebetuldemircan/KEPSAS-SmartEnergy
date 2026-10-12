namespace SmartEnergy.Web.Models;

public class BillViewModel
{
    public Guid Id { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public string SubscriptionNumber { get; set; } = string.Empty;
    public string BillingPeriod { get; set; } = string.Empty;
    public decimal ConsumptionKwh { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime IssueDate { get; set; }
    public string Status { get; set; } = "Bekliyor"; // Bekliyor, Ödendi, Gecikmiş, İptal
}
