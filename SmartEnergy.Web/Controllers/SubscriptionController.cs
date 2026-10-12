using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Web.Services;

namespace SmartEnergy.Web.Controllers;

public class SubscriptionController : Controller
{
    private readonly SubscriptionApiService _subscriptionApiService;

    public SubscriptionController(SubscriptionApiService subscriptionApiService)
    {
        _subscriptionApiService = subscriptionApiService;
    }

    public async Task<IActionResult> Index()
    {
        var subscriptions = await _subscriptionApiService.GetAllAsync();
        return View(subscriptions);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var subscriptions = await _subscriptionApiService.GetAllAsync();
        var sub = subscriptions.FirstOrDefault(s => s.Id == id) ?? subscriptions.FirstOrDefault();
        return View(sub);
    }
}
