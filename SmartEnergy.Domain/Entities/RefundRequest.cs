using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class RefundRequest : BaseEntity
{
    public string RefundNumber { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public string RefundMethod { get; private set; } = "IBAN";

    public string Iban { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string Status { get; private set; } = "İnceleniyor"; // İnceleniyor, Onaylandı, Ödendi, Reddedildi

    public Guid CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    private RefundRequest()
    {
    }

    public RefundRequest(
        string refundNumber,
        decimal amount,
        string refundMethod,
        string iban,
        string description,
        Guid customerId)
    {
        RefundNumber = refundNumber;
        Amount = amount;
        RefundMethod = refundMethod;
        Iban = iban;
        Description = description;
        CustomerId = customerId;
        Status = "İnceleniyor";
    }
}
