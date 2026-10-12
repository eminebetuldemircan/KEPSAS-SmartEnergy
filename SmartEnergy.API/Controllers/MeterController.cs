using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Application.DTOs.Meters;
using SmartEnergy.Application.Services;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeterController : ControllerBase
{
    private readonly IMeterService _meterService;

    public MeterController(
        IMeterService meterService)
    {
        _meterService = meterService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var meters =
            await _meterService.GetAllAsync();

        return Ok(meters);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var meter =
            await _meterService.GetByIdAsync(id);

        if (meter is null)
        {
            return NotFound();
        }

        return Ok(meter);
    }

    [HttpGet("number/{meterNumber}")]
    public async Task<IActionResult> GetByMeterNumber(
        string meterNumber)
    {
        var meter =
            await _meterService
                .GetByMeterNumberAsync(meterNumber);

        if (meter is null)
        {
            return NotFound();
        }

        return Ok(meter);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMeterRequest request)
    {
        try
        {
            var meter =
                await _meterService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = meter.Id },
                new
                {
                    meter.Id,
                    meter.MeterNumber,
                    meter.SerialNumber,
                    Type = (int)meter.Type,
                    Status = (int)meter.Status,
                    meter.InstallationDate,
                    meter.LastReadingDate,
                    meter.LastIndex
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}