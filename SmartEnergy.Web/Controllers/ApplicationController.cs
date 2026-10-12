using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Web.Services;

namespace SmartEnergy.Web.Controllers;

public class ApplicationController : Controller
{
    private readonly ApplicationApiService _applicationApiService;

    public ApplicationController(ApplicationApiService applicationApiService)
    {
        _applicationApiService = applicationApiService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.SuccessMessage = TempData["SuccessMessage"];

        Guid? customerId = null;
        if (Guid.TryParse(Request.Cookies["CustomerId"], out var parsedId))
        {
            customerId = parsedId;
        }

        var apps = await _applicationApiService.GetAllAsync(customerId);
        return View(apps);
    }
}
