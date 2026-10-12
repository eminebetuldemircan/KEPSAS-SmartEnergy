using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string TcknVkn { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string Role { get; private set; } = "Abone";

    public bool IsActive { get; private set; }

    public DateTime? LastLoginAt { get; private set; }

    public Guid? CustomerId { get; private set; }

    public Customer? Customer { get; private set; }

    public ICollection<Address> Addresses { get; private set; } = new List<Address>();

    public ICollection<Subscription> Subscriptions { get; private set; } = new List<Subscription>();

    private User()
    {
    }

    public User(
        string firstName,
        string lastName,
        string email,
        string passwordHash)
        : this(firstName, lastName, email, string.Empty, passwordHash, "Abone")
    {
    }

    public User(
        string firstName,
        string lastName,
        string email,
        string tcknVkn,
        string passwordHash,
        string role = "Abone")
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        TcknVkn = tcknVkn;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = true;
    }

    public void LinkCustomer(Guid customerId)
    {
        CustomerId = customerId;
        SetUpdatedAt();
    }

    public void UpdateProfile(
        string firstName,
        string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        SetUpdatedAt();
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        SetUpdatedAt();
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }
}