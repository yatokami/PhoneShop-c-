using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace DataAccessLayer
{
    public static class SqlHelper
    {
        public static readonly string connstr = ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString;
        public static SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            return conn;
        }
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

        public static string GetSqlAsString(string cmdText,params SqlParameter[] parameters)
        {
            using(SqlConnection conn=new SqlConnection(connstr))
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
       
    }
}