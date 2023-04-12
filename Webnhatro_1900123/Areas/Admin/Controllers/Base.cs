using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Webnhatro_1900123.Areas.Admin.Controllers
{
    public class Base: Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)//kiểm tra session dùng cho mỗi khi vào khu vực Admin
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("useradmin")))//nếu không có session
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Login", Action = "Index", Areas = "Admin" }));
            }
            base.Accepted();//cho phép tiếp tục hoạt động
        }
    }
}
