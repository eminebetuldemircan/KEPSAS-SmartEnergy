using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Web.Services;

namespace SmartEnergy.Web.Controllers;

public class NotificationController : Controller
{
    private readonly NotificationApiService _notificationApiService;

    public NotificationController(NotificationApiService notificationApiService)
    {
        _notificationApiService = notificationApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Guid? customerId = null;
        if (Guid.TryParse(Request.Cookies["CustomerId"], out var parsedId))
        {
            customerId = parsedId;
        }

        var list = await _notificationApiService.GetAllAsync(customerId);
        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> MarkRead(Guid id)
    {
        await _notificationApiService.MarkAsReadAsync(id);
        return Ok(new { success = true });
    }
}
