using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Application.DTOs.ConsumptionRecords;
using SmartEnergy.Application.Services;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsumptionRecordController
    : ControllerBase
{
    private readonly IConsumptionRecordService _service;

    public ConsumptionRecordController(
        IConsumptionRecordService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records =
            await _service.GetAllAsync();

        return Ok(records);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var record =
            await _service.GetByIdAsync(id);

        if (record is null)
        {
            return NotFound();
        }

        return Ok(record);
    }

    [HttpGet("meter/{meterId:guid}")]
    public async Task<IActionResult> GetByMeterId(
        Guid meterId)
    {
        var records =
            await _service.GetByMeterIdAsync(meterId);

        return Ok(records);
    }

    [HttpGet("meter/{meterId:guid}/date-range")]
    public async Task<IActionResult> GetByDateRange(
        Guid meterId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest(
                "Başlangıç tarihi bitiş tarihinden büyük olamaz.");
        }

        var records =
            await _service.GetByDateRangeAsync(
                meterId,
                startDate,
                endDate);

        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateConsumptionRecordRequest request)
    {
        try
        {
            var record =
                await _service.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = record.Id },
                new
                {
                    record.Id,
                    record.MeterId,
                    record.ReadingDate,
                    record.ConsumptionKwh,
                    record.MeterIndex,
                    record.IsEstimated
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}