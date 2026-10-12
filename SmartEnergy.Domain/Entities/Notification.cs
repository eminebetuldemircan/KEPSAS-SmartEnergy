using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class Notification : BaseEntity
{
    public string Title { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public string Type { get; private set; } = "Bilgi"; // Fatura, Ödeme, Başvuru, Evrak, Randevu, Genel

    public bool IsRead { get; private set; }

    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public Guid CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    private Notification()
    {
    }

    public Notification(
        string title,
        string message,
        string type,
        Guid customerId)
    {
        Title = title;
        Message = message;
        Type = type;
        CustomerId = customerId;
        IsRead = false;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        SetUpdatedAt();
    }
}
