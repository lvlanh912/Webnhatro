using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webnhatro_1900123.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Webnhatro_1900123.Controllers
{
    public class HouseController : Controller
    {
        private readonly QuanlynhatroContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _host;
        [Obsolete]
        public HouseController(QuanlynhatroContext context, IHostingEnvironment host)
        {
            _host = host;
            _context = context;
            
        }
        public async Task<IActionResult> Index(string key, int? pageNumber, string? price, string? S, string? sortOrder, string? T, string? Kv, string? x)
        {

            ViewData["T"] = T;
            ViewData["Kv"] = Kv;
            ViewData["x"] = x;
            ViewData["S"] = S;
            ViewData["price"] = price;
            ViewData["sortOrder"] = sortOrder;
            //if (string.IsNullOrEmpty(Search)) ;
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
            if (!string.IsNullOrEmpty(T))
            {
                query = query.Where(s => s.ObjTinh.Idtinh == T);
            }
            if (!string.IsNullOrEmpty(Kv))
            {
                query = query.Where(s => s.ObjKhuVuc.IdkhuVuc == Kv);
            }     
            if (!string.IsNullOrEmpty(x))
            {
                query = query.Where(s => s.ObjXa.Idxa == x);
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "moinhat":
                        query = query.OrderByDescending(s => s.ObjNhaTro.Thoigiandang);
                        break;
                    case "price":
                        query = query.OrderBy(s => s.ObjNhaTro.Gia);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(s => s.ObjNhaTro.Gia);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(price))
            {
                switch (price)
                {
                    case "1":
                        query = query.Where(s => s.ObjNhaTro.Gia < 1000000);
                        break;
                    case "2":
                        query = query.Where(s => s.ObjNhaTro.Gia >= 1000000 &&s.ObjNhaTro.Gia<3000000);
                        break;
                    case "3":
                        query = query.Where(s => s.ObjNhaTro.Gia >= 3000000 && s.ObjNhaTro.Gia<5000000);
                        break;
                    case "4":
                        query = query.Where(s => s.ObjNhaTro.Gia >= 5000000 && s.ObjNhaTro.Gia<7000000);
                        break;
                    case "5":
                        query = query.Where(s => s.ObjNhaTro.Gia > 7000000);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(S))
            {
                switch (S)
                {
                    case "1":
                        query = query.Where(s => s.ObjNhaTro.Dientich < 20);
                        break;
                    case "2":
                        query = query.Where(s => s.ObjNhaTro.Dientich >= 20&&s.ObjNhaTro.Dientich<40);
                        break;
                    case "3":
                        query = query.Where(s => s.ObjNhaTro.Dientich >=40 &&s.ObjNhaTro.Dientich<60);
                        break;
                    case "4":
                        query = query.Where(s => s.ObjNhaTro.Dientich > 60);
                        break;
                }
               
            }
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(s => s.ObjNhaTro.TieuDe.Contains(key) || s.ObjNhaTro.Thongtin.Contains(key)
                || s.ObjKhuVuc.TenKhuVuc.Contains(key) || s.ObjXa.TenXa.Contains(key));
            }
                int pageSize = 10;
            return View(await PaginatedList<Nhatrofull>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = from NhaTro in _context.NhaTros
                       where NhaTro.Idtro == id
                       join Xa in _context.Xas on NhaTro.Idxa equals Xa.Idxa
                       join KhuVuc in _context.KhuVucs on Xa.IdkhuVuc equals KhuVuc.IdkhuVuc
                       join Tinh in _context.Tinhs on KhuVuc.Idtinh equals Tinh.Idtinh
                       join Member in _context.Members on NhaTro.Username equals Member.Username
                       select new Nhatrofull
                       {
                           ObjNhaTro = NhaTro,
                           ObjXa = Xa,
                           ObjKhuVuc = KhuVuc,
                           ObjTinh = Tinh,
                           objMember = Member
                       };
            var query = (from NhaTro in _context.NhaTros
                         where NhaTro.Idtro != id
                         join Xa in _context.Xas on NhaTro.Idxa equals Xa.Idxa
                         join KhuVuc in _context.KhuVucs on Xa.IdkhuVuc equals KhuVuc.IdkhuVuc
                         join Tinh in _context.Tinhs on KhuVuc.Idtinh equals Tinh.Idtinh
                         select new Nhatrofull
                         {
                             ObjNhaTro = NhaTro,
                             ObjXa = Xa,
                             ObjKhuVuc = KhuVuc,
                             ObjTinh = Tinh
                         }).Take(5).ToList();
            ViewData["Samelocation"] = query;

            if (item != null)
            {
                return View(item.First());
            }
            return NotFound();

        }



    }
}
