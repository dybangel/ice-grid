using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using uniondemo.com.allinpay.syb;

namespace Consumer.main
{
    public partial class alipaypage : System.Web.UI.Page
    {


        public string companyID;
        public string money = "0";
        public string productID = "";
        public string mechineID = "";
        public string dgOrderDetailID = "";
        public string type = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    companyID = Request.QueryString["companyID"].ToString();
                    money = Request.QueryString["money"].ToString();
                    productID = Request.QueryString["productID"].ToString();
                    mechineID = Request.QueryString["mechineID"].ToString();
                    dgOrderDetailID = Request.QueryString["dgOrderDetailID"].ToString();
                    type = Request.QueryString["type"].ToString();
                   // money = "1";
                    Util.Debuglog("companyID=" + companyID+ ";money="+ money+ ";productID="+ productID+ ";mechineID="+mechineID+ ";dgOrderDetailID="+ dgOrderDetailID+ ";type="+ type, "_聚合支付支付宝.txt");
                    SybWxPayService sybService = new SybWxPayService(companyID);
                    string url = "https://wx.bingoseller.com/pay/JHNotify.aspx";
                    string data = "";
                    string sql = "select *from asm_product where productID="+productID;
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    Dictionary<String, String> rsp = sybService.pay(long.Parse((double.Parse(money)*100).ToString()), DateTime.Now.ToFileTime().ToString(), "A01", dt.Rows[0]["proName"].ToString(), "商品消费", "", "", url, "");
                    data = OperUtil.SerializeDictionaryToJsonString(rsp);
                    string json = (new JavaScriptSerializer()).Serialize(rsp);
                    Util.Debuglog("json=" + json, "_聚合支付支付宝.txt");
                    JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                    if (jo["retcode"].ToString() == "SUCCESS")
                    {
                        string appid = jo["appid"].ToString();
                        string cusid = jo["cusid"].ToString();
                        string trxid = jo["trxid"].ToString();
                        string reqsn = jo["reqsn"].ToString();
                        string payinfoUrl = jo["payinfo"].ToString();
                        Util.Debuglog("payinfoUrl=" + payinfoUrl, "_聚合支付支付宝.txt");
                        string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,statu,reqsn,type,payType,trxamt,createTime,mechineID,productID,companyID,dgOrderDetailID,orderType)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','0','" + reqsn + "',2,2," + double.Parse(money) * 100 + ",'" + DateTime.Now + "','"+mechineID+"','"+productID +"','"+ companyID + "','"+dgOrderDetailID+"','"+type+"')";
                        Util.Debuglog("insertSQL="+ insertSQL, "_聚合支付支付宝.txt");
                        DbHelperSQL.ExecuteSql(insertSQL);
                        Response.Write("<script>window.location.href='" + payinfoUrl + "';</script>");
                        // //插入预订单信息


                    }
                }
                catch (Exception ex)
                {
                    Util.Debuglog("Exception=" + ex.Message, "_聚合支付支付宝.txt");
                }

            }
        }
    }
}