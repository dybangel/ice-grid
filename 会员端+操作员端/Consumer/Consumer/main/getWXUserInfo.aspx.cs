
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
    public partial class getWXUserInfo : System.Web.UI.Page
    {
        public string appid = "";
        public string companyID = "";
        public string money = "0";
        public string mechineID = "";
        public string productID = "";
        public string dgOrderDetailID = "";//如果是半价出售的才会有值 且不是0
        public string type = "";//1订购 2零售 3半价
        public string sftj = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               
                companyID = Request.QueryString["companyID"].ToString();
                mechineID = Request.QueryString["mechineID"].ToString();
                money = Request.QueryString["money"].ToString();
                productID = Request.QueryString["productID"].ToString();
                dgOrderDetailID = Request.QueryString["dgOrderDetailID"].ToString();
                type = Request.QueryString["type"].ToString();
                sftj = Request.QueryString["sftj"].ToString();
                string sql2 = "select * from asm_company where id=" + companyID;
                DataTable d1 = DbHelperSQL.Query(sql2).Tables[0];
                if (d1.Rows.Count > 0)
                {
                    appid = d1.Rows[0]["appId"].ToString();
                }
                string url = "https://" + HttpContext.Current.Request.Url.Host + "/main/getWXInfo.aspx";
                string param = money + "|" + companyID + "|" + mechineID + "|" + productID +"|"+dgOrderDetailID+"|"+type+ "|"+ sftj;
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