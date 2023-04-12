using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Webnhatro_1900123.Models;

namespace Webnhatro_1900123.Controllers
{
    
    public class HomeController : Controller
    {
        private QuanlynhatroContext _context;
        public HomeController(QuanlynhatroContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            IQueryable<Nhatrofull> query = (
              from NhaTro in _context.NhaTros
              join Xa in _context.Xas on NhaTro.Idxa equals Xa.Idxa
              join KhuVuc in _context.KhuVucs on Xa.IdkhuVuc equals KhuVuc.IdkhuVuc
              join Tinh in _context.Tinhs on KhuVuc.Idtinh equals Tinh.Idtinh
              select new Nhatrofull
              {
                  ObjNhaTro = NhaTro,
                  ObjXa = Xa,
                  ObjKhuVuc = KhuVuc,
                  ObjTinh = Tinh
              });
            IQueryable<TinTuc> top5 = (from TinTuc in _context.TinTucs orderby TinTuc.NgayDang descending
                                      select TinTuc).Take(5);
            ViewData["News"]=top5.ToList();
            int pageSize = 10;
            return View(await PaginatedList<Nhatrofull>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
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
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Messenger"] = "Bạn đã đăng xuất";
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Dangnhap(string username, string password)
        {

            var query = from Member in _context.Members
                        select Member;
            foreach (var member in query)
            {
                if (username == member.Username && password == member.Password)
                {
                    HttpContext.Session.SetString("user", member.Username);
                    HttpContext.Session.SetString("Fullname", member.HoTen);
                    TempData["Messenger"] = "Đăng nhập thành công," + member.HoTen;
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["thongbao"] = "Đăng nhập thất bại,kiểm tra lại thông tin";
            return View(TempData["thongbao"]);
        }
        public IActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Dangky(Member member)
        {
            member.Ngaythamgia = DateTime.Now;
            if (validuser(member.Username))
            {
                TempData["thongbao"] = $"tên đăng nhập {member.Username} đã tồn tại vui lòng chọn tên đăng nhập khác";
                return View();
            }
            else if (ModelState.IsValid)
            {
                _context.Members.Add(member);
                _context.SaveChanges();
                HttpContext.Session.SetString("user", member.Username);
                HttpContext.Session.SetString("Fullname", member.HoTen);
                TempData["Messenger"] = "Đăng nhập thành công," + member.HoTen;
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
        bool validuser(string username)
        {
            List<string> listusername = _context.Members.Select(s => s.Username).ToList();
            foreach (var item in listusername)
            {
                if (item == username)
                {
                   return true;
                }
            }
            return false;
        }
    }
}