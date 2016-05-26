using BusinessEntities;
using Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLayer
{
    public class UserDataLayer
    {
        //用户登录
        public int IsValidUser(T_Users user)
        {
            int count = 0;
            try
            {
                count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Users where Uname = @Uname and Upwd = @Upwd", new SqlParameter("@Uname", user.Uname), new SqlParameter("@Upwd", user.Upwd));

            }
            catch
            {
                count = 0;
            }
            return count;
        }

        //个人信息返回
        public T_Users Show(string Uname)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Users where Uname=@Uname", new SqlParameter("@Uname", GetValue(Uname)));
                //这里调用了一个方法将dt转变为Ilist<T_Users>数据
                IList<T_Users> user = ModelConvertHelper<T_Users>.ConvertToModel(dt);
                return (T_Users)user[0];
            }
            catch
            {
                return null;
            }
        }

        //用户注册
        public bool GetRegister(T_Users user)
        {
            try
            {
                int index = (int)SqlHelper.ExecuteScalar("select count(*) from T_Users where Uname=@Uname", new SqlParameter("@Uname", user.Uname));
                int count = 0;
                if (index == 0)
                {
                    count = SqlHelper.ExecuteNonQuery("insert into T_Users(Uname,Upwd,Sex,RealName,Tel,Email,Address,PostCode,RegisterTime)Values(@Uname,@Upwd,@Sex,@RealName,@Tel,@Email,@Address,@PostCode,getdate())", new SqlParameter("@Uname", GetValue(user.Uname)), new SqlParameter("@Upwd", GetValue(user.Upwd)), new SqlParameter("@Sex", GetValue(user.Sex)), new SqlParameter("@RealName", GetValue(user.RealName)), new SqlParameter("@Tel", GetValue(user.Tel)), new SqlParameter("@Email", GetValue(user.Email)), new SqlParameter("@Address", GetValue(user.Address)), new SqlParameter("@PostCode", GetValue(user.PostCode)));
                }
                else
                {
                    count = 0;
                }
                return Convert.ToBoolean(count);
            }
            catch
            {
                return false;
            }
        }

        //用户个人信息更新
        public bool GetUpdate(T_Users user)
        {
            try
            {
                int count = SqlHelper.ExecuteNonQuery("update T_Users set Sex=@Sex, RealName=@RealName, Tel=@Tel, Email=@Email, Address=@Address, PostCode=@PostCode", new SqlParameter("@Sex", GetValue(user.Sex)), new SqlParameter("@RealName", GetValue(user.RealName)), new SqlParameter("Tel", GetValue(user.Tel)), new SqlParameter("@Email", GetValue(user.Email)), new SqlParameter("@Address", GetValue(user.Address)), new SqlParameter("@PostCode", GetValue(user.PostCode)));

                return Convert.ToBoolean(count);
            }
            catch
            {
                return false;
            }
        }

        //判断值是否为空
        object GetValue(object value)
        {
            if (value == null) return DBNull.Value;
            else return value;
        }

        //用户订单记录
        public List<T_Order> OrderShow(int Uid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Order where Uid = @Uid", new SqlParameter("@Uid", Uid));
                dt.Columns.Add("Num", Type.GetType("System.Int32"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Num"] = SqlHelper.GetSqlAsInt("select sum(Num) from T_OrderDetail where OrderID = @OrderID", new SqlParameter("@OrderID", Convert.ToInt32(dt.Rows[i]["OrderID"])));
                }

                IList<T_Order> orders = ModelConvertHelper<T_Order>.ConvertToModel(dt);
                return orders.ToList();
            }
            catch
            {
                return null;
            }

        }

        //订单详情
        public DataTable GetOrderDetails(int OrderID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select T_Goods.GoodsName, T_Goods.TypeID, T_Goods.GoodsPicture, T_OrderDetail.Num, T_OrderDetail.TotalMoney, T_OrderDetail.OrderID, T_OrderDetail.GoodsID From T_Goods inner join T_OrderDetail ON T_Goods.GoodsID = T_OrderDetail.GoodsID where  T_OrderDetail.OrderID = @OrderID", new SqlParameter("OrderID", OrderID));

                return dt;
            }
            catch
            {
                return null;
            }
        }

    }
}
