using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class Compensation : BaseEntity
{
    public string CompensationNumber { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public string Status { get; private set; } = "İnceleniyor"; // İnceleniyor, Onaylandı, Ödendi, Reddedildi

    public DateTime ApplicationDate { get; private set; }

    public string ResultDescription { get; private set; } = string.Empty;

    public Guid CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    private Compensation()
    {
    }

    public Compensation(
        string compensationNumber,
        decimal amount,
        string status,
        DateTime applicationDate,
        string resultDescription,
        Guid customerId)
    {
        CompensationNumber = compensationNumber;
        Amount = amount;
        Status = status;
        ApplicationDate = applicationDate;
        ResultDescription = resultDescription;
        CustomerId = customerId;
    }
}
