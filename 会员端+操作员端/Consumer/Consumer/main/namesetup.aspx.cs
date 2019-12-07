using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Consumer.main
{
    public partial class namesetup : System.Web.UI.Page
    {
        public string memberID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                this.member_ID.Value = Util.getMemberID();
            }
            else
            {
                string userAgent = Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    Response.Redirect("WXCallback.aspx?companyID=" + OperUtil.getCooki("companyID"));
                    return;
                }
            }


            string sql1 = "select * from asm_member where id='" + this.member_ID.Value + "'";
        
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.nickname.Value = dt.Rows[0]["nickname"].ToString();
            }
        }
        [WebMethod]
        public static string setupOk(string nickname, string memberID)
        {
            string sql1 = "select * from asm_member where id='"+memberID+"'";
            DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
           

            string sql = "update asm_member set nickname='" + nickname + "' where id='" + memberID + "'";
            Util.Debuglog("update=" + sql, "_修改姓名.txt");
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                if (dd.Rows.Count > 0)
                {
                    if (!dd.Rows[0]["certF"].ToString().Contains("name"))
                    {
                        string sql2 = "update asm_member set certF=isnull(certF,'')+',name' where id='" + memberID + "'";
                        DbHelperSQL.ExecuteSql(sql2);
                    }
                }
               
                return "1";
            }
            else
            {
                return "2";
            }

        }
    }
}