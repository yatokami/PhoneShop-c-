using BusinessEntities;
using PhoneShop.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels;

namespace PhoneShop.Controllers
{
    public class UserController : Controller
    {
        // GET: 用户注册界面
        public ActionResult Index()
        {
            return View();
        }
        //POST:用户注册
        [HttpPost]
        public ActionResult Insert(T_Users user)
        {
            if (ModelState.IsValid)
            { 
                UserBusinessLayer rbl = new UserBusinessLayer();
                if (rbl.GetRegister(user))
                {
                    return RedirectToAction("../Authentication/Login");
                }
                else
                {
                    ModelState.AddModelError("RegisterError", "注册失败");
                    return View("Index");
                }
            }
            else
            {
                return View("Index");
            }
        }

        //用户个人信息更新
        [Authorize]
        public ActionResult Update(T_Users user)
        {
            UserBusinessLayer rbl = new UserBusinessLayer();
            if (rbl.GetUpdate(user))
            {
                return RedirectToAction("UserShow");
            }
            else
            {
                return View("My_Info");
            }

        }

        //用户个人信息显示
        [Authorize]//用户认证非认证用户无法使用Show将会跳转至登录界面
        public ActionResult UserShow()
        {
            UserBusinessLayer ubl = new UserBusinessLayer();
            List<UserView> users = ubl.Show(User.Identity.Name);
            UserListView ulv = new UserListView();
            ulv.UserName = User.Identity.Name;
            ulv.Users = users;

            return View("My_Info", ulv);
        }

        //用户订单记录
        [Authorize]
        public ActionResult OrderShow(int pageIndex = 1)
        {
            UserBusinessLayer ubl = new UserBusinessLayer();
            int Uid = Convert.ToInt32(Request.Cookies["Uid"].Value);
            List<OrderView> lov = ubl.OrderShow(Uid);

            UserListView ulv = new UserListView();
            ulv.UserName = User.Identity.Name;
            OrderViews.data = lov;
            PageList<OrderView> StudentPaging = new PageList<OrderView>(4, OrderViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页
            ulv.OrderViews = StudentPaging;
            ulv.UserName = User.Identity.Name;

            return View("My_Order", ulv);

        }

        //订单详情
        [Authorize]
        public ActionResult Details(int OrderID, int pageIndex = 1)
        {
            UserBusinessLayer ubl = new UserBusinessLayer();
            List<OrderDetailsView> odvs = ubl.Details(OrderID);

            UserListView ulv = new UserListView();
            OrderDetailsViews.data = odvs;
            PageList<OrderDetailsView> StudentPaging = new PageList<OrderDetailsView>(4, OrderDetailsViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页
            ulv.OrderDetailsViews = StudentPaging;
            ulv.UserName = User.Identity.Name;

            return View("My_OrderDetail", ulv);

        }

        //加入购物车
        [Authorize]
        public ActionResult AddCart(int GoodsID, int Num)
        {
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            int Uid = Convert.ToInt32(Request.Cookies["Uid"].Value);
            string Uname = User.Identity.Name;

            if (gbl.AddCart(GoodsID, Uname, Num, Uid))
            {
                return this.Json("加入购物车成功");
            }

            return this.Json("失败");
        }

        //用户结算
        [Authorize]
        public ActionResult Clear(Cartstring datas)
        {
            string[] GoodsIDs = datas.GoodsIDs.Split(',');
            string[] Nums = datas.Nums.Split(',');
            string[] BuyIDs = datas.BuyIDs.Split(',');
            T_BuyInfo bi = new T_BuyInfo();

            bi.Uname = User.Identity.Name;
            bi.Uid = Convert.ToInt32(Request.Cookies["Uid"].Value);
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();

            if (gbl.Clears(bi, GoodsIDs, Nums, BuyIDs))
            {
                return Json("结账成功");
            }
            return Json("失败");
        }

        //删除购物车中商品
        [Authorize]
        public ActionResult DeleteCart(int BuyID)
        {
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();

            if (gbl.DeleteCart(BuyID))
            {
                return Content("<script>alert('删除成功');location.href = '/Home/Cart'</script>");
            }
            else
            {
                return Content("<script>alert('删除失败');location.href = '/Home/Cart'</script>");
            }
        }

        //点赞踩
        [Authorize]
        public ActionResult WellBad(int Index, int GoodsID)
        {
            string Uname = User.Identity.Name;
            GoodsBusinessLayer ubl = new GoodsBusinessLayer();
            WellBadView wbv = ubl.WellBad(Index, GoodsID, Uname);
            string jsonData = new JavaScriptSerializer().Serialize(wbv);

            return Content(jsonData);
        }

        [Authorize]
        //发表评论
        public ActionResult Commit(string CommentContent, int GoodsID)
        {
            string Uname = User.Identity.Name;
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            string s = gbl.Commit(CommentContent, GoodsID, Uname);

            return Json(s);
        }

        //public ActionResult Gets()
        //{
        //    GoodsBusinessLayer gbl = new GoodsBusinessLayer();
        //    gbl.Gets();
        //    return View();
        //}


    }
}