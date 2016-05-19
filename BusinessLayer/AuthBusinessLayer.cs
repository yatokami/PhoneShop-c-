using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using DataAccessLayer;

namespace ViewModels
{
    public class AuthBusinessLayer
    {
        #region 用户登录认证
        public int IsValidUser(T_Users user)
        {
            UserDataLayer adl = new UserDataLayer();
            int Uid = adl.IsValidUser(user);

            return Uid;
        }
        #endregion
    }
}
