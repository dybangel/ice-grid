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
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using uniondemo.com.allinpay.syb;
using WxPayAPI;

namespace Consumer.main
{
    public partial class allpay : System.Web.UI.Page
    {
        public static string wxEditAddrParam { get; set; }
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        public string openID = "";
        public string companyID = "";
        public string money = "0";
        public string idArr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                idArr = Request.QueryString["idArr"].ToString();
                if (string.IsNullOrEmpty(idArr))
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "参数不全" + "</span>");
                    return;
                }
                this._id.Value = idArr;
                Util.Debuglog("_id=" + idArr , "联合支付.txt");
                //根据idArr循环订单列表
                string sql = "select sum(totalMoney) totalMoney from asm_order where id in (" + idArr + ") and fkzt=0";
                DataTable ds = DbHelperSQL.Query(sql).Tables[0];
                if (ds.Rows.Count > 0)
                {
                    money = ds.Rows[0]["totalMoney"].ToString();
                }

                JsApiPay jsApiPayGet = new JsApiPay(this);
                try
                {
                    if (OperUtil.getCooki("vshop_openID") != "0")
                    {
                        this._companyID.Value = Request.QueryString["companyID"].ToString();
                        companyID = this._companyID.Value;
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
                        Util.Debuglog("openID=" + openID + ":companyID=" + companyID + "fen=" + fen, "联合支付.txt");
                        string url = "https://wx.bingoseller.com/pay/OrderNotify.aspx";
                        SybWxPayService sybService = new SybWxPayService(companyID);

                        Dictionary<String, String> rsp = sybService.pay((int)fen, DateTime.Now.ToFileTime().ToString(), "W02", "会员购物", "订单合并付款", openID, "", url, "");
                        Util.Debuglog("rsp=" + rsp, "联合支付.txt");
                        string json = (new JavaScriptSerializer()).Serialize(rsp);
                        Util.Debuglog("json=" + json, "联合支付.txt");
                        JObject jo = (JObject)JsonConvert.DeserializeObject(json);

                        if (jo["retcode"].ToString() == "SUCCESS")
                        {
                            string appid = jo["appid"].ToString();
                            string cusid = jo["cusid"].ToString();
                            string trxid = jo["trxid"].ToString();
                            string reqsn = jo["reqsn"].ToString();
                            this._trxid.Value = trxid;
                            // //插入预订单信息
                            string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,acct,statu,reqsn,[type],payType,trxamt)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','" + openID + "','0','" + reqsn + "',2,3," + fen + ")";
                            Util.Debuglog("insertSQL=" + insertSQL, "联合支付.txt");
                            DbHelperSQL.ExecuteSql(insertSQL);
                        }
                        foreach (var item in rsp)
                        {
                            if (item.Key == "payinfo")
                            {
                                wxJsApiParam = item.Value;
                                Util.Debuglog("wxJsApiParam=" + wxJsApiParam, "联合支付.txt");

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Debuglog("ex错误=" + ex.Message, "微信支付.txt");
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
        public static object createOr(string id,string trxID)
        {
            string sql = "select * from asm_order where id in (" + id + ") and fkzt=0";
            DataTable ds = DbHelperSQL.Query(sql).Tables[0];
            if (ds.Rows.Count > 0)
            {
                if (ds.Rows[0]["yhfs"].ToString().Contains("赠送"))
                {
                    string num = ds.Rows[0]["yhfs"].ToString();
                    num = num.Replace("赠送", "").Replace("天", "");
                    string update = "update asm_order set fkzt=1,trxID='" + trxID + "',totalNum=totalNum+"+num+",syNum=syNum+"+num+" where id in(" + id + ")";
                    DbHelperSQL.ExecuteSql(update);
                }
                else {
                    string update = "update asm_order set fkzt=1,trxID='" + trxID + "' where id in(" + id + ")";
                    DbHelperSQL.ExecuteSql(update);
                }
                ds = DbHelperSQL.Query(sql).Tables[0];
                for (int j=0;j<ds.Rows.Count;j++)
                {
                    string[] selDate = insertIntoOrderDetail(ds.Rows[j]["psfs"].ToString(), ds.Rows[j]["psStr"].ToString(), ds.Rows[j]["totalNum"].ToString(), ds.Rows[j]["qsDate"].ToString()).Split(',');
                    if (selDate.Length > 0)
                    {
                        string sql14 = "select * from asm_orderDetail where id=0";
                        DataTable dtNew = DbHelperSQL.Query(sql14).Tables[0];
                        for (int i = 0; i < selDate.Length; i++)
                        {
                            int code = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                            //zt   1-已完成；2-已失效；3-已转售；4-待取货；5-待配送
                            DataRow dr = dtNew.NewRow();
                            dr["mechineID"] = ds.Rows[j]["mechineID"].ToString(); //通过索引赋值
                            dr["productID"] = ds.Rows[j]["productID"].ToString();
                            dr["createTime"] = delTime(selDate[i]);//
                            dr["code"] = code;//
                            dr["memberID"] = ds.Rows[j]["memberID"].ToString(); //通过索引赋值
                            dr["zt"] = "5";
                            dr["ldNO"] = "";//
                            dr["orderNO"] = ds.Rows[j]["orderNO"].ToString();//
                            dr["statu"] = "0"; //通过索引赋值
                            dr["sellPrice"] = 0.0;
                            dr["sellTime"] = "";
                            dr["bz"] = "";
                            dtNew.Rows.Add(dr);
                        }
                        DbHelperSQL.BatchInsertBySqlBulkCopy(dtNew, "[dbo].[asm_orderDetail]");
                    }
                }
            }
            return new { code = 0, msg = "订单支付完成" };
        }

        public static string insertIntoOrderDetail(string psfs, string psStr, string zq,string qsDate)
        {
            string result = "";
            if (psfs == "1")//按天派送
            {
                if (psStr.IndexOf("每天配送") > -1)
                {
                    result = getDataTimeDay("0", zq, qsDate);
                }
                else if (psStr.IndexOf("隔一天") > -1)
                {
                    result = getDataTimeDay("1", zq, qsDate);
                }
                else if (psStr.IndexOf("隔两天") > -1)
                {
                    result = getDataTimeDay("2", zq, qsDate);
                }
                else if (psStr.IndexOf("隔三天") > -1)
                {
                    result = getDataTimeDay("3", zq, qsDate);
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
        public static string getDataTimeDay(string D, string zq,string qsDate)
        {
            string result = "";
            string day = D;
            int m = 0, n = int.Parse(zq);
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
            var N = 0;//自增变量
            while (N < m)
            {

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
                //var t = DateTime.Now.AddDays(N).ToString("yyyy-MM-dd");
                var t = DateTime.Parse(qsDate).AddDays(N - 1).ToString("yyyy-MM-dd");
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
        //处理时间
        public static string delTime(string time)
        {
            string[] timeArr = time.Split('-');
            if (timeArr.Length == 3)
            {
                timeArr[1] = timeArr[1].PadLeft(2, '0');
                timeArr[2] = timeArr[2].PadLeft(2, '0');
                return timeArr[0] + "-" + timeArr[1] + "-" + timeArr[2];
            }
            else
            {
                return time;
            }

        }
    }
}