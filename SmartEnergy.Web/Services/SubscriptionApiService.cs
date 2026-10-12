using System.Net.Http.Json;
using SmartEnergy.Web.Models;

namespace SmartEnergy.Web.Services;

public class SubscriptionApiService
{
    private readonly HttpClient _httpClient;

    public SubscriptionApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SubscriptionViewModel>> GetAllAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<SubscriptionViewModel>>("api/Subscription") ?? GetMockSubscriptions();
        }
        catch
        {
            return GetMockSubscriptions();
        }
    }

    private List<SubscriptionViewModel> GetMockSubscriptions()
    {
        return new List<SubscriptionViewModel>
        {
            new SubscriptionViewModel
            {
                Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                SubscriptionNumber = "SUB-380901",
                ContractNumber = "SZ-2022-771",
                Address = "Alpaslan Mah. Sivas Cad. No:42/5 Melikgazi / KAYSERİ",
                MeterNumber = "MTR-2026-991",
                TariffName = "Mesken Tek Zamanlı Tarife",
                Status = "Aktif",
                StartDate = new DateTime(2022, 5, 15)
            },
            new SubscriptionViewModel
            {
                Id = Guid.Parse("11111111-2222-3333-4444-666666666666"),
                SubscriptionNumber = "SUB-380902",
                ContractNumber = "SZ-2023-882",
                Address = "Sahabiye Mah. İstasyon Cad. No:12/1 Kocasinan / KAYSERİ",
                MeterNumber = "MTR-2026-992",
                TariffName = "Ticarethane 3 Zamanlı Tarife",
                Status = "Aktif",
                StartDate = new DateTime(2023, 8, 10)
            }
        };
    }
}
