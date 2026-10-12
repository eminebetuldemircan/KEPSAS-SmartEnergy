using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Web.Models;
using SmartEnergy.Web.Services;

public class MeterController : Controller
{
    private readonly MeterApiService _meterApiService;
    private readonly ConsumptionRecordApiService _consumptionRecordApiService;

    public MeterController(
        MeterApiService meterApiService,
        ConsumptionRecordApiService consumptionRecordApiService)
    {
        _meterApiService = meterApiService;
        _consumptionRecordApiService = consumptionRecordApiService;
    }

    public async Task<IActionResult> Index()
    {
        var meters = await _meterApiService.GetAllAsync();

        return View(meters);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMeterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _meterApiService.CreateAsync(model);

        if (!result)
        {
            ModelState.AddModelError(
                "",
                "Sayaç eklenirken bir hata oluştu.");

            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var meter = await _meterApiService.GetByIdAsync(id);

        if (meter is null)
        {
            return NotFound();
        }

        var readings =
            await _consumptionRecordApiService.GetByMeterIdAsync(id);

        ViewBag.Readings = readings;

        return View(meter);
    }

    [HttpGet]
    public IActionResult Reading(Guid meterId)
    {
        var model = new CreateConsumptionRecordViewModel
        {
            MeterId = meterId,
            ReadingDate = DateTime.Now
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Reading(
        CreateConsumptionRecordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result =
            await _consumptionRecordApiService.CreateAsync(model);

        if (!result)
        {
            ModelState.AddModelError(
                "",
                "Sayaç okuması kaydedilirken bir hata oluştu.");

            return View(model);
        }

        return RedirectToAction(
            nameof(Details),
            new { id = model.MeterId });
    }
}