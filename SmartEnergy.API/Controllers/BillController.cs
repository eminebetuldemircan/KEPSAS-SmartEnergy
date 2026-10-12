using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Application.DTOs.Bills;
using SmartEnergy.Application.Services;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillController : ControllerBase
{
    private readonly IBillService _billService;

    public BillController(IBillService billService)
    {
        _billService = billService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bills = await _billService.GetAllAsync();

        return Ok(bills);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var bill = await _billService.GetByIdAsync(id);

        if (bill is null)
            return NotFound();

        return Ok(bill);
    }

    [HttpGet("number/{billNumber}")]
    public async Task<IActionResult> GetByBillNumber(
        string billNumber)
    {
        var bill = await _billService
            .GetByBillNumberAsync(billNumber);

        if (bill is null)
            return NotFound();

        return Ok(bill);
    }

    [HttpGet("subscription/{subscriptionId:guid}")]
    public async Task<IActionResult> GetBySubscriptionId(
        Guid subscriptionId)
    {
        var bills = await _billService
            .GetBySubscriptionIdAsync(subscriptionId);

        return Ok(bills);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateBillRequest request)
    {
        var bill = await _billService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = bill.Id },
            bill);
    }
}