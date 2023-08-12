using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using gajabi.Models;

namespace gajabi.Controllers;

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

        public IActionResult singin()
    {
        return View();
    }

        public IActionResult showproduct()
    {
        return View();
    }
    public IActionResult resetpass()
    {
        return View();
    }

    public IActionResult singup()
    {
        return View();
    }

        public IActionResult cartdesktop()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult test()
    {
        return View();
    }

}
