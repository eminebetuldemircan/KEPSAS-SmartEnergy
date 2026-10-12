using SmartEnergy.Domain.Common;
using SmartEnergy.Domain.Enums;

namespace SmartEnergy.Domain.Entities;

public class Bill : BaseEntity
{
    public Guid SubscriptionId { get; private set; }

    public string BillNumber { get; private set; } = string.Empty;

    public BillType Type { get; private set; }

    public DateTime BillingPeriodStart { get; private set; }

    public DateTime BillingPeriodEnd { get; private set; }

    public DateTime IssueDate { get; private set; }

    public DateTime DueDate { get; private set; }

    public decimal PreviousIndex { get; private set; }

    public decimal CurrentIndex { get; private set; }

    public decimal ConsumptionKwh { get; private set; }

    public decimal EnergyAmount { get; private set; }

    public decimal DistributionAmount { get; private set; }

    public decimal TaxAmount { get; private set; }

    public decimal TotalAmount { get; private set; }

    public BillStatus Status { get; private set; }

    public Subscription Subscription { get; private set; } = null!;

    private Bill()
    {
    }

    public Bill(
        Guid subscriptionId,
        string billNumber,
        DateTime billingPeriodStart,
        DateTime billingPeriodEnd,
        DateTime issueDate,
        DateTime dueDate,
        decimal previousIndex,
        decimal currentIndex,
        decimal energyAmount,
        decimal distributionAmount,
        decimal taxAmount)
    {
        SubscriptionId = subscriptionId;
        BillNumber = billNumber;
        Type = BillType.Electricity;

        BillingPeriodStart = billingPeriodStart;
        BillingPeriodEnd = billingPeriodEnd;
        IssueDate = issueDate;
        DueDate = dueDate;

        PreviousIndex = previousIndex;
        CurrentIndex = currentIndex;

        ConsumptionKwh = currentIndex - previousIndex;

        EnergyAmount = energyAmount;
        DistributionAmount = distributionAmount;
        TaxAmount = taxAmount;

        TotalAmount =
            EnergyAmount +
            DistributionAmount +
            TaxAmount;

        Status = BillStatus.Unpaid;
    }

    public void MarkAsPaid()
    {
        Status = BillStatus.Paid;
        SetUpdatedAt();
    }

    public void MarkAsOverdue()
    {
        Status = BillStatus.Overdue;
        SetUpdatedAt();
    }

    public void Cancel()
    {
        Status = BillStatus.Cancelled;
        SetUpdatedAt();
    }
}