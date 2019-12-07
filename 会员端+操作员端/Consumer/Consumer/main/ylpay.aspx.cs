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
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using uniondemo.com.allinpay.syb;
using WxPayAPI;

namespace Consumer.main
{
    public partial class ylpay : System.Web.UI.Page
    {
        public static string wxEditAddrParam { get; set; }
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        public string openID = "";
        public string companyID = "";
        public string money = "0";
        public string dzMoney = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                openID = Request.QueryString["openID"].ToString();
                companyID = Request.QueryString["companyID"].ToString();
                money = Request.QueryString["money"].ToString();
                dzMoney = Request.QueryString["dzMoney"].ToString();
                this._money.Value = money;
                this._dzMOney.Value = dzMoney;

                JsApiPay jsApiPayGet = new JsApiPay(this);
                try
                {
                    if (OperUtil.getCooki("vshop_openID") != "0")
                    {
                        this._companyID.Value = Request.QueryString["companyID"].ToString();
                        this._openID.Value = OperUtil.getCooki("vshop_openID");
                        openID= OperUtil.getCooki("vshop_openID");
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
                     
                    int fen = 0;
                    try
                    {
                         fen = int.Parse(money) * 100;
                        //获取收货地址js函数入口参数
                        this._openID.Value = openID;
                        string url = "https://wx.bingoseller.com/pay/Notify.aspx";
                        SybWxPayService sybService = new SybWxPayService(companyID);
                        Dictionary<String, String> rsp = sybService.pay(fen, DateTime.Now.ToFileTime().ToString(), "W02", "会员充值", "备注", openID, "", url, "");
                        string json = (new JavaScriptSerializer()).Serialize(rsp);
                        Util.Debuglog("json=" +json, "_充值日志.txt");
                        JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                        if (jo["retcode"].ToString()== "SUCCESS")
                        {
                            string appid = jo["appid"].ToString();
                            string cusid = jo["cusid"].ToString();
                            string trxid = jo["trxid"].ToString();
                            string reqsn = jo["reqsn"].ToString();
                            //插入预订单信息 为了避免插入失败循环插入5次
                            for (int i=0;i<10;i++)
                            {
                                string sql = "select * from asm_pay_info where trxid='" + trxid + "'";
                                DataTable ds = DbHelperSQL.Query(sql).Tables[0];
                                if (ds.Rows.Count <= 0)
                                {
                                    string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,acct,statu,reqsn,[type],payType,trxamt,dzMoney)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','" + openID + "','0','" + reqsn + "',1,3," + fen + "," + dzMoney + ")";
                                    Util.Debuglog("insertSQL=" + insertSQL, "_充值日志.txt");
                                    DbHelperSQL.ExecuteSql(insertSQL);
                                }
                                else {
                                    break;
                                }
                                Thread.Sleep(300);
                            }
                             
                        }
                        foreach (var item in rsp)
                        {
                           
                            if (item.Key == "payinfo")
                            {
                                wxJsApiParam = item.Value;
                           
                            }
                        }
                    }
                    catch {
                        Response.Write("<span style='color:#FF0000;font-size:20px'>" + "金额有误，请重试" + "</span>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面加载出错，请重试" + "</span>");
                }
            }
        }
        [WebMethod]
        public static string addInsert(string money,string openID,string dzMoney,string companyID)
        {
             
            //判读是否是第一充值领红包是的话弹出红包已经到账
            string sql = "select * from asm_moneyChange where memberID in(select id from asm_member where openID='"+openID+"') and type=1";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count==1)
            {
                string sql1 = "select * from asm_moneyChange where memberID in(select id from asm_member where openID='" + openID + "') and type=7";
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                if (dt.Rows.Count>0)
                {
                    return "1";
                }
            }
            return "2";
        }
    }
}