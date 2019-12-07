using autosell_center.cls;
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

namespace autosell_center.main.member
{
    public partial class xfmx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.memberID.Value = Request.QueryString["memberID"].ToString();
        }
        [WebMethod]
        public static string getDataList(string memberID,string pageCurrentCount)
        {

           
            string sql1 = "select A.*,B.bh,(select payType from asm_pay_info api where api.trxid=A.billno) payType1,case A.type when '1' then '订购' when '2' then '零售' else '其他' end stu from (select asd.*,ap.proname from asm_sellDetail asd left join asm_product ap on asd.productID=ap.productID) A left join asm_mechine B on A.mechineID=B.id where 1=1 and  memberID="+memberID;

            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql1, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {

                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@" + OperUtil.DataTableToJsonWithJsonNet(dt);


                return ss;
            }
            else
            {
                return "1";
            }

        }
        [WebMethod]
        public static Object ok(string billno, string pwd, string companyID)
        {
            //首先验证二级退款密码是否正确
            string sql12 = "select * from asm_company where id=" + companyID;
            DataTable dd1 = DbHelperSQL.Query(sql12).Tables[0];
            if (dd1.Rows.Count > 0)
            {
                if (pwd == dd1.Rows[0]["pwd2"].ToString())
                {
                    //查询该笔订单信息
                    //string sql1 = "select * from asm_pay_info where trxid='" + billno + "'";
                    //DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                    //if (dt1.Rows.Count > 0)
                    //{
                        //查询订单信息
                        string sqlOrder = "select * from  asm_sellDetail where billno='" + billno + "'";
                        DataTable Dorder = DbHelperSQL.Query(sqlOrder).Tables[0];
                        if (Dorder.Rows.Count <= 0)
                        {
                            return new { result = 0, msg = "该笔订单查询失败" };
                        }
                        //判断出货状态如果是料道错误或者是交易序列号相同给退款 零售的怎么来怎么退 订购的退到钱包
                        if (!string.IsNullOrEmpty(Dorder.Rows[0]["code"].ToString()))
                        {
                            //订购的
                            if (Dorder.Rows[0]["bz"].ToString() == "料道错误" || Dorder.Rows[0]["bz"].ToString() == "交易序列号相同" || Dorder.Rows[0]["bz"].ToString() == "料道故障" || Dorder.Rows[0]["bz"].ToString() == "校验错误" || Dorder.Rows[0]["bz"].ToString() == "出货失败")
                            {
                                string sql = "select * from asm_sellDetail where billno='" + billno + "'";
                                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                                if (dd.Rows.Count > 0)
                                {
                                    //更新会员钱包 并插入资金变动记录
                                    string update = "update asm_member set  AvailableMoney=AvailableMoney+" + dd.Rows[0]["totalMoney"].ToString() + " where id=" + dd.Rows[0]["memberID"].ToString();
                                    int a = DbHelperSQL.ExecuteSql(update);
                                    string sqlm = "select * from asm_member where id=" + dd.Rows[0]["memberID"].ToString();
                                    DataTable dt = DbHelperSQL.Query(sqlm).Tables[0];
                                    if (a > 0)
                                    {
                                        string sqlu = "update asm_sellDetail set bz='退款成功' where billno='" + billno + "'";
                                        DbHelperSQL.ExecuteSql(sqlu);
                                        Util.insertNotice(dt.Rows[0]["id"].ToString(), "出货异常退款", "您于" + Dorder.Rows[0]["orderTime"].ToString() + "取货异常退还金额:" + dd.Rows[0]["totalMoney"].ToString() + ";请查收钱包");
                                        Util.moneyChange(dt.Rows[0]["id"].ToString(), dd.Rows[0]["totalMoney"].ToString(), dt.Rows[0]["AvailableMoney"].ToString(), "退款通知", "7", "", "取货异常退还金额:" + dd.Rows[0]["totalMoney"].ToString());
                                        return new { result = 0, msg = "退款成功，成功到会员钱包中" };
                                    }
                                }
                            }
                        }
                        else
                        {
                            //零售的
                            if (Dorder.Rows[0]["bz"].ToString() == "料道错误" || Dorder.Rows[0]["bz"].ToString() == "交易序列号相同" || Dorder.Rows[0]["bz"].ToString() == "料道故障" || Dorder.Rows[0]["bz"].ToString() == "校验错误" || Dorder.Rows[0]["bz"].ToString() == "出货失败")
                            {
                                if (Dorder.Rows[0]["payType"].ToString() == "3")
                                {
                                    //退到钱包
                                    string sql = "select * from asm_sellDetail where billno='" + billno + "'";
                                    DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                                    if (dd.Rows.Count > 0)
                                    {
                                        //更新会员钱包 并插入资金变动记录
                                        string update = "update asm_member set  AvailableMoney=AvailableMoney+" + dd.Rows[0]["totalMoney"].ToString() + " where id=" + dd.Rows[0]["memberID"].ToString();
                                        int a = DbHelperSQL.ExecuteSql(update);
                                        string sqlm = "select * from asm_member where id=" + dd.Rows[0]["memberID"].ToString();
                                        DataTable dt = DbHelperSQL.Query(sqlm).Tables[0];
                                        if (a > 0)
                                        {
                                            string update1 = "update asm_pay_info set statu=2,fintime='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' where trxid='" + billno + "'";
                                            DbHelperSQL.ExecuteSql(update1);
                                            string sqlu = "update asm_sellDetail set bz='退款成功' where billno='" + billno + "'";
                                            DbHelperSQL.ExecuteSql(sqlu);
                                            Util.insertNotice(dt.Rows[0]["id"].ToString(), "出货异常退款", "您于" + Dorder.Rows[0]["orderTime"].ToString() + "购买商品出货异常退还金额:" + dd.Rows[0]["totalMoney"].ToString() + ";请查收钱包");
                                            Util.moneyChange(dt.Rows[0]["id"].ToString(), dd.Rows[0]["totalMoney"].ToString(), dt.Rows[0]["AvailableMoney"].ToString(), "退款通知", "7", "", "取货异常退还金额:" + dd.Rows[0]["totalMoney"].ToString());
                                            return new { result = 0, msg = "退款成功，成功到会员钱包中" };
                                        }
                                    }
                                }
                                else if (Dorder.Rows[0]["payType"].ToString() == "1" || Dorder.Rows[0]["payType"].ToString() == "2")
                                {
                                    //退到1微信或者2支付宝
                                    string sql = "select * from asm_pay_info where trxid='" + billno + "'";
                                    DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                                    if (dd.Rows.Count > 0 && dd.Rows[0]["statu"].ToString() == "1")
                                    {
                                        SybWxPayService sybService = new SybWxPayService(Dorder.Rows[0]["mechineID"].ToString());
                                        long fen = long.Parse(dd.Rows[0]["trxamt"].ToString());
                                        Random rd = new Random();
                                        int rand = rd.Next(10000, 99999);
                                        string reqsn = ConvertDateTimeToInt(DateTime.Now).ToString() + rand;
                                        string oldtrxid = dd.Rows[0]["trxid"].ToString();
                                        string oldreqsn = dd.Rows[0]["reqsn"].ToString();
                                        Dictionary<String, String> rsp = sybService.cancel(fen, reqsn, oldtrxid, oldreqsn);
                                        string data = OperUtil.SerializeDictionaryToJsonString(rsp);
                                        Util.Debuglog("微信支付链接data=" + data, "_退款.txt");
                                        //插入预处理订单信息
                                        string json = (new JavaScriptSerializer()).Serialize(rsp);
                                        Util.Debuglog("微信支付链接json=" + json, "_退款.txt");
                                        JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                                        if (jo["retcode"].ToString() == "SUCCESS")
                                        {
                                            string cusid = jo["cusid"].ToString();//商户号
                                            string trxid = jo["trxid"].ToString();//交易单号
                                            string fintime = jo["fintime"].ToString();//交易完成时间
                                            string reqsn1 = jo["reqsn"].ToString();//商户订单号
                                            string trxstatus = jo["trxstatus"].ToString();//交易状态
                                            //更新asm_pay_info
                                            if (trxstatus == "0000")
                                            {
                                                string update = "update asm_pay_info set statu=2,tkreqsn='" + reqsn1 + "',fintime='" + fintime + "',trxstatus='" + trxstatus + "',errmsg='' where trxid='" + oldtrxid + "'";
                                                DbHelperSQL.ExecuteSql(update);
                                                string sqlu = "update asm_sellDetail set bz='退款成功' where billno='" + billno + "'";
                                                DbHelperSQL.ExecuteSql(sqlu);
                                                return new { result = 0, msg = "退款成功，已经退到会员的微信或支付宝" };
                                            }
                                            else
                                            {
                                                string errmsg = jo["errmsg"].ToString();//交易失败信息
                                                string update = "update asm_pay_info set tkreqsn='" + reqsn1 + "',fintime='" + fintime + "',trxstatus='" + trxstatus + "',errmsg='" + errmsg + "' where trxid='" + oldtrxid + "'";
                                                DbHelperSQL.ExecuteSql(update);
                                                if (trxstatus == "3008")
                                                {
                                                    return new { result = 0, msg = errmsg };
                                                }
                                                else
                                                {
                                                    return new { result = 0, msg = "退款失败" };
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return new { result = 0, msg = "退款失败" };
                                        }
                                    }
                                }
                            }
                        }
                    //}
                   // else
                    //{
                      //  return new { result = 0, msg = "该笔订单查询失败" };
                   // }
                }
                else
                {
                    return new { result = 0, msg = "退款密码错误" };//密码错误
                }
            }
            return new { result = 0, msg = "退款失败" };
        }
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
    }
}