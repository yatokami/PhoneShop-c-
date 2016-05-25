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
    public class OrderDataLayer
    {
        public List<T_Order> GetOrders(string uname, int v)
        {
            DataTable dt = new DataTable();
            IList<T_Order> orders = null;
            string OrderStatus = "";
            if (v == 1)
            {
                OrderStatus = "已接单";
            }
            else if(v == 2)
            {
                OrderStatus = "未接单";
            }
            if (uname == null)
            {
                dt = SqlHelper.ExecuteDataTable("select * from T_Order where OrderStatus = @OrderStatus", new SqlParameter("@OrderStatus", OrderStatus));
                dt.Columns.Add("Num", Type.GetType("System.Int32"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Num"] = SqlHelper.GetSqlAsInt("select sum(Num) from T_OrderDetail where OrderID = @OrderID",  new SqlParameter("@OrderID", Convert.ToInt32(dt.Rows[i]["OrderID"].ToString())));
                }

                orders = ModelConvertHelper<T_Order>.ConvertToModel(dt);
                
            }
            return orders.ToList();
        }

        public int Update_Order(int orderID, int v)
        {
            string status = "";
            if (v == 1)
            {
                status = "未接单";
            }
            else if(v == 2)
            {
                status = "已接单";
            }
            int count = SqlHelper.ExecuteNonQuery("Update T_Order set OrderStatus = @OrderStatus where OrderID = @OrderID", new SqlParameter("@OrderStatus", status), new SqlParameter("@OrderID", orderID));

            return count;
        }
    }
}
