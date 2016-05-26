using BusinessEntities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Core;
namespace DataAccessLayer
{
    public class AdminDataLayer
    {
        //获取管理员登录权限
        public int IsValidUser(T_Admin admin)
        {
            try
            {
                int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_Admin where AdminName = @AdminName and AdminPwd = @AdminPwd", new SqlParameter("@AdminName", admin.AdminName), new SqlParameter("@AdminPwd", admin.AdminPwd));
                return count;
            }
            catch
            {
                return 0;
            }
        }

        //获取用户信息列表
        public List<T_Users> GetUsers(string Uname)
        {
            Uname = "%" + Uname + "%";
            DataTable dt = null;
            if (Uname == null || Uname == "")
            {
                dt = SqlHelper.ExecuteDataTable("select * from T_Users");

            }
            else
            {
                dt = SqlHelper.ExecuteDataTable("select * from T_Users where Uname  like @Uname", new SqlParameter("@Uname", Uname));
            }
            IList<T_Users> users = ModelConvertHelper<T_Users>.ConvertToModel(dt);

            return users.ToList();
        }

        //修改用户密码
        public int Changepwd(string Uname, string Upwd)
        {
            int count = SqlHelper.ExecuteNonQuery("update T_Users set Upwd = @Upwd where Uname = @Uname", new SqlParameter("@Upwd", Upwd), new SqlParameter("@Uname", Uname));

            return count;
        }
    }
}
