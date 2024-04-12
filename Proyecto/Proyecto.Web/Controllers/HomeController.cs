using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Web.Models;
using System.Diagnostics;

namespace Proyecto.Web.Controllers;

[Authorize(Roles = "Admin,Procesos,Reportes")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult ErrorHandler(string messagesJson)
    {
        var errorMessages = JsonConvert.
            DeserializeObject<ErrorMiddlewareViewModel>(messagesJson);
        ViewBag.ErrorMessages = errorMessages;
        return View("ErrorHandler");
    }
}
