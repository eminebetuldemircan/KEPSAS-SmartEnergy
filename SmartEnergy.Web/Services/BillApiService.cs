using System.Net.Http.Json;
using SmartEnergy.Web.Models;

namespace SmartEnergy.Web.Services;

public class BillApiService
{
    private readonly HttpClient _httpClient;

    public BillApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BillViewModel>> GetAllAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<BillViewModel>>("api/Bill") ?? GetMockBills();
        }
        catch
        {
            return GetMockBills();
        }
    }

    private List<BillViewModel> GetMockBills()
    {
        return new List<BillViewModel>
        {
            new BillViewModel
            {
                Id = Guid.NewGuid(),
                BillNumber = "FTR-2026-005",
                SubscriptionNumber = "SUB-380901",
                BillingPeriod = "Nisan 2026",
                ConsumptionKwh = 286.40m,
                TotalAmount = 742.50m,
                DueDate = DateTime.Now.AddDays(7),
                Status = "Bekliyor",
                IssueDate = new DateTime(2026, 5, 2)
            },
            new BillViewModel
            {
                Id = Guid.NewGuid(),
                BillNumber = "FTR-2026-004",
                SubscriptionNumber = "SUB-380901",
                BillingPeriod = "Mart 2026",
                ConsumptionKwh = 250.00m,
                TotalAmount = 655.00m,
                DueDate = new DateTime(2026, 4, 17),
                Status = "Ödendi",
                IssueDate = new DateTime(2026, 4, 2)
            },
            new BillViewModel
            {
                Id = Guid.NewGuid(),
                BillNumber = "FTR-2026-003",
                SubscriptionNumber = "SUB-380902",
                BillingPeriod = "Mart 2026",
                ConsumptionKwh = 210.10m,
                TotalAmount = 575.00m,
                DueDate = new DateTime(2026, 4, 17),
                Status = "Ödendi",
                IssueDate = new DateTime(2026, 4, 2)
            }
        };
    }
}
