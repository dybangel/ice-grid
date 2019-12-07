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
    public partial class dhpay : System.Web.UI.Page
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

                this._orderNO.Value = Request.QueryString["orderNO"].ToString();
                this._syMoney.Value = Request.QueryString["syMoney"].ToString();
                this._need_money.Value= Request.QueryString["need_money"].ToString();//实际花费
                this._zq.Value = Request.QueryString["zq"].ToString();
                this._productID.Value = Request.QueryString["productID"].ToString();
                this._yhfs.Value = Request.QueryString["yhfs"].ToString();
                this._mechineID.Value = Request.QueryString["mechineID"].ToString();
                string sql = "select * from asm_mechine where id=" + this._mechineID.Value;
                
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                companyID = dt.Rows[0]["companyID"].ToString();
                money = this._need_money.Value;
                string sqlpro = "select * from asm_product where productID="+this._productID.Value;
                proname = DbHelperSQL.Query(sqlpro).Tables[0].Rows[0]["proName"].ToString();
                
                JsApiPay jsApiPayGet = new JsApiPay(this);
                try
                {
                    if (OperUtil.getCooki("vshop_openID") != "0")
                    {
                        this._companyID.Value = OperUtil.getCooki("companyID");
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
                       
                        fen = (double.Parse(money)-double.Parse(this._syMoney.Value)) * 100;
                        //获取收货地址js函数入口参数
                        this._openID.Value = openID;
                        string url = "https://wx.bingoseller.com/pay/dhpayNotify.aspx";
                        SybWxPayService sybService = new SybWxPayService(companyID);
                        Dictionary<String, String> rsp = sybService.pay((int)fen, DateTime.Now.ToFileTime().ToString(), "W02", "会员商品兑换", proname, openID, "", url, "");
                        string json = (new JavaScriptSerializer()).Serialize(rsp);
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
                            DbHelperSQL.ExecuteSql(insertSQL);
                        }
                        foreach (var item in rsp)
                        {
                            if (item.Key == "payinfo")
                            {
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
        [WebMethod]
        public static string dh(string orderNO, string syMoney, string need_money, string zq, string productID, string yhfs)
        {
            Util.Debuglog("orderNO=" + orderNO+ ";syMoney="+ syMoney+ ";need_money="+ Math.Abs(double.Parse(need_money)) +";zq="+zq+ ";productID="+productID+ ";yhfs="+ yhfs, "_商品兑换.txt");
            //1需要先把旧的的订单的状态更改为已兑换
            //2按照旧的订单的配送方式重新生成新的订单 2 天之后配送
            //3钱款多退少补
            //先判断当前状态不是完成的才可以兑换
            string sql1 = "select * from asm_order where orderNO='" + orderNO + "' and zt in (0,1)";
            Util.Debuglog("sql1=" + sql1, "_商品兑换.txt");
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                return "2";
            }
            string sql22 = "update asm_order set zt=4 where orderNO='" + orderNO + "'";
            Util.Debuglog("sql22=" + sql22, "_商品兑换.txt");
            DbHelperSQL.ExecuteSql(sql22);
            string sql = "update asm_orderDetail set zt=7 where orderNO='" + orderNO + "' and zt=5";
            Util.Debuglog("sql=" + sql, "_商品兑换.txt");
            DbHelperSQL.ExecuteSql(sql);
            if (dt.Rows.Count > 0)
            {
                string sellDate = insertIntoOrderDetail(dt.Rows[0]["psfs"].ToString(), dt.Rows[0]["psStr"].ToString(), zq);
                Util.Debuglog("sellDate=" + sellDate, "_商品兑换.txt");
                string[] sellArr = sellDate.Split(',');
                string order_NO = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
                string fkzt = "1";
                //创建订单
                string sqlInsert = @"INSERT INTO [dbo].[asm_order](
                                                       [mechineID],
                                                       [productID],
                                                       [memberID],
                                                       [totalNum],
                                                       [consumeNum],
                                                       [syNum],
                                                       [createTime],
                                                       [zq],
                                                       [qsDate],
                                                       [zdDate],
                                                       [psStr],
                                                       [psfs],
                                                       [orderNO],
                                                       [fkzt],
                                                       [zt],
                                                       [qhAddress],
                                                       [totalMoney],
                                                       [yhfs])
                        VALUES('" + dt.Rows[0]["mechineID"].ToString() + "','" + productID + "','" + dt.Rows[0]["memberID"].ToString() + "','" + zq + "',0,'" + zq + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + zq + "','" + DateTime.Now.AddDays(2).ToString("yyyy-MM-dd") + "','" + sellArr[sellArr.Length - 1] + "','" + dt.Rows[0]["psStr"].ToString() + "','" + dt.Rows[0]["psfs"].ToString() + "','" + order_NO + "','" + fkzt + "',0,'" + dt.Rows[0]["qhAddress"].ToString() + "','" + Math.Abs(double.Parse(need_money)) + "','" + yhfs + "')";
                Util.Debuglog("sqlInsert=" + sqlInsert, "_商品兑换.txt");
                int a = DbHelperSQL.ExecuteSql(sqlInsert);
                if (a > 0)
                {
                    //更新商品销售数量
                    string ss = "update asm_product set ljxs=CONVERT(float,ISNULL(ljxs,0))+1 where productID=" + productID;
                    Util.Debuglog("ss=" + ss, "_商品兑换.txt");
                    DbHelperSQL.ExecuteSql(ss);
                    string[] selDate = sellArr;
                    if (selDate.Length > 0)
                    {
                        for (int i = 0; i < selDate.Length; i++)
                        {
                            int code = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                            //zt   1-已完成；2-已失效；3-已转售；4-待取货；5-待配送
                            string sql2 = @"INSERT INTO [dbo].[asm_orderDetail](
                                                    [mechineID],
                                                    [productID],
                                                    [createTime],
                                                    [code],
                                                    [memberID],
                                                    [zt],
                                                    [ldNO],
                                                    [orderNO],
                                                    [statu],
                                                    [sellPrice])
                                VALUES('" + dt.Rows[0]["mechineID"].ToString() + "','" + productID + "','" + selDate[i] + "','" + code + "'," + dt.Rows[0]["memberID"].ToString() + ",5,'','" + order_NO + "',0,0)";
                            DbHelperSQL.ExecuteSql(sql2);
                        }
                    }
                    //给会员绑定机器
                    string sql4 = "update asm_member set mechineID=" + dt.Rows[0]["mechineID"].ToString() + ",LastTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',consumeCount=consumeCount+1,sumConsume=sumConsume+" + need_money + " where id=" + dt.Rows[0]["memberID"].ToString();
                    Util.Debuglog("sql4=" + sql4, "_商品兑换.txt");
                    DbHelperSQL.ExecuteSql(sql4);
                    string sql5 = "select * from asm_product where productID=" + productID;
                    Util.Debuglog("sql5=" + sql5, "_商品兑换.txt");
                    DataTable dd5 = DbHelperSQL.Query(sql5).Tables[0];
                    string sql6 = "select * from asm_mechine where id=" + dt.Rows[0]["mechineID"].ToString();
                    Util.Debuglog("sql6=" + sql6, "_商品兑换.txt");
                    DataTable dd6 = DbHelperSQL.Query(sql6).Tables[0];
                    wxHelper wx = new wxHelper(OperUtil.getCooki("companyID"));
                    string data = TemplateMessage.comsume(OperUtil.getCooki("vshop_openID"), "ti4Dkcm1ELNqaskSYsCYMzqL87nPqapNeOgwhvSci_Q", "亲，你的购买的商品信息如下", "" + dd5.Rows[0]["proName"].ToString() + "", need_money, order_NO, dd6.Rows[0]["bh"].ToString(), "欢迎惠顾");
                    Util.Debuglog("companyID=" + OperUtil.getCooki("companyID")+ ";vshop_openID="+ OperUtil.getCooki("vshop_openID"), "_商品兑换.txt");
                    TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(OperUtil.getCooki("companyID")), data);
                    //多退少补
                    if (double.Parse(need_money) < double.Parse(syMoney))
                    {
                        //退给会员钱包
                        string sqlUpdate = "update asm_member set AvailableMoney=AvailableMoney+" + (double.Parse(syMoney)-double.Parse(need_money)) + " where id=" + dt.Rows[0]["memberID"].ToString();
                        Util.Debuglog("sqlUpdate=" + sqlUpdate, "_商品兑换.txt");
                        DbHelperSQL.ExecuteSql(sqlUpdate);
                        string sqlMM = "select * from asm_member where id="+ dt.Rows[0]["memberID"].ToString();
                        DataTable dmm = DbHelperSQL.Query(sqlMM).Tables[0];
                        Util.moneyChange(dmm.Rows[0]["id"].ToString(), (double.Parse(syMoney) - double.Parse(need_money)).ToString(), dmm.Rows[0]["AvailableMoney"].ToString(), "会员订单兑换", "6", "");
                        Util.insertNotice(dmm.Rows[0]["id"].ToString(), "会员订单兑换通知", "订单号为:"+orderNO+"已兑换成功","");
                    }
                }
            }
            return "1";
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
            int m = 0, n = int.Parse(zq);
            var date = DateTime.Now.AddDays(1);
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
                var t = date.AddDays(N).ToString("yyyy-MM-dd");
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