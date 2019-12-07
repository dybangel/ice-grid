using autosell_center.util;
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

namespace autosell_center.main.Administrators
{
    public partial class Administrators : System.Web.UI.Page
    {
        private string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this.companyID.Value = comID;
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
        }
        [WebMethod]
        public static string getadminList(string companyID)
        {
            
            string sql = "select * from asm_opera where companyID="+companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]
        public static string setPwd(string operaID,string pwd,string phone)
        {
           
            string sql = "select * from asm_opera where id="+operaID+" and pwd='"+ pwd + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string sql1 = "update asm_opera set pwd='"+ pwd + "',linkphone='"+phone+"' where id=" + operaID;
                
                DbHelperSQL.ExecuteSql(sql1);
                return "2";
            }
            else {
                return "1";
            }
        }
        [WebMethod]
        public static string setQX(string operaID, string qx)
        {
           
            string sql1 = "update asm_opera set qx='" + qx + "' where id=" + operaID;
            int a=DbHelperSQL.ExecuteSql(sql1);
            if (a > 0)
            {
                return "1";
            }
            else {
                return "2";
            }
        }
    }
}