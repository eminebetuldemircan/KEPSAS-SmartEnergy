using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Application.Interfaces;

namespace SmartEnergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // GET: api/Dashboard/summary
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] Guid meterId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var summary = await _dashboardService.GetSummaryAsync(
            meterId,
            startDate,
            endDate);

        return Ok(summary);
    }

    // GET: api/Dashboard/consumption
    [HttpGet("consumption")]
    public async Task<IActionResult> GetConsumptionChart(
        [FromQuery] Guid meterId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var data = await _dashboardService.GetConsumptionChartAsync(
            meterId,
            startDate,
            endDate);

        return Ok(data);
    }
}