using Consumer.cls;
using DBUtility;
using OpenPlatForm.Common;
using System;
using System.Configuration;
using System.Data;
using System.Web;

namespace Mine.Members
{
    public partial class WXCallback : System.Web.UI.Page
    {
        public string appid = "";
        public string companyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            companyID = Request.QueryString["companyID"].ToString();
            string sql2 = "select * from asm_company where id=" + companyID;
            DataTable d1 = DbHelperSQL.Query(sql2).Tables[0];
            if (d1.Rows.Count > 0)
            {
                appid = d1.Rows[0]["appId"].ToString();
            }
            string url = "https://" + HttpContext.Current.Request.Url.Host + "/main/WXinformation.aspx?companyID=" + companyID;
            String shouquan = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=" + url + "&response_type=code&scope=snsapi_userinfo&component_appid=" + OpenPFConfig.Appid + "#wechat_redirect";
            Util.Debuglog("shouquan1=" + shouquan, "微信回调_.txt");
            Util.Debuglog("shouquan1="+ shouquan,"微信回调_.txt");
            Response.Redirect(shouquan);
        }
    }
}