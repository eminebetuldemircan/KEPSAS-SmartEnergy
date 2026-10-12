using System.Net.Http.Json;
using SmartEnergy.Web.Models;

namespace SmartEnergy.Web.Services;

public class MeterApiService
{
    private readonly HttpClient _httpClient;

    public MeterApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<MeterViewModel>> GetAllAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<MeterViewModel>>(
                "api/Meter")
            ?? new List<MeterViewModel>();
    }

    public async Task<bool> CreateAsync(
        CreateMeterViewModel model)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/Meter",
            model);

        return response.IsSuccessStatusCode;
    }
    public async Task<MeterViewModel?> GetByIdAsync(Guid id)
    {
        return await _httpClient
            .GetFromJsonAsync<MeterViewModel>(
                $"api/Meter/{id}");
    }
}