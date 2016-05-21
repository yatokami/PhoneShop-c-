using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AdminDataLayer
    {
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
    }
}
