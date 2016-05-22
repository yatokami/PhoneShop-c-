using BusinessEntities;
using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ViewModels
{
    //商品业务处理层
    public class GoodsBusinessLayer
    {
        #region 左侧导航栏显示手机类型
        public List<GoodsTypeView> GetTypes()
        {
            GoodsDataLayer gdl = new GoodsDataLayer();
            List<T_GoodsType> lgt = gdl.GetTypes();
            List<GoodsTypeView> lgtv = new List<GoodsTypeView>();
            foreach (T_GoodsType tgt in lgt)
            {
                GoodsTypeView gtv = new GoodsTypeView();
                gtv.TypeID = tgt.TypeID;
                gtv.TypeName = tgt.TypeName;

                lgtv.Add(gtv);
            }

            return lgtv;
        }
        #endregion

        #region 商品展示

        public List<GoodsView> GetGoods(int? TypeID)
        {
            GoodsDataLayer gdl = new GoodsDataLayer();
            List<GoodsView> lgv = new List<GoodsView>();
            List<T_Goods> ltg = new List<T_Goods>();
            //但主页为首页时随机抽取6件商品展示
            //否则根据商品类型ID进行抽取商品展示
            if (TypeID == null)
            {
                int count = gdl.GoodsCount();
                int[] index = GenerateRandom(count);
                ltg = gdl.GetGoods(index);
            }
            else
            {
                ltg = gdl.GetGoods(TypeID);
            }
            foreach (T_Goods tg in ltg)
            {
                GoodsView gv = new GoodsView();
                gv.GoodsID = tg.GoodsID;
                gv.GoodsName = tg.GoodsName;
                gv.GoodsPicture = "/Public/images/" + tg.TypeID + "/" + tg.GoodsPicture.Trim() + ".jpg";
                gv.TypeID = tg.TypeID;
                gv.Price = tg.Price;
                lgv.Add(gv);
            }
            return lgv;
        }
        #endregion

        #region 生成随机数
        //生成随机数
        private int[] GenerateRandom(int count)
        {
            int[] index = new int[count];
            for (int i = 0; i < count; i++)
                index[i] = i;
            Random r = new Random();
            //用来保存随机生成的不重复的10个数 
            int[] result = new int[6];
            int site = count;//设置上限 
            int id;
            for (int j = 0; j < 6; j++)
            {
                id = r.Next(1, site - 1);
                //在随机位置取出一个数，保存到结果数组 
                result[j] = index[id];
                //最后一个数复制到当前位置 
                index[id] = index[site - 1];
                //位置的上限减少一 
                site--;
            }
            return result;
        }
        #endregion

        #region 单件商品展示

        public GoodsView GetGood(int? GoodsID)
        {
            GoodsDataLayer gdl = new GoodsDataLayer();
            T_Goods tg = gdl.GetGood(GoodsID);
            GoodsView gv = new GoodsView();
            gv.GoodsID = tg.GoodsID;
            gv.GoodsName = tg.GoodsName;
            gv.GoodsPicture = "/Public/images/" + tg.TypeID + "/" + tg.GoodsPicture.Trim() + ".jpg";
            gv.TypeID = gv.TypeID;
            gv.Price = tg.Price;

            return gv;
        }
        #endregion

        #region 加入购物车
        public bool AddCart(int GoodsID, string Uname, int Num, int Uid)
        {
            GoodsDataLayer gdl = new GoodsDataLayer();
            int count = gdl.AddCart(GoodsID, Uname, Num, Uid);
            return Convert.ToBoolean(count);
        }
        #endregion

        #region 显示购物车
        public List<CartView> GetCart(string Uname)
        {
            GoodsDataLayer gdl = new GoodsDataLayer();
            DataTable dt = gdl.GetCart(Uname);
            IList<CartView> lcv = ModelConvertHelper<CartView>.ConvertToModel(dt);

            return lcv.ToList();
        }
        #endregion

        #region 结算
        public bool Clears(T_BuyInfo bi, string[] GoodsIDs, string[] Nums, string[] BuyIDs)
        {
            GoodsDataLayer gdl = new GoodsDataLayer();
            bool status = gdl.Clears(bi, GoodsIDs, Nums, BuyIDs);

            return status;
        }
        #endregion

        #region 删除购物车中物品
        public bool DeleteCart(int BuyID)
        {
            GoodsDataLayer gbl = new GoodsDataLayer();
            bool status = gbl.DeleteCart(BuyID);

            return status;
        }
        #endregion

        #region 商品赞踩信息
        public WellBadView GetWellBad(int? GoodsID)
        {
            GoodsDataLayer gdl = new GoodsDataLayer();
            T_WellBad wb = gdl.GetWellBad(GoodsID);
            WellBadView wbv = new WellBadView();
            if (wb != null)
            {
                wbv.Bad = Convert.ToString(wb.Bads);
                wbv.Well = Convert.ToString(wb.Wells);
            }
            else
            {
                wbv.Bad = "0";
                wbv.Well = "0";
            }

            return wbv;
        }
        #endregion

        #region 点赞踩
        public WellBadView WellBad(int Index, int GoodsID, string Uname)
        {
            GoodsDataLayer gdl = new GoodsDataLayer();
            T_WellBad wb = gdl.WellBad(Index, GoodsID, Uname);
            WellBadView wbv = new WellBadView();

            wbv.Bad = Convert.ToString(wb.Bads);
            wbv.Well = Convert.ToString(wb.Wells);

            return wbv;
        }
        #endregion

        #region 显示用户评论
        public List<CommentView> GetComment(int? GoodsID)
        {
            GoodsDataLayer gdl = new GoodsDataLayer();
            List<T_Comment> lc = gdl.GetComment(GoodsID);
            List<CommentView> lcv = new List<CommentView>();
            if (lc != null)
            {

                for (int i = lc.Count - 1; i >= 0; i--)
                {
                    CommentView cv = new CommentView();
                    cv.CommentContent = lc[i].CommentContent;
                    cv.CommentStartTime = lc[i].CommentStarTime.ToString();
                    cv.Uname = lc[i].Uname;

                    lcv.Add(cv);
                }

                return lcv;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 用户评论
        public string Commit(string CommentContent, int GoodsID, string Uname)
        {
            GoodsDataLayer gdl = new GoodsDataLayer();
            int count = gdl.Commit(CommentContent, GoodsID, Uname);
            if (count == 1)
            {
                return "用户已评论过";
            }
            else if (count == 2)
            {
                return "评论成功";
            }
            else
            {
                return "评论失败";
            }
        }

        #endregion

        //public void Gets()
        //{
        //    GoodsDataLayer gdl = new GoodsDataLayer();
        //    gdl.Gets();
        //}
    }
}
