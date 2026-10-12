using System.Net.Http.Json;

namespace SmartEnergy.Web.Services;

public class PaymentApiService
{
    private readonly HttpClient _httpClient;

    public PaymentApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> PayBillAsync(string billNumber, decimal amount, string? cardNumber = null)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Payment", new
            {
                BillNumber = billNumber,
                Amount = amount,
                CardNumber = cardNumber,
                PaymentMethod = "Online Kredi Kartı"
            });

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<PaymentHistoryItemModel>> GetHistoryAsync(Guid? customerId = null)
    {
        try
        {
            var url = customerId.HasValue && customerId.Value != Guid.Empty
                ? $"api/Payment/history?customerId={customerId.Value}"
                : "api/Payment/history";

            return await _httpClient.GetFromJsonAsync<List<PaymentHistoryItemModel>>(url) ?? new();
        }
        catch
        {
            return new();
        }
    }
}

public class PaymentHistoryItemModel
{
    public Guid Id { get; set; }
    public Guid BillId { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
}
