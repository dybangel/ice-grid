using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using uniondemo.com.allinpay.syb;
using WxPayAPI;
namespace Consumer.main
{
    public partial class purchase : System.Web.UI.Page
    {
        
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        public static string wxEditAddrParam { get; set; }
        public string yue = "0";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                this._companyID.Value = OperUtil.getCooki("companyID");
                this._openID.Value = OperUtil.getCooki("vshop_openID");
                this.memberID.Value = Util.getMemberID();
                string sql = "select * from asm_member where id="+this.memberID.Value;
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                yue = dd.Rows[0]["AvailableMoney"].ToString();
                if (dd.Rows[0]["pwd"].ToString()=="000000")
                {
                    Response.Write("<script>alert('您当前交易密码为初始密码，请前往个人中心修改!');</script>");
                }
            }
            else
            {
                this._companyID.Value = OperUtil.getCooki("companyID");
                string userAgent = Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    Response.Redirect("WXCallback.aspx?companyID=" + OperUtil.getCooki("companyID"));
                    return;
                }
            }
            if (!IsPostBack)
            {

            }
        }

        [WebMethod]
        public static string getPayList(string companyID)
        {
            string sql = "select * from asm_pay_activity where companyID=" + companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            
            if (dt.Rows.Count > 0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            else
            {
                return "1";
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
           
        }
        private void printRsp(Dictionary<String, String> rspDic)
        {
            string rsp = "请求返回数据:\n";
            foreach (var item in rspDic)
            {
                rsp += item.Key + "-----" + item.Value + ";\n";
            }
        }
    }
}