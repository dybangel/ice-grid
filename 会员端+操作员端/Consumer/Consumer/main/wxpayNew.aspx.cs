using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using uniondemo.com.allinpay.syb;
using WxPayAPI;

namespace autosell_center
{
    public partial class wxpayNew : System.Web.UI.Page
    {
        public static string wxEditAddrParam { get; set; }
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        public string openID = "";
        public string companyID = "";
        public string money = "0";
        public string productName = "";
        public string mechineID = "";
        public string unionID = "";
        public string productID = "";
        public string dgOrderDetailID = "";
        public string type = "";
        public string sftj = "";
        public string reqsn = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                companyID = Request.QueryString["companyID"].ToString();
                money = Request.QueryString["money"].ToString();
                mechineID = Request.QueryString["mechineID"].ToString();
                unionID = Request.QueryString["unionID"].ToString();
                productID = Request.QueryString["productID"].ToString();
                openID = Request.QueryString["openID"].ToString();
                dgOrderDetailID = Request.QueryString["dgOrderDetailID"].ToString();
                type = Request.QueryString["type"].ToString();
                sftj = Request.QueryString["sftj"].ToString();
                reqsn = Request.QueryString["reqsn"].ToString();
                string asm_pay_infosql = "select 1 from asm_pay_info  where reqsn='" + reqsn + "' ";
                DataTable asm_pay_infodt = DbHelperSQL.Query(asm_pay_infosql).Tables[0];
                if (asm_pay_infodt.Rows.Count > 0)
                {
                    return;
                }
                string sql1 = "select * from asm_member where unionID='" + unionID+"'";
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                if (d1.Rows.Count > 0) {
                    if (!Util.xgCount(productID, d1.Rows[0]["id"].ToString(), mechineID))
                    {
                        Util.Debuglog("限购" + unionID, "是否限购.txt");
                        string url13 = "https://wx.bingoseller.com/main/xg.aspx";
                        //限购不让购买
                        Response.Write("<script>window.location.href='" + url13 + "';</script>");
                        return;
                    }
                    Util.Debuglog("memberid = " + d1.Rows[0]["id"].ToString(), "聚合微信支付.txt");
                }
                Util.Debuglog("openID=" + openID+ ";productName="+ productName+";productID="+ productID + ";unionID="+ unionID+ ";mechineID="+ mechineID, "聚合微信支付.txt");
                this._money.Value = money;
                
                //JsApiPay jsApiPayGet = new JsApiPay(this);
                try
                {
                    string ldno = Util.getLDNO(mechineID, productID);
                    if (string.IsNullOrEmpty(ldno))
                    {
                        Response.Write("<span style='color:#FF0000;font-size:20px'>" + "未获取到出货料道编号，请重试" + "</span>");
                        return;
                    }
                    long fen = 0;
                    try
                    {
                        fen = long.Parse((double.Parse(money) * 100).ToString());
                        //fen = 1;
                        //获取收货地址js函数入口参数
                        this._openID.Value = openID;
                        string url = "https://wx.bingoseller.com/pay/JHNotifyNew.aspx";
                        SybWxPayService sybService = new SybWxPayService(companyID);
                        Dictionary<String, String> rsp = sybService.pay(fen, reqsn, "W02", "购买产品", "商品消费", openID, "", url, "");
                        string json = (new JavaScriptSerializer()).Serialize(rsp);
                        Util.Debuglog("json=" + json, "聚合微信支付.txt");
                        JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                        if (jo["retcode"].ToString() == "SUCCESS")
                        {
                            string pickupdate = "update  asm_product_pick set payStatus=2,paytype='1',startPayTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where reqsnNo='" + reqsn + "' ";
                            Util.Debuglog("sqlInsert=" + pickupdate, "获取预生成订单号.txt");
                            DbHelperSQL.ExecuteSql(pickupdate);
                            string appid = jo["appid"].ToString();
                            string cusid = jo["cusid"].ToString();
                            string trxid = jo["trxid"].ToString();
                            RedisHelper.SetRedisModel<string>(trxid, trxid, new TimeSpan(0, 2, 0));
                            string reqsnNew = jo["reqsn"].ToString();
                            //正式用时换表asm_pay_info_new
                            string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,statu,reqsn,type,payType,trxamt,createTime,acct,unionID,mechineID,productID,companyID,dgOrderDetailID,orderType,sftj)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','0','" + reqsnNew + "',2,1," + double.Parse(money) * 100 + ",'" + DateTime.Now + "','"+openID+"','"+ unionID + "',"+ mechineID + ",'"+ productID + "','"+ companyID + "','"+dgOrderDetailID+"','"+type+"','"+ sftj + "')";
                            Util.Debuglog("insertSQL=" + insertSQL, "获取预生成订单号.txt");
                            DbHelperSQL.ExecuteSql(insertSQL);
                        }
                        foreach (var item in rsp)
                        {
                            if (item.Key == "payinfo")
                            {
                                Util.Debuglog("payinfo=" + item.Value, "聚合微信支付.txt");
                                wxJsApiParam = item.Value;
                            }
                        }
                    }
                    catch
                    {
                        Response.Write("<span style='color:#FF0000;font-size:20px'>" + "金额有误，请重试" + "</span>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面加载出错，请重试" + "</span>");
                }
            }
        }
    }
}