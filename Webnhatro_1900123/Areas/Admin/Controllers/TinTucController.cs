using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webnhatro_1900123.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Webnhatro_1900123.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TinTucController : Base
    {
        
        private readonly QuanlynhatroContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public TinTucController(QuanlynhatroContext context,IHostingEnvironment host)
        {
           _host = host;
            _context = context;
        }

        // GET: TinTuc
        public async Task<IActionResult> Index(string sortOrder, string Search, string filter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["Date"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["Tieude"] = sortOrder == "title" ? "title_desc" : "title";
            ViewData["Search"] = Search;
            if (Search != null)
            {
                pageNumber = 1;
            }
            else
            {
                Search = filter;
            }
            ViewData["CurrentFilter"] = Search;

            var query = from TinTuc in _context.TinTucs
                                      select TinTuc;
            if (!String.IsNullOrEmpty(Search))//nếu có querystring search=
            {
                query = query.Where(s => s.Title.Contains(Search)
                                       || s.NoiDung.Contains(Search));
                //tìm kiếm bằng Title, nội dung
            }
            switch (sortOrder)// mặc định sx theo ngày đăng
            {
                case "date_desc":
                    query = query.OrderByDescending(s => s.NgayDang); break;
                case "title_desc":
                    query = query.OrderByDescending(s => s.Title); break;
                case "title":
                    query = query.OrderBy(s => s.Title); break;
                default:
                    query = query.OrderBy(s => s.NgayDang);break;
            }
            int pageSize = 5;
            return View(await PaginatedList<TinTuc>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
            //return View(await query.ToListAsync());
        }



        // GET: TinTuc/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TinTucs == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs.FirstOrDefaultAsync(m => m.Idnews == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

        // GET: TinTu/Create
        public IActionResult Create()
        {
                return View();
        }

        // POST: TinTu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create( TinTuc tinTuc,List<IFormFile> files)
        {
            if (tinTuc.Title!=null&&tinTuc.NoiDung!=null&&files!=null) 
            { 
                var tintuclast = _context.TinTucs.OrderByDescending(s => s.NgayDang).Take(1).ToList();//lấy tin tức có số ID lớn nhất
                tinTuc.Idnews = "News" + Convert.ToString(Convert.ToInt32(tintuclast[0].Idnews.Replace("News", "")) + 1);//gán Id tintuc = News+số ID lớn nhất +1
                tinTuc.NgayDang = DateTime.Now;
                    //lưu ảnh vào thư mục 
                    foreach (var file in files)
                    {
                        string filename = file.FileName;
                        string fileextension = Path.GetExtension(filename);
                        filename = tinTuc.Idnews + "_Anh" + fileextension;
                        var pathsave = Path.Combine(_host.WebRootPath + @"\Upload\News", filename);//đường đãn lưu file  _host.WebRootPath:chỉ wwwroot 
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
                        tinTuc.Anh += $"{filename}";

                    }
                    //
                        _context.Add(tinTuc);
                        await _context.SaveChangesAsync();
                        TempData["Thongbao"] = "Đã thêm thành công";
                        return RedirectToAction(nameof(Index));
            }
            else
            TempData["Thongbao"] = "Thất bại, kiểm tra lại dữ liệu nhập";
            return View(tinTuc);
        }

        // GET: TinTu/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TinTucs == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs.FindAsync(id);
            if (tinTuc == null)
            {
                return NotFound();
            }
            return View(tinTuc);
        }

        // POST: TinTu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Edit(string id, [Bind("Idnews,Title,NoiDung")] TinTuc tinTuc, List<IFormFile> files)
        {
            tinTuc.NgayDang = DateTime.Now;
            //lưu ảnh vào thư mục 
            int i = 1;
            if (files.Count != 0)
            {
                foreach (var file in files)
                {
                    string filename = file.FileName;
                    string fileextension = Path.GetExtension(filename);
                    filename = tinTuc.Idnews + "_Anh" + i.ToString() + fileextension;
                    var pathsave = Path.Combine(_host.WebRootPath + @"\Upload\News", filename);//đường đãn lưu file  _host.WebRootPath:chỉ wwwroot 
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
                    tinTuc.Anh = $"{filename}";
                    i++;
                }
            }
            else//nếu không có ảnh upload
            {
                var oldnews= await _context.TinTucs.FindAsync(id);//lấy tintuc dang sửa
                if(oldnews!=null)
                    tinTuc.Anh = oldnews.Anh;//gán tên ảnh bằng tên cũ;
            }
           
            //
            if (id != tinTuc.Idnews)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tinTuc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TinTucExists(tinTuc.Idnews))
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
            return View(tinTuc);
        }
        // POST: TinTu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.TinTucs == null)
            {
                return Problem("Entity set 'QuanlynhatroContext.TinTucs'  is null.");
            }
            var tinTuc = await _context.TinTucs.FindAsync(id);
            if (tinTuc != null)
            {   
                if(tinTuc.Anh != null)
                {
                    var pathsave = Path.Combine(_host.WebRootPath + @"\Upload\News", tinTuc.Anh);
                        System.IO.File.SetAttributes(pathsave, System.IO.FileAttributes.Normal);
                        System.IO.File.Delete(pathsave);
                }
                _context.TinTucs.Remove(tinTuc);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        string format(string data)
        {
            data = data.Replace(';', ';');
            return data;
        }
        private bool TinTucExists(string id)
        {
          return _context.TinTucs.Any(e => e.Idnews == id);
        }
    }
}
