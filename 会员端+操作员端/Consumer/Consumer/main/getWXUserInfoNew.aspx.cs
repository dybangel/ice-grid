
using Consumer.cls;
using DBUtility;
using OpenPlatForm.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center
{
    public partial class getWXUserInfoNew : System.Web.UI.Page
    {
        public string appid = "";
        public string reqsn = "";
        public string companyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                reqsn = Request.QueryString["reqsn"].ToString();
                companyID = Request.QueryString["companyID"].ToString();
                string pickupdate = "update  asm_product_pick set sacntype=1 where reqsnNo='" + reqsn + "' ";
                Util.Debuglog("sqlInsert=" + pickupdate, "获取预生成订单号.txt");
                DbHelperSQL.ExecuteSql(pickupdate);
                string sql2 = "select * from asm_company where id=" + companyID;
                DataTable d1 = DbHelperSQL.Query(sql2).Tables[0];
                if (d1.Rows.Count > 0)
                {
                    appid = d1.Rows[0]["appId"].ToString();
                }
                string url = "https://" + HttpContext.Current.Request.Url.Host + "/main/getWXInfoNew.aspx";
                string param =   companyID + "|"+reqsn ;
                String shouquan = "https://open.weixin.qq.com/connect/oauth2/authorize?appid="+appid+"&redirect_uri="+url+ "&response_type=code&scope=snsapi_userinfo&state="+param+"&component_appid=" + OpenPFConfig.Appid+"#wechat_redirect";
                Response.Redirect(shouquan);
            }
            catch
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "参数不全请重试" + "</span>");
            }
        }
    }
}