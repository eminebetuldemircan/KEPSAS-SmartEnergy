using System.Net.Http.Json;
using SmartEnergy.Web.Models;

namespace SmartEnergy.Web.Services;

public class DashboardApiService
{
    private readonly HttpClient _httpClient;

    public DashboardApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DashboardSummaryViewModel?> GetSummaryAsync(
        Guid meterId,
        DateTime startDate,
        DateTime endDate)
    {
        var url =
            $"api/Dashboard/summary" +
            $"?meterId={meterId}" +
            $"&startDate={startDate:yyyy-MM-dd}" +
            $"&endDate={endDate:yyyy-MM-dd}";

        return await _httpClient
            .GetFromJsonAsync<DashboardSummaryViewModel>(url);
    }

    public async Task<List<DashboardConsumptionViewModel>> GetConsumptionAsync(
        Guid meterId,
        DateTime startDate,
        DateTime endDate)
    {
        var url =
            $"api/Dashboard/consumption" +
            $"?meterId={meterId}" +
            $"&startDate={startDate:yyyy-MM-dd}" +
            $"&endDate={endDate:yyyy-MM-dd}";

        return await _httpClient
            .GetFromJsonAsync<List<DashboardConsumptionViewModel>>(url)
            ?? new List<DashboardConsumptionViewModel>();
    }
}