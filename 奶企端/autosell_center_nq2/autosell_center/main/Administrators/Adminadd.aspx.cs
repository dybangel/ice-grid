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
    public partial class Adminadd : System.Web.UI.Page
    {
        private string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this.companyId.Value = comID;
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
        }
        [WebMethod]
        public static string add(string bh,string pwd1,string qx,string companyID,string phone)
        {
            
            string sql = "select * from asm_opera where name='"+bh+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
           
            if (dt.Rows.Count > 0)
            {
                return "1";//账号重复
            }
            else {
                string sql1 = "insert into asm_opera(name,companyID,pwd,createTime,qx,linkphone) values('" + bh+"',"+companyID+",'"+pwd1+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+qx+",'"+phone+"')";
              
                DbHelperSQL.ExecuteSql(sql1);
          
                return "2";
            }
           
        }
    }
}