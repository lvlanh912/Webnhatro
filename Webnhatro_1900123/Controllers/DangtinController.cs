using Microsoft.AspNetCore.Mvc;
using Webnhatro_1900123.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Webnhatro_1900123.Controllers
{
    public class DangtinController : sessioncheck
    {
        private QuanlynhatroContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public DangtinController(QuanlynhatroContext context, IHostingEnvironment host)
        {
            _host = host;
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public IActionResult Index(NhaTro nhaTro, List<IFormFile> files)
        {

            nhaTro.Thoigiandang = DateTime.Now;
            var Nhatrolast = _context.NhaTros.OrderByDescending(s => s.Idtro).Take(1).ToList();//lấy Phòng trọ có số ID lớn nhất
            if (Nhatrolast.Count == 0)
                nhaTro.Idtro = "TRO1";
            else
            nhaTro.Idtro = "TRO" + Convert.ToString(Convert.ToInt32(Nhatrolast[0].Idtro.Replace("TRO", "")) + 1);//gán Id trọ = TRO+số ID lớn nhất +1
            nhaTro.Username = HttpContext.Session.GetString("user");
            ModelState.Remove("Idtro");//không cần kiểm tra key Idtro có hợp lệ hay k
                                       //thêm ảnh vào thư mục upload
            if (files != null && ModelState.IsValid)
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
                _context.SaveChanges();//lưu database
                TempData["Messenger"] = "Tin của bạn đã được đăng thành công";
                return RedirectToAction("Index", "House");
            }
            else
            {
                TempData["Thongbao"] = "Lỗi! vui lòng kiểm tra lại dữ liệu nhập";
                return View();
            }

        }
    }
}
