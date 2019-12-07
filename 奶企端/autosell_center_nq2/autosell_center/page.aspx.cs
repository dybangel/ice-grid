using autosell_center.util;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using uniondemo.com.allinpay.syb;

namespace autosell_center
{
    public partial class page : System.Web.UI.Page
    {
        public string mechineID = "";
        public string payMoney = "0";
        public string asmpayid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userAgent = Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    this.type.Value = "wx";
                }
                else if (userAgent.ToLower().Contains("alipayclient"))
                {
                    this.type.Value = "zfb";
                }
                mechineID = Request.QueryString["mechineID"].ToString();
                payMoney = Request.QueryString["payMoney"].ToString();
                asmpayid = Request.QueryString["asmpayid"].ToString();
              
                SybWxPayService sybService = new SybWxPayService(mechineID);
                string url = "http://nq.bingoseller.com/pay/Notify.aspx";
                string data = "";
                if (this.type.Value == "wx")
                {
                    long f = long.Parse((double.Parse(payMoney) * 100).ToString());
                    Dictionary<String, String> rsp = sybService.pay(f, DateTime.Now.ToFileTime().ToString(), "W01", "", "", "", "", url, "");
                    data = OperUtil.SerializeDictionaryToJsonString(rsp);
                    //插入预处理订单信息
                    string json = (new JavaScriptSerializer()).Serialize(rsp);
                    JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                    if (jo["retcode"].ToString() == "SUCCESS")
                    {
                        string appid = jo["appid"].ToString();
                        string cusid = jo["cusid"].ToString();
                        string trxid = jo["trxid"].ToString();
                        string reqsn = jo["reqsn"].ToString();
                        string payinfo = jo["payinfo"].ToString();
                        string update = "update asm_pay_info set appid='" + appid + "',cusid='" + cusid + "',trxid='" + trxid + "',statu='0',reqsn='" + reqsn + "',type='2',payType='1',trxamt=" + (double.Parse(payMoney) * 100) + " where asmpayid='" + asmpayid + "'";
                        url = payinfo;
                        DbHelperSQL.ExecuteSql(update);
                        Response.Write("<script>top.location.href='"+payinfo+"';</script>");
                    }
                }
                else if (this.type.Value == "zfb")
                {
                    Dictionary<String, String> rsp = sybService.pay(long.Parse((double.Parse(payMoney) * 100).ToString()), DateTime.Now.ToFileTime().ToString(), "A01", "", "", "", "", url, "");
                    data = OperUtil.SerializeDictionaryToJsonString(rsp);
                    string json = (new JavaScriptSerializer()).Serialize(rsp);
                    JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                    if (jo["retcode"].ToString() == "SUCCESS")
                    {
                        string appid = jo["appid"].ToString();
                        string cusid = jo["cusid"].ToString();
                        string trxid = jo["trxid"].ToString();
                        string reqsn = jo["reqsn"].ToString();
                        string payinfo = jo["payinfo"].ToString();
                       
                        string update = "update asm_pay_info set appid='" + appid + "',cusid='" + cusid + "',trxid='" + trxid + "',statu='0',reqsn='" + reqsn + "',type='2',payType='2',trxamt=" + (double.Parse(payMoney) * 100) + " where asmpayid='" + asmpayid + "'";
                        DbHelperSQL.ExecuteSql(update);
                        Response.Redirect(payinfo);
                    }
                }
                else
                {
                    Response.Write("请选择微信或者支付宝打开");
                }
            }
            
        }
    }
}