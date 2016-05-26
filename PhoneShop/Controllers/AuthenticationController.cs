using BusinessEntities;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ViewModels;

namespace PhoneShop.Controllers
{
    //用户认证模块
    public class AuthenticationController : Controller
    {
        // GET: 用户登录
        [HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }

        //POST:用户登录
        [HttpPost]
        public ActionResult Login(T_Users user)
        {
            if (ModelState.IsValid)
            {
                AuthBusinessLayer abl = new AuthBusinessLayer();
                int Uid = abl.IsValidUser(user);
                if (Uid != 0)
                {
                    //用户认证方法
                    FormsAuthentication.SetAuthCookie(user.Uname, true);
                    HttpCookie cookie = new HttpCookie("Uid");
                    cookie.Value = Convert.ToString(Uid);
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    ModelState.AddModelError("LoginError", "无效用户名或密码");
                    return View("Login");
                }
            }
            else
            {
                return View("Login");
            }

        }

        //GET：用户退出
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("../Home/Index");
        }

        //管理员登录
        [HttpGet]
        public ActionResult Admin()
        {
            return View("Admin");
        }

        //管理员登录权限认证
        [HttpPost]
        public ActionResult Admin(T_Admin admin)
        {
            AuthBusinessLayer abl = new AuthBusinessLayer();
            bool AuthenticatedAdmin = abl.IsValidAdmin(admin);
            if (AuthenticatedAdmin != false)
            {
                Session["IsAdmin"] = AuthenticatedAdmin;
                Session["AdminName"] = admin.AdminName;
            }
            else
            {
                Session["IsAdmin"] = AuthenticatedAdmin;
                return Content("<script>alert('登录失败');location.href='Admin'</script>");
            }
            return RedirectToAction("../Admin/Index");
        }
    }
}