using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Web.Services;

namespace SmartEnergy.Web.Controllers;

public class AccountController : Controller
{
    private readonly AuthApiService _authApiService;

    public AccountController(AuthApiService authApiService)
    {
        _authApiService = authApiService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (Request.Cookies["IsLoggedIn"] == "true")
        {
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string tcknVkn, string password)
    {
        if (string.IsNullOrWhiteSpace(tcknVkn) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.ErrorMessage = "Lütfen TCKN / VKN ve Şifrenizi giriniz.";
            return View();
        }

        var result = await _authApiService.LoginAsync(tcknVkn, password);

        if (result != null)
        {
            Response.Cookies.Append("UserName", result.FullName);
            Response.Cookies.Append("TcknVkn", result.TcknVkn);
            Response.Cookies.Append("UserEmail", result.Email);
            Response.Cookies.Append("CustomerId", result.CustomerId.ToString());
            Response.Cookies.Append("IsLoggedIn", "true");

            return RedirectToAction("Index", "Dashboard");
        }

        ViewBag.ErrorMessage = "TCKN / VKN veya Şifre hatalı.";
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> SendVerificationCode([FromBody] SendCodeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return Json(new { success = false, message = "E-posta adresi zorunludur." });
        }

        var res = await _authApiService.SendVerificationCodeAsync(dto.Email, dto.FullName ?? "Abonemiz");
        return Json(new { success = res.Success, message = res.Message, debugCode = res.DebugCode });
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName) ||
            string.IsNullOrWhiteSpace(model.TcknVkn) || string.IsNullOrWhiteSpace(model.Email) ||
            string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.VerificationCode))
        {
            ViewBag.ErrorMessage = "Lütfen tüm zorunlu alanları ve SMS/E-posta doğrulama kodunu eksiksiz doldurunuz.";
            return View(model);
        }

        var result = await _authApiService.RegisterAsync(model);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Kayıt işleminiz başarıyla tamamlandı. Belirlediğiniz şifre ile giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        ViewBag.ErrorMessage = result.Message;
        return View(model);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("UserName");
        Response.Cookies.Delete("TcknVkn");
        Response.Cookies.Delete("UserEmail");
        Response.Cookies.Delete("CustomerId");
        Response.Cookies.Delete("IsLoggedIn");
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Profile()
    {
        var userName = Request.Cookies["UserName"] ?? "Betül Demircan";
        var tcknVkn = Request.Cookies["TcknVkn"] ?? "12345678901";
        var email = Request.Cookies["UserEmail"] ?? "betul.demircan@kepsas.com.tr";

        ViewBag.UserName = userName;
        ViewBag.TcknVkn = tcknVkn;
        ViewBag.Email = email;
        ViewBag.Phone = "0532 555 12 34";
        ViewBag.CustomerNumber = "K-784920";

        return View();
    }
}

public class SendCodeDto
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
