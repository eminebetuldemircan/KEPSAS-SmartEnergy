using System.Net.Http.Json;
using SmartEnergy.Web.Models;

namespace SmartEnergy.Web.Services;

public class AuthApiService
{
    private readonly HttpClient _httpClient;

    public AuthApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResultModel?> LoginAsync(string tcknVkn, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", new
            {
                TcknVkn = tcknVkn,
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResultModel>();
            }
        }
        catch
        {
            // Network / API offline error
        }

        return null;
    }

    public async Task<(bool Success, string Message, string? DebugCode)> SendVerificationCodeAsync(string email, string fullName)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/send-code", new
            {
                Email = email,
                FullName = fullName,
                Purpose = "Register"
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<SendCodeResponse>();
                var receivedCode = result?.Code ?? result?.DebugCode;
                return (true, result?.Message ?? "Doğrulama kodu oluşturuldu.", receivedCode);
            }

            var err = await response.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(err) ? "Kod gönderilemedi." : err, null);
        }
        catch (Exception ex)
        {
            return (false, $"API bağlantı hatası: {ex.Message}", null);
        }
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterViewModel model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", model);
            if (response.IsSuccessStatusCode)
            {
                return (true, "Kayıt işlemi başarıyla tamamlandı. Giriş yapabilirsiniz.");
            }

            var err = await response.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(err) ? "Kayıt işlemi başarısız oldu." : err);
        }
        catch (Exception ex)
        {
            return (false, $"API bağlantı hatası: {ex.Message}");
        }
    }
}

public class LoginResultModel
{
    public Guid UserId { get; set; }
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TcknVkn { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class SendCodeResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? DebugCode { get; set; }
}

public class RegisterViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string TcknVkn { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
}
