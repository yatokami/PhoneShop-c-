using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
namespace DataAccessLayer
{
    public static class SqlHelper
    {
        //读取配置文件中sql链接数据库语句
        public static readonly string connstr = ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString;

        //打开数据库
        public static SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            return conn;
        }

        //返回受影响语句成功是否有
        public static int ExecuteNonQuery(string cmdText,params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteNonQuery(conn, cmdText, parameters);
            }
        }
        public static int ExecuteNonQuery(SqlConnection conn,string cmdText,params SqlParameter[] parameters)
        {
            using(SqlCommand cmd=conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        
        public static object ExecuteScalar(string cmdText, params SqlParameter[] parameters)
        {
            using(SqlConnection conn=new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteScalar(conn, cmdText, parameters);
            }
        }
        public static object ExecuteScalar(SqlConnection conn,string cmdText,params SqlParameter[] parameters)
        {
            using(SqlCommand cmd=conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        //返回数据库中数据集合
        public static DataTable ExecuteDataTable(string cmdText,params SqlParameter[] parameters)
        {
            using(SqlConnection conn=new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteDataTable(conn, cmdText, parameters);
            }
        }
        public static DataTable ExecuteDataTable(SqlConnection conn,string cmdText,params SqlParameter[] parameters)
        {
            using(SqlCommand cmd=conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                using(SqlDataAdapter da=new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        //返回int类型的一个字段一条的数据
        public static int GetSqlAsInt(string cmdText,params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return GetSqlAsInt(conn, cmdText, parameters);
            }
        }
        public static int GetSqlAsInt(SqlConnection conn, string cmdText, params SqlParameter[] parameters)
        {
            int result = 0;
            SqlDataReader reader;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                reader = cmd.ExecuteReader();
                if (reader != null)
                    if (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
            }
            return result;
        }

        //返回String类型的一个字段一条的数据
        public static string GetSqlAsString(string cmdText,params SqlParameter[] parameters)
        {
            using(SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return GetSqlAsString(conn, cmdText, parameters);
            }
        }

        public static string GetSqlAsString(SqlConnection conn,string cmdText,params SqlParameter[] parameters)
        {
            string result = "";
            SqlDataReader reader;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                reader = cmd.ExecuteReader();
                if (reader != null)
                    if (reader.Read())
                    {
                        result = reader.GetString(0);
                    }
            }
            return result;
        }

        //进行事务操作
        public static int ExecuteSqlTran(Hashtable SQLStringList)
        {
            using(SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteSqlTran(conn, SQLStringList);
            }
        }

        public static int ExecuteSqlTran(SqlConnection conn, Hashtable SQLStringList)
        {
            int val = 0;
            using(SqlTransaction trans = conn.BeginTransaction())
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    //循环
                    foreach (DictionaryEntry myDE in SQLStringList)
                    {
                        string cmdText = myDE.Key.ToString();
                        SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                        PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                        val += cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
            return val;
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

    }
}