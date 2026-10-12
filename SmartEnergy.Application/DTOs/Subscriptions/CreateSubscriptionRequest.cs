namespace SmartEnergy.Application.DTOs.Subscriptions;

public class CreateSubscriptionRequest
{
    public Guid UserId { get; set; }
    public Guid AddressId { get; set; }
    public Guid MeterId { get; set; }

    public string SubscriptionNumber { get; set; } = string.Empty;
    public string ContractNumber { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
}