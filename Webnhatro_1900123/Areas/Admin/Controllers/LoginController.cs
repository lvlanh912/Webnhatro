using Microsoft.AspNetCore.Mvc;
using Webnhatro_1900123.Models;
using Webnhatro_1900123.Areas.Admin;

namespace Webnhatro_1900123.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly QuanlynhatroContext _context;
       public LoginController(QuanlynhatroContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]

        public IActionResult Index(string username,string password)
        {
          
                var query=from Admin in _context.Admins
                          select Admin;
                   foreach(var admin in query)
                    {
                    if(username==admin.Username && password == admin.Pass)
                    {
                    HttpContext.Session.SetString("useradmin", admin.Username);
                    HttpContext.Session.SetString("Hoten", admin.HoTen);
                    return RedirectToAction("Index", "Home");
                    }
                    }
            TempData["thongbao"] = "Đang nhập thất bại";
            return View(TempData["thongbao"]);
        }
    }
}
