using System.Net.Http.Json;

namespace SmartEnergy.Web.Services;

public class ApplicationApiService
{
    private readonly HttpClient _httpClient;

    public ApplicationApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PortalApplicationItemModel>> GetAllAsync(Guid? customerId = null)
    {
        try
        {
            var url = customerId.HasValue && customerId.Value != Guid.Empty
                ? $"api/PortalApplication?customerId={customerId.Value}"
                : "api/PortalApplication";

            return await _httpClient.GetFromJsonAsync<List<PortalApplicationItemModel>>(url) ?? GetFallbackApplications();
        }
        catch
        {
            return GetFallbackApplications();
        }
    }

    public async Task<bool> CreateApplicationAsync(CreateApplicationModel model)
    {
        try
        {
            var resp = await _httpClient.PostAsJsonAsync("api/PortalApplication", model);
            return resp.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private List<PortalApplicationItemModel> GetFallbackApplications()
    {
        return new List<PortalApplicationItemModel>
        {
            new PortalApplicationItemModel
            {
                Id = Guid.NewGuid(),
                ApplicationNumber = "BAS-2026-089",
                ApplicationType = "BilgiEdinme",
                Title = "Tarife Bilgisi Talebi",
                Description = "Mesken aboneliği için zamanlı tarife geçiş şartları hakkında bilgi talep ediyorum.",
                Status = "İnceleniyor",
                CreatedAt = DateTime.Now.AddDays(-2)
            },
            new PortalApplicationItemModel
            {
                Id = Guid.NewGuid(),
                ApplicationNumber = "BAS-2026-092",
                ApplicationType = "Randevu",
                Title = "Müşteri Hizmetleri Randevusu",
                Description = "Sayaç kontrol ve inceleme talebi için randevu oluşturuldu.",
                Status = "Onaylandı",
                CreatedAt = DateTime.Now.AddDays(-5)
            }
        };
    }
}

public class PortalApplicationItemModel
{
    public Guid Id { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;
    public string ApplicationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateApplicationModel
{
    public Guid CustomerId { get; set; }
    public Guid? SubscriptionId { get; set; }
    public string ApplicationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? DocumentUrl { get; set; }
}
