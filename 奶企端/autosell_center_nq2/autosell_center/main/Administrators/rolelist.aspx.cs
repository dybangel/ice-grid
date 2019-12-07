using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.Adminlist
{
    public partial class rolelist : System.Web.UI.Page
    {
        private string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this.operID.Value = OperUtil.Get("operaID");
            this.companyID.Value = comID;
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
        }
        [WebMethod]
        public static object judge(string operaID, string menuID)
        {
            Boolean b = Util.judge(operaID, menuID);
            if (b)
            {
                return new { code = 200 };
            }
            else
            {
                return new { code = 500 };
            }
        }
        [WebMethod]
        public static string sear(string companyID)
        {
           
            string sql = "select * from asm_role where companyID="+ companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            return "1";
        }
        [WebMethod]
        public static string add(string name,string qx,string companyID)
        {
          
            string sql = "select * from asm_role where name='"+name+"' and companyID="+companyID;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count > 0)
            {
                return "1";//名称已经存在
            }
            else {
               
              
                string insert = "insert into asm_role(name,companyID) values('"+name+ "',"+companyID+");select @@IDENTITY";
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
        public static string updateQX(string name,string qx,string roleID,string companyID)
        {
          
            string sql = "select * from asm_role where id!="+roleID+" and name='"+name+"' and companyID="+companyID;
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
                    string[] qx_one = qxArr[i].Split('-');
                    string sql2 = "select * from [dbo].[asm_qx] where roleid="+roleID+" and menuID='"+ qx_one[0] + "'";
                    DataTable dt = DbHelperSQL.Query(sql2).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        if (qxArr[i].IndexOf('-') > -1)
                        {
                            string sql1 = "update asm_qx set flag=" + qx_one[1] + " where roleID=" + roleID + " and menuID='" + qx_one[0] + "'";
                            DbHelperSQL.ExecuteSql(sql1);
 
                        }
                    }
                    else {
                        if (qxArr[i]!="")
                        {
                            string sql1 = "insert into asm_qx (roleID,menuID,flag) values('" + roleID + "','" + qx_one[0] + "','" + qx_one[1] + "')";
                            DbHelperSQL.ExecuteSql(sql1);
                           
                        }
                        
                    }
                }
            }
            return "2";
        }
        [WebMethod]
        public static string del(string id)
        {
          
            string sql2 = "select * from asm_opera where qx="+id;
            DataTable dt= DbHelperSQL.Query(sql2).Tables[0];
            if (dt.Rows.Count>0)
            {
                return "3";
            }
            string sql = "delete from asm_role where id="+id;
            int a=DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                string sql1 = "delete from asm_qx where roleID="+id;
                DbHelperSQL.ExecuteSql(sql1);

                return "1";
            }
            else {
                return "2";
            }
        }
    }
}