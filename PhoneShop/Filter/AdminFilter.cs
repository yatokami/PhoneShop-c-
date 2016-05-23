using System;
using System.Web.Mvc;

namespace PhoneShop.Filter
{
    public class AdminFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Convert.ToBoolean(filterContext.HttpContext.Session["IsAdmin"]))
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "<script>alert('请先登录管理员账号');location.href='/Auth/Admin'</script>"
                };
            }
        }
    }
}