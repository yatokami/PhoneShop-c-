using BusinessLayer;
using System.Collections.Generic;
using System.Web.Mvc;
using ViewModels;

namespace PhoneShop.Controllers
{

    //[AdminFilter]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(string Uname, int pageIndex = 1)
        {
            AdminBusinessLayer abl = new AdminBusinessLayer();

            List<UserView> luv = abl.GetUsers(Uname);
            AdminListView alv = new AdminListView();

            UserViews.data = luv;
            PageList<UserView> StudentPaging = new PageList<UserView>(10, UserViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页

            alv.Users = StudentPaging;
            alv.AdminUser = User.Identity.Name;

            return View("Index", alv);
        }
    }
}