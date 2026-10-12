using System.Net.Http.Json;

namespace SmartEnergy.Web.Services;

public class NotificationApiService
{
    private readonly HttpClient _httpClient;

    public NotificationApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NotificationItemModel>> GetAllAsync(Guid? customerId = null)
    {
        try
        {
            var url = customerId.HasValue && customerId.Value != Guid.Empty
                ? $"api/Notification?customerId={customerId.Value}"
                : "api/Notification";

            var result = await _httpClient.GetFromJsonAsync<List<NotificationItemModel>>(url);
            if (result != null && result.Any())
            {
                return result;
            }
        }
        catch
        {
            // API offline fallback
        }

        return GetFallbackNotifications();
    }

    public async Task<bool> MarkAsReadAsync(Guid id)
    {
        try
        {
            var resp = await _httpClient.PostAsync($"api/Notification/{id}/read", null);
            return resp.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private List<NotificationItemModel> GetFallbackNotifications()
    {
        return new List<NotificationItemModel>
        {
            new NotificationItemModel
            {
                Id = Guid.NewGuid(),
                Title = "Yeni Faturanız Oluşturuldu",
                Message = "Nisan 2026 dönemine ait 742,50 ₺ tutarındaki faturanız oluşturulmuştur. Son ödeme tarihi: 14 Mayıs 2026.",
                Type = "Fatura",
                CreatedAtUtc = DateTime.UtcNow.AddHours(-2),
                IsRead = false
            },
            new NotificationItemModel
            {
                Id = Guid.NewGuid(),
                Title = "Randevu Hatırlatması",
                Message = "Melikgazi MHM randevunuza 3 gün kalmıştır. Lütfen kimlik belgenizi yanınızda bulundurunuz.",
                Type = "Randevu",
                CreatedAtUtc = DateTime.UtcNow.AddDays(-1),
                IsRead = false
            },
            new NotificationItemModel
            {
                Id = Guid.NewGuid(),
                Title = "Ödemeniz Alındı",
                Message = "FTR-2026-004 numaralı faturanıza ait 655,00 ₺ ödeme başarıyla sistemimize yansımıştır.",
                Type = "Ödeme",
                CreatedAtUtc = DateTime.UtcNow.AddDays(-3),
                IsRead = true
            },
            new NotificationItemModel
            {
                Id = Guid.NewGuid(),
                Title = "Planlı Bakım ve Şebeke İyileştirmesi",
                Message = "Alpaslan Mahallesi bölgesinde 12 Mayıs tarihinde saat 09:00 - 13:00 saatleri arasında planlı şebeke bakım çalışması gerçekleştirilecektir.",
                Type = "Duyuru",
                CreatedAtUtc = DateTime.UtcNow.AddDays(-4),
                IsRead = true
            },
            new NotificationItemModel
            {
                Id = Guid.NewGuid(),
                Title = "Enerji Tasarrufu Bildirimi",
                Message = "Geçen aya göre tüketiminiz %12 azaldı. Tasarruflu kullanımınız için teşekkür ederiz!",
                Type = "Tasarruf",
                CreatedAtUtc = DateTime.UtcNow.AddDays(-5),
                IsRead = true
            }
        };
    }
}

public class NotificationItemModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
