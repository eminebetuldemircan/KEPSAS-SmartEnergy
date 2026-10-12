using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEnergy.Domain.Entities;
using SmartEnergy.Infrastructure.Persistence;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortalApplicationController : ControllerBase
{
    private readonly SmartEnergyDbContext _context;

    public PortalApplicationController(SmartEnergyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? customerId)
    {
        var query = _context.PortalApplications.AsQueryable();

        if (customerId.HasValue && customerId.Value != Guid.Empty)
        {
            query = query.Where(a => a.CustomerId == customerId.Value);
        }

        var apps = await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return Ok(apps);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateApplicationRequest request)
    {
        var appNumber = $"BAS-2026-{Random.Shared.Next(100, 999)}";
        var app = new PortalApplication(
            applicationNumber: appNumber,
            applicationType: request.ApplicationType,
            title: request.Title,
            description: request.Description,
            customerId: request.CustomerId,
            subscriptionId: request.SubscriptionId,
            documentUrl: request.DocumentUrl
        );

        _context.PortalApplications.Add(app);
        await _context.SaveChangesAsync();

        // Also add a notification
        var notif = new Notification(
            title: "Başvurunuz Alındı",
            message: $"{appNumber} numaralı {request.Title} başvurunuz sisteme başarıyla kaydedilmiştir.",
            type: "Başvuru",
            customerId: request.CustomerId
        );
        _context.Notifications.Add(notif);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { id = app.Id }, app);
    }
}

public class CreateApplicationRequest
{
    public Guid CustomerId { get; set; }
    public Guid? SubscriptionId { get; set; }
    public string ApplicationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? DocumentUrl { get; set; }
}
