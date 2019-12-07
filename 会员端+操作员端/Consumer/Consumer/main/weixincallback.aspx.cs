using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Consumer
{
    public partial class weixincallback : System.Web.UI.Page
    {
        public string appid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string companyID = Request.QueryString["companyID"].ToString();
            string req = Request.QueryString["req"].ToString();
            string sql2 = "select * from asm_company where id=" + companyID;
            DataTable d1 = DbHelperSQL.Query(sql2).Tables[0];
            Util.SetSession("companyID", companyID);
            if (d1.Rows.Count > 0)
            {
                appid = d1.Rows[0]["appId"].ToString();
            }
            //String shouquan = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=http://" + HttpContext.Current.Request.Url.Host + "/main/weixinInfo.aspx?req=" + req +"&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect";
            String shouquan = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=https://" + HttpContext.Current.Request.Url.Host + "/main/weixinInfo.aspx?req=" + req + "&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect";

            Response.Redirect(shouquan);
        }
    }
}