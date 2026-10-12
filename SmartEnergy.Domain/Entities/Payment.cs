using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid BillId { get; private set; }

    public Bill Bill { get; private set; } = null!;

    public decimal Amount { get; private set; }

    public DateTime PaymentDate { get; private set; }

    public string PaymentMethod { get; private set; } = "Kredi Kartı"; // Kredi Kartı, Banka Havalesi, Otomatik Ödeme

    public string TransactionId { get; private set; } = string.Empty;

    public string Status { get; private set; } = "Başarılı";

    private Payment()
    {
    }

    public Payment(
        Guid billId,
        decimal amount,
        string paymentMethod,
        string transactionId)
    {
        BillId = billId;
        Amount = amount;
        PaymentDate = DateTime.UtcNow;
        PaymentMethod = paymentMethod;
        TransactionId = transactionId;
        Status = "Başarılı";
    }
}
