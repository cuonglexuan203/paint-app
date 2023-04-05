using paint.src.Layers.DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace paint.src.Layers.BussinessLayer
{
    internal class BSAction
    {
        DBMain dbmain = null;
        //
        public BSAction()
        {
            dbmain = new DBMain();
        }

        public BSAction(string strConn)
        {
            dbmain = new DBMain(strConn);
        }
        //
        public string GetAction(int id, ref string errMsg, bool isDeleteItem = false)
        {
            string query = $"select controlItem from PaintActions where id = {id}";

            string res =  dbmain.ExecuteScalarByString(query, System.Data.CommandType.Text, ref errMsg);
            if(isDeleteItem)
            {
                string err = string.Empty;
                DeleteAction(id, ref err);
                if (string.IsNullOrEmpty(err))
                {
                    MessageBox.Show(err);
                }
            }
            return res;
        }
        public string GetFirstAction(ref string errMsg, bool isDeleteItem = false)
        {
            int id = GetFirstActionId(ref errMsg);
            return GetAction(id, ref errMsg, isDeleteItem);
        }
        public string GetLastAction(ref string errMsg, bool isDeleteItem = false)
        {
            int id = GetLastActionId(ref errMsg);
            return GetAction(id, ref errMsg, isDeleteItem);
        }
        public int GetLastActionId(ref string errMsg)
        {
            string query = $"select top 1 * from PaintActions order by id desc";
            return dbmain.ExecuteScalar(query, System.Data.CommandType.Text, ref errMsg);
        }
        public int GetFirstActionId(ref string errMsg)
        {
            string query = $"select top 1 * from PaintActions";
            return dbmain.ExecuteScalar(query, System.Data.CommandType.Text, ref errMsg);
        }
        //
        public bool AddAction(string actionStr, int actionType,  ref string errMsg)
        {
            // actionType ( -1: mouse down, 0: click, 1: mouse up)
            string query = $"insert into PaintActions(controlItem, actionType) values ('{actionStr}', { actionType})";
            return dbmain.ExecuteNonQuery(query, System.Data.CommandType.Text, ref errMsg);
        }
        //
        public bool DeleteAction(int id, ref string errMsg)
        {
            string query = $"delete from PaintActions where id = {id}";
            return dbmain.ExecuteNonQuery(query, System.Data.CommandType.Text, ref errMsg);
        }
    }
}
