using autosell_center.util;
using DBUtility;
using Maticsoft.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenPlatForm.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using uniondemo.com.allinpay.syb;

namespace Consumer.cls
{
    public static class Util
    {
        public static void Debuglog(string log, string logname = "_Debuglog.txt")
        {
            try
            {
                StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "log/" + (DateTime.Now.ToString("yyyyMMdd") + logname));
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
                writer.WriteLine("---------------");
                writer.Close();
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception)
            {

            }
        }
        public static string ToJSON(this object o)
        {
            if (o == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(o);
        }
        public static string getMemberID()
        {
           
            string openID = OperUtil.getCooki("vshop_openID").ToString();
            string sql = "select * from asm_member where openID='" + openID + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
           
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["id"].ToString();
            }
            else
            {
                return "0";
            }

        }
        /// <summary>
        /// 取时间戳，高并发情况下会有重复。想要解决这问题请使用sleep线程睡眠1毫秒。
        /// </summary>
        /// <param name="AccurateToMilliseconds">精确到毫秒</param>
        /// <returns>返回一个长整数时间戳</returns>
        public static long GetTimeStamp(bool AccurateToMilliseconds = false)
        {
            if (AccurateToMilliseconds)
            {

                // 使用当前时间计时周期数（636662920472315179）减去1970年01月01日计时周期数（621355968000000000）除去（删掉）后面4位计数（后四位计时单位小于毫秒，快到不要不要）再取整（去小数点）。

                //备注：DateTime.Now.ToUniversalTime不能缩写成DateTime.Now.Ticks，会有好几个小时的误差。

                //621355968000000000计算方法 long ticks = (new DateTime(1970, 1, 1, 8, 0, 0)).ToUniversalTime().Ticks;

                return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;

            }
            else
            {

                //上面是精确到毫秒，需要在最后除去（10000），这里只精确到秒，只要在10000后面加三个0即可（1秒等于1000毫米）。
                return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            }
        }
        //存入缓存
        public static void SetSession(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 120;
        }
        //读取缓存
        public static string GetSession(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            return HttpContext.Current.Session[strSessionName].ToString();
        }
        public static void insertRecord(string memberID,string money,string avaMoney, string bz)
        {
          
            string sql = "INSERT INTO [dbo].[asm_transfer]([memberID],[mone],[time],[avaMoney],[bz])VALUES('" + memberID+"',"+money+",'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+avaMoney+",'"+bz+"')";
            DbHelperSQL.ExecuteSql(sql);
          
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="money"></param>
        /// <param name="bz"></param>
        /// <param name="description"></param>
        /// <param name="payType">1 会员充值 2 会员消费 3会员转账收入 4转账支出 5 售卖</param>
        public static void moneyChange(string memberID, string money, string avaMoney, string bz,string type,string skID,string description)
        {
            //string sql1 = "insert into asm_moneyChange(payTime,money,AvaiilabMOney,memberID,type,bz,skID) values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + money + "," + avaMoney + "," + memberID + ","+type+",'"+bz+"','"+skID+"')";
            //DbHelperSQL.ExecuteSql(sql1);

            string sql = "insert into asm_chgMoney(memberID,payTime,type,money,bz,description) values('" + memberID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + type + "','" + money + "','" + bz + "','" + description + "')";
            DbHelperSQL.ExecuteSql(sql);

        }
        public static void insertNotice(string memberID,string title,string con)
        {
            string sql = "insert into asm_notice (memberID,title,con,time) values('"+memberID+"','"+title+"','"+con+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')";
            DbHelperSQL.ExecuteSql(sql);
          
        }
        public static bool CheckServeStatus(string urls)
        {
            if (!LocalConnectionStatus())
            {
                //MessageBox.Show("网络异常~无连接");
                return false;
            }
            else if (!MyPing(urls))
            {
                //MessageBox.Show("网络异常");
                return false;
            }
            else
            {
                return true;
            }
        }
        private const int INTERNET_CONNECTION_MODEM = 1;
        private const int INTERNET_CONNECTION_LAN = 2;

        [System.Runtime.InteropServices.DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);
        /// <summary>
        /// 判断本地的连接状态
        /// </summary>
        /// <returns></returns>
        private static bool LocalConnectionStatus()
        {
            System.Int32 dwFlag = new Int32();
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                Console.WriteLine("LocalConnectionStatus--未连网!");
                return false;
            }
            else
            {
                if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
                {
                    Console.WriteLine("LocalConnectionStatus--采用调制解调器上网。");
                    return true;
                }
                else if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
                {
                    Console.WriteLine("LocalConnectionStatus--采用网卡上网。");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Ping命令检测网络是否畅通
        /// </summary>
        /// <param name="urls">URL数据</param>
        /// <param name="errorCount">ping时连接失败个数</param>
        /// <returns></returns>
        public static bool MyPing(string urls)
        {
            bool isconn = true;
            Ping ping = new Ping();
            try
            {
                PingReply pr;

                pr = ping.Send(urls);
                if (pr.Status != IPStatus.Success)
                {
                    isconn = false;
                }
            }
            catch
            {
                isconn = false;
            }


            return isconn;
        }

        //根据过期时间获取最新的token
        public static string getComToken()
        {
           
            string sql = "select * from asm_platformInfo ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            DateTime YouXRQ;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    YouXRQ = Convert.ToDateTime(dt.Rows[0]["lastUpdateTime"].ToString());
                    if (DateTime.Now > YouXRQ)
                    {
                        DateTime _youxrq = DateTime.Now.AddHours(1);
                        //过期从新获取更新数据库
                        string comToken = GetToken(OpenPFConfig.Appid, OpenPFConfig.Appsecret, dt.Rows[0]["ticket"].ToString());
                        string update = "update asm_platformInfo set comToken='" + comToken + "',lastUpdateTime='" + _youxrq + "'";
                        DbHelperSQL.ExecuteSql(update);
                        return comToken;
                    }
                    else
                    {
                        if (dt.Rows[0]["comToken"].ToString() == "")
                        {
                            DateTime _youxrq = DateTime.Now.AddHours(1);
                            string comToken = GetToken(OpenPFConfig.Appid, OpenPFConfig.Appsecret, dt.Rows[0]["ticket"].ToString());
                            string update = "update asm_platformInfo set comToken='" + comToken + "',lastUpdateTime='" + _youxrq + "'";
                            DbHelperSQL.ExecuteSql(update);
                            return comToken;
                        }
                        else {
                            return dt.Rows[0]["comToken"].ToString();
                        }
                        //返回数据中的
                      
                    }
                }
                catch
                {
                    //从新获取并更新数据库
                    DateTime _youxrq = DateTime.Now.AddHours(1);
                    string comToken = GetToken(OpenPFConfig.Appid, OpenPFConfig.Appsecret, dt.Rows[0]["ticket"].ToString());
                    string update = "update asm_platformInfo set comToken='" + comToken + "',lastUpdateTime='" + _youxrq + "'";
                    DbHelperSQL.ExecuteSql(update);
                    return comToken;
                }

            }
            return null;
        }
        public static string GetToken(string appid, string secret, string ticket)
        {
            var obj = new
            {
                component_appid = appid,
                component_appsecret = secret,
                component_verify_ticket = ticket
            };
            string responseStr =
                WebService.PostFunction("https://api.weixin.qq.com/cgi-bin/component/api_component_token", obj);

            CommonMethod.Token tokenModel = CommonMethod.JsonHelper.ParseFromJson<CommonMethod.Token>(responseStr);
            if (tokenModel != null && !string.IsNullOrEmpty(tokenModel.component_access_token))
            {
                return tokenModel.component_access_token;
            }
            else
            {
                return "";
            }
        }
        //post方法获取authorizer_access_token
        public static CommonMethod.RefreshToken GetTokenInfo(string appid, string authAppid, string componentToken, string refresh_token)
        {
            var obj = new
            {
                component_appid = appid,
                authorizer_appid = authAppid,
                authorizer_refresh_token = refresh_token
            };
            string responseStr = OpenPlatForm.Common.WebService.PostFunction("https://api.weixin.qq.com/cgi-bin/component/api_authorizer_token?component_access_token=" + componentToken, obj);
            CommonMethod.RefreshToken authInfo = CommonMethod.JsonHelper.ParseFromJson<CommonMethod.RefreshToken>(responseStr);
            if (authInfo != null)
            {
                return authInfo;
            }
            else
            {
                return new CommonMethod.RefreshToken();
            }
        }

