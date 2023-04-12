using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Webnhatro_1900123.Models;

namespace Webnhatro_1900123.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Base
    {
        public IActionResult Index()
        {
            ViewData["Hoten"] = HttpContext.Session.GetString("Hoten");
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
    }
}