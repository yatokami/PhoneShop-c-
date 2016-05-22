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

        #region 管理员登录认证权限
        public bool IsValidAdmin(T_Admin admin)
        {
            AdminDataLayer adl = new AdminDataLayer();
            int count = adl.IsValidUser(admin);
            bool AuthenticatedAdmin = false;
            if (count == 1)
            {
                AuthenticatedAdmin = true;
            }
            return AuthenticatedAdmin;
        }
        #endregion
    }
}
