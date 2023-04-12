using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webnhatro_1900123.Models;

namespace Webnhatro_1900123.Controllers
{
    public class NewsController : Controller
    {
        private readonly QuanlynhatroContext _context;
        public NewsController(QuanlynhatroContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var query = from TinTuc in _context.TinTucs
                        orderby TinTuc.NgayDang descending
                        select TinTuc;
           int pageSize = 10;
            return View(await PaginatedList<TinTuc>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> Chitiet(string id)
        {
          var objtintuc= await _context.TinTucs.FindAsync(id);
            var query= _context.TinTucs.Where(s => s.Idnews != id).Take(5).ToList();

            ViewBag.List = query;
            if (objtintuc == null)
            {
                return NotFound();
            }
            return View(objtintuc);
        }
       
           
    }
}
