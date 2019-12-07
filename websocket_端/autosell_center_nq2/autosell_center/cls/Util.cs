
using autosell_center.cls;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using WZHY.Common;

namespace autosell_center.api
{
    public class Util
    {

        
        public static Dictionary<string, int> Read(string name)
        {
            string path = HttpRuntime.AppDomainAppPath.ToString() + "log/video/" + name;
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            String str = "";
            Dictionary<string, int> dit = new Dictionary<string, int>();
            while ((line = sr.ReadLine()) != null)
            {
                if (line != str)
                {
                    try
                    {
                        JObject jo = JObject.Parse(line);
                        string videoID = jo["videoID"].ToString();
                        int times = int.Parse(jo["times"].ToString());
                        int num = 0;
                        if (dit.ContainsKey(videoID))
                        {
                            dit.TryGetValue(videoID, out num);
                            num = num + 1;
                            dit.Remove(videoID);
                            dit.Add(videoID, num);
                        }
                        else
                        {
                            dit.Add(videoID, 1);
                        }
                        str = line;
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            return dit;
        }
        public static async System.Threading.Tasks.Task sendorderAsync(string jiqiid, string text)
        {
            if (webSocket.CONNECT_POOL.ContainsKey(jiqiid))//判断客户端是否在线
            {
                WebSocket destSocket = webSocket.CONNECT_POOL[jiqiid];//目的客户端
                if (destSocket != null && destSocket.State == WebSocketState.Open)
                    await destSocket.SendAsync(webSocket.sendmsg(text), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {

            }
        }
        public static async System.Threading.Tasks.Task sendorderAsync2(string jiqiid, string text)
        {
            operaSocket.log("sendorderAsync2=" + text + ";jiqiid=" + jiqiid, "11.txt");
            if (operaSocket.CONNECT_POOL.ContainsKey(jiqiid))//判断客户端是否在线
            {
                WebSocket destSocket = operaSocket.CONNECT_POOL[jiqiid];//目的客户端
                if (destSocket != null && destSocket.State == WebSocketState.Open)
                    await destSocket.SendAsync(operaSocket.sendmsg(text), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {

            }
        }
        /// <summary>
        /// 处理视频广告次数的统计
        /// </summary>
        /// <param name="mechineID">机器编号</param>
        /// <param name="videoID">广告视频编号</param>
        /// <returns></returns>
        public static async Task playCount(string mechineID, string videoID)

        {

            try
            {

                //{"cmd":"video","mechineID":"51","videoID":"175","times":1,"time":"2019-08-06 00:00:05"}
                //到redis用video+mechineID获取该机器的今日播放数据
                //不存在就创建今日播放数据

                //取出播放记录的视频ID，到redis今日播放数据里面取播放量，不存在，则增加该视频ID到redis记录里，存在则数据加1，更新redis
                //播放数据的日期是一小时前更新播放数据到数据库并且清除该redis


                //Redis里面有数据


                //查到该Video_mechineID的redis的hash集合
                Dictionary<string, string> videoList = RedisHelper.GetAllEntriesFromHash("Video_"+mechineID);
                //如果存在数据进入
                if (videoList.Count > 0)
                {
                    //如果不包含此Video_mechineID数据进入
                    if (!RedisHelper.HashContainsEntry("Video_" + mechineID,  videoID))
                    {
                        //在此Video_mechineID的redis中添加该键值
                        RedisHelper.SetEntryInHash("Video_" + mechineID,  videoID, "1");
                    }
                    //否则就在有此Video_mechineID的在数据加一
                    else
                    {

                        RedisHelper.IncrementValueInHash("Video_" + mechineID,  videoID,1);
                    };
                    //如果创建时间小于此刻时间一小时，更新实际库并删掉redis
                    if (Convert.ToDateTime(RedisHelper.GetValueFromHash("Video_" + mechineID, "createTime")) <= DateTime.Now.AddMinutes(-1))
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (KeyValuePair<string, string> kvp in videoList)
                        {
                            if (kvp.Key != "createTime") {
                                int num = int.Parse(kvp.Value);
                                //更新数据
                                
                                if (kvp.Key ==  videoID)
                                {
                                    num++;
                                }
                                sb.Append("update asm_videoAddMechine set times=times+" + num + " where mechineID='" + mechineID + "' and videoID='" + kvp.Key + "' ;");
                                sb.Append("update asm_video set totalTimes=totalTimes+" + num + " where  id='" + kvp.Key + "' ;");
                            }

                            

                        }
                        log("机器号:" + mechineID+"sql"+ sb.ToString(), "samtype_更新视频播放记录.txt");
                        DbHelperSQL.ExecuteSql(sb.ToString());
                        RedisHelper.Remove(mechineID + "_VideoAddMechine");
                        RedisUtil.getVideoList(mechineID);
                        sb.Clear();
                        //使该redis过期失效
                        RedisHelper.Remove("Video_" + mechineID);


                    }
                    else {

                    }

                }
                //如果不存在数据进入
                else
                    {
                    //创建创建时间以及该视频id的key,value
                    RedisHelper.SetEntryInHash("Video_" + mechineID, videoID,"1");
                    RedisHelper.SetEntryInHash("Video_" + mechineID, "createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }









            }
            catch (Exception e)
            {

                //log("AA7:" + e.ToString(), "samtype_" + mechineID + "_所有指令.txt");
            }



        }
        /// <summary>
        /// 处理下位机长时间没有工作的异常警告
        /// </summary>
        /// <param name="mechineID">机器编号</param>
        /// <param name="samtypeString">下位机传回的json 数据</param>
        /// <returns></returns>
        public static async Task samtype(string mechineID,string samtypeString)

        {

            try
            {
                log("AA1:"+samtypeString, "samtype_" + mechineID + "_所有指令.txt");
                JObject samtype = JObject.Parse(samtypeString);
         

                string Rsamtype = RedisHelper.GetRedisModel<string>(mechineID + "_samtype");
                log("AA2:" + Rsamtype, "samtype_" + mechineID + "_所有指令.txt");
            
                if (!string.IsNullOrEmpty(Rsamtype))
                {
                    log("AASSS:" , "samtype_" + mechineID + "_所有指令.txt");
                    bool bs = false;
                    //Redis里面有数据
                    JObject Redisjson = JObject.Parse(Rsamtype);
                    Redisjson["samtype"] = samtypeString;
                    int t1 = int.Parse(samtype["t1"].ToString()) ;
                    int mech_C1 = int.Parse(Redisjson["mech_C1"].ToString());
                    if (t1 > mech_C1)
                    {
                        bs = true;
                        //长时间没连接上下位机
                        Redisjson["mech_C1"] = t1;

                        string msg = "机器下位机连接发生异常：" + int.Parse(samtype["t1"].ToString()) + "分钟请及时处理";
                        log("AA3:" + msg, "samtype_" + mechineID + "_所有指令.txt");
                        MechineUtil.sendErrorMsg(mechineID, "5", msg, "");//推送错误给管理员



                    }
                    if (mech_C1 > 0 && t1 == 0)
                    {
                        bs = true;
                        Redisjson["mech_C1"] = 0;

                    }
                    int t4 = int.Parse(samtype["t4"].ToString());
                    int t5 = int.Parse(samtype["t5"].ToString()) / 5;
                    int mech_C2 = int.Parse(Redisjson["mech_C2"].ToString());
                    if (t1 == 0&&t4 > 0 && t5 > mech_C2 && mech_C2==0)
                    {

                        bs = true;
                        Redisjson["mech_C2"] = t5;
                        string msg = "出货门异常：" + int.Parse(samtype["t5"].ToString()) + "分钟请及时处理";
                        log("AA4:" + msg, "samtype_" + mechineID + "_所有指令.txt");

                        MechineUtil.sendErrorMsg(mechineID, "6", msg, "");

                    }
                    if (mech_C2 > 0 && t4 == 0)
                    {
                        bs = true;
                        Redisjson["mech_C2"] = 0;

                    }

                    if (bs)
                    {//发生更改 更新 Redis
                        log("AA5:" + Redisjson.ToString(), "samtype_" + mechineID + "_所有指令.txt");
                        RedisHelper.SetRedisModel<string>(mechineID + "_samtype", Redisjson.ToString(), new TimeSpan(12, 0, 0));

                    }



                }
                else
                {//"samtype":"{\"TEXT\":\"Video_的一些状态\",\"t1\":0,\"t2\":4,\"t3\":1,\"t4\":0,\"t5\":0}
                    log("AASBB:", "samtype_" + mechineID + "_所有指令.txt");
                    JObject Redisjson = new JObject();


                    Redisjson.Add("samtype", samtypeString);//
                    Redisjson.Add("mech_C1", 0);//Video_连接不到下位机发送次数
                    Redisjson.Add("mech_C2", 0);//Video_不复位发送短信次数
                    log("AA6:" + Redisjson.ToString(), "samtype_" + mechineID + "_所有指令.txt");
                    RedisHelper.SetRedisModel<string>(mechineID + "_samtype", Redisjson.ToString(), new TimeSpan(12, 0, 0));

                }

            }
            catch (Exception e)
            {

                log("AA7:" + e.ToString(), "samtype_" + mechineID + "_所有指令.txt");
            }
           
           

        }

        /// <summary>
        /// 处理下位机长时间没有工作的异常警告
        /// </summary>
        /// <param name="mechineID">机器编号</param>
        /// <param name="samtypeString">下位机传回的json 数据</param>
        /// <returns></returns>
        public static async Task samtypeNew(string mechineID, string samtypeString, string mechineString)

        {

            try
            {

                
                JObject samtype = JObject.Parse(samtypeString);

                JArray jay = RedisUtil.DeserializeObject(mechineString);
               
                bool bs = false;
                //现在的   
                int lastLXTime = int.Parse(samtype["lastLXTime"].ToString());
                //之前的
                int mech_lastLXTime = int.Parse(jay[0]["lastLXTime"].ToString());
                Util.log("lastLXTime" + lastLXTime + ";" + mech_lastLXTime, mechineID + "heartbeat.txt");
                if (lastLXTime > mech_lastLXTime)
                {
                    bs = true;
                    //长时间没连接上下位机
                    jay[0]["lastLXTime"] = lastLXTime;

                    string msg = "机器下位机连接发生异常：" + int.Parse(samtype["lastLXTime"].ToString()) + "分钟请及时处理";
                    MechineUtil.updateMechineStatus(mechineID, "5", "1", msg);
                    MechineUtil.sendErrorMsg(mechineID, "5", msg, "");//推送错误给管理员



                }
                if (mech_lastLXTime > 0 && lastLXTime == 0)
                {
                    bs = true;
                    jay[0]["lastLXTime"] = 0;
                    string msg = "机器下位机已重新连接！" ;
                    MechineUtil.updateMechineStatus(mechineID, "5", "0", msg);
                }
                int openDoorTime = int.Parse(samtype["openDoorTime"].ToString());
                int mech_openDoorTime = int.Parse(jay[0]["openDoorTime"].ToString());
                Util.log("openDoorTime" + openDoorTime + ";" + mech_openDoorTime, mechineID + "heartbeat.txt");
                if (openDoorTime > mech_openDoorTime&& (openDoorTime/(60*5)>1))
                {
                    bs = true;
                    
                    jay[0]["openDoorTime"] = openDoorTime;

                    string msg = "出货门异常：" + (int)(openDoorTime /60) + "分钟请及时处理";
                    log("AA4:" + msg, "samtype_" + mechineID + "_所有指令.txt");
                   
                    MechineUtil.updateMechineStatus(mechineID, "6", "1", msg);
                    MechineUtil.sendErrorMsg(mechineID, "6", msg, "");



                }
                if (mech_openDoorTime > 0 && openDoorTime == 0)
                {
                    bs = true;
                    jay[0]["openDoorTime"] = 0;
                    string msg = "出货门已关闭！";
                    MechineUtil.updateMechineStatus(mechineID, "6", "0", msg);
                }

              
                if (bs)
                {//发生更改 更新 Redis
                    Util.log("jay.ToString()" + jay.ToString(), mechineID + "heartbeat.txt");
                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", jay.ToString());

                }



               

            }
            catch (Exception e)
            {

                log("AA7:" + e.ToString(), "samtype_" + mechineID + "_所有指令.txt");
            }



        }





        //发送给客户端消息 不在线则存储进来
        public static bool faxiaoxi(string[] applist, StringBuilder sb)
        {
            operaSocket.log("applist="+applist[0]+";sb="+sb.ToString(), "11.txt");
            try
            {
                for (int i = 0; i < applist.Length; i++)
                {

                    if (!operaSocket.msglist.ContainsKey(applist[i]))
                    {
                        List<StringBuilder> sblist = new List<StringBuilder>();
                        sblist.Add(sb);
                        operaSocket.log("不存在追加信息并创建", "11.txt");
                        operaSocket.msglist.Add(applist[i], sblist);
                    }
                    else
                    {
                        List<StringBuilder> sblist = operaSocket.msglist[applist[i]];
                        sblist.Add(sb);
                        operaSocket.log("存在追加信息", "11.txt");
                        operaSocket.msglist[applist[i]] = sblist;
                    }

                    //机器在线
                    //operaSocket.log("是否连接="+ operaSocket.CONNECT_POOL.ContainsKey(applist[i])+ ";applist[i]="+ applist[i], "11.txt");
                    //if (operaSocket.CONNECT_POOL.ContainsKey(applist[i]))
                    //{
                    //    if (operaSocket.msglist.ContainsKey(applist[i]))
                    //    {

                    //        operaSocket.log("机器在线发送", "11.txt");
                    //        List<StringBuilder> mssb = operaSocket.msglist[applist[i]];

                    //        for (int j = 0; j < mssb.Count; j++)
                    //        {

                    //            sendorderAsync2(applist[i], mssb[j].ToString());
                    //            operaSocket.log("发送完成", "11.txt");
                    //        }

                    //        operaSocket.msglist.Remove(applist[i]);

                    //    }
                        
                    //}




                }
                return true;
            }
            catch (Exception)
            {

                return false;
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
        public static String makeChecksum(String dat)
        {
            dat = dat.Replace("0x", "");
            dat = dat.Replace(',', ' ');
            string[] array = dat.Split(' ');
            int sum = 0;
            foreach (string arrayElement in array)
            {
                sum += int.Parse(arrayElement, System.Globalization.NumberStyles.HexNumber);
            }
            return sum.ToString();
        }
        private static String url = "http://nq.bingoseller.com/api/mechineService.asmx";
        public static async System.Threading.Tasks.Task uploadVideoRecord(string recordList)
        {
            try
            {
                Hashtable pars = new Hashtable();
                String Url = "http://nq.bingoseller.com/api/mechineService.asmx";
                pars.Add("recordList", recordList);
                XmlDocument doc = HttpUtil.QuerySoapWebService(Url, "uploadVideoRecordList", pars);
                
            }
            catch (Exception e)
            {
                log("eeee=" + e.Message, "提交播放记录到服务器.txt");
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

        /// <summary>
        /// 时间戳反转为时间，有很多中翻转方法，但是，请不要使用过字符串（string）进行操作，大家都知道字符串会很慢！
        /// </summary>
        /// <param name="TimeStamp">时间戳</param>
        /// <param name="AccurateToMilliseconds">是否精确到毫秒</param>
        /// <returns>返回一个日期时间</returns>
        public static DateTime GetTime(long TimeStamp, bool AccurateToMilliseconds = false)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            if (AccurateToMilliseconds)
            {
                return startTime.AddTicks(TimeStamp * 10000);
            }
            else
            {
                return startTime.AddTicks(TimeStamp * 10000000);
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
        public static void asm_ld_change(string mechineID, string productID, string ldNO, int changeNum, string type, int status)
        {
            string sqlLdInfo = "select * from asm_ldinfo where mechineID=" + mechineID + " and productID=" + productID + " and ldNO='" + ldNO + "'";
            DataTable dt = DbHelperSQL.Query(sqlLdInfo).Tables[0];
            string sqlM = "select * from asm_mechine where id=" + mechineID;
            DataTable DM = DbHelperSQL.Query(sqlM).Tables[0];
            string companyID = DM.Rows[0]["companyID"].ToString();
            if (dt.Rows.Count > 0)
            {

                string afterNum = (int.Parse(dt.Rows[0]["ld_productNum"].ToString()) + status * changeNum).ToString();
                string beforeNum = dt.Rows[0]["ld_productNum"].ToString();

                string insert = "insert into asm_ld_change(companyID,mechineID,productID,changeNum,afterNum,beforeNum,ldNO,chgTime,status,type) " +
                    "values('" + companyID + "','" + mechineID + "','" + productID + "','" + status * changeNum + "','" + afterNum + "','" + beforeNum + "','" + ldNO + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + status + "','" + type + "')";
                DbHelperSQL.ExecuteSql(insert);
            }
            else
            {

                string insert = "insert into asm_ld_change(companyID,mechineID,productID,changeNum,afterNum,beforeNum,ldNO,chgTime,status,type) " +
                    "values('" + companyID + "','" + mechineID + "','" + productID + "','" + status * changeNum + "','" + changeNum + "','0','" + ldNO + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',1,'" + type + "')";
                DbHelperSQL.ExecuteSql(insert);
            }
        }
        //之前用的减料道库存的方法
        public static string update_reduceLDKC(string mechineID, string ldNO)
        {
           
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
        //之前用的减料道库存的方法
        public static string update_addLDKC(string mechineID, string ldNO)
        {

            string sql = "select * from asm_ldinfo where ldNO='" + ldNO + "' and mechineID=" + mechineID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];

            if (dt.Rows.Count > 0 && int.Parse(dt.Rows[0]["ld_productNum"].ToString()) > 0)
            {
                string sql1 = "update asm_ldInfo set ld_productNum=ld_productNum+1,lastUpTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ldNO='" + ldNO + "' and mechineID=" + mechineID;
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
        public static void update_KCDetail(string mechineID, string productID)
        {

            string mechineInfo = RedisUtil.getMechine(mechineID);
            JArray jay = RedisUtil.DeserializeObject(mechineInfo);
            if (jay!=null) {
                string companyID = jay[0]["companyID"].ToString();
                string sql = "select productID,sum(ld_productNum)ld_productNum,sum(csldNum) csldNum,sum(csldNum)-sum(ld_productNum) cha from asm_ldInfo where mechineID=" + mechineID + " and productID='" + productID + "'  group by productID";
                //Util.Debuglog("sql=" + sql, "update_KC.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string orderDetail = "select COUNT(*) num from asm_orderlistDetail where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and mechineID=" + mechineID + " and productID=" + productID + " and zt=4";
                    //Util.Debuglog("orderDetail=" + orderDetail, "update_KC.txt");
                    DataTable dtDetail = DbHelperSQL.Query(orderDetail).Tables[0];
                    string orderDetailTotal = "select COUNT(*) num from asm_orderlistDetail where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and mechineID=" + mechineID + " and zt<7 and productID=" + productID;

                    DataTable dtDetailT = DbHelperSQL.Query(orderDetailTotal).Tables[0];
                    //剩余订购
                    string dgNum = dtDetail.Rows[0]["num"].ToString();
                    //剩余零售=总剩余-剩余订购
                    string lsNum = (int.Parse(dt.Rows[0]["ld_productNum"].ToString()) - int.Parse(dgNum)).ToString();
                    if (int.Parse(lsNum) < 0)
                    {
                        lsNum = "0";
                    }

                    //totalLsNum 计算规则 用该商品所占料道的最大初始料道数量之和 -当日订购的数量=总的零售数量
                    string totalLsNum = (int.Parse(dt.Rows[0]["csldNum"].ToString())).ToString();
                    string cha = (int.Parse(totalLsNum) - int.Parse(lsNum)).ToString();
                    if (int.Parse(cha) < 0)
                    {
                        cha = "0";
                    }
                    string insert = "insert into asm_kcDetail(companyID,mechineID,productID,dateTime,dgNum,lsNum,totalLsNum,imbalance,totalDgNum) "
                       + " values('" + companyID + "','" + mechineID + "','" + productID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + dgNum + "','" + lsNum + "','" + totalLsNum + "','" + cha + "','" + dtDetailT.Rows[0]["num"].ToString() + "')";
                    //Util.Debuglog("insert=" + insert, "update_KC.txt");
                    DbHelperSQL.ExecuteSql(insert);
                    RedisHelper.Remove(mechineID + "_KcProduct");
                }
            }
            
        }
        /// <summary>
        /// 库存变动记录表 会员购买， 配送员加货，备注：改版前的
        /// </summary>
        /// <param name="mechineID"></param>
        /// <param name="companyID"></param>
        /// <param name="productID"></param>
        public static void update_KC(string mechineID, string companyID, string productID)
        {
            string sql = "select productID,sum(ld_productNum)ld_productNum,sum(csldNum) csldNum,sum(csldNum)-sum(ld_productNum) cha from asm_ldInfo where mechineID=" + mechineID + " and productID='" + productID + "'  group by productID";
           // Util.Debuglog("sql=" + sql, "update_KC.txt");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string orderDetail = "select COUNT(*) num from asm_orderlistDetail where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and mechineID=" + mechineID + " and productID=" + productID + " and zt=4";
                //Util.Debuglog("orderDetail=" + orderDetail, "update_KC.txt");
                DataTable dtDetail = DbHelperSQL.Query(orderDetail).Tables[0];
                string orderDetailTotal = "select COUNT(*) num from asm_orderlistDetail where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and mechineID=" + mechineID + " and zt<7 and productID=" + productID;

                DataTable dtDetailT = DbHelperSQL.Query(orderDetailTotal).Tables[0];
                //剩余订购
                string dgNum = dtDetail.Rows[0]["num"].ToString();
                //剩余零售=总剩余-剩余订购
                string lsNum = (int.Parse(dt.Rows[0]["ld_productNum"].ToString()) - int.Parse(dgNum)).ToString();
                if (int.Parse(lsNum) < 0)
                {
                    lsNum = "0";
                }

                //totalLsNum 计算规则 用该商品所占料道的最大初始料道数量之和 -当日订购的数量=总的零售数量
                string totalLsNum = (int.Parse(dt.Rows[0]["csldNum"].ToString())).ToString();
                string cha = (int.Parse(totalLsNum) - int.Parse(lsNum)).ToString();
                if (int.Parse(cha) < 0)
                {
                    cha = "0";
                }
                string insert = "insert into asm_kcDetail(companyID,mechineID,productID,dateTime,dgNum,lsNum,totalLsNum,imbalance,totalDgNum) "
                   + " values('" + companyID + "','" + mechineID + "','" + productID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + dgNum + "','" + lsNum + "','" + totalLsNum + "','" + cha + "','" + dtDetailT.Rows[0]["num"].ToString() + "')";
                //Util.Debuglog("insert=" + insert, "update_KC.txt");
                DbHelperSQL.ExecuteSql(insert);
                RedisHelper.Remove(mechineID + "_KcProduct");
            }
        }
        /// <summary>
        /// 更新会员等级
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="mechineID"></param>
        public static void updateMemberDJ(string memberID, string mechineID)
        {
            try
            {
                string sqlday = "select * from View_member_consumeCount30 where id=" + memberID;
                DataTable dt1 = DbHelperSQL.Query(sqlday).Tables[0];
                string sql = "select * from asm_dj where companyID in (select companyID from asm_mechine where id=" + mechineID + ") and consumeDay<=" + dt1.Rows[0]["num"].ToString() + " order by djID desc";
                DataTable dt2 = DbHelperSQL.Query(sql).Tables[0];
                if (dt2.Rows.Count > 0)
                {

                    string update = "update asm_member set dj=" + dt2.Rows[0]["djId"].ToString() + " where phone!='' and phone is not null and id=" + memberID + " and hjhyDays=0";
                    DbHelperSQL.ExecuteSql(update);
                    string update1 = "update asm_member set dj=0 where phone='' or phone is null";
                    DbHelperSQL.ExecuteSql(update1);
                }
            }
            catch
            {

            }

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
        public static void insertNotice(string memberID, string title, string con)
        {
            string sql = "insert into asm_notice (memberID,title,con,time) values('" + memberID + "','" + title + "','" + con + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
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
        public static void moneyChange(string memberID, string money, string avaMoney, string bz, string type, string skID, string description)
        {
            //string sql1 = "insert into asm_moneyChange(payTime,money,AvaiilabMOney,memberID,type,bz,skID) values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + money + "," + avaMoney + "," + memberID + ","+type+",'"+bz+"','"+skID+"')";
            //DbHelperSQL.ExecuteSql(sql1);

            string sql = "insert into asm_chgMoney(memberID,payTime,type,money,bz,description) values('" + memberID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + type + "','" + money + "','" + bz + "','" + description + "')";
            DbHelperSQL.ExecuteSql(sql);

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
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
    }
}