using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEnergy.Infrastructure.Persistence;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly SmartEnergyDbContext _context;

    public NotificationController(SmartEnergyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? customerId)
    {
        var query = _context.Notifications.AsQueryable();

        if (customerId.HasValue && customerId.Value != Guid.Empty)
        {
            query = query.Where(n => n.CustomerId == customerId.Value);
        }

        var list = await query.OrderByDescending(n => n.CreatedAtUtc).ToListAsync();
        return Ok(list);
    }

    [HttpPost("{id:guid}/read")]
    public async Task<IActionResult> MarkRead(Guid id)
    {
        var notif = await _context.Notifications.FindAsync(id);
        if (notif == null) return NotFound();

        notif.MarkAsRead();
        await _context.SaveChangesAsync();
        return Ok(notif);
    }
}
