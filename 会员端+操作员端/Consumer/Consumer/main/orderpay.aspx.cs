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
using WxPayAPI;

namespace Consumer.main
{
    public partial class orderpay : System.Web.UI.Page
    {
        public static string wxEditAddrParam { get; set; }
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        public string openID = "";
        public string companyID = "";
        public string money = "0";
        public string proname = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //支付信息
                this.mechine_id.Value = Request.QueryString["mechine_id"].ToString();
                this.product_id.Value= Request.QueryString["product_id"].ToString();
                Util.Debuglog("机器id=" + this.mechine_id.Value + ";产品id=" + this.product_id.Value + ";", "会员下订单.txt");
                this._pszq.Value = Request.QueryString["_pszq"].ToString();
                Util.Debuglog("配送周期=" + this._pszq.Value, "会员下订单.txt");
                this._qsDate.Value = Request.QueryString["_qsDate"].ToString();
                Util.Debuglog("起送周期=" + this._qsDate.Value, "会员下订单.txt");
                this._zdDate.Value = Request.QueryString["_zdDate"].ToString();
                Util.Debuglog("止订周期=" + this._zdDate.Value, "会员下订单.txt");
                this._psStr.Value = Request.QueryString["_psStr"].ToString();
                Util.Debuglog("配送方式_psStr=" + this._psStr.Value, "会员下订单.txt");
                this._psfs.Value = Request.QueryString["_psfs"].ToString();
                Util.Debuglog("_psfs=" + this._psfs.Value, "会员下订单.txt");
                //this._selDate.Value = Request.QueryString["_selDate"].ToString();
                //Util.Debuglog("_selDate=" + this._selDate.Value, "会员下订单.txt");
                this._orderNO.Value = Request.QueryString["_orderNO"].ToString();
                Util.Debuglog("_orderNO=" + this._orderNO.Value, "会员下订单.txt");
                this._createTime.Value = Request.QueryString["_createTime"].ToString();
                Util.Debuglog("_createTime=" + this._createTime.Value, "会员下订单.txt");
                this._fkzt.Value = Request.QueryString["_fkzt"].ToString();
                Util.Debuglog("_fkzt=" + this._fkzt.Value, "会员下订单.txt");
                this._totalMoney.Value = Request.QueryString["_totalMoney"].ToString();
                Util.Debuglog("_totalMoney=" + this._totalMoney.Value, "会员下订单.txt");
                this._yhfs.Value = Request.QueryString["_yhfs"].ToString();
                Util.Debuglog("_yhfs=" + this._yhfs.Value, "会员下订单.txt");

