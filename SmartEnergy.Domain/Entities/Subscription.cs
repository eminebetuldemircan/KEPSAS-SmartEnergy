using SmartEnergy.Domain.Common;
using SmartEnergy.Domain.Enums;

namespace SmartEnergy.Domain.Entities;

public class Subscription : BaseEntity
{
    public Guid UserId { get; private set; }

    public Guid AddressId { get; private set; }

    public Guid MeterId { get; private set; }

    public string SubscriptionNumber { get; private set; } = string.Empty;

    public string ContractNumber { get; private set; } = string.Empty;

    public DateTime StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public SubscriptionStatus Status { get; private set; }

    public User User { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    public Meter Meter { get; private set; } = null!;

    public ICollection<Bill> Bills { get; private set; } = new List<Bill>();

    private Subscription()
    {
    }

    public Subscription(
        Guid userId,
        Guid addressId,
        Guid meterId,
        string subscriptionNumber,
        string contractNumber,
        DateTime startDate)
    {
        UserId = userId;
        AddressId = addressId;
        MeterId = meterId;
        SubscriptionNumber = subscriptionNumber;
        ContractNumber = contractNumber;
        StartDate = startDate;
        Status = SubscriptionStatus.Active;
    }

    public void CloseSubscription(DateTime endDate)
    {
        EndDate = endDate;
        Status = SubscriptionStatus.Closed;
        SetUpdatedAt();
    }

    public void Suspend()
    {
        Status = SubscriptionStatus.Suspended;
        SetUpdatedAt();
    }

    public void Activate()
    {
        Status = SubscriptionStatus.Active;
        SetUpdatedAt();
    }
}