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

namespace Consumer.pay
{
    public partial class OrderNotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<String, String> reqParams = new Dictionary<String, String>();
            /**
             * 此处注意,因为通联收银宝以后可能增加字段,所以,这里一定要动态遍历获取所有的请求参数
             * 
             * */
            for (int i = 0; i < Request.Form.Count; i++)
            {
                reqParams.Add(Request.Form.Keys[i], Request.Form[i].ToString());


            }
            if (!reqParams.ContainsKey("sign"))//如果不包含sign,则不进行处理
            {

                Response.Write("error");
                return;
            }
            if (reqParams.ContainsKey("trxid"))
            {
                string json = (new JavaScriptSerializer()).Serialize(reqParams);
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                string sql = "select * from asm_pay_info where trxid='" + jo["trxid"].ToString() + "'";

                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    //获取openid查询这个openid的companyID 的appkey
                    string sql1 = "select * from asm_member where openID='" + dt.Rows[0]["acct"].ToString() + "'";

                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    string sql2 = "select * from asm_company where id=" + d1.Rows[0]["companyID"].ToString();

                    DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                    //AppUtil.validSign(reqParams, d2.Rows[0]["tl_APPKEY"].ToString(), d1.Rows[0]["companyID"].ToString())
                    if (1 == 1 && dt.Rows[0]["statu"].ToString() == "0")//验签成功
                    {
                        //验签成功后,进行业务处理,处理完毕返回成功
                        string trxdate = Request.Form["trxdate"];
                        string paytime = Request.Form["paytime"];
                        string acct = Request.Form["acct"];
                        string chnltrxid = Request.Form["chnltrxid"];
                        double trxamtY = double.Parse(Request.Form["trxamt"]) / 100;
                        //支付成功向asm_pay 表 更新记录
                        string updateSQL = "update asm_pay_info set paytime='" + paytime + "',statu='1',trxdate='" + trxdate + "',chnltrxid='" + chnltrxid + "' where trxid='" + jo["trxid"].ToString() + "'";

                        DbHelperSQL.ExecuteSql(updateSQL);
                        //需要更新会员的消费信息  
                        string update = "update asm_member set sumConsume=sumConsume+" + trxamtY + ",LastTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where openID='" + acct + "'";
                        DbHelperSQL.ExecuteSql(update);
                        // 插入订单明细 根据trxid查询

                        string sqlO = "select * from  asm_order where trxID='" + jo["trxid"].ToString() + "'";
                        Util.Debuglog("sqlO="+ sqlO,"_11.txt");
                        DataTable d12 = DbHelperSQL.Query(sqlO).Tables[0];
                        if (d12.Rows.Count <= 0)
                        {

                        }
                        else
                        {
                            //判断当前的优惠方式如果是赠送天数的话需要更新totalNum
                            if (d12.Rows[0]["yhfs"].ToString().Contains("赠送"))
                            {
                                string num = d12.Rows[0]["yhfs"].ToString();
                                num = num.Replace("赠送", "").Replace("天", "");
                                string update1 = "update asm_order set fkzt=1,trxID='" + jo["trxid"].ToString() + "',totalNum=totalNum+" + num + ",syNum=syNum+" + num + " where id=" + d12.Rows[0]["id"].ToString();
                                DbHelperSQL.ExecuteSql(update1);
                            }
                            else
                            {
                                string update1 = "update asm_order set fkzt=1,trxID='" + jo["trxid"].ToString() + "' where id=" + d12.Rows[0]["id"].ToString();
                                DbHelperSQL.ExecuteSql(update1);
                            }
                            //更新状态
                        }
                        if (1 == 1)
                        {
                            d12 = DbHelperSQL.Query(sqlO).Tables[0];
                            //更新商品销售数量
                            string ss = "update asm_product set ljxs=CONVERT(float,ISNULL(ljxs,0))+1 where productID=" + d12.Rows[0]["productID"].ToString();
                            DbHelperSQL.ExecuteSql(ss);
                            //string[] selDate = _selDate.Split(',');

                            string[] selDate = insertIntoOrderDetail(d12.Rows[0]["psfs"].ToString(), d12.Rows[0]["psStr"].ToString(), d12.Rows[0]["totalNum"].ToString(), d12.Rows[0]["qsDate"].ToString()).Split(',');

                            Util.Debuglog(insertIntoOrderDetail(d12.Rows[0]["psfs"].ToString(), d12.Rows[0]["psStr"].ToString(), d12.Rows[0]["totalNum"].ToString(), d12.Rows[0]["qsDate"].ToString()), "时间格式.txt");
                            if (selDate.Length > 0)
                            {
                                string sql14 = "select * from asm_orderDetail where id=0";
                                DataTable dtNew = DbHelperSQL.Query(sql14).Tables[0];
                                for (int i = 0; i < selDate.Length; i++)
                                {
                                    int code = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                                    
                                    //zt   1-已完成；2-已失效；3-已转售；4-待取货；5-待配送
                                    DataRow dr = dtNew.NewRow();
                                    dr["mechineID"] = d12.Rows[0]["mechineID"].ToString(); //通过索引赋值
                                    dr["productID"] = d12.Rows[0]["productID"].ToString();
                                    dr["createTime"] = delTime(selDate[i]);//
                                    dr["code"] = code;//
                                    dr["memberID"] = d12.Rows[0]["memberID"].ToString(); //通过索引赋值
                                    dr["zt"] = "5";
                                    dr["ldNO"] = "";//
                                    dr["orderNO"] = d12.Rows[0]["orderNO"].ToString();//
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
                    else
                    {
                        Response.Write("error");
                        return;
                    }
                }
                else
                {
                    Response.Write("error");
                    return;
                }
            }
        }
        //处理时间
        public string delTime(string time)
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
        public static string insertIntoOrderDetail(string psfs, string psStr, string zq, string qsDate)
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
        public static string getDataTimeDay(string D, string zq, string qsDate)
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

    }

}