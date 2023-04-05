using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paint.src.Layers.DatabaseLayer
{
    internal class DBMain
    {
        string strConnection = "Data Source=.;" +
                                "Initial Catalog=PaintApp;" +
                                "Integrated Security=True";

        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataAdapter adapter = null;
        SqlDataReader reader = null;
        //
        public DBMain()
        {
            conn = new SqlConnection(strConnection);
            cmd = conn.CreateCommand();
        }
        public DBMain(string strConn)
        {
            conn = new SqlConnection(strConn);
            cmd = conn.CreateCommand();
        }
        //

        public DataSet ExecuteQueryDataSet(string query, CommandType type)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            conn.Close();
            return ds;
        }

        public SqlDataReader ExecuteQueryDataReader(string query, CommandType type)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            conn.Close();
            return reader;
        }
        //
        public bool ExecuteNonQuery(string query, CommandType type, ref string errMsg)
        {
            bool successed = false;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.CommandText = query;
            cmd.CommandType = type;
            try
            {
                cmd.ExecuteNonQuery();
                successed = true;
            }
            catch (SqlException e)
            {
                errMsg = e.Message;
            }
            finally { conn.Close(); }
            return successed;
        }
        public int ExecuteScalar(string query, CommandType type, ref string errMsg)
        {
            int count = 0;
            string res = string.Empty;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.CommandText = query;
            cmd.CommandType = type;
            try
            {
                count = (int)cmd.ExecuteScalar();

            }
            catch (SqlException e)
            {
                errMsg = e.Message;
            }
            finally { conn.Close(); }
            return count;
        }
        public string ExecuteScalarByString(string query, CommandType type, ref string errMsg)
        {
            string res = string.Empty;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.CommandText = query;
            cmd.CommandType = type;
            try
            {
                res = ((string)cmd.ExecuteScalar()).Trim();

            }
            catch (SqlException e)
            {
                errMsg = e.Message;
            }
            finally { conn.Close(); }
            return res;
        }
    }
}
