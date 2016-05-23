using BusinessEntities;
using BusinessLayer;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace PhoneShop.Controllers
{

    //[AdminFilter]
    public class AdminController : Controller
    {
        // GET: Admin
        //用户列表查询
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
        //用户列表显示
        public ActionResult User_Password(string Uname, int pageIndex = 1)
        {
            AdminBusinessLayer abl = new AdminBusinessLayer();

            List<UserView> luv = abl.GetUsers(Uname);
            AdminListView alv = new AdminListView();

            UserViews.data = luv;
            PageList<UserView> StudentPaging = new PageList<UserView>(10, UserViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页
            alv.Users = StudentPaging;
            alv.AdminUser = User.Identity.Name;


            return View("User_Password", alv);
        }
        //密码修改
        public ActionResult Changepwd(string Uname, string Upwd)
        {
            AdminBusinessLayer abl = new AdminBusinessLayer();

            bool status = abl.Changepwd(Uname, Upwd);

            return Json(status);

        }
        [HttpGet]
        public ActionResult Add_Goods()
        {
            AdminListView alv = new AdminListView();
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            List<GoodsTypeView> lgv = gbl.GetGoodType();

            alv.GoodsType = lgv;
            alv.AdminUser = User.Identity.Name;
            return View(alv);
        }
        public ActionResult Add_Goods(T_Goods goods, HttpPostedFileBase GoodsPicture)
        {


            return View();
        }


    }
}