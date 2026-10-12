using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Web.Models;
using SmartEnergy.Web.Services;

namespace SmartEnergy.Web.Controllers;

public class DashboardController : Controller
{
    private readonly BillApiService _billApiService;
    private readonly NotificationApiService _notificationApiService;

    public DashboardController(BillApiService billApiService, NotificationApiService notificationApiService)
    {
        _billApiService = billApiService;
        _notificationApiService = notificationApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (Request.Cookies["IsLoggedIn"] != "true")
        {
            return RedirectToAction("Login", "Account");
        }

        Guid? customerId = null;
        if (Guid.TryParse(Request.Cookies["CustomerId"], out var parsedId))
        {
            customerId = parsedId;
        }

        var bills = await _billApiService.GetAllAsync();
        var notifications = await _notificationApiService.GetAllAsync(customerId);

        var viewModel = new DashboardViewModel
        {
            Bills = bills,
            Notifications = notifications
        };

        return View(viewModel);
    }
}

public class DashboardViewModel
{
    public List<BillViewModel> Bills { get; set; } = new();
    public List<NotificationItemModel> Notifications { get; set; } = new();
}