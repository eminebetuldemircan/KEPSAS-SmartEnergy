using System.Net.Http.Json;
using SmartEnergy.Web.Models;

namespace SmartEnergy.Web.Services;

public class ConsumptionRecordApiService
{
    private readonly HttpClient _httpClient;

    public ConsumptionRecordApiService(
        HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ConsumptionRecordViewModel>>
        GetAllAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<ConsumptionRecordViewModel>>(
                "api/ConsumptionRecord")
            ?? new List<ConsumptionRecordViewModel>();
    }

    public async Task<bool> CreateAsync(
        CreateConsumptionRecordViewModel model)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/ConsumptionRecord",
            model);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<ConsumptionRecordViewModel>>
        GetByMeterIdAsync(Guid meterId)
    {
        return await _httpClient
            .GetFromJsonAsync<List<ConsumptionRecordViewModel>>(
                $"api/ConsumptionRecord/meter/{meterId}")
            ?? new List<ConsumptionRecordViewModel>();
    }

    public async Task<ConsumptionRecordViewModel?>
        GetByIdAsync(Guid id)
    {
        return await _httpClient
            .GetFromJsonAsync<ConsumptionRecordViewModel>(
                $"api/ConsumptionRecord/{id}");
    }
}