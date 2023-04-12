using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Webnhatro_1900123.Controllers
{
    public class sessioncheck : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)//kiểm tra session dùng cho mỗi khi vào khu vực Admin
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("user")))//nếu không có session user
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", Action = "Dangnhap"}));
            }
            base.Accepted();//cho phép tiếp tục hoạt động
        }
    }
}
