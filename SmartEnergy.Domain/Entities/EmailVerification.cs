using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class EmailVerification : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private set; }
    public bool IsUsed { get; private set; }
    public string Purpose { get; private set; } = "Register"; // Register, PasswordReset, etc.

    private EmailVerification()
    {
    }

    public EmailVerification(string email, string code, DateTime expiresAtUtc, string purpose = "Register")
    {
        Email = email;
        Code = code;
        ExpiresAtUtc = expiresAtUtc;
        Purpose = purpose;
        IsUsed = false;
    }

    public bool IsValid(string code)
    {
        return !IsUsed && Code == code && DateTime.UtcNow <= ExpiresAtUtc;
    }

    public void MarkAsUsed()
    {
        IsUsed = true;
        SetUpdatedAt();
    }
}
