using autosell_center.util;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenPlatForm.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Web;

namespace Consumer.cls
{
    public static class Util
    {
        
        //获取小程序的appid
        public static string getMinAppid(string companyID)
        {
            string appid = RedisHelper.GetRedisModel<string>("minAppid_" + companyID);
            if (string.IsNullOrEmpty(appid))
            {
                string sql = "select * from asm_company where id=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel("minAppid_" + companyID, dt.Rows[0]["minAppid"].ToString(), new TimeSpan(24 * 365, 0, 0));
                    return dt.Rows[0]["minAppid"].ToString();
                }
                else
                {
                    RedisHelper.SetRedisModel("minAppid_" + companyID, "", new TimeSpan(24 * 365, 0, 0));
                    return null;
                }
            }
            else {
                return appid;
            }
        }
        public static string getMinSecret(string companyID)
        {
            string minSecret = RedisHelper.GetRedisModel<string>("minSecret_" + companyID);
            if (string.IsNullOrEmpty(minSecret))
            {
                string sql = "select * from asm_company where id=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RedisHelper.SetRedisModel("minSecret_" + companyID, dt.Rows[0]["minSecret"].ToString(), new TimeSpan(24 * 365, 0, 0));
                    return dt.Rows[0]["minSecret"].ToString();
                }
                else
                {
                    RedisHelper.SetRedisModel("minSecret_" + companyID, "", new TimeSpan(24 * 365, 0, 0));
                    return null;
                }
            }
            else
            {
                return minSecret;
            }
        }
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            // 当地时区

            DateTime dt = startTime.AddMilliseconds(double.Parse(timeStamp));
            return dt;
        }
        //获取开放平台的component_access_token
        public static string getComAccessToken()
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
                        else
                        {
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
        public static void Debuglog(string log, string logname = "_Debuglog.txt")
        {
            try
            {
                StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "log/" + (DateTime.Now.ToString("yyyyMMdd") + logname));
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
                writer.WriteLine("---------------");
                writer.Close();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="money"></param>
        /// <param name="bz"></param>
        /// <param name="description"></param>
        /// <param name="payType">1 会员充值 2 会员消费 3会员转账收入 4转账支出 5 售卖</param>
        public static void chgMoney(string memberID, string money, string bz, string description, string payType)
        {
            string sql = "insert into asm_chgMoney(memberID,payTime,type,money,bz,description) values('" + memberID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + payType + "','" + money + "','" + bz + "','" + description + "')";
            DbHelperSQL.ExecuteSql(sql);
        }
        public static string SerializeDictionaryToJsonString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict.Count == 0)
                return "";

            string jsonStr = JsonConvert.SerializeObject(dict);
            return jsonStr;
        }
        //存入缓存
        public static void SetSession(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 30;
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
        public static void insertRecord(string memberID, string money, string avaMoney, string bz)
        {
            string sql = "INSERT INTO [dbo].[asm_transfer]([memberID],[mone],[time],[avaMoney],[bz])VALUES('" + memberID + "'," + money + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + avaMoney + ",'" + bz + "')";
            DbHelperSQL.ExecuteSql(sql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberID"></param>会员id
        /// <param name="money"></param>变动金额
        /// <param name="avaMoney"></param>可用余额
        /// <param name="bz"></param>备注
        /// <param name="type"></param>1充值2购物3转账
        public static void moneyChange(string memberID, string money, string avaMoney, string bz, string type, string skID)
        {
            string sql1 = "insert into asm_moneyChange(payTime,money,AvaiilabMOney,memberID,[type],bz,skID) values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + money + "," + avaMoney + "," + memberID + "," + type + ",'" + bz + "','" + skID + "')";
            Util.Debuglog("金额变动记录语句=" + sql1, "_.金额变动.txt");
            DbHelperSQL.ExecuteSql(sql1);
        }
        public static void insertNotice(string memberID, string title, string con,string mechineID)
        {
            string sql = "insert into asm_notice (memberID,title,con,time,mechineID) values('" + memberID + "','" + title + "','" + con + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','"+ mechineID + "')";
            Util.Debuglog("插入消息通知=" + sql, "_.插入消息通知.txt");
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
            Util.Debuglog("GetToken=" + responseStr, "_获取会员token.txt");
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
            Util.Debuglog("GetTokenInfo=" + responseStr, "_获取会员token.txt");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ldNO"></param>
        /// <param name="mechineID"></param>
        /// <param name="billno"></param>
        /// <param name="payType">1微信2支付宝3 微信公众号4 会员卡余额支付</param>
        /// <returns></returns>
        public static void ch(string ldNO, string mechineID, string billno, string payType, string productID, string money)
        {
            try
            {
                string url = "http://114.116.16.200/api/api.ashx";
                Util.Debuglog("11ldNO=" + ldNO + ";mechineID=" + mechineID + ";billno=" + billno + ";payType=" + payType + ";productID=" + productID + ";money=" + money, "ch.txt");
                using (var client = new WebClient())
                { 
                    var values = new NameValueCollection();
                    values["action"] = "ch";
                    values["ldNO"] = ldNO;
                    values["mechineID"] = mechineID;//支付宝订单号
                    values["billno"] = billno;
                    values["payType"] = payType;
                    values["productID"] = productID;
                    values["money"] = money;
                    values["type"] = "2";
                    Util.Debuglog("ldNO="+ ldNO+ ";mechineID="+ mechineID+ ";billno="+ billno+ ";payType="+ payType+ ";productID="+ productID+ ";money="+money, "ch.txt");
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.GetEncoding("utf-8").GetString(response);
                    Util.Debuglog("responseString=" + responseString, "ch.txt");
                }
            }
            catch (Exception e)
            {
                Util.Debuglog("e="+e.Message, "ch.txt");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ldNO"></param>
        /// <param name="mechineID"></param>
        /// <param name="billno"></param>
        /// <param name="payType">1微信2支付宝3 微信公众号4 会员卡余额支付</param>
        /// <returns></returns>
        public static void chNew(string ldNO, string mechineID, string billno, string payType, string productID, string money)
        {
            try
            {
                string url = "http://alisocket.bingoseller.com/api/api.ashx";
                Util.Debuglog("11ldNO=" + ldNO + ";mechineID=" + mechineID + ";billno=" + billno + ";payType=" + payType + ";productID=" + productID + ";money=" + money, "ch.txt");
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["action"] = "chNew";
                    values["ldNO"] = ldNO;
                    values["mechineID"] = mechineID;//支付宝订单号
                    values["billno"] = billno;
                    values["payType"] = payType;
                    values["productID"] = productID;
                    values["money"] = money;
                    values["type"] = "2";
                    Util.Debuglog("ldNO=" + ldNO + ";mechineID=" + mechineID + ";billno=" + billno + ";payType=" + payType + ";productID=" + productID + ";money=" + money, "ch.txt");
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.GetEncoding("utf-8").GetString(response);
                    Util.Debuglog("responseString=" + responseString, "ch.txt");
                }
            }
            catch (Exception e)
            {
                Util.Debuglog("e=" + e.Message, "ch.txt");
            }
        }
        public static async System.Threading.Tasks.Task dgch(string ldNO, string mechineID, string memberID,string productID,string code,string money)
        {
            try
            {
                Util.Debuglog("ldNO=" + ldNO + ";mechineID=" + mechineID + ";memberID=" + memberID + ";productID=" + productID + ";code=" + code, "订购出货.txt");
                string url = "http://114.116.16.200/api/api.ashx";
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["action"] = "dgch";
                    values["ldNO"] = ldNO;
                    values["mechineID"] = mechineID;
                    values["memberID"] = memberID;
                    values["productID"] = productID;
                    values["type"] = "1";
                    values["money"] = money;
                    values["code"] = code;
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.Default.GetString(response);
                    Util.Debuglog("responseStrin=" + responseString, "订购出货.txt");
                }
            }
            catch (Exception e)
            {

            }
        }
        public static async System.Threading.Tasks.Task dgchNew(string ldNO, string mechineID, string memberID, string productID, string code, string money)
        {
            try
            {
                Util.Debuglog("ldNO=" + ldNO + ";mechineID=" + mechineID + ";memberID=" + memberID + ";productID=" + productID + ";code=" + code, "订购出货.txt");
                string url = "http://alisocket.bingoseller.com/api/api.ashx";
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["action"] = "dgchNew";
                    values["ldNO"] = ldNO;
                    values["mechineID"] = mechineID;
                    values["memberID"] = memberID;
                    values["productID"] = productID;
                    values["type"] = "1";
                    values["money"] = money;
                    values["code"] = code;
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.Default.GetString(response);
                    Util.Debuglog("responseStrin=" + responseString, "订购出货.txt");
                }
            }
            catch (Exception e)
            {

            }
        }
        public static  string chStatus(string type, string mechineID)
        {
            try
            {
                string url = "http://114.116.16.200/api/api.ashx";
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["action"] = "chStatus";
                    values["type"] = type;
                    values["mechineID"] = mechineID;
                   
                    var response = client.UploadValues(url, values);
                    var responseString = Encoding.Default.GetString(response);
                    Util.Debuglog("mechineID"+ mechineID + responseString, "chStatus.txt");
                    JObject jo = JObject.Parse(responseString);
                    if (jo["code"].ToString()=="200")
                    {
                        return "200";
                    }
                    return "500";
                }
            }
            catch (Exception e)
            {
                return "500";
            }
        }
        /// <summary>
        /// 获取订购料道编号
        /// </summary>
        /// <param name="mechineID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static string getDGLDNO(string mechineID, string productID)
        {
            try
            {
                string sql = "SELECT * from asm_ldInfo where  mechineID=" + mechineID + " AND productID=" + productID + " and ld_productNum>0 and (zt is null or zt=0) ORDER BY yxch DESC";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count>0)
                {
                    return dt.Rows[0]["ldNO"].ToString();
                }
                return null;
            }
            catch {
                return null;
            }
        }
        /// <summary>
        /// 零售时候 获取出货料道
        /// </summary>
        /// <param name="mechineID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static string getLDNO(string mechineID,string productID)
        {
            try
            {
                string sql = "select SUM(ld_productNum) ld_productNum from  asm_ldinfo where mechineID=" + mechineID + " and productID=" + productID + " and ld_productNum>0 and (zt is null or zt=0) group by productID";
                Util.Debuglog("sql=" + sql, "getLD.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string sql1 = "select COUNT(*) num  from asm_orderlistDetail where mechineID=" + mechineID + " and productID=" + productID + " and zt=4 and createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                    Util.Debuglog("sql1=" + sql1, "getLD.txt");
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (int.Parse(dt.Rows[0]["ld_productNum"].ToString()) <= int.Parse(d1.Rows[0]["num"].ToString()))
                    {
                        Util.Debuglog("sql=1111", "getLD.txt");
                        return null;
                    }
                    string sql3 = "select * from  asm_ldinfo where mechineID=" + mechineID + " and productID=" + productID + " and ld_productNum>0 and (zt is null or zt=0) order by yxch";
                    Util.Debuglog("sql3=" + sql3, "getLD.txt");
                    DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];
                    return d3.Rows[0]["ldNO"].ToString();
                }
                else
                {
                    Util.Debuglog("sql=222", "getLD.txt");
                    return null;
                }
            }
            catch (Exception ex){
                Util.Debuglog("ex="+ex.Message, "getLD.txt");
                return null;
            }
        }
        public static string DataTableToJsonWithJsonNet(DataTable table)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.None);
            return jsonString;
        }
        //地球半径，单位米
        public const double EARTH_RADIUS = 6378137;
        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="lat1">第一点纬度</param>
        /// <param name="lng1">第一点经度</param>
        /// <param name="lat2">第二点纬度</param>
        /// <param name="lng2">第二点经度</param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }
        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }
        /// <summary>
        /// 用户判断会员购物的时候升级
        /// </summary>
        public static void growUpMember(string unionID, string member_ID)
        {
            string sql3 = "select * from asm_member where  unionID='" + unionID + "' or id='"+ member_ID + "' order by id desc";
            DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];
            if (d3.Rows.Count>0)
            {
                if (d3.Rows[0]["dj"].ToString()=="3")
                {
                    return;
                }
                string memberID = d3.Rows[0]["id"].ToString();
                string sql2 = "select COUNT(*) num,LEFT(CONVERT(varchar,CONVERT(datetime,orderTime),23),10) from asm_sellDetail where memberID='" + memberID + "' and  DATEDIFF(DD,orderTime,GETDATE())<=30 group by LEFT(CONVERT(varchar,CONVERT(datetime,orderTime),23),10)";
                DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                string sql = "select * from asm_dj where companyID in(select companyID from asm_member where id="+d3.Rows[0]["id"].ToString()+") and consumeDay<=" + dt2.Rows.Count + " order by id desc";
                DataTable ds = DbHelperSQL.Query(sql).Tables[0];
                if (ds.Rows.Count > 0)
                {
                    
                    string sqlM = "update asm_member set dj=" + ds.Rows[0]["djID"].ToString() + " where phone!='' and phone is not null and id=" + memberID;
                    Util.Debuglog("sqlM="+ sqlM, "growUpMember.txt");
                    DbHelperSQL.ExecuteSql(sqlM);
                    if (d3.Rows[0]["dj"].ToString()!= ds.Rows[0]["djID"].ToString()&&!string.IsNullOrEmpty(d3.Rows[0]["openID"].ToString()))
                    {
                        try
                        {
                            string company = Util.getCompany(d3.Rows[0]["companyID"].ToString());
                            JArray array = (JArray)JsonConvert.DeserializeObject(company);
                            wxHelper wx = new wxHelper(d3.Rows[0]["companyID"].ToString());
                            string data = TemplateMessage.getDJChange(d3.Rows[0]["openID"].ToString(),
                                OperUtil.getMessageID(d3.Rows[0]["companyID"].ToString(), "OPENTM406811407"),
                                "尊敬的会员，您的会员等级发生变更", "" + d3.Rows[0]["dj"].ToString() + "", "" + ds.Rows[0]["djID"].ToString() + "",
                                "如有疑问，请拨打会员服务热线" + array[0]["customerPhone"].ToString());
                            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(d3.Rows[0]["companyID"].ToString()), data);
                        }
                        catch(Exception e)
                        {
                            Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                        }
                    }
                }
                else
                {
                    //string sqlM = "update asm_member set dj=0 where id=" + memberID+ " and (phone='' or phone is null)";
                    //DbHelperSQL.ExecuteSql(sqlM);
                }
            }
        }
        /// <summary>
        /// 预定产品判断是否成为黄金会员
        /// unionID
        /// zq 订购周期
        /// </summary>
        /// <param name="unionID"></param>
        public static void growUpMemberBYDG(string minOpenID,int zq,string companyID)
        {
            //检测是否开启
            string sql1 = "select * from asm_tqlist where companyID="+companyID;
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count>0&&dt1.Rows[0]["ydbuy"].ToString()=="1")
            {
                string sqlM = "select * from  asm_member where minOpenID='" + minOpenID + "'";
                DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
                //判断剩余天数是否达到上限
                if (int.Parse(dm.Rows[0]["hjhydays"].ToString()) + zq < int.Parse(dt1.Rows[0]["totalDay"].ToString()))
                {
                    string sql = "update asm_member set hjhyDays=hjhyDays+" + zq + ",dj=3 where minOpenID='" + minOpenID + "'";
                    int a = DbHelperSQL.ExecuteSql(sql);
                }
                else {
                    //分两种情况如果剩余的天数大于系统设置的上限则不变这样保证会员原来的剩余天数不会减少 否则设置成上限
                    if (int.Parse(dm.Rows[0]["hjhydays"].ToString())<int.Parse(dt1.Rows[0]["totalDay"].ToString()))
                    {
                        string sql = "update asm_member set hjhyDays=" + dt1.Rows[0]["totalDay"].ToString() + ",dj=3 where phone!='' and phone is not null and minOpenID='" + minOpenID + "'";
                        int a = DbHelperSQL.ExecuteSql(sql);
                        if (a>0&&dm.Rows[0]["dj"].ToString()!="3"&&!string.IsNullOrEmpty(dm.Rows[0]["openID"].ToString()))
                        {
                            try
                            {
                                string company = Util.getCompany(dm.Rows[0]["companyID"].ToString());
                                JArray array = (JArray)JsonConvert.DeserializeObject(company);
                                wxHelper wx = new wxHelper(dm.Rows[0]["companyID"].ToString());
                                string data = TemplateMessage.getDJChange(dm.Rows[0]["openID"].ToString(),
                                    OperUtil.getMessageID(dm.Rows[0]["companyID"].ToString(), "OPENTM406811407"),
                                    "尊敬的会员，您的会员等级发生变更", "" + dm.Rows[0]["dj"].ToString() + "", "3",
                                    "如有疑问，请拨打会员服务热线" + array[0]["customerPhone"]);
                                TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dm.Rows[0]["companyID"].ToString()), data);
                            }
                            catch(Exception e)
                            {
                                Util.Debuglog("e="+e.Message,"会员等级消息模板.txt");
                            }
                        }
                    }
                }
              
            }
           
        }
        /// <summary>
        /// 会员充值享受黄金会员等级
        /// </summary>返回享受的特权ID 便于按照时间段查询会员时间段内是否可以无限享受
        /// <param name="unionID"></param>
        /// <param name="czMoney"></param>
        public static string growUpMemberBYCZ(string minOpenID, string czMoney,string companyID)
        {
            string sql1 = "select * from asm_tqlist where companyID=" + companyID;
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0 && dt1.Rows[0]["czswitch"].ToString() == "1")
            {
                
                //需要判断时间范围
                string sql = "select * from [dbo].[asm_tqdetail] where companyID="+companyID+"  and money<="+czMoney+ " order by day desc";
                DataTable d2 = DbHelperSQL.Query(sql).Tables[0];
                if (d2.Rows.Count>0)
                {
                    string sqlM = "select * from  asm_member where minOpenID='" + minOpenID + "'";
                    DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
                    //判断是否设置时间段
                    if (!string.IsNullOrEmpty(d2.Rows[0]["startTime"].ToString()) && !string.IsNullOrEmpty(d2.Rows[0]["endTime"].ToString()))
                    {
                        //判断该会在该时间段内是否已经享受过该当次的特权
                        string sql12 = "select * from [dbo].[asm_partActivity] where type=2 and partTime>='"+d2.Rows[0]["startTime"].ToString()+"' and partTime<='"+d2.Rows[0]["endTime"].ToString()+"' and memberID="+dm.Rows[0]["id"].ToString()+" and tqID='"+d2.Rows[0]["id"].ToString()+"'";
                        DataTable d12 = DbHelperSQL.Query(sql12).Tables[0];
                        if (d12.Rows.Count > 0)//证明已经享受过不在享受
                        {
                            return "0";
                        }
                    }
                    if (int.Parse(dm.Rows[0]["hjhydays"].ToString()) + int.Parse(d2.Rows[0]["day"].ToString()) < int.Parse(dt1.Rows[0]["totalDay"].ToString()))
                    {
                        string sqla = "update asm_member set hjhyDays=hjhyDays+" + d2.Rows[0]["day"].ToString() + ",dj=3 where phone!='' and phone is not null and minOpenID='" + minOpenID + "'";
                        DbHelperSQL.ExecuteSql(sqla);

                        //发送模板消息
                        if (dm.Rows[0]["dj"].ToString()!="3"&&!string.IsNullOrEmpty(dm.Rows[0]["openID"].ToString()))
                        {
                            try
                            {
                                string company = Util.getCompany(dm.Rows[0]["companyID"].ToString());
                                JArray array = (JArray)JsonConvert.DeserializeObject(company);
                                wxHelper wx = new wxHelper(dm.Rows[0]["companyID"].ToString());
                                string data = TemplateMessage.getDJChange(dm.Rows[0]["openID"].ToString(),
                                    OperUtil.getMessageID(dm.Rows[0]["companyID"].ToString(), "OPENTM406811407"),
                                    "尊敬的会员，您的会员等级发生变更", "" + dm.Rows[0]["dj"].ToString() + "", "3",
                                    "如有疑问，请拨打会员服务热线" + array[0]["customerPhone"]);
                                TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dm.Rows[0]["companyID"].ToString()), data);
                            }
                            catch (Exception e)
                            {
                                Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                            }
                        }
                        return d2.Rows[0]["id"].ToString();
                    }
                    else
                    {
                        //分两种情况如果剩余的天数大于系统设置的上限则不变这样保证会员原来的剩余天数不会减少 否则设置成上限
                        if (int.Parse(dm.Rows[0]["hjhydays"].ToString()) < int.Parse(dt1.Rows[0]["totalDay"].ToString()))
                        {
                            string sqla = "update asm_member set hjhyDays=" + dt1.Rows[0]["totalDay"].ToString() + ",dj=3 where minOpenID='" + minOpenID + "'";
                            DbHelperSQL.ExecuteSql(sqla);

                            //发送模板消息
                            if (dm.Rows[0]["dj"].ToString() != "3"&&!string.IsNullOrEmpty(dm.Rows[0]["openID"].ToString()))
                            {
                                try
                                {
                                    string company = Util.getCompany(dm.Rows[0]["companyID"].ToString());
                                    JArray array = (JArray)JsonConvert.DeserializeObject(company);
                                    wxHelper wx = new wxHelper(dm.Rows[0]["companyID"].ToString());
                                    string data = TemplateMessage.getDJChange(dm.Rows[0]["openID"].ToString(),
                                        OperUtil.getMessageID(dm.Rows[0]["companyID"].ToString(), "OPENTM406811407"),
                                        "尊敬的会员，您的会员等级发生变更", "" + dm.Rows[0]["dj"].ToString() + "", "3",
                                        "如有疑问，请拨打会员服务热线" + array[0]["customerPhone"]);
                                    TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dm.Rows[0]["companyID"].ToString()), data);
                                }
                                catch (Exception e)
                                {
                                    Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                                }
                            }
                            return d2.Rows[0]["id"].ToString();
                        }
                    }
                }
            }
            return "0";
           
        }

        /// <summary>
        /// 返回这件产品的价格
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="mechineID"></param>
        /// <returns></returns>
        public static string getNewProductPrice(string productID, string mechineID, string dj)
        {
            string sql = "select * from asm_xstj where mechineID=" + mechineID + " and productID=" + productID + " and startTime<GETDATE() and endTime>GETDATE()";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string _productID = dt.Rows[i]["productID"].ToString();//限时活动产品ID
                    string type = dt.Rows[i]["type"].ToString();
                    string startTime = dt.Rows[i]["startTime"].ToString();
                    string endTime = dt.Rows[i]["endTime"].ToString();
                    string timeSpan = dt.Rows[i]["timeSpan"].ToString();
                    string price0 = dt.Rows[i]["price0"].ToString();
                    string price1 = dt.Rows[i]["price1"].ToString();
                    string price2 = dt.Rows[i]["price2"].ToString();
                    string price3 = dt.Rows[i]["price3"].ToString();
                    if (DateTime.Parse(startTime) < DateTime.Now && DateTime.Parse(endTime) > DateTime.Now)//有效期内
                    {
                        if (type == "1")//周期特价
                        {
                            string time1 = timeSpan.Split('-')[0];
                            string time2 = timeSpan.Split('-')[1];
                            if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + time1 + ":00") < DateTime.Now && DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + time2 + ":00") > DateTime.Now)
                            {
                                return getMoney(dj, price0, price1, price2, price3);
                            }
                        }
                        else if (type == "2")//阶段特价
                        {
                            return getMoney(dj, price0, price1, price2, price3);
                        }
                    }
                    else
                    {
                        return getMoneyBYID(productID, dj);
                    }
                }
                return getMoneyBYID(productID, dj);
            }
            else
            {
                //此时间段没有限时特价
                return getMoneyBYID(productID, dj);
            }
        }
        public static string getCompany(string companyID)
        {
            string result = RedisHelper.GetRedisModel<string>(companyID + "_companyInfo");
            if (string.IsNullOrEmpty(result))
            {
                string sql = "select * from asm_company where id=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                result = OperUtil.DataTableToJsonWithJsonNet(dt);
                RedisHelper.SetRedisModel<string>(companyID + "_companyInfo", result, new TimeSpan(2, 0, 0));
            }
            return result;
        }
        /// <summary>
        /// 获取不参与特价的产品价格
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="dj"></param>
        /// <returns></returns>
        public static string getMoneyBYID(string productID, string dj)
        {
            string sqlp = "select * from asm_product where productID=" + productID;
            DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
            string price0 = dp.Rows[0]["price0"].ToString();
            string price1 = dp.Rows[0]["price1"].ToString();
            string price2 = dp.Rows[0]["price2"].ToString();
            string price3 = dp.Rows[0]["price3"].ToString();
            return getMoney(dj, price0, price1, price2, price3);
        }
        /// <summary>
        /// 根据等级获取需要支付的会员价格
        /// </summary>
        /// <param name="dj"></param>
        /// <param name="price0"></param>
        /// <param name="price1"></param>
        /// <param name="price2"></param>
        /// <param name="price3"></param>
        /// <returns></returns>
        public static string getMoney(string dj, string price0, string price1, string price2, string price3)
        {
            string money = "";
            if (dj == "0")
            {
                money = price0;
            }
            else if (dj == "1")
            {
                if (string.IsNullOrEmpty(price1) || price1 == "0")
                {
                    money = price0;
                }
                else
                {
                    money = price1;
                }

            }
            else if (dj == "2")
            {
                if (string.IsNullOrEmpty(price2) || price2 == "0")
                {
                    money = price0;
                }
                else
                {
                    money = price2;
                }

            }
            else if (dj == "3")
            {
                if (string.IsNullOrEmpty(price3) || price3 == "0")
                {
                    money = price0;
                }
                else
                {
                    money = price3;
                }

            }
            return money;
        }
        /// <summary>
        /// 清空商品redis
        /// </summary>
        /// <param name="companyID"></param>
        public static void ClearRedisProductInfo(string companyID)
        {
            string sql = "select * from asm_mechine where companyID=" + companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string mechineID = dt.Rows[i]["id"].ToString();
                    RedisHelper.SetRedisModel<string>(mechineID + "_productInfo", "", new TimeSpan(0, 0, 0));
                }
            }
        }
        public static void ClearRedisProductInfoByMechineID(string mechineID)
        {
           RedisHelper.SetRedisModel<string>(mechineID + "_productInfo", "", new TimeSpan(0, 0, 0));
        }
        /// <summary>
        /// 判断是否达到限购条件
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public static Boolean xgCount(string productID,string memberID,string mechineID)
        {
            if (string.IsNullOrEmpty(productID)||string.IsNullOrEmpty(memberID))
            {
                return true;
            }

            string sql = "select * from  asm_xstj where mechineID="+mechineID+" and productID="+productID;
            Debuglog("sql=" + sql+ "memberID:"+ memberID, "是否限购.txt");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                string sql1 = "select * from asm_member where id="+memberID;
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                if (d1.Rows.Count<=0)
                {
                    return true;
                }
                string hour = dt.Rows[0]["hours"].ToString();
                string count= dt.Rows[0]["buycount"].ToString();
                string sqlceshi = "SELECT  *   from asm_pay_info where   unionID = '" + d1.Rows[0]["unionID"].ToString() + "'  and productID=" + productID +" order by createtime desc";
                Debuglog("sqlceshi=" + sqlceshi, "是否限购.txt");
                DataTable dceshi = DbHelperSQL.Query(sqlceshi).Tables[0];
                if (dceshi.Rows.Count>0) {
                    Debuglog("dceshi=" + dceshi.Rows[0]["id"].ToString() +";"+ dceshi.Rows[0]["statu"].ToString() + ";" + dceshi.Rows[0]["paytime"].ToString() + ";" + dceshi.Rows[0]["sftj"].ToString() + ";", "是否限购.txt");
                }
                Debuglog("dceshicount=" + dceshi.Rows.Count, "是否限购.txt");
                string sql2 = "SELECT DATEDIFF(hh,orderTime,GETDATE()),* from("
                           + " SELECT (case when paytime>'' then ( SUBSTRING(paytime,0, 5)+'-' + SUBSTRING(paytime, 5, 2) + '-' + SUBSTRING(paytime, 7, 2) + ' ' + SUBSTRING(paytime, 9, 2) + ':' +SUBSTRING(paytime, 11, 2) + ':' + SUBSTRING(paytime, 13, 2)) else GETDATE() end )  as orderTime,*"
                           + "          from asm_pay_info where statu=1 or (statu = 0 and DATEDIFF(MINUTE, createTime, GETDATE()) <= 5) "
                           +"         ) A where DATEDIFF(hh, orderTime, GETDATE()) <"+hour+" AND unionID = '"+d1.Rows[0]["unionID"].ToString()+ "' and sftj=1 and productID="+productID;
                Debuglog("sql2=" + sql2, "是否限购.txt");
                DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                Debuglog("count=" + count+ "d2.Rows.Count"+ d2.Rows.Count, "是否限购.txt");
                
                if (double.Parse(count)<=d2.Rows.Count)
                {
                    return false;
                }
            }
            
            return true;
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
                if (daysdiff >= startSendCount)
                {
                    startSendCount = daysdiff + 1;
                }
                //处理恢复时按照psmode来
                if (daysdiff > 0 && psMode == "2" && ((startSendCount - daysdiff) < 2))
                {
                    startSendCount = startSendCount + 2 - (startSendCount - daysdiff);
                }
                if (daysdiff > 0 && psMode == "3" && ((startSendCount - daysdiff) < 3))
                {
                    startSendCount = startSendCount + 3 - (startSendCount - daysdiff);
                }
                string startTime = DateTime.Now.AddDays(startSendCount).ToString("yyyy-MM-dd");


                Debuglog("totalNum=" + totalNum + ";psMode=" + psMode + ";startTime=" + startTime + ";nowdate=" + DateTime.Now.ToString("yyyy-MM-dd") + ";orderid=" + dtInfo.Rows[0]["id"].ToString(), "恢复订单.txt");
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
        public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            //你想转的格式
            return ts3.TotalMilliseconds.ToString();
        }
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
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
        public static void asm_ld_change(string mechineID, string productID, string ldNO, int changeNum, string type, int status,string operaID)
        {
            string sqlLdInfo = "select * from asm_ldinfo where mechineID=" + mechineID + " and productID=" + productID + " and ldNO='" + ldNO + "'";
            DataTable dt = DbHelperSQL.Query(sqlLdInfo).Tables[0];
            string sqlM = "select * from asm_mechine where id=" + mechineID;
            DataTable DM = DbHelperSQL.Query(sqlM).Tables[0];
            string companyID = DM.Rows[0]["companyID"].ToString();
            if (changeNum==0)
            {
                return;
            }
            if (dt.Rows.Count > 0)
            {

                string afterNum = (int.Parse(dt.Rows[0]["ld_productNum"].ToString()) + status * changeNum).ToString();
                string beforeNum = dt.Rows[0]["ld_productNum"].ToString();

                string insert = "insert into asm_ld_change(companyID,mechineID,productID,changeNum,afterNum,beforeNum,ldNO,chgTime,status,type,operaID) " +
                    "values('" + companyID + "','" + mechineID + "','" + productID + "','" + status * changeNum + "','" + afterNum + "','" + beforeNum + "','" + ldNO + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + status + "','" + type + "','"+ operaID + "')";
                DbHelperSQL.ExecuteSql(insert);
            }
            else
            {

                string insert = "insert into asm_ld_change(companyID,mechineID,productID,changeNum,afterNum,beforeNum,ldNO,chgTime,status,type) " +
                    "values('" + companyID + "','" + mechineID + "','" + productID + "','" + status * changeNum + "','" + changeNum + "','0','" + ldNO + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',1,'" + type + "','"+ operaID + "')";
                DbHelperSQL.ExecuteSql(insert);
            }
        }
    }
}