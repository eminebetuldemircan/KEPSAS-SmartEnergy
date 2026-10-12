using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Web.Services;

namespace SmartEnergy.Web.Controllers;

public class PaymentController : Controller
{
    private readonly PaymentApiService _paymentApiService;

    public PaymentController(PaymentApiService paymentApiService)
    {
        _paymentApiService = paymentApiService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.SuccessMessage = TempData["SuccessMessage"];

        Guid? customerId = null;
        if (Guid.TryParse(Request.Cookies["CustomerId"], out var parsedId))
        {
            customerId = parsedId;
        }

        var history = await _paymentApiService.GetHistoryAsync(customerId);
        return View(history);
    }

    [HttpGet]
    public IActionResult Pay(string billNumber, decimal amount)
    {
        ViewBag.BillNumber = string.IsNullOrEmpty(billNumber) ? "FTR-2026-005" : billNumber;
        ViewBag.Amount = amount > 0 ? amount : 742.50m;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PayConfirm(string billNumber, decimal amount, string cardNumber)
    {
        var result = await _paymentApiService.PayBillAsync(billNumber, amount, cardNumber);
        TempData["SuccessMessage"] = $"{billNumber} numaralı {amount:N2} ₺ tutarındaki faturanız başarıyla ödendi ve dekontunuz oluşturuldu.";
        return RedirectToAction("Index");
    }
}
