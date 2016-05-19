using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using BusinessEntities;
using System.Reflection;

namespace PhoneShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        //网站主页显示
   
        public ActionResult Index(int? TypeID, int pageIndex = 1)
        {
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            UserListView ulv = new UserListView();
            List<GoodsTypeView> gtv = gbl.GetTypes();
            List<GoodsView> lgv = gbl.GetGoods(TypeID);

            GoodsViews.data = lgv;
            PageList<GoodsView> StudentPaging = new PageList<GoodsView>(6, GoodsViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页

            ulv.GoodsView = StudentPaging;
            ulv.UserName = User.Identity.Name;
            ulv.GoodsTypeView = gtv;
            ulv.GoodView = lgv;

            return View(ulv);
        }
        
        //单件商品展示
        public ActionResult Show(int? GoodsID)
        {
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            UserListView ulv = new UserListView();
            GoodsView lgv = gbl.GetGood(GoodsID);
            
            ulv.Goods = lgv;
            ulv.UserName = User.Identity.Name;
            return View("Goods",ulv);
        }

        //显示购物车
        public ActionResult Cart(int pageIndex = 1)
        {
            string Uname = User.Identity.Name;
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            UserListView ulv = new UserListView();
            List<CartView> lcv = gbl.GetCart(Uname);

            CartViews.data = lcv;
            PageList<CartView> StudentPaging = new PageList<CartView>(4, CartViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页
            ulv.CartViews = StudentPaging; 
            ulv.UserName = User.Identity.Name;
            return View(ulv);
        }

    }
}