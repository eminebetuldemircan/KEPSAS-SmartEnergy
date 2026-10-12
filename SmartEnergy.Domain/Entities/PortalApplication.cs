using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class PortalApplication : BaseEntity
{
    public string ApplicationNumber { get; private set; } = string.Empty;

    public string ApplicationType { get; private set; } = string.Empty; // Fesih, Alacakİadesi, GüvenceBedeli, Tazminat, Evrak, Kaçakİhbar, BilgiEdinme, İtirazŞikayet, Randevu

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string Status { get; private set; } = "Alındı"; // Alındı, İnceleniyor, Eksik Evrak, Sonuçlandı, Reddedildi

    public string? ResultNote { get; private set; }

    public string? DocumentUrl { get; private set; }

    public Guid CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    public Guid? SubscriptionId { get; private set; }

    public Subscription? Subscription { get; private set; }

    private PortalApplication()
    {
    }

    public PortalApplication(
        string applicationNumber,
        string applicationType,
        string title,
        string description,
        Guid customerId,
        Guid? subscriptionId = null,
        string? documentUrl = null)
    {
        ApplicationNumber = applicationNumber;
        ApplicationType = applicationType;
        Title = title;
        Description = description;
        CustomerId = customerId;
        SubscriptionId = subscriptionId;
        DocumentUrl = documentUrl;
        Status = "Alındı";
    }

    public void UpdateStatus(string status, string? resultNote = null)
    {
        Status = status;
        ResultNote = resultNote;
        SetUpdatedAt();
    }
}
