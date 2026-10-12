using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEnergy.Domain.Entities;
using SmartEnergy.Infrastructure.Persistence;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly SmartEnergyDbContext _context;

    public AppointmentController(SmartEnergyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? customerId)
    {
        var query = _context.Appointments.AsQueryable();

        if (customerId.HasValue && customerId.Value != Guid.Empty)
        {
            query = query.Where(a => a.CustomerId == customerId.Value);
        }

        var appointments = await query
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        return Ok(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
    {
        var appNo = $"RND-2026-{Random.Shared.Next(100, 999)}";
        var appointment = new Appointment(
            appointmentNumber: appNo,
            serviceType: request.ServiceType,
            location: request.Location,
            appointmentDate: request.AppointmentDate,
            timeSlot: request.TimeSlot,
            customerId: request.CustomerId,
            notes: request.Notes
        );

        _context.Appointments.Add(appointment);

        var notif = new Notification(
            title: "Randevu Oluşturuldu",
            message: $"{appNo} numaralı {request.ServiceType} randevunuz {request.AppointmentDate:dd.MM.yyyy} - {request.TimeSlot} tarihine oluşturulmuştur.",
            type: "Randevu",
            customerId: request.CustomerId
        );
        _context.Notifications.Add(notif);

        await _context.SaveChangesAsync();

        return Ok(appointment);
    }
}

public class CreateAppointmentRequest
{
    public Guid CustomerId { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
