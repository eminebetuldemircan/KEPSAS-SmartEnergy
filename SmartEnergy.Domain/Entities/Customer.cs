using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class Customer : BaseEntity
{
    public string CustomerNumber { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string TcknVkn { get; private set; } = string.Empty;

    public string Phone { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string CustomerType { get; private set; } = "Bireysel"; // Bireysel / Kurumsal

    public Guid? UserId { get; private set; }

    public User? User { get; private set; }

    public ICollection<Address> Addresses { get; private set; } = new List<Address>();

    public ICollection<Subscription> Subscriptions { get; private set; } = new List<Subscription>();

    public ICollection<Bill> Bills { get; private set; } = new List<Bill>();

    public ICollection<PortalApplication> Applications { get; private set; } = new List<PortalApplication>();

    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();

    public ICollection<Notification> Notifications { get; private set; } = new List<Notification>();

    public ICollection<SecurityDeposit> SecurityDeposits { get; private set; } = new List<SecurityDeposit>();

    public ICollection<Compensation> Compensations { get; private set; } = new List<Compensation>();

    public ICollection<RefundRequest> RefundRequests { get; private set; } = new List<RefundRequest>();

    private Customer()
    {
    }

    public Customer(
        string customerNumber,
        string firstName,
        string lastName,
        string tcknVkn,
        string phone,
        string email,
        string customerType = "Bireysel")
    {
        CustomerNumber = customerNumber;
        FirstName = firstName;
        LastName = lastName;
        TcknVkn = tcknVkn;
        Phone = phone;
        Email = email;
        CustomerType = customerType;
    }

    public void LinkUser(Guid userId)
    {
        UserId = userId;
        SetUpdatedAt();
    }

    public void UpdateProfile(string phone, string email)
    {
        Phone = phone;
        Email = email;
        SetUpdatedAt();
    }
}
