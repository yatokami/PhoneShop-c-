using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using ViewModels;
using System.Data;

namespace ViewModels
{
    public class UserBusinessLayer
    {
        #region//用户注册方法
        public bool GetRegister(T_Users user)
        {
            UserDataLayer sql = new UserDataLayer();
            bool status = sql.GetRegister(user);
            return status;
        }
        #endregion

        #region//用户更新个人信息方法
        public bool GetUpdate(T_Users user)
        {
            UserDataLayer sql = new UserDataLayer();
            bool status = sql.GetUpdate(user);
            return status;
        }
        #endregion

        #region //显示用户个人信息方法
        public List<UserView> Show(string Uname)
        {
            UserDataLayer sql = new UserDataLayer();
            T_Users user = sql.Show(Uname);
            UserView uv= new UserView();
            List<UserView> luv = new List<UserView>();

            uv.RealName = user.RealName;
            uv.Sex = user.Sex;
            uv.Tel = user.Tel;
            uv.Address = user.Address;
            uv.Email = user.Email;
            uv.PostCode = user.PostCode;

            luv.Add(uv);
            return luv;
        }
        #endregion

        #region //显示用户订单列表
        public List<OrderView> OrderShow(int Uid)
        {
            UserDataLayer udl = new UserDataLayer();
            List<T_Order> lto = udl.OrderShow(Uid);

            List<OrderView> lov = new List<OrderView>();

            foreach(T_Order to in lto)
            {
                OrderView ov = new OrderView();
                ov.Num = to.Num;
                ov.OrderDate = to.OrderDate;
                ov.OrderID = to.OrderID;
                ov.OrderStatus = to.OrderStatus;
                ov.TotalMoney = to.TotalMoney;

                lov.Add(ov);
            }

            return lov;
        }
        #endregion

        #region //显示订单详情
        public List<OrderDetailsView> Details(int OrderID)
        {
            UserDataLayer udl = new UserDataLayer();
            DataTable dt = udl.GetOrderDetails(OrderID);

            IList<OrderDetailsView> odvs = ModelConvertHelper<OrderDetailsView>.ConvertToModel(dt);

            return odvs.ToList();
        }
        #endregion
    }
}
