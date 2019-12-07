using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.enterprise
{
    public partial class Thepublicjb : System.Web.UI.Page
    {
        public string companyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            companyID = Request.QueryString["companyID"].ToString();
            this.company_ID.Value = companyID;
            initData();
        }
        public void initData()
        {
            string sql = "select * from asm_company where id="+companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                this.appID.Value = dt.Rows[0]["wx_appid"].ToString();
                
                this.appSecret.Value = dt.Rows[0]["wx_appsecret"].ToString();
                
                this.APP_ID.Value = dt.Rows[0]["tl_APPID"].ToString();
                this.CUSID.Value = dt.Rows[0]["tl_CUSID"].ToString();
                this.APPKEY.Value= dt.Rows[0]["tl_APPKEY"].ToString();
            }
           
        }
        [WebMethod]
        public static string update(string val,string str,string companyID)
        {
            //var 1  appID  2appSecret   3token 4mch_id 5partnerKey
            if (val == "1")
            {
                string sql = "update asm_company set wx_appid='" + str + "' where id=" + companyID;
                DbHelperSQL.ExecuteSql(sql);
                return "1";
            } else if (val=="2")
            {
                string sql = "update asm_company set wx_appsecret='" + str + "' where id=" + companyID;
                DbHelperSQL.ExecuteSql(sql);
                return "1";
            }
            else if (val == "3")
            {
                string sql = "update asm_company set wx_token='" + str + "' where id=" + companyID;
                DbHelperSQL.ExecuteSql(sql);
                return "1";
            }
            else if (val == "4")
            {
                string sql = "update asm_company set tl_APPID='" + str + "' where id=" + companyID;
                DbHelperSQL.ExecuteSql(sql);
                return "1";
            }
            else if (val == "5")
            {
                string sql = "update asm_company set tl_CUSID='" + str + "' where id=" + companyID;
                DbHelperSQL.ExecuteSql(sql);
                return "1";
            }
            else if (val == "6")
            {
                string sql = "update asm_company set tl_APPKEY='" + str + "' where id=" + companyID;
                DbHelperSQL.ExecuteSql(sql);
                return "1";
            }

            return "2";
        }
    }
}