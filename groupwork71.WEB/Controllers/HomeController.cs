using System.Diagnostics;
using groupwork71.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace groupwork71.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("RestaurantList", "Restaurant");
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
}