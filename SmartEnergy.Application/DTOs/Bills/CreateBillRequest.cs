namespace SmartEnergy.Application.DTOs.Bills;

public class CreateBillRequest
{
    public Guid SubscriptionId { get; set; }

    public string BillNumber { get; set; } = string.Empty;

    public DateTime BillingPeriodStart { get; set; }
    public DateTime BillingPeriodEnd { get; set; }

    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }

    public decimal PreviousIndex { get; set; }
    public decimal CurrentIndex { get; set; }

    public decimal EnergyAmount { get; set; }
    public decimal DistributionAmount { get; set; }
    public decimal TaxAmount { get; set; }
}