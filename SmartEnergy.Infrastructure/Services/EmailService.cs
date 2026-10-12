using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartEnergy.Application.Interfaces;

namespace SmartEnergy.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendVerificationCodeAsync(string toEmail, string code, string fullName)
    {
        var subject = "KEPSAŞ SmartEnergy Doğrulama Kodunuz: " + code;
        var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: 'Segoe UI', Arial, sans-serif; background-color: #f8fafc; margin: 0; padding: 20px; color: #1e293b; }}
        .card {{ background: #ffffff; max-width: 500px; margin: 0 auto; border-radius: 12px; border: 1px solid #e2e8f0; padding: 32px; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.05); }}
        .header {{ text-align: center; border-bottom: 2px solid #002B49; padding-bottom: 16px; margin-bottom: 24px; }}
        .logo-text {{ font-size: 24px; font-weight: 800; color: #002B49; letter-spacing: 0.5px; }}
        .logo-sub {{ color: #F58220; font-weight: 700; }}
        .code-box {{ background: #fff7ed; border: 2px dashed #F58220; border-radius: 8px; text-align: center; padding: 18px; margin: 24px 0; font-size: 36px; font-weight: 800; letter-spacing: 10px; color: #c2410c; }}
        .footer {{ font-size: 12px; color: #64748b; text-align: center; margin-top: 30px; border-top: 1px solid #f1f5f9; padding-top: 16px; }}
    </style>
</head>
<body>
    <div class='card'>
        <div class='header'>
            <div class='logo-text'>KEPSAŞ <span class='logo-sub'>SmartEnergy</span></div>
            <div style='font-size: 13px; color: #64748b; margin-top: 4px;'>Kayseri Elektrik Perakende Satış A.Ş.</div>
        </div>
        <p style='font-size: 16px; margin-bottom: 12px; color: #002B49;'>Sayın tüketicimiz <strong>{fullName}</strong>,</p>
        <p style='font-size: 15px; line-height: 1.6; color: #1e293b; margin-bottom: 18px;'>
            <strong>{code}</strong> doğrulama kodu ile kayıt işlemine devam edebilirsiniz.
        </p>
        <div class='code-box'>{code}</div>
        <p style='font-size: 13px; color: #64748b; margin-top: 16px;'>Bu güvenlik kodu <strong>5 dakika</strong> süreyle geçerlidir. Güvenliğiniz için lütfen kimseyle paylaşmayınız.</p>
        <div class='footer'>
            Bu e-posta KEPSAŞ Müşteri Portalı tarafından otomatik olarak gönderilmiştir.<br/>
            Destek: 444 7 784 | www.kepsas.com.tr
        </div>
    </div>
</body>
</html>";

        return await SendEmailInternalAsync(toEmail, subject, body);
    }

    public async Task<bool> SendNotificationEmailAsync(string toEmail, string subject, string body)
    {
        return await SendEmailInternalAsync(toEmail, subject, body);
    }

    private async Task<bool> SendEmailInternalAsync(string toEmail, string subject, string htmlBody)
    {
        try
        {
            var host = _configuration["Smtp:Host"] ?? "smtp.gmail.com";
            var portString = _configuration["Smtp:Port"] ?? "587";
            int.TryParse(portString, out var port);
            if (port == 0) port = 587;

            var username = _configuration["Smtp:Username"];
            var password = _configuration["Smtp:Password"];
            var fromEmail = _configuration["Smtp:FromEmail"];
            if (string.IsNullOrWhiteSpace(fromEmail)) fromEmail = username;
            if (string.IsNullOrWhiteSpace(fromEmail)) fromEmail = "noreply@kepsas.com.tr";

            var fromName = _configuration["Smtp:FromName"] ?? "KEPSAŞ SmartEnergy";
            var enableSsl = bool.Parse(_configuration["Smtp:EnableSsl"] ?? "true");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("⚠️ SMTP bilgileri henüz girilmemiş! Doğrulama e-postası gönderilemedi. appsettings.json dosyasındaki Smtp:Username ve Smtp:Password alanlarını doldurunuz. Alıcı: {Email}, Konu: {Subject}", toEmail, subject);
                return false;
            }

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation("✅ E-posta başarıyla Gmail üzerinden iletildi: {Email}", toEmail);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ E-posta gönderiminde hata oluştu: {Email}. Detay: {Message}", toEmail, ex.Message);
            return false;
        }
    }
}
