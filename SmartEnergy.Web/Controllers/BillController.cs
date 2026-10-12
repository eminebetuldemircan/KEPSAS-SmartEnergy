using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Web.Services;

namespace SmartEnergy.Web.Controllers;

public class BillController : Controller
{
    private readonly BillApiService _billApiService;

    public BillController(BillApiService billApiService)
    {
        _billApiService = billApiService;
    }

    public async Task<IActionResult> Index()
    {
        var bills = await _billApiService.GetAllAsync();
        return View(bills);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var bills = await _billApiService.GetAllAsync();
        var bill = bills.FirstOrDefault(b => b.Id == id) ?? bills.FirstOrDefault();
        return View(bill);
    }
}
