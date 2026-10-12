namespace SmartEnergy.Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendVerificationCodeAsync(string toEmail, string code, string fullName);
    Task<bool> SendNotificationEmailAsync(string toEmail, string subject, string body);
}
