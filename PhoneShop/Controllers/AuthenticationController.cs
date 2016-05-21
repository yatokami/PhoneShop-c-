using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntities;
using ViewModels;
using System.Web.Security;

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

        [HttpGet]
        public ActionResult Admin()
        {
            return View("Admin");
        }

        [HttpPost]
        public ActionResult Admin(T_Admin admin)
        {
            AuthBusinessLayer abl = new AuthBusinessLayer();
            string AuthenticatedAdmin = abl.IsValidAdmin(admin);
            if (AuthenticatedAdmin != "")
            {
                Session["Isadmin"] = AuthenticatedAdmin; 
            }
            else
            {
                return Content("<script>alert('登录失败');location.href='Admin'</script>");
            }
            return RedirectToAction("../Admin/Index");
        }
    }
}