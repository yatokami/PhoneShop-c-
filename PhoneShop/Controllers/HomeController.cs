using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels;

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
        public ActionResult Show(int? GoodsID, int pageIndex = 1)
        {
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            UserListView ulv = new UserListView();
            if (GoodsID != null)
            {
                GoodsView lgv = gbl.GetGood(GoodsID);
                WellBadView wbv = gbl.GetWellBad(GoodsID);
                List<CommentView> lcv = gbl.GetComment(GoodsID);

                CommentViews.data = lcv;
                PageList<CommentView> StudentPaging = new PageList<CommentView>(6, CommentViews.data);//初始化分页器
                StudentPaging.PageIndex = pageIndex;//指定当前页

                ulv.CommentViews = StudentPaging;
                ulv.WellBad = wbv;
                ulv.Goods = lgv;
                ulv.UserName = User.Identity.Name;
                return View("Goods", ulv);
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        //评论换页
        public ActionResult Paging(int GoodsID, int pageIndex)
        {
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            List<CommentView> lcv = gbl.GetComment(GoodsID);
            CommentViews.data = lcv;
            PageList<CommentView> StudentPaging = new PageList<CommentView>(6, CommentViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页
            List<CommentView> lcvs = StudentPaging.GetPagingData().ToList();

            int MaxPageIndex = StudentPaging.PageCount;
            var data = new { lcvs, MaxPageIndex };
            string jsonData = new JavaScriptSerializer().Serialize(data);

            return Content(jsonData);
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