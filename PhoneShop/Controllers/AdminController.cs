﻿using BusinessEntities;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace PhoneShop.Controllers
{

    //[AdminFilter]
    public class AdminController : Controller
    {
        //用户列表查询
        public ActionResult Index(string Uname, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            alv = PageUser(Uname, pageIndex = 1);

            return View("Index", alv);
        }
        //用户列表显示
        public ActionResult User_Password(string Uname, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            alv = PageUser(Uname, pageIndex = 1);

            return View("User_Password", alv);
        }

        //用户列表分页
        public AdminListView PageUser(string Uname, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            AdminBusinessLayer abl = new AdminBusinessLayer();

            List<UserView> luv = abl.GetUsers(Uname);


            UserViews.data = luv;
            PageList<UserView> StudentPaging = new PageList<UserView>(10, UserViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页

            alv.Users = StudentPaging;
            alv.AdminUser = (string)Session["AdminName"];
            alv.Uname = Uname;

            return alv;
        }

        //密码修改
        public JsonResult Changepwd(string Uname, string Upwd)
        {
            AdminBusinessLayer abl = new AdminBusinessLayer();

            bool status = abl.Changepwd(Uname, Upwd);

            return Json(status);

        }

        //添加已型号新商品
        [HttpGet]
        public ActionResult Add_Goods()
        {
            AdminListView alv = new AdminListView();
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            List<GoodsTypeView> lgv = gbl.GetGoodType();

            alv.GoodsType = lgv;
            alv.AdminUser = (string)Session["AdminName"];
            return View(alv);
        }

        //添加型号新商品
        [HttpPost]
        public ActionResult Add_Goods(T_Goods goods, HttpPostedFileBase GoodsPicture, string BtnSubmit)
        {
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            string status = "";

            if (BtnSubmit == "提交1")
            {
                string filename = UploadPicture1(goods, GoodsPicture);
                if (filename != "1" && filename != "2" && filename != "3")
                {
                    goods.GoodsPicture = filename;
                    status = gbl.Add_Goods(goods);
                    return Content("<script>alert('" + status + "');location.href = 'Add_Goods'</script>");
                }
                else
                {
                    return Content("<script>alert('上传失败');location.href='Add_Goods';</script>");
                }

            }
            else if (BtnSubmit == "提交2")
            {
                string TypeNmae = Request.Form["TypeNmae"];
                string[] files = UploadPicture2(goods, GoodsPicture);
                if (files[0] != "1" && files[0] != "2" && files[0] != "3")
                {
                    goods.GoodsPicture = files[0];
                    goods.TypeID = Convert.ToInt32(files[1]);
                    status = gbl.Add_Goods(goods, TypeNmae);
                    return Content("<script>alert('" + status + "');location.href='Add_Goods';</script>");
                }
                else
                {
                    return Content("<script>alert('上传失败');location.href='Add_Goods';</script>");
                }

            }

            return RedirectToAction("Add_Goods");
        }

        //上传商品图片文件
        public string UploadPicture1(T_Goods goods, HttpPostedFileBase GoodsPicture)
        {
            string FileExt = "";     //檢查扩展名
            int intFileSize = 0;     //檢查大小(KB)

            if (GoodsPicture != null)
            {
                intFileSize = (GoodsPicture.ContentLength / 1024);
                if (GoodsPicture.InputStream.Length != 0)
                {
                    if (GoodsPicture.InputStream.Length < 3048576)
                    {
                        FileExt = Path.GetFileNameWithoutExtension(GoodsPicture.FileName);
                        FileExt = Path.GetExtension(GoodsPicture.FileName);
                        if (FileExt.Equals(".jpg") || FileExt.Equals(".jpeg") || FileExt.Equals(".png") || FileExt.Equals(".gif") || FileExt.Equals(".bmp"))
                        {
                            string filename = goods.GoodsName + Path.GetExtension(GoodsPicture.FileName);
                            GoodsPicture.SaveAs(Server.MapPath("~/Public/images/" + goods.TypeID + "/" + filename));//save file 

                            return filename;
                        }
                    }
                    return "3";
                }
                return "2";
            }
            return "1";
        }

        //上传商品图片文件
        public string[] UploadPicture2(T_Goods goods, HttpPostedFileBase GoodsPicture)
        {
            string FileExt = "";     //檢查扩展名
            int intFileSize = 0;     //檢查大小(KB)
            string[] files = new string[2];
            if (GoodsPicture != null)
            {
                intFileSize = (GoodsPicture.ContentLength / 1024);
                if (GoodsPicture.InputStream.Length != 0)
                {
                    if (GoodsPicture.InputStream.Length < 3048576)
                    {
                        FileExt = Path.GetFileNameWithoutExtension(GoodsPicture.FileName);
                        FileExt = Path.GetExtension(GoodsPicture.FileName);
                        if (FileExt.Equals(".jpg") || FileExt.Equals(".jpeg") || FileExt.Equals(".png") || FileExt.Equals(".gif") || FileExt.Equals(".bmp"))
                        {
                            string filename = goods.GoodsName + Path.GetExtension(GoodsPicture.FileName);
                            string f1 = Server.MapPath("~/Public/images/").ToString();
                            DirectoryInfo theFolder = new DirectoryInfo(f1);
                            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
                            int length = dirInfo.Length + 1;
                            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(f1 + length.ToString());
                            di.Create();
                            GoodsPicture.SaveAs(Server.MapPath("~/Public/images/" + length + "/" + filename));//save file 

                            files[0] = filename;
                            files[1] = length.ToString();
                            return files;
                        }
                    }
                    files[0] = "3";
                    return files;
                }
                files[0] = "2";
                return files;
            }
            files[0] = "1";
            return files;
        }

        //更新商品界面
        [HttpGet]
        public ActionResult Update_Goods(string GoodsName, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            alv = PageGoods(GoodsName, pageIndex);
            return View(alv);
        }

        //更新商品
        [HttpPost]
        public JsonResult Update_Goods(T_Goods good)
        {
            GoodsBusinessLayer abl = new GoodsBusinessLayer();
            bool status = abl.Update_Goods(good);

            return Json(status);
        }

        //查看商品信息  
        public ActionResult Index_Goods(string GoodsName, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            alv = PageGoods(GoodsName, pageIndex);
            return View(alv);
        }

        //删除商品界面
        [HttpGet]
        public ActionResult Delete_Goods(string GoodsName, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            alv = PageGoods(GoodsName, pageIndex);
            return View(alv);
        }

        //删除商品
        [HttpPost]
        public JsonResult Delete_Goods(T_Goods good)
        {
            GoodsBusinessLayer abl = new GoodsBusinessLayer();
            bool status = abl.Delete_Goods(good);

            return Json(status);
        }

        //分页展示商品
        public AdminListView PageGoods(string GoodsName, int pageIndex)
        {
            AdminListView alv = new AdminListView();
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            List<GoodsView> lgv = gbl.GetGoods(GoodsName);

            GoodsViews.data = lgv;
            PageList<GoodsView> StudentPaging = new PageList<GoodsView>(10, GoodsViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页
            alv.GoodsView = StudentPaging;
            alv.AdminUser = (string)Session["AdminName"];
            alv.GoodsName = GoodsName;
            return alv;
        }

        //已接单订单显示
        [HttpGet]
        public ActionResult Pass_Order(string Uname, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            alv = PageOrder(Uname, pageIndex, 1);
            return View(alv);
        }

        //未接单订单显示
        [HttpGet]
        public ActionResult Unpass_Order(string Uname, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            alv = PageOrder(Uname, pageIndex, 2);
            return View(alv);
        }

        //取消订单
        [HttpPost]
        public JsonResult Cancel_Order(int OrderID)
        {
            GoodsBusinessLayer abl = new GoodsBusinessLayer();
            bool status = abl.Cancel_Order(OrderID, 1);

            return Json(status);
        }

        //接受订单
        [HttpPost]
        public JsonResult Upass_Order(int OrderID)
        {
            GoodsBusinessLayer abl = new GoodsBusinessLayer();
            bool status = abl.Upass_Order(OrderID, 2);

            return Json(status);
        }

        //订单情况列表
        [HttpGet]
        public ActionResult Index_Order(string Uname, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            alv = PageOrder(Uname, pageIndex, 0);
            return View(alv);
        }

        //订单分页
        public AdminListView PageOrder(string Uname, int pageIndex, int index)
        {
            AdminListView alv = new AdminListView();
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            List<AdminOrderView> aovs = gbl.GetOrders(Uname, index);

            AdminOrderViews.data = aovs;
            PageList<AdminOrderView> StudentPaging = new PageList<AdminOrderView>(10, AdminOrderViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页
            alv.AdminOrderViews = StudentPaging;
            alv.AdminUser = (string)Session["AdminName"];
            alv.Uname = Uname;

            return alv;
        }

        //评论列表显示
        [HttpGet]
        public ActionResult Index_Comment(string GoodsID, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            List<CommentView> lcv = gbl.GetComments(GoodsID);

            CommentViews.data = lcv;
            PageList<CommentView> StudentPaging = new PageList<CommentView>(10, CommentViews.data);
            StudentPaging.PageIndex = pageIndex;//指定当前页
            alv.CommentViews = StudentPaging;
            alv.AdminUser = (string)Session["AdminName"];
            alv.GoodsID = GoodsID;

            return View(alv);
        }

        //删除评论
        [HttpPost]
        public JsonResult Delete_Comment(int CommentID)
        {
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            bool status = gbl.Delete_Comment(CommentID);

            return Json(status);
        }

        //显示添加公告页面
        [HttpGet]
        public ActionResult Add_Bulletin()
        {
            AdminListView alv = new AdminListView();
            alv.AdminUser = (string)Session["AdminName"];
            return View(alv);
        }

        //添加公告
        [HttpPost]
        public ActionResult Add_Bulletin(T_Bulletin bulletin, HttpPostedFileBase BulletiImg)
        {
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            string status = "";
            string filename = UploadPicture3(bulletin, BulletiImg);
            if (filename != "1" && filename != "2" && filename != "3")
            {
                bulletin.BulletinImg = filename;
                status = gbl.Add_Bulletin(bulletin);

                return Content("<script>alert('" + status + "');location.href = 'Add_Bulletin'</script>");
            }
            else
            {
                return Content("<script>alert('发布失败');location.href='Add_Bulletin';</script>");
            }

        }

        //上传公告图片
        public string UploadPicture3(T_Bulletin bulletin, HttpPostedFileBase BulletiImg)
        {
            string FileExt = "";     //檢查扩展名
            int intFileSize = 0;     //檢查大小(KB)

            if (BulletiImg != null)
            {
                intFileSize = (BulletiImg.ContentLength / 1024);
                if (BulletiImg.InputStream.Length != 0)
                {
                    if (BulletiImg.InputStream.Length < 3048576)
                    {
                        FileExt = Path.GetFileNameWithoutExtension(BulletiImg.FileName);
                        FileExt = Path.GetExtension(BulletiImg.FileName);
                        if (FileExt.Equals(".jpg") || FileExt.Equals(".jpeg") || FileExt.Equals(".png") || FileExt.Equals(".gif") || FileExt.Equals(".bmp"))
                        {
                            string filename = Path.GetExtension(BulletiImg.FileName);
                            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                            string ds = Convert.ToInt64(ts.TotalSeconds).ToString();
                            filename = ds + filename;
                            BulletiImg.SaveAs(Server.MapPath("~/Content/img/Bulletin/" + filename));//save file 
                            return filename;
                        }
                    }
                    return "3";
                }
                return "2";
            }
            return "1";
        }

    }
}