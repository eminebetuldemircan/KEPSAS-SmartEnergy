using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEnergy.Application.Interfaces;
using SmartEnergy.Domain.Entities;
using SmartEnergy.Infrastructure.Persistence;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SmartEnergyDbContext _context;
    private readonly IEmailService _emailService;

    public AuthController(SmartEnergyDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.TcknVkn) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("TCKN/VKN ve Şifre alanları zorunludur.");
        }

        var user = await _context.Users
            .Include(u => u.Customer)
            .FirstOrDefaultAsync(u => u.TcknVkn == request.TcknVkn || u.Email == request.TcknVkn);

        if (user == null || user.PasswordHash != request.Password)
        {
            return Unauthorized("TCKN/VKN veya Şifre hatalı.");
        }

        user.UpdateLastLogin();
        await _context.SaveChangesAsync();

        return Ok(new LoginResponse
        {
            UserId = user.Id,
            CustomerId = user.CustomerId ?? Guid.Empty,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            TcknVkn = user.TcknVkn,
            Role = user.Role
        });
    }

    [HttpPost("send-code")]
    public async Task<IActionResult> SendCode([FromBody] SendCodeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("E-posta adresi gereklidir.");
        }

        // Generate 4 digit code
        var code = Random.Shared.Next(1000, 10000).ToString();
        var expiresAt = DateTime.UtcNow.AddMinutes(5);

        // Invalidate older unused codes for this email
        var existingCodes = await _context.EmailVerifications
            .Where(e => e.Email == request.Email && !e.IsUsed)
            .ToListAsync();
        foreach (var c in existingCodes)
        {
            c.MarkAsUsed();
        }

        var verification = new EmailVerification(request.Email, code, expiresAt, request.Purpose ?? "Register");
        _context.EmailVerifications.Add(verification);
        await _context.SaveChangesAsync();

        // Send Email via Gmail SMTP
        var name = string.IsNullOrWhiteSpace(request.FullName) ? "Değerli Abonemiz" : request.FullName;
        var emailSent = await _emailService.SendVerificationCodeAsync(request.Email, code, name);

        return Ok(new
        {
            Success = true,
            Message = emailSent 
                ? "4 haneli doğrulama kodu Gmail adresinize iletildi." 
                : $"Doğrulama kodunuz üretildi: {code} (SMTP henüz yapılandırılmadığı için kod doğrudan ekranda sağlandı)",
            Code = code
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.VerificationCode))
        {
            return BadRequest("E-postanıza gönderilen doğrulama kodunu girmelisiniz.");
        }

        // Verify Code
        var verification = await _context.EmailVerifications
            .Where(e => e.Email == request.Email && !e.IsUsed)
            .OrderByDescending(e => e.CreatedAt)
            .FirstOrDefaultAsync();

        if (verification == null || !verification.IsValid(request.VerificationCode))
        {
            return BadRequest("Girdiğiniz doğrulama kodu geçersiz veya süresi dolmuş.");
        }

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.TcknVkn == request.TcknVkn || u.Email == request.Email);

        if (existingUser != null)
        {
            return BadRequest("Bu TCKN veya E-posta ile zaten kayıtlı bir kullanıcı var.");
        }

        var customer = new Domain.Entities.Customer(
            customerNumber: $"K-{Random.Shared.Next(100000, 999999)}",
            firstName: request.FirstName,
            lastName: request.LastName,
            tcknVkn: request.TcknVkn,
            phone: request.Phone,
            email: request.Email
        );

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var user = new Domain.Entities.User(
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.Email,
            tcknVkn: request.TcknVkn,
            passwordHash: request.Password
        );
        user.LinkCustomer(customer.Id);
        _context.Users.Add(user);

        // Mark code as used
        verification.MarkAsUsed();

        // Add welcome notification
        var welcomeNotif = new Notification(
            title: "KEPSAŞ Portalına Hoş Geldiniz",
            message: $"Sayın {request.FirstName} {request.LastName}, KEPSAŞ SmartEnergy Abone Portalına üyeliğiniz başarıyla tamamlanmıştır. Müşteri numaranız: {customer.CustomerNumber}",
            type: "Genel",
            customerId: customer.Id
        );
        _context.Notifications.Add(welcomeNotif);

        await _context.SaveChangesAsync();

        customer.LinkUser(user.Id);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Success = true,
            Message = "Kayıt işlemi başarıyla tamamlandı.",
            CustomerNumber = customer.CustomerNumber
        });
    }
}

public class LoginRequest
{
    public string TcknVkn { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public Guid UserId { get; set; }
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TcknVkn { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class SendCodeRequest
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Purpose { get; set; } = "Register";
}

public class RegisterRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string TcknVkn { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
}