                string sql = "select * from asm_mechine where id="+this.mechine_id.Value;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                companyID =dt.Rows[0]["companyID"].ToString();
                money = Request.QueryString["money"].ToString();
                proname = Request.QueryString["proname"].ToString();
                JsApiPay jsApiPayGet = new JsApiPay(this);
                try
                {
                    if (OperUtil.getCooki("vshop_openID") != "0")
                    {
                        this._companyID.Value = Request.QueryString["companyID"].ToString();
                        this._openID.Value = OperUtil.getCooki("vshop_openID");
                        openID = OperUtil.getCooki("vshop_openID");
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
                    double fen = 0;
                    try
                    {
                        fen = double.Parse(money) * 100;
                       
                        //获取收货地址js函数入口参数
                        this._openID.Value = openID;
                        Util.Debuglog("openID=" + openID+ ":companyID="+ companyID+"fen="+fen, "微信支付.txt");
                        string url = "https://wx.bingoseller.com/pay/OrderNotify.aspx";
                        SybWxPayService sybService = new SybWxPayService(companyID);
                        Util.Debuglog("111=", "微信支付.txt");
                        Dictionary<String, String> rsp = sybService.pay((int)fen, DateTime.Now.ToFileTime().ToString(), "W02", "会员购物", proname, openID, "", url, "");
                        Util.Debuglog("rsp="+ rsp, "微信支付.txt");
                        string json = (new JavaScriptSerializer()).Serialize(rsp);
                        Util.Debuglog("json=" + json, "微信支付.txt");
                        JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                        
                        if (jo["retcode"].ToString() == "SUCCESS")
                        {
                            string appid = jo["appid"].ToString();
                            string cusid = jo["cusid"].ToString();
                            string trxid = jo["trxid"].ToString();
                            string reqsn = jo["reqsn"].ToString();
                            this._trxid.Value = trxid;
                            //插入预订单信息
                            string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,acct,statu,reqsn,type,payType,trxamt)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','" + openID + "','0','" + reqsn + "',2,3," + fen + ")";
                            Util.Debuglog("insertSQL=" + insertSQL, "微信支付.txt");
                            //根据订单编号更新 订单表  在回调方法里添加订单明细记录
                            string update = "update asm_order set trxID='"+trxid+ "' where orderNO='"+ this._orderNO.Value+ "'";
                            Util.Debuglog("update11111=" + update, "_11.txt");
                            DbHelperSQL.ExecuteSql(insertSQL);
                            DbHelperSQL.ExecuteSql(update);
                        }
                        foreach (var item in rsp)
                        {
                            if (item.Key == "payinfo")
                            {
                                wxJsApiParam = item.Value;
                                Util.Debuglog("wxJsApiParam="+ wxJsApiParam,"微信下单支付_.txt");
                                
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Util.Debuglog("ex错误="+ex.Message, "微信支付.txt");
                        Response.Write("<span style='color:#FF0000;font-size:20px'>" + "金额有误，请重试" + "</span>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面加载出错，请重试" + "</span>");
                }

            }
        }
        public static string insertIntoOrderDetail(string psfs, string psStr, string zq)
        {
            string result = "";
            if (psfs == "1")//按天派送
            {
                if (psStr.IndexOf("每天配送") > -1)
                {
                    result = getDataTimeDay("0", zq);
                }
                else if (psStr.IndexOf("隔一天") > -1)
                {
                    result = getDataTimeDay("1", zq);
                }
                else if (psStr.IndexOf("隔两天") > -1)
                {
                    result = getDataTimeDay("2", zq);
                }
                else if (psStr.IndexOf("隔三天") > -1)
                {
                    result = getDataTimeDay("3", zq);
                }
            }
            else if (psfs == "2")
            {
                //自定义派送
                result = getDataTimeWeek(psStr, int.Parse(zq));
            }
            //创建订单
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="D"></param>
        /// <param name="zq"></param>
        /// <returns></returns>
        public static string getDataTimeDay(string D, string zq)
        {
            string result = "";
            string day = D;
            int m = 0, n = 30;
            if (day == "1")//隔天派送
            {
                m = n * 2 - 1;
            }
            else if (day == "2")//隔2天派送
            {
                m = n * 3 - 2;
            }
            else if (day == "3")//隔3天派送
            {
                m = n * 4 - 3;
            }
            else
            {
                m = n * 1;//每天派送
            }
            //获取应该配送的日期 应该循环m
            var N = 1;//自增变量
            while (N <= m)
            {
                var t = DateTime.Now.AddDays(N).ToString("yyyy-MM-dd");
                if (day == "1")
                {
                    N = N + 2;
                }
                else if (day == "2")
                {
                    N = N + 3;
                }
                else if (day == "3")
                {
                    N = N + 4;
                }
                else if (day == "0")
                {
                    N = N + 1;
                }

                result += t + ",";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
        public static string getDataTimeWeek(string psStr, int zq)
        {
            int count = 0;
            int n = zq;
            string result = "";
            for (int i = 1; i < 1000000; i++)
            {
                var time = getDate(i);
                var week = Week(time);
                if (psStr.IndexOf(week) > -1)
                {
                    result += time + ",";
                    count++;
                }
                if (count == n)
                {
                    break;
                }
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
        public static string getDate(int D)
        {
            return DateTime.Now.AddDays(D).ToString("yyyy-MM-dd");
        }
        public static string Week(string time)
        {
            string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            //string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];
            string week = weekdays[Convert.ToInt32(DateTime.Parse(time).DayOfWeek)];
            return week;
        }
    }
}