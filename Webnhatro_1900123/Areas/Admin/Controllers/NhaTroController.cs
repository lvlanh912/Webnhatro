using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webnhatro_1900123.Models;
using System.Web;
using System.IO;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.Net.Http.Headers;

namespace Webnhatro_1900123.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhaTroController : Base
    {
       
        private readonly QuanlynhatroContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _host;
        [Obsolete]
        public NhaTroController(QuanlynhatroContext context, IHostingEnvironment host)
        {
            _host = host;
            _context = context;
        }
        String getdiachi(string id)
        {
            string diachi="";
            var query = (
              from NhaTro in _context.NhaTros
              join Xa in _context.Xas on NhaTro.Idxa equals Xa.Idxa
              join KhuVuc in _context.KhuVucs on Xa.IdkhuVuc equals KhuVuc.IdkhuVuc
              join Tinh in _context.Tinhs on KhuVuc.Idtinh equals Tinh.Idtinh
              where NhaTro.Idtro == id
              select new
              {
                  Xa = Xa.TenXa,
                  KhuVuc = KhuVuc.TenKhuVuc,
                  Tinh = Tinh.TenTinh
              });
            foreach(var item in query.ToList())
            {
                diachi = $"{item.Xa}-{item.KhuVuc}-{item.Tinh}";
            }
            return diachi;
        }
       List<Tinh> GetTinh()
        {
            var query = from Tinh in _context.Tinhs select Tinh;
            return query.ToList();
        }
        List<KhuVuc> GetKhuVuc(string idtinh)
        {
           // List<KhuVuc> kv = new List<KhuVuc>();
            var Khuvuc = from KhuVuc in _context.KhuVucs
                                where KhuVuc.Idtinh == idtinh
                                select KhuVuc;
            return Khuvuc.ToList();
            
        }
    // GET: NhaTro
    public async Task<IActionResult> Index( string? Search, int? pageNumber,string? price, string? sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["UserName"] = sortOrder=="username"? "username_desc" : "username";
            ViewData["Gia"] = sortOrder == "gia" ? "gia_desc" : "gia";
            ViewData["Dientich"] = sortOrder == "dientich" ? "dientich_desc" : "dientich";
            ViewData["Date"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["Search"] = Search;
            if (Search != null)
            {
                pageNumber = 1;
            }
            ViewData["CurrentFilter"] = Search;

            IQueryable<Nhatrofull> query = (
              from NhaTro in _context.NhaTros
              join Xa in _context.Xas on NhaTro.Idxa equals Xa.Idxa
              join KhuVuc in _context.KhuVucs on Xa.IdkhuVuc equals KhuVuc.IdkhuVuc
              join Tinh in _context.Tinhs on KhuVuc.Idtinh equals Tinh.Idtinh
              select new Nhatrofull 
              {
                  ObjNhaTro=NhaTro,
                  ObjXa = Xa,
                  ObjKhuVuc = KhuVuc,
                  ObjTinh = Tinh
              });
            if (!string.IsNullOrEmpty(price))
            {
                switch (price)
                {
                    case "1":
                        query = query.Where(s => s.ObjNhaTro.Gia <= 1000000);
                        break;
                    case "2":
                        query = query.Where(s => s.ObjNhaTro.Gia > 1000000 && s.ObjNhaTro.Gia<=3000000);
                        break;
                    case "3":
                        query = query.Where(s => s.ObjNhaTro.Gia >3000000 && s.ObjNhaTro.Gia<=7000000);
                        break;
                    case "4":
                        query = query.Where(s => s.ObjNhaTro.Gia >7000000);
                        break;
                }
            }
            if (!String.IsNullOrEmpty(Search))//nếu có querystring search=
            {
                query = query.Where(s => s.ObjNhaTro.TieuDe.Contains(Search)
                                       || s.ObjXa.TenXa.Contains(Search)||s.ObjNhaTro.Username.Contains(Search));
                //tìm kiếm bằng tiêu đề, username, giá,diện tích
            }
            switch (sortOrder)
            {
                case "username_desc":
                    query = query.OrderByDescending(s => s.ObjNhaTro.Username);
                    break;
                case "username":
                    query = query.OrderBy(s => s.ObjNhaTro.Username);
                    break;
                case "date":
                    query = query.OrderBy(s => s.ObjNhaTro.Thoigiandang);
                    break;
                case "date_desc":
                    query = query.OrderByDescending(s => s.ObjNhaTro.Thoigiandang);
                    break;
                case "gia":
                    query = query.OrderBy(s => s.ObjNhaTro.Gia);
                    break;
                case "gia_desc":
                    query = query.OrderByDescending(s => s.ObjNhaTro.Gia);
                    break;
                case "dientich":
                    query = query.OrderBy(s => s.ObjNhaTro.Dientich);
                    break;
                case "dientich_desc":
                    query = query.OrderByDescending(s => s.ObjNhaTro.Dientich);
                    break;
                default:
                    query = query.OrderBy(s => s.ObjNhaTro.Idtro);
                    break;
            }
            int pageSize = 5;
            //var quanlynhatroContext = _context.NhaTros.Include(n => n.IdxaNavigation).Include(n => n.UsernameNavigation);
            //ViewData["full"] = full();
            //return View(await quanlynhatroContext.ToListAsync());
              //return View();
              
            return View(await PaginatedList <Nhatrofull>.CreateAsync(query.AsNoTracking(),pageNumber??1,pageSize));
        }

        // GET: NhaTro/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.NhaTros == null)
            {
                return NotFound();
            }
            ViewData["DiaChi"] = getdiachi(id);
            var nhaTro = await _context.NhaTros
                .Include(n => n.IdxaNavigation)
                .Include(n => n.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.Idtro == id);
            if (nhaTro == null)
            {
                return NotFound();
            }

            return View(nhaTro);
        }

        // GET: NhaTro/Create
        public IActionResult Create()
        {
            ViewData["Username"]=new SelectList(_context.Members, "Username","HoTen");
             return View();
        }

        // POST: NhaTro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create(NhaTro nhaTro,List<IFormFile> files )
        {
            nhaTro.Thoigiandang = DateTime.Now;
            var Nhatrolast = _context.NhaTros.OrderByDescending(s => s.Idtro).Take(1).ToList();//lấy Phòng trọ có số ID lớn nhất
            nhaTro.Idtro = "TRO" + Convert.ToString(Convert.ToInt32(Nhatrolast[0].Idtro.Replace("TRO", "")) + 1);//gán Id trọ = TRO+số ID lớn nhất +1
            //thêm ảnh vào thư mục upload
            if (files != null&& nhaTro.Idtro != null && nhaTro.Idxa != null &&
               nhaTro.TieuDe != null && nhaTro.Thongtin != null &&
               nhaTro.Gia != 0 && nhaTro.Dientich != 0)
            {
                int i = 1;
                foreach (var file in files)
                {
                    string filename = file.FileName;
                    string fileextension = Path.GetExtension(filename);
                    filename = nhaTro.Idtro + "_Anh" + i.ToString() + fileextension;
                    var pathsave = Path.Combine(_host.WebRootPath + @"\Upload\NhaTro", filename);//đường đãn lưu file  _host.WebRootPath:chỉ wwwroot 
                    if (System.IO.File.Exists(pathsave))//nếu đã tồn tại-->xoá-->lưu
                    {
                        System.IO.File.SetAttributes(pathsave, FileAttributes.Normal);
                        System.IO.File.Delete(pathsave);
                        using var stream = new FileStream(pathsave, FileMode.Create);
                        file.CopyTo(stream);
                    }
                    else//chưa có thực hiện lưu vào thư mục
                    {
                        using var stream = new FileStream(pathsave, FileMode.Create);
                        file.CopyTo(stream);
                        
                    }
                    nhaTro.Anh += $"{filename};";
                    i++;
                }
                    _context.Add(nhaTro);
                   await _context.SaveChangesAsync();
                 ViewBag.Messenger = "success";

                return  RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["Username"] = new SelectList(_context.Members, "Username", "Username");
                TempData["Thongbao"] = "Lỗi! vui lòng kiểm tra lại dữ liệu nhập";
                    return View();
            }
          
        }
    
        // GET: NhaTro/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.NhaTros == null)
            {
                return NotFound();
            }
            ViewData["DiaChi"] = getdiachi(id);
            var nhaTro = await _context.NhaTros.FindAsync(id);
            IQueryable getmember=from Member in _context.Members select Member;
            ViewData["Member"] = getmember;
            if (nhaTro == null)
            {
                return NotFound();
            }
            return View(nhaTro);
        }

        // POST: NhaTro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Edit(string id, NhaTro nhaTro, List<IFormFile> files)
        {
            
            if (id != nhaTro.Idtro)
            {
                return NotFound();
            }

            //thêm ảnh vào thư mục upload
            if (files.Count != 0)//nếu có ảnh được up thì xoá ảnh cũ thêm ảnh mới
            {
                int i = 1;
                foreach (var file in files)
                {
                    string filename = file.FileName;
                    string fileextension = Path.GetExtension(filename);
                    filename = nhaTro.Idtro + "_Anh" + i.ToString() + fileextension;
                    var pathsave = Path.Combine(_host.WebRootPath + @"\Upload\NhaTro", filename);//đường đãn lưu file  _host.WebRootPath:chỉ wwwroot 
                    if (System.IO.File.Exists(pathsave))
                    {
                        System.IO.File.Delete(pathsave);
                        using var stream = new FileStream(pathsave, FileMode.Create);
                        file.CopyTo(stream);
                    }
                    else
                    {
                        using var stream = new FileStream(pathsave, FileMode.Create); 
                        file.CopyTo(stream);
                    }
                    nhaTro.Anh += $"{filename};";
                    i++;
                }
            }
            else//nếu không upload ảnh thì giữ nguyên
            {
                var oldnhatro= await _context.NhaTros.FindAsync(id);
                if(oldnhatro!=null)
                    nhaTro.Anh = oldnhatro.Anh;
            }
            nhaTro.Thoigiandang = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaTro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaTroExists(nhaTro.Idtro))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Username"] = new SelectList(_context.Members, "Username", "Username", nhaTro.Username);
            return View(nhaTro);
        }
        // POST: NhaTro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.NhaTros == null)
            {
                return Problem("Entity set 'QuanlynhatroContext.NhaTros'  is null.");
            }
            var nhaTro = await _context.NhaTros.FindAsync(id);
            if (nhaTro != null&&nhaTro.Anh!=null)
            {
               
                string[] anhs = nhaTro.Anh.Split(';');
                foreach (string anh in anhs)
                {
                    var pathsave = Path.Combine(_host.WebRootPath + @"\Upload\NhaTro", anh);
                    if (System.IO.File.Exists(pathsave))
                    {
                        System.IO.File.SetAttributes(pathsave, System.IO.FileAttributes.Normal);
                        System.IO.File.Delete(pathsave);
                    }
                   
                }
                _context.NhaTros.Remove(nhaTro);

            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhaTroExists(string id)
        {
          return _context.NhaTros.Any(e => e.Idtro == id);
        }

    }


}
