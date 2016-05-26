using BusinessEntities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Core;

namespace DataAccessLayer
{
    public class GoodsDataLayer
    {
        //查询商品数量
        public int GoodsCount()
        {
            int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Goods");
            return count;
        }

        //查询商品类型
        public List<T_GoodsType> GetTypes()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_GoodsType");
            //可将dt类型数据转化成IList接口数据
            IList<T_GoodsType> Ilgt = ModelConvertHelper<T_GoodsType>.ConvertToModel(dt);
            return Ilgt.ToList();
        }

        //随机抽取商品展示
        public List<T_Goods> GetGoods(int[] index)
        {
            string ids = "";
            for (int i = 0; i < index.Length; i++)
            {
                ids += index[i];
                if (i < index.Length - 1)
                {
                    ids += ",";
                }
            }
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Goods where GoodsID in(" + ids + ")");

            IList<T_Goods> ILgoods = ModelConvertHelper<T_Goods>.ConvertToModel(dt);
            return ILgoods.ToList();
        }

        //根据商品类型查询商品
        public List<T_Goods> GetGoods(int? TypeID)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Goods where TypeID = @TypeID", new SqlParameter("@TypeID", TypeID));
            IList<T_Goods> ILgoods = ModelConvertHelper<T_Goods>.ConvertToModel(dt);
            return ILgoods.ToList();
        }

        //根据商品id查询商品
        public T_Goods GetGood(int? GoodsID)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Goods where GoodsID = @GoodID", new SqlParameter("@GoodID", GoodsID));
            IList<T_Goods> ILg = ModelConvertHelper<T_Goods>.ConvertToModel(dt);

            return (T_Goods)ILg[0];
        }

        //加入购物车
        public int AddCart(int GoodsID, string Uname, int Num, int Uid)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Goods where GoodsID = @GoodsID", new SqlParameter("@GoodsID", GoodsID));
            IList<T_Goods> ILg = ModelConvertHelper<T_Goods>.ConvertToModel(dt);
            int SumPrice = ILg[0].Price;
            int count = SqlHelper.ExecuteNonQuery("insert into T_BuyInfo(Uname, GoodsID, Uid, Num, Price, BuyDate)Values(@Uname, @GoodsID, @Uid, @Num, @Price, getdate())", new SqlParameter("@Uname", Uname), new SqlParameter("@GoodsID", GoodsID), new SqlParameter("@Price", SumPrice), new SqlParameter("@Num", Num), new SqlParameter("@Uid", Uid));
            return count;

        }

        //显示购物车
        public DataTable GetCart(string Uname)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("SELECT T_Goods.GoodsID, T_Goods.GoodsName, T_Goods.GoodsPicture, T_Goods.TypeID, T_BuyInfo.BuyID, T_BuyInfo.Num, T_BuyInfo.Price, T_BuyInfo.BuyDate FROM T_Goods INNER JOIN T_BuyInfo ON T_Goods.GoodsID = T_BuyInfo.GoodsID Where T_BuyInfo.Uname=@Uname ORDER BY T_BuyInfo.BuyDate desc", new SqlParameter("@Uname", Uname));

            return dt;
        }

        //结算
        public bool Clears(T_BuyInfo bi, string[] GoodsIDs, string[] Nums, string[] BuyIDs)
        {
            int TotalMoney = 0;
            int[] TotalMoneys = new int[GoodsIDs.Length - 1];
            int count = 0;
            for (int i = 0; i < GoodsIDs.Length - 1; i++)
            {
                int Price = SqlHelper.GetSqlAsInt("select Price from T_Goods where GoodsID = @GoodsID", new SqlParameter("@GoodsID", GoodsIDs[i]));

                TotalMoneys[i] = Price * Convert.ToInt32(Nums[i]);
                TotalMoney += Price * Convert.ToInt32(Nums[i]);
            }
            DataTable dt1 = SqlHelper.ExecuteDataTable("select * from T_Users where Uid = @Uid", new SqlParameter("@Uid", bi.Uid));
            IList<T_Users> Ilu = ModelConvertHelper<T_Users>.ConvertToModel(dt1);
            T_Users user = Ilu[0];

            int OrderID = Convert.ToInt32(SqlHelper.ExecuteScalar("insert into T_Order(Uid, Uname, TotalMoney, OrderDate, OrderStatus, PayType, IsPayed, ReceiverName, ReceiverTel, Address, PostCode, Email)Values(@Uid, @Uname, @TotalMoney, getdate(), @OrderStatus, @PayType, @IsPayed, @ReceiverName, @ReceiverTel, @Address, @PostCode, @Email);select @@IDENTITY", new SqlParameter("@Uid", user.Uid), new SqlParameter("@Uname", user.Uname), new SqlParameter("@TotalMoney", TotalMoney), new SqlParameter("@OrderStatus", "未接单"), new SqlParameter("@PayType", "网络支付"), new SqlParameter("@IsPayed", "已付款"), new SqlParameter("@ReceiverName", user.RealName), new SqlParameter("@ReceiverTel", user.Tel), new SqlParameter("@Address", user.Address), new SqlParameter("@PostCode", user.PostCode), new SqlParameter("@Email", user.Email)));

            for (int j = 0; j < GoodsIDs.Length - 1; j++)
            {
                count += SqlHelper.ExecuteNonQuery("insert into T_OrderDetail(OrderID, GoodsID, Num, TotalMoney)Values(@OrderID, @GoodsID, @Num, @TotalMoney)", new SqlParameter("@OrderID", OrderID), new SqlParameter("@GoodsID", GoodsIDs[j]), new SqlParameter("@Num", Nums[j]), new SqlParameter("@TotalMoney", TotalMoneys[j]));
                SqlHelper.ExecuteNonQuery("delete from T_BuyInfo where BuyID = @BuyID", new SqlParameter("@BuyID", BuyIDs[j]));
            }

            if (count == (GoodsIDs.Length - 1))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //删除商品
        public bool DeleteCart(int BuyID)
        {
            int count = SqlHelper.ExecuteNonQuery("delete from T_BuyInfo where BuyID = @BuyID", new SqlParameter("@BuyID", BuyID));

            return Convert.ToBoolean(count);
        }

        //查看商品赞踩信息
        public T_WellBad GetWellBad(int? GoodsID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_WellBad where GoodsID = @GoodsID", new SqlParameter("@GoodsID", GoodsID));

                IList<T_WellBad> wbs = ModelConvertHelper<T_WellBad>.ConvertToModel(dt);

                return (T_WellBad)wbs[0];
            }
            catch
            {
                return null;
            }
        }

        //点击赞踩
        public T_WellBad WellBad(int Index, int GoodsID, string Uname)
        {

            int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_User_Produce where GoodsID = @GoodsID and Uname = @Uname", new SqlParameter("@GoodsID", GoodsID), new SqlParameter("@Uname", Uname));

            if (count == 0)
            {
                Hashtable ht1 = new Hashtable();
                string s1 = "update T_WellBad set Wells += 1 where GoodsID = @GoodsID";
                string s2 = "update T_WellBad set Bads += 1 where GoodsID = @GoodsID";
                string s3 = "insert into T_User_Produce(GoodsID, Uname)Values(@GoodsID, @Uname)";
                SqlParameter[] paras1 = new SqlParameter[1];
                paras1[0] = new SqlParameter("@GoodsID", GoodsID);
                SqlParameter[] paras2 = new SqlParameter[2];
                paras2[0] = new SqlParameter("@GoodsID", GoodsID);
                paras2[1] = new SqlParameter("@Uname", Uname);
                if (Index == 1)
                {
                    ht1.Add(s1, paras1);
                    ht1.Add(s3, paras2);
                }
                else if (Index == -1)
                {
                    ht1.Add(s2, paras1);
                    ht1.Add(s3, paras2);
                }

                SqlHelper.ExecuteSqlTran(ht1);
            }

            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_WellBad where GoodsID = @GoodsID", new SqlParameter("@GoodsID", GoodsID));

            IList<T_WellBad> wbs = ModelConvertHelper<T_WellBad>.ConvertToModel(dt);

            return (T_WellBad)wbs[0];
        }

        //显示用户评论
        public List<T_Comment> GetComment(int? GoodsID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Comment where GoodsID = @GoodsID", new SqlParameter("@GoodsID", GoodsID));

                IList<T_Comment> lc = ModelConvertHelper<T_Comment>.ConvertToModel(dt);

                return lc.ToList();

            }
            catch
            {
                return null;
            }

        }

        //提交评论
        public int Commit(string CommentContent, int GoodsID, string Uname)
        {
            try
            {
                int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_User_Produce2 where GoodsID = @GoodsID and Uname = @Uname", new SqlParameter("@GoodsID", GoodsID), new SqlParameter("@Uname", Uname));

                if (count == 0)
                {
                    Hashtable ht = new Hashtable();
                    string s1 = "Insert into T_User_Produce2(GoodsID, Uname)Values(@GoodsID, @Uname)";
                    string s2 = "Insert into T_Comment(CommentContent ,GoodsID, Uname, CommentStarTime)Values(@CommentContent, @GoodsID, @Uname, getdate())";
                    SqlParameter[] paras1 = new SqlParameter[2];
                    paras1[0] = new SqlParameter("@GoodsID", GoodsID);
                    paras1[1] = new SqlParameter("@Uname", Uname);
                    SqlParameter[] paras2 = new SqlParameter[3];
                    paras2[0] = new SqlParameter("@CommentContent", CommentContent);
                    paras2[1] = new SqlParameter("@GoodsID", GoodsID);
                    paras2[2] = new SqlParameter("@Uname", Uname);
                    ht.Add(s1, paras1);
                    ht.Add(s2, paras2);

                    count = SqlHelper.ExecuteSqlTran(ht);

                    return count;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }

        //下拉框显示手机品牌
        public List<T_GoodsType> GetGoodType()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_GoodsType");

            IList<T_GoodsType> gts = ModelConvertHelper<T_GoodsType>.ConvertToModel(dt);

            return gts.ToList();
        }


        //public void Gets()
        //{
        //    for(int i = 2; i < 83; i++)
        //    {
        //        SqlHelper.ExecuteNonQuery("insert into T_WellBad(GoodsID, Wells, Bads)Values(@GoodsID, 0, 0)", new SqlParameter("@GoodsID", i));
        //    }
        //}

        //添加已存在品牌手机
        public int Add_Goods(T_Goods goods)
        {
            int count = SqlHelper.ExecuteNonQuery("insert into T_Goods(GoodsName, Price, GoodsPicture, TypeID, AddDate)Values(@GoodsName, @Price, @GoodsPicture, @TypeID, getdate())", new SqlParameter("@GoodsName", goods.GoodsName), new SqlParameter("@Price", goods.Price), new SqlParameter("@GoodsPicture", goods.GoodsPicture), new SqlParameter("@TypeID", goods.TypeID));

            return count;
        }

        //添加未存在品牌手机
        public int Add_Goods(T_Goods goods, string TypeName)
        {

            string s1 = "insert into T_GoodsType(TypeID,TypeName)Values(@TypeID,@TypeName)";
            string s2 = "insert into T_Goods(GoodsName, Price, GoodsPicture, TypeID, AddDate)Values(@GoodsName, @Price, @GoodsPicture, @TypeID, getdate())";
            SqlParameter[] paras1 = new SqlParameter[2];
            paras1[0] = new SqlParameter("@TypeID", goods.TypeID);
            paras1[1] = new SqlParameter("@TypeName", TypeName);
            SqlParameter[] paras2 = new SqlParameter[4];
            paras2[0] = new SqlParameter("@GoodsName", goods.GoodsName);
            paras2[1] = new SqlParameter("@Price", goods.Price);
            paras2[2] = new SqlParameter("@GoodsPicture", goods.GoodsPicture);
            paras2[3] = new SqlParameter("@TypeID", goods.TypeID);
            Hashtable ht1 = new Hashtable();
            ht1.Add(s1, paras1);
            ht1.Add(s2, paras2);
            int count = SqlHelper.ExecuteSqlTran(ht1);
            return count;
        }

        //删除评论
        public bool Delete_Comment(int commentID)
        {
            int count = SqlHelper.ExecuteNonQuery("delete from T_Comment where CommentID = @CommentID", new SqlParameter("@CommentID", commentID));

            return Convert.ToBoolean(count);
        }

        //添加公告
        public int Add_Bulletin(T_Bulletin bulletin)
        {
            int count = SqlHelper.ExecuteNonQuery("insert into T_Bulletin(BulletinTitle, BulletinContent, BulletinStarTime, BulletinImg)Values(@BulletinTitle, @BulletinContent, getdate(), @BulletinImg)",new SqlParameter("@BulletinTitle", bulletin.BulletinTitle), new SqlParameter("@BulletinContent", bulletin.BulletinContent), new SqlParameter("@BulletinImg", bulletin.BulletinImg));
            return count;
        }

        //获取评论列表
        public List<T_Comment> GetComments(string goodsID)
        {
            DataTable dt = null;
            if (goodsID == null || goodsID == "")
            {
                dt = SqlHelper.ExecuteDataTable("select * from T_Comment");
            }
            else
            {
                int GoodsID = Convert.ToInt32(goodsID);
                dt = SqlHelper.ExecuteDataTable("select * from T_Comment where GoodsID = @GoodsID", new SqlParameter("@GoodsId", GoodsID));
            }
            IList<T_Comment> coments = ModelConvertHelper<T_Comment>.ConvertToModel(dt);

            return coments.ToList();
        }

        //手机信息查看
        public List<T_Goods> GetGoods(string GoodsName)
        {
            DataTable dt = null;
            if (GoodsName == null || GoodsName == "")
            {
                dt = SqlHelper.ExecuteDataTable("select * from T_Goods");

            }
            else
            {
                GoodsName = "%" + GoodsName + "%";
                dt = SqlHelper.ExecuteDataTable("select * from T_Goods where GoodsName  like @GoodsName", new SqlParameter("@GoodsName", GoodsName));
            }
            IList<T_Goods> goods = ModelConvertHelper<T_Goods>.ConvertToModel(dt);

            return goods.ToList();
        }
        //更新手机价格
        public int Update_Goods(T_Goods good)
        {
            int count = SqlHelper.ExecuteNonQuery("update T_Goods set Price = @Price where GoodsID = @GoodsID", new SqlParameter("@Price", good.Price), new SqlParameter("@GoodsID", good.GoodsID));

            return count;
        }
        //删除手机
        public int Delete_Goods(T_Goods good)
        {
            int count = SqlHelper.ExecuteNonQuery("delete from T_Goods where GoodsID = @GoodsID", new SqlParameter("@GoodsID", good.GoodsID));

            return count;
        }
    }
}
