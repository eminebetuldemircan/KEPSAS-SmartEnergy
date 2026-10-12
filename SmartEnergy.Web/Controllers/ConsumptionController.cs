using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Web.Models;
using SmartEnergy.Web.Services;

public class ConsumptionController : Controller
{
    private readonly ConsumptionRecordApiService
        _consumptionRecordApiService;

    private readonly MeterApiService
        _meterApiService;

    public ConsumptionController(
        ConsumptionRecordApiService consumptionRecordApiService,
        MeterApiService meterApiService)
    {
        _consumptionRecordApiService =
            consumptionRecordApiService;

        _meterApiService =
            meterApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        Guid? meterId,
        DateTime? startDate,
        DateTime? endDate)
    {
        var meters =
            await _meterApiService.GetAllAsync();

        var records =
            await _consumptionRecordApiService.GetAllAsync();

        if (meterId.HasValue)
        {
            records = records
                .Where(x => x.MeterId == meterId.Value)
                .ToList();
        }

        if (startDate.HasValue)
        {
            records = records
                .Where(x => x.ReadingDate.Date >= startDate.Value.Date)
                .ToList();
        }

        if (endDate.HasValue)
        {
            records = records
                .Where(x => x.ReadingDate.Date <= endDate.Value.Date)
                .ToList();
        }

        var model = new ConsumptionFilterViewModel
        {
            MeterId = meterId,
            StartDate = startDate,
            EndDate = endDate,
            Meters = meters,
            Records = records
        };

        return View(model);
    }
}