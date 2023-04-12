using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webnhatro_1900123.Models;


namespace Webnhatro_1900123.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class MemberController : Base
    {
        private readonly QuanlynhatroContext _context;

        public MemberController(QuanlynhatroContext context)
        {
            _context = context;
        }
        

        // GET: Member
        public async Task<IActionResult> Index( string sortOrder,string Search,string filter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["UserName"]= String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewData["Name"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["Date"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["search"] = Search;
            if (Search != null)
            {
                pageNumber = 1;
            }
            else
            {
                Search = filter;
            }
            ViewData["CurrentFilter"] = Search;
            //var query = (from Member in _context.Members
            //select Member).OrderBy(x => x.HoTen);
            var query = from Member in _context.Members
                         select Member;
            if (!String.IsNullOrEmpty(Search))//nếu có querystring search=
            {
                query = _context.Members.Where(s => s.HoTen.Contains(Search)
                                       || s.Username.Contains(Search)
                                       || s.Email.Contains(Search));
                //tìm kiếm bằng họ tên, username, email thành viên
            }
            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(s => s.HoTen);
                    break;
                case "name":
                    query = query.OrderBy(s => s.HoTen);
                    break;
                case "date":
                    query = query.OrderBy(s => s.Ngaythamgia);
                    break;
                case "date_desc":
                    query = query.OrderByDescending(s => s.Ngaythamgia);
                    break;
                case "username_desc":
                    query = query.OrderByDescending(s => s.Username);
                    break;
                default:
                    query = query.OrderBy(s => s.Username);
                    break;
            }
           
            int pageSize = 5;
            //int pageNumber = (page ?? 1);
            //return View(query.ToList());

           return View(await PaginatedList<Member>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        // GET: Member/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Username == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Member/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (member.HoTen != null && member.Password != null && member.Username != null && member.Diachi != null && member.Email != null
                && member.Phone != null)
            {
                member.Ngaythamgia = DateTime.Now;
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else {
                TempData["Thongbao"] = "Lỗi! vui lòng kiểm tra lại dữ liệu nhập";
                return View();
            }
               
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,Password,Phone,Email,HoTen,Diachi,Ngaythamgia")] Member member)
        {

            if (id != member.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Username))
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
            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.Members == null)
            {
                return Problem("Entity set 'QuanlynhatroContext.Members'  is null.");
            }
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(string id)
        {
          return _context.Members.Any(e => e.Username == id);
        }
    }
}
