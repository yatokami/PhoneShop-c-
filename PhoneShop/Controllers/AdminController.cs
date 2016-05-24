using BusinessEntities;
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
        //添加已型号新商品
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
        //添加未有型号新商品
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
        //上传图片文件
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
        //上传图片文件
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
        //更新商品
        [HttpGet]
        public ActionResult Update_Goods(string GoodsName, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            List<GoodsView> lgv = gbl.GetGoods(GoodsName);

            GoodsViews.data = lgv;
            PageList<GoodsView> StudentPaging = new PageList<GoodsView>(10, GoodsViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页
            alv.GoodsView = StudentPaging;
            alv.AdminUser = User.Identity.Name;
            return View(alv);
        }
        //更新商品
        [HttpPost]
        public ActionResult Update_Goods(T_Goods good)
        {
            GoodsBusinessLayer abl = new GoodsBusinessLayer();
            bool status = abl.Update_Goods(good);

            return Json(status);
        }

        //查看商品信息
        public ActionResult Index_Goods(string GoodsName, int pageIndex = 1)
        {
            AdminListView alv = new AdminListView();
            GoodsBusinessLayer gbl = new GoodsBusinessLayer();
            List<GoodsView> lgv = gbl.GetGoods(GoodsName);

            GoodsViews.data = lgv;
            PageList<GoodsView> StudentPaging = new PageList<GoodsView>(10, GoodsViews.data);//初始化分页器
            StudentPaging.PageIndex = pageIndex;//指定当前页
            alv.GoodsView = StudentPaging;
            alv.AdminUser = User.Identity.Name;
            return View(alv);
        }
    }
}