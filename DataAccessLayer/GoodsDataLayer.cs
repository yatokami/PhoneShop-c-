using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data;
using System.Data.SqlClient;

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
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Goods where GoodsID in("+ids+")");
            
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
            int[] TotalMoneys = new int[GoodsIDs.Length-1];
            int count = 0;
            for(int i = 0 ; i < GoodsIDs.Length - 1; i++)
            {
                int Price = SqlHelper.GetSqlAsInt("select Price from T_Goods where GoodsID = @GoodsID", new SqlParameter("@GoodsID", GoodsIDs[i]));
             
                TotalMoneys[i] = Price * Convert.ToInt32(Nums[i]);
                TotalMoney += Price * Convert.ToInt32(Nums[i]);
            }
            DataTable dt1 = SqlHelper.ExecuteDataTable("select * from T_Users where Uid = @Uid", new SqlParameter("@Uid", bi.Uid));
            IList<T_Users> Ilu = ModelConvertHelper<T_Users>.ConvertToModel(dt1);
            T_Users user = Ilu[0];

            int OrderID = Convert.ToInt32(SqlHelper.ExecuteScalar("insert into T_Order(Uid, Uname, TotalMoney, OrderDate, OrderStatus, PayType, IsPayed, ReceiverName, ReceiverTel, Address, PostCode, Email)Values(@Uid, @Uname, @TotalMoney, getdate(), @OrderStatus, @PayType, @IsPayed, @ReceiverName, @ReceiverTel, @Address, @PostCode, @Email);select @@IDENTITY", new SqlParameter("@Uid", user.Uid), new SqlParameter("@Uname", user.Uname), new SqlParameter("@TotalMoney", TotalMoney), new SqlParameter("@OrderStatus", "未接单"), new SqlParameter("@PayType", "网络支付"), new SqlParameter("@IsPayed", "已付款"), new SqlParameter("@ReceiverName", user.RealName), new SqlParameter("@ReceiverTel", user.Tel), new SqlParameter("@Address", user.Address), new SqlParameter("@PostCode", user.PostCode), new SqlParameter("@Email", user.Email)));

            for (int j = 0; j < GoodsIDs.Length - 1; j++ )
            {
                 count += SqlHelper.ExecuteNonQuery("insert into T_OrderDetail(OrderID, GoodsID, Num, TotalMoney)Values(@OrderID, @GoodsID, @Num, @TotalMoney)", new SqlParameter("@OrderID", OrderID), new SqlParameter("@GoodsID", GoodsIDs[j]), new SqlParameter("@Num", Nums[j]), new SqlParameter("@TotalMoney", TotalMoneys[j]));
                 SqlHelper.ExecuteNonQuery("delete from T_BuyInfo where BuyID = @BuyID", new SqlParameter("@BuyID", BuyIDs[j]));
            }

            if(count == (GoodsIDs.Length - 1))
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
        public T_WellBad WellBad(int Index, int GoodsID)
        {
            int count = 0;
            if (Index == 1)
            {
                 count = SqlHelper.ExecuteNonQuery("update T_WellBad set Wells += 1 where GoodsID = @GoodsID",new SqlParameter("@GoodsID", GoodsID));
            }
            else if(Index == -1)
            {
                count = SqlHelper.ExecuteNonQuery("update T_WellBad set Bads += 1 where GoodsID = @GoodsID", new SqlParameter("@GoodsID", GoodsID));
            }

            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_WellBad where GoodsID = @GoodsID", new SqlParameter("@GoodsID", GoodsID));

            IList<T_WellBad> wbs = ModelConvertHelper<T_WellBad>.ConvertToModel(dt);

            return (T_WellBad)wbs[0];
        }


        //public void Gets()
        //{
        //    for(int i = 2; i < 83; i++)
        //    {
        //        SqlHelper.ExecuteNonQuery("insert into T_WellBad(GoodsID, Wells, Bads)Values(@GoodsID, 0, 0)", new SqlParameter("@GoodsID", i));
        //    }
        //}
      
    }
}
