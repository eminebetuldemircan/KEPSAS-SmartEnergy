using Microsoft.AspNetCore.Mvc;

namespace SmartEnergy.Web.Controllers;

public class QuickAccessController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GuvenceBedeli()
    {
        return View();
    }

    public IActionResult Tazminat()
    {
        return View();
    }

    public IActionResult Randevu()
    {
        return View();
    }

    public IActionResult OdemeKanallari()
    {
        return View();
    }

    public IActionResult Evrak()
    {
        return View();
    }

    public IActionResult KacakIhbar()
    {
        return View();
    }

    public IActionResult BilgiEdinme()
    {
        return View();
    }

    public IActionResult ItirazSikayet()
    {
        return View();
    }

    public IActionResult Fesih()
    {
        return View();
    }

    public IActionResult AlacakIade()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SubmitForm(string formType, string title, string description)
    {
        TempData["SuccessMessage"] = $"{formType} talebiniz başarıyla oluşturulmuştur. Takip Numarası: BAS-2026-{Random.Shared.Next(100, 999)}";
        return RedirectToAction("Index", "Application");
    }
}
