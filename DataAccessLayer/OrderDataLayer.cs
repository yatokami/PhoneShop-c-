using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data;
using System.Data.SqlClient;
using Core;

namespace DataAccessLayer
{
    public class OrderDataLayer
    {
        //商品列表获取
        public List<T_Order> GetOrders(string uname, int v)
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            IList<T_Order> orders = null;
            string OrderStatus = "";
            if (v != 0)
            {
                if (v == 1)
                {
                    OrderStatus = "已接单";
                }
                else if (v == 2)
                {
                    OrderStatus = "未接单";
                }
                if (uname == null || uname == "")
                {
                    dt1 = SqlHelper.ExecuteDataTable("select * from T_Order where OrderStatus = @OrderStatus", new SqlParameter("@OrderStatus", OrderStatus));
                    dt2 = AddColums(dt1);
                }
                else
                {
                    uname = "%" + uname + "%";
                    dt1 = SqlHelper.ExecuteDataTable("select * from T_Order where OrderStatus = @OrderStatus and Uname like @Uname", new SqlParameter("@OrderStatus", OrderStatus), new SqlParameter("@Uname", uname));
                    dt2 = AddColums(dt1);
                }
            }
            else
            {
                if (uname == null || uname == "")
                {
                    dt1 = SqlHelper.ExecuteDataTable("select * from T_Order");
                    dt2 = AddColums(dt1);
                }
                else
                {
                    uname = "%" + uname + "%";
                    dt1 = SqlHelper.ExecuteDataTable("select * from T_Order where Uname like @Uname", new SqlParameter("@Uname", uname));
                    dt2 = AddColums(dt1);

                }
            }
            orders = ModelConvertHelper<T_Order>.ConvertToModel(dt2);
            return orders.ToList();
        }

        //添加表格列
        public DataTable AddColums(DataTable dt)
        {
            dt.Columns.Add("Num", Type.GetType("System.Int32"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Num"] = SqlHelper.GetSqlAsInt("select sum(Num) from T_OrderDetail where OrderID = @OrderID", new SqlParameter("@OrderID", Convert.ToInt32(dt.Rows[i]["OrderID"].ToString())));
            }
            return dt;
        }

        //更新订单状态
        public int Update_Order(int orderID, int v)
        {
            string status = "";
            if (v == 1)
            {
                status = "未接单";
            }
            else if (v == 2)
            {
                status = "已接单";
            }
            int count = SqlHelper.ExecuteNonQuery("Update T_Order set OrderStatus = @OrderStatus where OrderID = @OrderID", new SqlParameter("@OrderStatus", status), new SqlParameter("@OrderID", orderID));

            return count;
        }
    }
}
