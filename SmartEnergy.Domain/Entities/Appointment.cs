using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class Appointment : BaseEntity
{
    public string AppointmentNumber { get; private set; } = string.Empty;

    public string ServiceType { get; private set; } = string.Empty; // Abonelik Devir, Abonelik İptali, Sayaç İşlemleri, Genel Bilgi

    public string Location { get; private set; } = string.Empty; // Müşteri Hizmetleri Merkezi

    public DateTime AppointmentDate { get; private set; }

    public string TimeSlot { get; private set; } = string.Empty; // e.g. "10:00 - 10:30"

    public string Status { get; private set; } = "Onaylandı"; // Onaylandı, Tamamlandı, İptal

    public string? Notes { get; private set; }

    public Guid CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    private Appointment()
    {
    }

    public Appointment(
        string appointmentNumber,
        string serviceType,
        string location,
        DateTime appointmentDate,
        string timeSlot,
        Guid customerId,
        string? notes = null)
    {
        AppointmentNumber = appointmentNumber;
        ServiceType = serviceType;
        Location = location;
        AppointmentDate = appointmentDate;
        TimeSlot = timeSlot;
        CustomerId = customerId;
        Notes = notes;
        Status = "Onaylandı";
    }

    public void Cancel()
    {
        Status = "İptal";
        SetUpdatedAt();
    }
}
