using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Application.DTOs.Subscriptions;
using SmartEnergy.Application.Services;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(
        ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subscriptions =
            await _subscriptionService.GetAllAsync();

        return Ok(subscriptions);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var subscription =
            await _subscriptionService.GetByIdAsync(id);

        if (subscription is null)
            return NotFound();

        return Ok(subscription);
    }

    [HttpGet("number/{subscriptionNumber}")]
    public async Task<IActionResult> GetBySubscriptionNumber(
        string subscriptionNumber)
    {
        var subscription =
            await _subscriptionService
                .GetBySubscriptionNumberAsync(subscriptionNumber);

        if (subscription is null)
            return NotFound();

        return Ok(subscription);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var subscriptions =
            await _subscriptionService
                .GetByUserIdAsync(userId);

        return Ok(subscriptions);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateSubscriptionRequest request)
    {
        var subscription =
            await _subscriptionService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = subscription.Id },
            subscription);
    }
}