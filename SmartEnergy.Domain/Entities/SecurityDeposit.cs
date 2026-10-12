using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class SecurityDeposit : BaseEntity
{
    public string DepositNumber { get; private set; } = string.Empty;

    public decimal TotalAmount { get; private set; }

    public decimal PaidAmount { get; private set; }

    public decimal RefundedAmount { get; private set; }

    public string Status { get; private set; } = "Aktif"; // Aktif, İade Edildi, Kısmi İade

    public Guid CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    public Guid SubscriptionId { get; private set; }

    public Subscription Subscription { get; private set; } = null!;

    private SecurityDeposit()
    {
    }

    public SecurityDeposit(
        string depositNumber,
        decimal totalAmount,
        decimal paidAmount,
        Guid customerId,
        Guid subscriptionId)
    {
        DepositNumber = depositNumber;
        TotalAmount = totalAmount;
        PaidAmount = paidAmount;
        RefundedAmount = 0;
        Status = "Aktif";
        CustomerId = customerId;
        SubscriptionId = subscriptionId;
    }
}
