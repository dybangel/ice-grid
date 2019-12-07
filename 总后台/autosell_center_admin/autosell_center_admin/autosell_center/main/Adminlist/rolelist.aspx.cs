using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.Adminlist
{
    public partial class rolelist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string sear()
        {
            string sql = "select * from asm_role where companyID=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            return "1";
        }
        [WebMethod]
        public static string add(string name,string qx)
        {
            string sql = "select * from asm_role where name='"+name+"'";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count > 0)
            {
                return "1";//名称已经存在
            }
            else {
                string insert = "insert into asm_role(name,companyID) values('"+name+ "',0);select @@IDENTITY";
                object obj = DbHelperSQL.GetSingle(insert);
                if (obj == null)
                {
                    return "2";//插入失败
                }
                else
                {
                    //插入权限
                    string[] qxArr = qx.Split(',');
                    if (qxArr.Length>0)
                    {
                        for (int i=0;i<qxArr.Length; i++)
                        {
                            if (qxArr[i].IndexOf('-')>-1)
                            {
                                string[] qx_one = qxArr[i].Split('-');
                                string sql1 = "insert into asm_qx (roleID,menuID,flag) values('" + obj + "','" + qx_one[0] + "','" + qx_one[1] + "')";
                                DbHelperSQL.ExecuteSql(sql1);
                            }
                          
                        }
                    }
                }
            }
            return "3";
        }
        [WebMethod]
        public static string getQX(string qxID)
        {
            string sql = "select * from asm_qx where roleID="+qxID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            return "1";
        }
        [WebMethod]
        public static string updateQX(string name,string qx,string roleID)
        {
            string sql = "select * from asm_role where id!="+roleID+" and name='"+name+"'";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count>0)
            {
                return "1";//不允许重名
            }
            string updateSQL = "update asm_role set name='"+name+"' where id="+roleID;
            DbHelperSQL.ExecuteSql(updateSQL);
            string[] qxArr = qx.Split(',');
            if (qxArr.Length > 0)
            {
                for (int i = 0; i < qxArr.Length; i++)
                {
                    if (qxArr[i].IndexOf('-') > -1)
                    {
                        string[] qx_one = qxArr[i].Split('-');
                        string sql1 = "update asm_qx set flag="+ qx_one[1] + " where roleID="+roleID+" and menuID='"+ qx_one[0] + "'";

                        DbHelperSQL.ExecuteSql(sql1);
                    }

                }
            }
            return "2";
        }
    }
}