        public  static async System.Threading.Tasks.Task  ch(string ldNO, string mechineID)
        {
            try
            {
                string url = "http://114.116.16.200/api/api.ashx";
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["action"] = "ch";
                    values["ldNO"] = ldNO;
                    values["mechineID"] = mechineID;//支付宝订单号
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.Default.GetString(response);
                    log(responseString, "商户回调结果.txt");
                }
            }
            catch (Exception e)
            {
                log(e.ToString(), "通知商户错误.txt");
            }
        }
        public static async System.Threading.Tasks.Task addMechineToList( string mechineID)
        {
            try
            {
                string url = "http://alisocket.bingoseller.com/api/api.ashx";
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["action"] = "addMechineToList";
                   
                    values["mechineID"] = mechineID;//支付宝订单号
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.Default.GetString(response);
                    log(responseString, "商户回调结果.txt");
                }
            }
            catch (Exception e)
            {
                log(e.ToString(), "通知商户错误.txt");
            }
        }
        public static void log(string log, string logname = "_Debuglog.txt")
        {
            if (Directory.Exists(HttpRuntime.AppDomainAppPath.ToString() + "log/" + DateTime.Now.ToString("yyyyMMdd")) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(HttpRuntime.AppDomainAppPath.ToString() + "log/" + DateTime.Now.ToString("yyyyMMdd"));
            }
            try
            {
                StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "log/" + DateTime.Now.ToString("yyyyMMdd") + "/" + (DateTime.Now.ToString("HH") + logname));
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
                writer.WriteLine("---------------");
                writer.Close();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 更新会员等级
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="mechineID"></param>
        public static void updateMemberDJ(string memberID,string mechineID)
        {
            try
            {
                string sqlday = "select * from View_member_consumeCount30 where id=" + memberID;
                DataTable dt1 = DbHelperSQL.Query(sqlday).Tables[0];
                string sql = "select * from asm_dj where companyID in (select companyID from asm_mechine where id="+mechineID+") and consumeDay<="+dt1.Rows[0]["num"].ToString()+" order by djID desc";
                DataTable dt2 = DbHelperSQL.Query(sql).Tables[0];
                if (dt2.Rows.Count>0)
                {

                    string update = "update asm_member set dj="+dt2.Rows[0]["djId"].ToString()+" where phone!='' and phone is not null and id="+memberID+ " and hjhyDays=0";
                    DbHelperSQL.ExecuteSql(update);
                    string update1 = "update asm_member set dj=0 where phone='' or phone is null";
                    DbHelperSQL.ExecuteSql(update1);
                }
            }
            catch {

            }
           
        }
        /// <summary>
        /// 获取配送日期
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="psMode">1 隔一天 2 隔两天 3 隔三天 4周一到周五 5 周末</param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public static string getSelDate(string totalDays, string psMode, string startTime)
        {
            string str = "";
            string zq = totalDays;
            if (psMode == "1")
            {
                str = getDataTimeDay(psMode, zq, startTime);
            }
            else if (psMode == "2")
            {
                str = getDataTimeDay(psMode, zq, startTime);
            }
            else if (psMode == "3")
            {
                str = getDataTimeDay(psMode, zq, startTime);
            }
            else if (psMode == "4")
            {
                str = getDataTimeWeek("周一|周二|周三|周四|周五", int.Parse(zq), startTime);
            }
            else if (psMode == "5")
            {
                str = getDataTimeWeek("周六|周日", int.Parse(zq), startTime);
            }
            return str;
        }
        public static string getDate(int D, string startTime)
        {
            return DateTime.Parse(startTime).AddDays(D).ToString("yyyy-MM-dd");

        }
        public static string Week(string time)
        {
            string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            //string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];
            string week = weekdays[Convert.ToInt32(DateTime.Parse(time).DayOfWeek)];
            return week;
        }
        public static string getDataTimeWeek(string psStr, int zq, string startTime)
        {
            int count = 0;
            int n = zq;
            string result = "";
            for (int i = 0; i < 1000000; i++)
            {
                var time = getDate(i, startTime);
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
        public static string getDataTimeDay(string D, string zq, string qsDate)
        {
            string result = "";
            string day = D;
            int m = 0, n = int.Parse(zq);
            if (day == "1")//
            {

                m = n * 1;//每天派送
            }
            else if (day == "2")//2天1送
            {
                m = n * 2 - 1;
            }
            else if (day == "3")//3天1送
            {
                m = n * 3 - 2;
            }

            //获取应该配送的日期 应该循环m
            var N = 0;//自增变量
            while (N < m)
            {
                var t = "";
                if (day == "1")
                {
                    // N = N + 2;
                    N = N + 1;
                    t = DateTime.Parse(qsDate).AddDays(N - 1).ToString("yyyy-MM-dd");
                }
                else if (day == "2")
                {
                    N = N + 2;
                    t = DateTime.Parse(qsDate).AddDays(N - 2).ToString("yyyy-MM-dd");
                }
                else if (day == "3")
                {
                    N = N + 3;
                    t = DateTime.Parse(qsDate).AddDays(N - 3).ToString("yyyy-MM-dd");
                }

                result += t + ",";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
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
        public static void SleepOrder(DataTable dtInfo)
        {
            try
            {
                string totalNum = dtInfo.Rows[0]["syNum"].ToString();
                string psMode = dtInfo.Rows[0]["psMode"].ToString();

                int daysdiff = 0;

                //查询此刻状态为4,5的数量,按照时间倒序，字段添加相差天数，第一条为最近的时间和现在的差距
                string sqlRelNum = "select  DATEDIFF(dd,GETDATE(), createtime ) daysdiff,*  from asm_orderlistDetail   where orderno = '" + dtInfo.Rows[0]["orderNO"].ToString() + "'  and zt in (4,5) order by createtime desc";
                Debuglog("sqlRelNum=" + sqlRelNum, "恢复订单.txt");
                DataTable relNum = DbHelperSQL.Query(sqlRelNum).Tables[0];
                
                if (relNum.Rows.Count > 0)
                {
                    totalNum = (int.Parse(totalNum) - relNum.Rows.Count).ToString();
                    daysdiff = int.Parse(relNum.Rows[0]["daysdiff"].ToString());

                }
                //正常流程下的开始时间
                int startSendCount = 0;
                string sqlGetStartSend = "select isnull(startSend,0) startSend from asm_product  where  productid=" + dtInfo.Rows[0]["productID"].ToString();
                Debuglog("sqlGetStartSend=" + sqlGetStartSend, "恢复订单.txt");
                DataTable sqlstartSendCount = DbHelperSQL.Query(sqlGetStartSend).Tables[0];
                //首送日期
                if (sqlstartSendCount.Rows.Count > 0)
                {
                    startSendCount = int.Parse(sqlstartSendCount.Rows[0]["startSend"].ToString());
                }
                //还未送完的如果大于等于首送时间，首送时间定为未送完的时间+1
                if (daysdiff >= startSendCount) {
                    startSendCount = daysdiff+1;
                } 
                //处理恢复时按照psmode来
                if (daysdiff >0&& psMode == "2"&& ((startSendCount- daysdiff)<2))
                {
                    startSendCount = startSendCount + 2- (startSendCount - daysdiff);
                }
                if (daysdiff > 0&&psMode == "3" && ((startSendCount - daysdiff) < 3))
                {
                    startSendCount = startSendCount + 3 - (startSendCount - daysdiff);
                }
                string startTime = DateTime.Now.AddDays(startSendCount).ToString("yyyy-MM-dd");

              
                Debuglog("totalNum=" + totalNum+ ";psMode=" + psMode + ";startTime=" + startTime + ";nowdate=" + DateTime.Now.ToString("yyyy-MM-dd") + ";orderid=" + dtInfo.Rows[0]["id"].ToString(), "恢复订单.txt");
                string[] selDate = getSelDate(totalNum, psMode, startTime).Split(',');

                
                if (selDate.Length > 0)
                {
                    string sql14 = "select * from asm_orderlistDetail where id=0";
                    DataTable dtNew = DbHelperSQL.Query(sql14).Tables[0];
               //     DataTable sss = new DataTable();
                   
                    for (int i = 0; i < selDate.Length; i++)
                    {
                        int code = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                        //zt   1-已完成；2-已失效；3-已转售；4-待取货；5-待配送
                        DataRow dr = dtNew.NewRow();
                        dr["mechineID"] = dtInfo.Rows[0]["mechineID"].ToString(); //通过索引赋值
                        dr["productID"] = dtInfo.Rows[0]["productID"].ToString();
                        dr["createTime"] = delTime(selDate[i]);//
                        dr["code"] = code;//
                        dr["memberID"] = dtInfo.Rows[0]["memberID"].ToString(); //通过索引赋值
                        dr["zt"] = "5";
                        dr["ldNO"] = "";//
                        dr["orderNO"] = dtInfo.Rows[0]["orderNO"].ToString();//
                        dr["statu"] = "0"; //通过索引赋值
                        dr["sellPrice"] = 0.0;
                        dr["sellTime"] = "";
                        dr["bz"] = "暂停后恢复";
                        dr["companyID"] = dtInfo.Rows[0]["companyID"].ToString();
                        dtNew.Rows.Add(dr);
                    }
                    DbHelperSQL.BatchInsertBySqlBulkCopy(dtNew, "[dbo].[asm_orderlistDetail]");
                    string endtime = delTime(selDate[selDate.Length - 1]);
                    string update = "update asm_orderlist set orderZT=1 ,endTime='" + endtime + "' where id=" + dtInfo.Rows[0]["id"].ToString();
                    int a = DbHelperSQL.ExecuteSql(update);
                }
            }
            catch (Exception e)
            {
                Debuglog("错误=" + e.ToString(), "订单暂停.txt");
                throw;
            }
           


        }

       
            
        public static void SleepOrder11111(DataTable dtInfo)
        {
            try
            {
                string totalNum = dtInfo.Rows[0]["syNum"].ToString();
                string psMode = dtInfo.Rows[0]["psMode"].ToString();
                //string update2 = "update asm_orderlistDetail set zt=8 where orderNO='" + dtInfo.Rows[0]["orderNO"].ToString() + "' and zt=5 ";
                //Debuglog("update2=" + update2, "恢复订单.txt");
                //int a2 = DbHelperSQL.ExecuteSql(update2);
                int startSendCount = 3;
                string sqlGetStartSend = "select isnull(startSend,0) startSend from asm_product  where  productid=" + dtInfo.Rows[0]["productID"].ToString();
                Debuglog("sqlGetStartSend=" + sqlGetStartSend, "恢复订单.txt");
                DataTable sqlstartSendCount = DbHelperSQL.Query(sqlGetStartSend).Tables[0];
                if (sqlstartSendCount.Rows.Count > 0)
                {
                    startSendCount = int.Parse(sqlstartSendCount.Rows[0]["startSend"].ToString()) > 3 ? int.Parse(sqlstartSendCount.Rows[0]["startSend"].ToString()) : startSendCount;
                }
                string startTime = DateTime.Now.AddDays(startSendCount).ToString("yyyy-MM-dd");
                string[] selDate = getSelDate(totalNum, psMode, startTime).Split(',');
                if (selDate.Length > 0)
                {
                    string sql14 = "select * from asm_orderlistDetail where id=0";
                    DataTable dtNew = DbHelperSQL.Query(sql14).Tables[0];
                    for (int i = 0; i < selDate.Length; i++)
                    {
                        int code = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                        //zt   1-已完成；2-已失效；3-已转售；4-待取货；5-待配送
                        DataRow dr = dtNew.NewRow();
                        dr["mechineID"] = dtInfo.Rows[0]["mechineID"].ToString(); //通过索引赋值
                        dr["productID"] = dtInfo.Rows[0]["productID"].ToString();
                        dr["createTime"] = delTime(selDate[i]);//
                        dr["code"] = code;//
                        dr["memberID"] = dtInfo.Rows[0]["memberID"].ToString(); //通过索引赋值
                        dr["zt"] = "5";
                        dr["ldNO"] = "";//
                        dr["orderNO"] = dtInfo.Rows[0]["orderNO"].ToString();//
                        dr["statu"] = "0"; //通过索引赋值
                        dr["sellPrice"] = 0.0;
                        dr["sellTime"] = "";
                        dr["bz"] = "暂停后恢复";
                        dr["companyID"] = dtInfo.Rows[0]["companyID"].ToString();
                        dtNew.Rows.Add(dr);
                    }
                    DbHelperSQL.BatchInsertBySqlBulkCopy(dtNew, "[dbo].[asm_orderlistDetail]");
                    string endtime = delTime(selDate[selDate.Length - 1]);
                    string update = "update asm_orderlist set orderZT=1 ,endTime='" + endtime + "' where id=" + dtInfo.Rows[0]["id"].ToString();
                    int a = DbHelperSQL.ExecuteSql(update);
                }
            }
            catch (Exception e)
            {
                Debuglog("错误=" + e.ToString(), "订单暂停.txt");
                throw;
            }



        }

        public static void orderDelay()
        {
           
            string sql1 = "select * from asm_orderlist where orderzt=6";
            Util.Debuglog("sql1=" + sql1, "订单暂停.txt");
            DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
            if (d1.Rows.Count > 0)
            {
                //需要在明细表里插入一条记录 并把当天的明细状态改为zt=8
                string sql2 = "update asm_orderlistDetail set zt=8,bz='订单延期' where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and orderNO='" + d1.Rows[0]["orderNO"].ToString() + "'";
                Util.Debuglog("sql2=" + sql2, "订单暂停.txt");
                DbHelperSQL.ExecuteSql(sql2);
                //取最后一条记录的最后一天在延期一天
                string sql4 = "select * from asm_orderlistdetail where orderNO='" + d1.Rows[0]["orderNO"].ToString() + "' order by id desc";
                Util.Debuglog("sql4=" + sql4, "订单暂停.txt");
                DataTable d4 = DbHelperSQL.Query(sql4).Tables[0];
                if (d4.Rows.Count > 0)
                {
                    Random rand = new Random();
                    int code = rand.Next(100000, 999999);
                    string insert = "insert into asm_orderlistdetail(mechineID,productID,memberID,createTime,code,zt,orderNO,sellPrice,statu,companyID) "
                    + "values(" + d4.Rows[0]["mechineID"].ToString() + "," + d4.Rows[0]["productID"].ToString() + "," + d4.Rows[0]["memberID"].ToString() + ",'" + (DateTime.Parse(d4.Rows[0]["createTime"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "','" + code + "',5,'" + d4.Rows[0]["orderNO"].ToString() + "',0,0," + d4.Rows[0]["companyID"].ToString() + ")";
                    Util.Debuglog("insert=" + insert, "订单暂停.txt");
                    DbHelperSQL.ExecuteSql(insert);
                    //更新派送最后一条时间
                    string updateO = "update asm_orderlist set endTime='" + (DateTime.Parse(d4.Rows[0]["createTime"].ToString()).AddDays(1)).ToString("yyyy-MM-dd") + "' where orderNO='" + d4.Rows[0]["orderNO"].ToString() + "'";
                    DbHelperSQL.ExecuteSql(updateO);
                }
            }
        }
        public static string HashtableToWxJson(Hashtable data)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                foreach (object key in data.Keys)
                {
                    object value = data[key];
                    sb.Append("\"");
                    sb.Append(key);
                    sb.Append("\":\"");
                    if (!String.IsNullOrEmpty(value.ToString()) && value != DBNull.Value)
                    {
                        sb.Append(value).Replace("\\", "/");
                    }
                    else
                    {
                        sb.Append("");
                    }
                    sb.Append("\",");
                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append("}");
                return sb.ToString();
            }
            catch (Exception ex)
            {

                return "";
            }
        }
       
        public static void ClearRedisProductInfo()
        {

            string sql = "select * from asm_mechine";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string mechineID = dt.Rows[i]["id"].ToString();
                    RedisHelper.SetRedisModel<string>(mechineID + "_productInfo", null, new TimeSpan(0, 0, 0));
                      
                }
            }
        }
        public static void ClearRedisProductInfoByMechineID(string mechineID)
        {
            RedisHelper.SetRedisModel<string>(mechineID + "_productInfo", "", new TimeSpan(0, 0, 0));
        }
        public static void ClearRedisMechineInfoByMechineID(string mechineID)
        {
            RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfo", "", new TimeSpan(0, 0, 0));
        }
        public static void ClearRedisVideoByMechineID(string mechineID)
        {
            RedisHelper.SetRedisModel<string>(mechineID + "_videoInfo", "", new TimeSpan(0, 0, 0));
        }
        public static void ClearRedisMemberprice(string company)
        {
            RedisHelper.SetRedisModel<string>(company + "_memberprice", "", new TimeSpan(0, 0, 0));
        }
        public static string getCompany(string companyID)
        {
            string result = RedisHelper.GetRedisModel<string>(companyID+"_companyInfo");
            if (string.IsNullOrEmpty(result))
            {
                string sql = "select * from asm_company where id="+companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                result = OperUtil.DataTableToJsonWithJsonNet(dt);
                RedisHelper.SetRedisModel<string>(companyID+ "_companyInfo",result,new TimeSpan(2,0,0));
            }
            return result;
        }
        public static JObject getProductInfo(string productID,string companyID)
        {
            string productList = RedisHelper.GetRedisModel<string>(companyID+ "_productList");
            if (string.IsNullOrEmpty(productList))
            {
                string sql = "select * from asm_product where companyID=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    productList = OperUtil.DataTableToJsonWithJsonNet(dt);
                    RedisHelper.SetRedisModel<string>(companyID + "_productList", productList, new TimeSpan(6, 0, 0));
                   
                }
            }
            JArray jsonArray = (JArray)JsonConvert.DeserializeObject(productList);
            if (jsonArray.Count>0)
            {
                foreach (var ss in jsonArray)
                {
                    if (ss["productID"].ToString()==productID)
                    {
                        return (JObject)ss;
                    }
                }
            }
            return null;
        }

        //更新零售
        public static string uploadSellDetail(string recordList)
        {
            try
            {
                log("Util开始recordList="+ recordList, "upSellRecord1.txt");
                List<asm_sellDetail> twoList = JsonConvert.DeserializeObject<List<asm_sellDetail>>(recordList);
                foreach (asm_sellDetail stu in twoList)
                {
                    //清空redis
                    Util.ClearRedisProductInfoByMechineID(stu.mechineID.ToString());
                    string sql12 = "select * from asm_sellDetail where billno='" + stu.billno + "'";
                    DataTable dd2 = DbHelperSQL.Query(sql12).Tables[0];
                    if (dd2.Rows.Count > 0)
                    {
                        return "1";
                    }
                    string type = "";
                    string payType = "0";
                    if (!string.IsNullOrEmpty(stu.code))
                    {
                        type = "1";//订购
                        if (stu.bz == "交易成功")
                        {
                            //更新code的码的状态
                            string sql1 = "update asm_orderDetail set zt=1 where code='" + stu.code + "' and memberID=" + stu.memberID;
                            DbHelperSQL.ExecuteSql(sql1);
                            //新的订单明细表
                            sql1 = "update asm_orderlistDetail set zt=1 where code='" + stu.code + "' and memberID=" + stu.memberID;
                            DbHelperSQL.ExecuteSql(sql1);
                        }
                    }
                    else
                    {
                        type = "2";//零售
                        payType = stu.payType.ToString();
                    }
                    if (string.IsNullOrEmpty(stu.billno))
                    {
                        //判断是否是会员购买
                        double money = 0;
                        string sqlp = "select * from asm_product where productID='" + stu.productID + "'";
                        DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                        if (stu.memberID != "0" && stu.memberID != "")
                        {
                            money = double.Parse(dp.Rows[0]["price2"].ToString());
                        }
                        else
                        {
                            money = double.Parse(stu.totalMoney + "");
                        }
                        string sql = "INSERT INTO [dbo].[asm_sellDetail](companyID,productname,[productID],[num] ,[totalMoney],[orderTime],[proLD],[type],[orderNO],[memberID],[code] ,[payType],[mechineID],[bz],[billno],oldPrice)"
                        + " VALUES( '" + dp.Rows[0]["companyID"].ToString() + "','" + dp.Rows[0]["proName"].ToString() + "'," + stu.productID + "," + stu.num + "," + money + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + stu.proLD + "'," + type + ",'" + stu.orderNO + "','" + stu.memberID + "','" + stu.code + "'," + payType + "," + stu.mechineID + ",'" + stu.bz + "','" + stu.billno + "','" + dp.Rows[0]["price0"].ToString() + "')";
                        Util.Debuglog("sql=" + sql, "插入售卖记录.txt");
                        //int a= DbHelperSQL.ExecuteSql(sql);
                        //if (a>0)
                        //{
                        //    //更新购买次数
                        //    string update = "update asm_member set consumeCount=consumeCount+1,LastTime='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' where id='" + stu.memberID + "'";
                        //    DbHelperSQL.ExecuteSql(update);
                        //}
                    }
                    else
                    {
                        if (type == "2")
                        {
                            //先查询有没有没有则插入
                            string sql1 = "select * from asm_sellDetail where billno='" + stu.billno + "'";
                            DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
                            if (dd.Rows.Count <= 0)
                            {
                                string sqlMM = "select id from  asm_member where openID in (select acct from asm_pay_info where trxid='" + stu.billno + "') OR unionID in (select unionID from asm_pay_info where trxid='" + stu.billno + "')";
                                DataTable dMM = DbHelperSQL.Query(sqlMM).Tables[0];
                                string memberID = "0";
                                if (dMM.Rows.Count > 0)
                                {
                                    memberID = dMM.Rows[0]["id"].ToString();
                                }

                                string sqlp = "select * from asm_product where productID='" + stu.productID + "'";
                                DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                                string sql = "INSERT INTO [dbo].[asm_sellDetail](companyID,productname,[productID],[num] ,[totalMoney],[orderTime],[proLD],[type],[orderNO],[memberID],[code] ,[payType],[mechineID],[bz],[billno],oldPrice)"
                            + " VALUES( '" + dp.Rows[0]["companyID"].ToString() + "','" + dp.Rows[0]["proName"].ToString() + "'," + stu.productID + "," + stu.num + "," + stu.totalMoney + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + stu.proLD + "'," + type + ",'" + stu.orderNO + "','" + memberID + "','" + stu.code + "'," + payType + "," + stu.mechineID + ",'" + stu.bz + "','" + stu.billno + "','"+ dp.Rows[0]["price0"].ToString()+ "')";
                                Util.Debuglog("零售sql=" + sql, "插入售卖记录.txt");
                     
                                int a = DbHelperSQL.ExecuteSql(sql);
                                if (stu.bz == "出货失败")
                                {

                                }
                                else {
                                    asm_ld_change(stu.mechineID.ToString(), stu.productID.ToString(), stu.proLD.ToString(), 1, "2", -1);//库存变动记录
                                }
                                
                                //此处更新会员等级
                                Util.updateMemberDJ(memberID, stu.mechineID.ToString());
                            }
                        }
                        else if (type == "1")
                        {
                            if (stu.bz == "出货失败")
                            {

                            }
                            else
                            {
                                asm_ld_change(stu.mechineID.ToString(), stu.productID.ToString(), stu.proLD.ToString(), 1, "3", -1);//库存变动记录
                            }
                           
                            string sqlp = "select * from asm_product where productID='" + stu.productID + "'";
                            DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                            string sql = "INSERT INTO [dbo].[asm_sellDetail](companyID,productname,[productID],[num] ,[totalMoney],[orderTime],[proLD],[type],[orderNO],[memberID],[code] ,[payType],[mechineID],[bz],[billno])"
                        + " VALUES( '" + dp.Rows[0]["companyID"].ToString() + "','" + dp.Rows[0]["proName"].ToString() + "'," + stu.productID + "," + stu.num + "," + stu.totalMoney + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + stu.proLD + "'," + type + ",'" + stu.orderNO + "','" + stu.memberID + "','" + stu.code + "'," + payType + "," + stu.mechineID + ",'" + stu.bz + "','" + stu.billno + "')";
                            Util.Debuglog("订购sql=" + sql, "插入售卖记录.txt");
                            int a = DbHelperSQL.ExecuteSql(sql);
                        }

                    }
                    string sqlM = "select * from asm_company where id in(select companyID from asm_mechine where id=" + stu.mechineID + ")";
                    DataTable dM = DbHelperSQL.Query(sqlM).Tables[0];
                    if (stu.bz == "交易成功" && dM.Rows.Count > 0)
                    {

                        //这几个机器是通过设备app端调的更新库存接口更新的库存
                        if (stu.mechineID != 33 && stu.mechineID != 34 && stu.mechineID != 35 && stu.mechineID != 36 && stu.mechineID != 37)
                        {
                            update_KC1(stu.mechineID.ToString(), stu.proLD.ToString());
                        }
                        update_KC(stu.mechineID.ToString(), dM.Rows[0]["id"].ToString(), stu.productID.ToString());
                    }
                    #region 出货错误发送短信
                       //
                    if (stu.bz == "料道错误" || stu.bz == "交易序列号相同" || stu.bz == "料道故障" || stu.bz == "校验错误" || stu.bz == "出货失败")
                    {
                        sendMsgWhenLDError(stu.mechineID.ToString(), stu.proLD.ToString());//发送短信
                        //更新错误料道
                        string update = "update asm_ldInfo set zt='1',lastUpTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ldNO=" + stu.proLD + " and mechineID=" + stu.mechineID;
                        log("更新错误料道" + update, "upSellRecord1.txt");
                        DbHelperSQL.ExecuteSql(update);
                        ClearRedisProductInfoByMechineID(stu.mechineID.ToString());//清除该设备的产品信息缓存
                                                                                   //插入异常记录
                                                                                   //string sqlM1 = "SELECT phone FROM asm_member where unionID in (SELECT unionID FROM asm_pay_info  WHERE trxid='"+stu.billno+"')";
                        string sqlM1 =  "select b.phone from asm_sellDetail a left join asm_member b  on a.memberID=b.id  where orderno='"+ stu.orderNO+ "'";
                        
                            
                        log("插入异常记录" + sqlM1, "upSellRecord1.txt");
                        DataTable dm1 = DbHelperSQL.Query(sqlM1).Tables[0];
                        string phone = "";
                        if (dm1.Rows.Count>0&&!string.IsNullOrEmpty(dm1.Rows[0]["phone"].ToString()))
                        {
                             phone = "购买人手机号:" + dm1.Rows[0]["phone"].ToString();
                        }
                        log("料道错误"+sqlM1, "upSellRecord1.txt");
                        string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu,bz) values(" + stu.mechineID + ",3,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'出错料道号" + stu.proLD +phone+ "')";
                        DbHelperSQL.ExecuteSql(insert);
                        string up1 = "update asm_mechine set ldStatus=1 where id=" + stu.mechineID;
                        DbHelperSQL.ExecuteSql(up1);

                        SendLDError(stu.proLD.ToString(), stu.mechineID.ToString());//给操作员发送错误
                    }
                    #endregion

                 
                    #region  退款操作
                    if (dM.Rows.Count > 0 && dM.Rows[0]["tkbs"].ToString() == "1")
                    {
                        //判断出货状态如果是料道错误或者是交易序列号相同给退款 零售的怎么来怎么退 订购的退到钱包
                        if (!string.IsNullOrEmpty(stu.code))
                        {
                            //订购的
                            if (stu.bz == "料道错误" || stu.bz == "交易序列号相同" || stu.bz == "料道故障" || stu.bz == "校验错误" || stu.bz == "出货失败")
                            {
                                string sql = "select * from asm_sellDetail where memberID='" + stu.memberID + "' and mechineID='" + stu.mechineID + "' and bz='退款成功' and code='" + stu.code + "' ";
                                Util.Debuglog("查询sql=" + sql, "退款记录.txt");
                                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                                if (dd.Rows.Count <= 0)
                                {
                                    //更新会员钱包 并插入资金变动记录
                                    string update = "update asm_member set  AvailableMoney=AvailableMoney+" + stu.totalMoney.ToString() + ",sumConsume=sumConsume-" + stu.totalMoney.ToString() + " where id=" + stu.memberID.ToString();
                                    Util.Debuglog("变动余额sql=" + sql, "退款记录.txt");
                                    int a = DbHelperSQL.ExecuteSql(update);
                                    string sqlm = "select * from asm_member where id=" + stu.memberID.ToString();
                                    DataTable dt = DbHelperSQL.Query(sqlm).Tables[0];
                                    if (a > 0)
                                    {
                                        string sqlu = "update asm_sellDetail set bz='退款成功' where billno='" + stu.billno + "'";
                                        Util.Debuglog("更改状态sql=" + sql, "退款记录.txt");
                                        DbHelperSQL.ExecuteSql(sqlu);
                                        Util.insertNotice(dt.Rows[0]["id"].ToString(), "出货异常退款", "您于" + stu.orderTime + "取货异常退还金额:" + stu.totalMoney.ToString() + ";请查收钱包");
                                        Util.moneyChange(dt.Rows[0]["id"].ToString(), stu.totalMoney.ToString(), dt.Rows[0]["AvailableMoney"].ToString(), "退款通知", "7", "", "取货异常退还金额:" + stu.totalMoney.ToString());
                                        if (!string.IsNullOrEmpty(dt.Rows[0]["openID"].ToString()))
                                        {
                                            try
                                            {
                                                string company = Util.getCompany(dt.Rows[0]["companyID"].ToString());

                                                wxHelper wx = new wxHelper(dt.Rows[0]["companyID"].ToString());
                                                string data = TemplateMessage.tk(dt.Rows[0]["openID"].ToString(), OperUtil.getMessageID(dt.Rows[0]["companyID"].ToString(), "OPENTM410089600"), "退款通知", stu.totalMoney.ToString(), "您购买的商品没有出货成功，钱已退还到账户");
                                                TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dt.Rows[0]["companyID"].ToString()), data);
                                            }
                                            catch (Exception e)
                                            {
                                                Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //零售的
                            if (stu.bz == "料道错误" || stu.bz == "交易序列号相同" || stu.bz == "料道故障" || stu.bz == "校验错误" || stu.bz == "出货失败")
                            {
                                if (stu.payType == "3")
                                {
                                    //退到钱包
                                    string sql = "select * from asm_sellDetail where billno='" + stu.billno + "'";
                                    DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                                    if (dd.Rows.Count > 0)
                                    {
                                        //更新会员钱包 并插入资金变动记录
                                        string update = "update asm_member set  AvailableMoney=AvailableMoney+" + dd.Rows[0]["totalMoney"].ToString() + ",sumConsume=sumConsume-" + dd.Rows[0]["totalMoney"].ToString() + " where id=" + dd.Rows[0]["memberID"].ToString();
                                        int a = DbHelperSQL.ExecuteSql(update);
                                        string sqlm = "select * from asm_member where id=" + dd.Rows[0]["memberID"].ToString();
                                        DataTable dt = DbHelperSQL.Query(sqlm).Tables[0];
                                        if (a > 0)
                                        {
                                            string update1 = "update asm_pay_info set statu=2,fintime='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' where trxid='" + stu.billno + "'";
                                            DbHelperSQL.ExecuteSql(update1);
                                            string sqlu = "update asm_sellDetail set bz='退款成功' where billno='" + stu.billno + "'";
                                            DbHelperSQL.ExecuteSql(sqlu);
                                            Util.insertNotice(dt.Rows[0]["id"].ToString(), "出货异常退款", "您于" + stu.orderTime + "购买商品出货异常退还金额:" + dd.Rows[0]["totalMoney"].ToString() + ";请查收钱包");
                                            Util.moneyChange(dt.Rows[0]["id"].ToString(), dd.Rows[0]["totalMoney"].ToString(), dt.Rows[0]["AvailableMoney"].ToString(), "退款通知", "7", "", "取货异常退还金额:" + dd.Rows[0]["totalMoney"].ToString());
                                            if (!string.IsNullOrEmpty(dt.Rows[0]["openID"].ToString()))
                                            {
                                                try
                                                {
                                                    string company = Util.getCompany(dt.Rows[0]["companyID"].ToString());

                                                    wxHelper wx = new wxHelper(dt.Rows[0]["companyID"].ToString());
                                                    string data = TemplateMessage.tk(dt.Rows[0]["openID"].ToString(), OperUtil.getMessageID(dt.Rows[0]["companyID"].ToString(), "OPENTM410089600"), "退款通知", dd.Rows[0]["totalMoney"].ToString(), "您购买的商品没有出货成功，钱已退还到账户");
                                                    TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dt.Rows[0]["companyID"].ToString()), data);
                                                }
                                                catch (Exception e)
                                                {
                                                    Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (stu.payType == "1" || stu.payType == "2")
                                {
                                    //退到1微信或者2支付宝
                                    string sql = "select * from asm_pay_info where trxid='" + stu.billno + "'";
                                    DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                                    if (dd.Rows.Count > 0 && dd.Rows[0]["statu"].ToString() == "1")
                                    {
                                        SybWxPayService sybService = new SybWxPayService(stu.mechineID.ToString());
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
                                        JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                                        if (jo["retcode"].ToString() == "SUCCESS")
                                        {
                                            string cusid = jo["cusid"].ToString();//商户号
                                            string trxid = jo["trxid"].ToString();//交易单号
                                            string fintime = jo["fintime"].ToString();//交易完成时间
                                            string reqsn1 = jo["reqsn"].ToString();//商户订单号
                                            string trxstatus = jo["trxstatus"].ToString();//交易状态
                                            if (trxstatus == "0000")
                                            {
                                                //更新asm_pay_info
                                                string update = "update asm_pay_info set statu=2,tkreqsn='" + reqsn1 + "',fintime='" + fintime + "',trxstatus='" + trxstatus + "' where trxid='" + oldtrxid + "'";
                                                DbHelperSQL.ExecuteSql(update);
                                                string sqlu = "update asm_sellDetail set bz='退款成功' where billno='" + stu.billno + "'";
                                                DbHelperSQL.ExecuteSql(sqlu);

                                                string sqlm = "select * from asm_member where openID='" + dd.Rows[0]["acct"].ToString() + "'";
                                                DataTable dt = DbHelperSQL.Query(sqlm).Tables[0];
                                                if (dt.Rows.Count > 0)
                                                {

                                                    Util.insertNotice(dt.Rows[0]["id"].ToString(), "出货异常退款", "您于" + stu.orderTime + "购买商品出货异常退还金额:" + double.Parse(dd.Rows[0]["trxamt"].ToString()) / 100 + ";请查收微信或支付宝");
                                                    Util.moneyChange(dt.Rows[0]["id"].ToString(), stu.totalMoney.ToString(), dt.Rows[0]["AvailableMoney"].ToString(), "退款通知", "7", "", "取货异常退还金额:" + stu.totalMoney);
                                                    string update1 = "update asm_member set  sumConsume=sumConsume-" + (double.Parse(dd.Rows[0]["trxamt"].ToString()) / 100) + " where openID='" + dd.Rows[0]["acct"].ToString() + "'";
                                                    int a = DbHelperSQL.ExecuteSql(update1);
                                                    if (!string.IsNullOrEmpty(dt.Rows[0]["openID"].ToString()))
                                                    {
                                                        try
                                                        {
                                                            string sqlPayInfo = "select * from asm_pay_info where trxid='" + stu.billno + "'";
                                                            DataTable ddpayInfo = DbHelperSQL.Query(sqlPayInfo).Tables[0];
                                                            string company = Util.getCompany(dt.Rows[0]["companyID"].ToString());
                                                            wxHelper wx = new wxHelper(dt.Rows[0]["companyID"].ToString());
                                                            data = TemplateMessage.tk(dt.Rows[0]["openID"].ToString(), OperUtil.getMessageID(dt.Rows[0]["companyID"].ToString(), "OPENTM410089600"), "退款通知", (double.Parse(ddpayInfo.Rows[0]["trxamt"].ToString()) / 100).ToString("f2"), "您购买的商品没有出货成功，钱已退还到账户");
                                                            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dt.Rows[0]["companyID"].ToString()), data);
                                                        }
                                                        catch (Exception e)
                                                        {
                                                            Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                                                        }
                                                    }

                                                }

                                            }
                                            else
                                            {
                                                string errmsg = jo["errmsg"].ToString();//交易失败信息
                                                string update = "update asm_pay_info set tkreqsn='" + reqsn1 + "',fintime='" + fintime + "',trxstatus='" + trxstatus + "',errmsg='" + errmsg + "' where trxid='" + oldtrxid + "'";
                                                DbHelperSQL.ExecuteSql(update);
                                            }

                                        }
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                log("error=" + ex.Message, "_上传商品销售记录1.txt");
            }
            log("Util结束recordList=", "upSellRecord1.txt");
            return "1";
        }
        public static  string update_KC1(string mechineID, string ldNO)
        {
            Util.ClearRedisProductInfoByMechineID(mechineID);
            string sql = "select * from asm_ldinfo where ldNO='" + ldNO + "' and mechineID=" + mechineID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];

            if (dt.Rows.Count > 0 && int.Parse(dt.Rows[0]["ld_productNum"].ToString()) > 0)
            {
                string sql1 = "update asm_ldInfo set ld_productNum=ld_productNum-1,lastUpTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ldNO='" + ldNO + "' and mechineID=" + mechineID;
                OperUtil.Debuglog("sql1=" + sql1, "更新料道库存_.txt");
                int a = DbHelperSQL.ExecuteSql(sql1);
                if (a > 0)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            return "0";
        }
        /// <summary>
        /// 库存变动记录表 会员购买， 配送员加货，
        /// </summary>
        /// <param name="mechineID"></param>
        /// <param name="companyID"></param>
        /// <param name="productID"></param>
        public static void update_KC(string mechineID, string companyID, string productID)
        {
            string sql = "select productID,sum(ld_productNum)ld_productNum,sum(csldNum) csldNum,sum(csldNum)-sum(ld_productNum) cha from asm_ldInfo where mechineID=" + mechineID + " and productID='" + productID + "'  group by productID";
            Util.Debuglog("sql=" + sql, "update_KC.txt");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string orderDetail = "select COUNT(*) num from asm_orderlistDetail where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and mechineID=" + mechineID + " and productID=" + productID + " and zt=4";
                Util.Debuglog("orderDetail=" + orderDetail, "update_KC.txt");
                DataTable dtDetail = DbHelperSQL.Query(orderDetail).Tables[0];
                string orderDetailTotal = "select COUNT(*) num from asm_orderlistDetail where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and mechineID=" + mechineID + " and zt<7 and productID=" + productID;

                DataTable dtDetailT = DbHelperSQL.Query(orderDetailTotal).Tables[0];
                //剩余订购
                string dgNum = dtDetail.Rows[0]["num"].ToString();
                //剩余零售=总剩余-剩余订购
                string lsNum = (int.Parse(dt.Rows[0]["ld_productNum"].ToString()) - int.Parse(dgNum)).ToString();
                if (int.Parse(lsNum)<0)
                {
                    lsNum = "0";
                }
                
                //totalLsNum 计算规则 用该商品所占料道的最大初始料道数量之和 -当日订购的数量=总的零售数量
                string totalLsNum = (int.Parse(dt.Rows[0]["csldNum"].ToString())).ToString();
                string cha = (int.Parse(totalLsNum) - int.Parse(lsNum)).ToString();
                if (int.Parse(cha)<0)
                {
                    cha = "0";
                }
                string insert = "insert into asm_kcDetail(companyID,mechineID,productID,dateTime,dgNum,lsNum,totalLsNum,imbalance,totalDgNum) "
                   + " values('" + companyID + "','" + mechineID + "','" + productID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + dgNum + "','" + lsNum + "','" + totalLsNum + "','" + cha + "','" + dtDetailT.Rows[0]["num"].ToString() + "')";
                Util.Debuglog("insert=" + insert, "update_KC.txt");
                DbHelperSQL.ExecuteSql(insert);
                RedisHelper.Remove(mechineID + "_KcProduct");
            }
        }
        public static async System.Threading.Tasks.Task SendLDError(string ldNO, string mechineID)
        {
            try
            {
                string url = "http://114.116.16.200/api/api.ashx";
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["action"] = "SendLDError";
                    values["ldno"] = ldNO;
                    values["mechineID"] = mechineID;//支付宝订单号
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.Default.GetString(response);

                }
            }
            catch (Exception e)
            {

            }
        }
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        /// <summary>
        /// 料道异常的时候发送短信
        /// </summary>
        /// <param name="mechineID"></param>
        public static void sendMsgWhenLDError(string mechineID, string ldNO)
        {
            string sqlT3 = "select * from asm_mechine where id=" + mechineID;
            DataTable dt3 = DbHelperSQL.Query(sqlT3).Tables[0];
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    try
                    {
                        string sql21 = "select linkphone from asm_opera where id='" + dt3.Rows[i]["operaID"].ToString() + "'";
                        string sql22 = "select linkphone from asm_company where id='" + dt3.Rows[i]["companyID"].ToString() + "'";
                        DataTable d21 = DbHelperSQL.Query(sql21).Tables[0];
                        DataTable d22 = DbHelperSQL.Query(sql22).Tables[0];
                        if (d21.Rows.Count > 0 && d21.Rows[0]["linkphone"].ToString() != "")
                        {
                           
                            OperUtil.sendMessage4(d21.Rows[0]["linkphone"].ToString(), dt3.Rows[0]["mechineName"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 5), ldNO);
                        }
                        if (d22.Rows.Count > 0 && d22.Rows[0]["linkphone"].ToString() != "")
                        {
                            OperUtil.sendMessage4(d22.Rows[0]["linkphone"].ToString(), dt3.Rows[0]["mechineName"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 5), ldNO);
                        }

                    }
                    catch (Exception ex)
                    {
                        OperUtil.Debuglog("cuowu=" + ex.Message, "料道异常短信_.txt");
                    }
                }
            }
        }
        /// <summary>
        /// 此方法在变动之前调用
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="mechineID"></param>
        /// <param name="productID"></param>
        /// <param name="ldNO"></param>
        /// 1 补货任务 2购买、3订购取货、4料道纠错
        /// <param name="changeNum">变化数量</param>
        /// status-1 减少 +1增加
        public static void asm_ld_change(string mechineID,string productID,string ldNO,int changeNum,string type,int status)
        {
            string sqlLdInfo = "select * from asm_ldinfo where mechineID="+mechineID+" and productID="+productID+" and ldNO='"+ldNO+"'";
            DataTable dt = DbHelperSQL.Query(sqlLdInfo).Tables[0];
            string sqlM = "select * from asm_mechine where id="+mechineID;
            DataTable DM = DbHelperSQL.Query(sqlM).Tables[0];
            string companyID = DM.Rows[0]["companyID"].ToString();
            if (dt.Rows.Count > 0)
            {
                
                string afterNum = (int.Parse(dt.Rows[0]["ld_productNum"].ToString()) + status* changeNum).ToString();
                string beforeNum = dt.Rows[0]["ld_productNum"].ToString();
                
                string insert = "insert into asm_ld_change(companyID,mechineID,productID,changeNum,afterNum,beforeNum,ldNO,chgTime,status,type) " +
                    "values('" + companyID + "','" + mechineID + "','" + productID + "','" + status*changeNum + "','" + afterNum + "','"+beforeNum+"','" + ldNO + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','"+status+"','" + type + "')";
                DbHelperSQL.ExecuteSql(insert);
            }
            else {
                
                string insert = "insert into asm_ld_change(companyID,mechineID,productID,changeNum,afterNum,beforeNum,ldNO,chgTime,status,type) "+
                    "values('"+companyID+"','"+mechineID+"','"+productID+"','"+ status * changeNum + "','"+changeNum+"','0','"+ldNO+ "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',1,'"+type+"')";
                DbHelperSQL.ExecuteSql(insert);
            }
        }
        public static Boolean judge(string operaID, string menu)
        {
            if (operaID=="0")
            {
                return true;
            }
            string sql = "select * from asm_opera where id='" + operaID + "'";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count > 0)
            {
                string sql2 = "select * from asm_qx where roleID=" + dd.Rows[0]["qx"].ToString() + " and menuID='" + menu + "' ";
                DataTable dt = DbHelperSQL.Query(sql2).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["flag"].ToString() == "0")
                    {
                        //不允许登录
                        return false;
                    }
                    else if (dt.Rows[0]["flag"].ToString() == "1")
                    {
                        //允许登录
                        return true;
                    }
                }
            }
            return false;
        }
    }
}