using BusinessEntities;
using DataAccessLayer;
using System.Collections.Generic;
using ViewModels;

namespace BusinessLayer
{
    public class AdminBusinessLayer
    {

        #region 获取用户信息列表
        public List<UserView> GetUsers(string Uname)
        {
            AdminDataLayer adl = new AdminDataLayer();
            List<T_Users> users = adl.GetUsers(Uname);
            List<UserView> luv = new List<UserView>();
            foreach (T_Users user in users)
            {
                UserView uv = new UserView();
                uv.Uname = user.Uname;
                uv.Tel = user.Tel;
                uv.RegisterTime = user.RegisterTime;
                uv.RealName = user.RealName;
                uv.Sex = user.Sex;
                uv.Address = user.Address;
                uv.Email = user.Email;

                luv.Add(uv);
            }

            return luv;
        }
        #endregion

    }
}
