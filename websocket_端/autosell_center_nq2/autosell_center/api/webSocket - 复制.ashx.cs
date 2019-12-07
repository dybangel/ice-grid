using autosell_center.cls;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.WebSockets;
//原版
namespace autosell_center.api
{
    /// <summary>
    /// 离线消息
    /// </summary>
    public class MessageInfo1
    {
        public MessageInfo1(DateTime _MsgTime, ArraySegment<byte> _MsgContent)
        {
            MsgTime = _MsgTime;
            MsgContent = _MsgContent;
        }
        public DateTime MsgTime { get; set; }
        public ArraySegment<byte> MsgContent { get; set; }
    }


    /// <summary>
    /// webSocket 的摘要说明
    /// </summary>
    public class webSocket1 : IHttpHandler
    {

        public static Dictionary<string, WebSocket> CONNECT_POOL = new Dictionary<string, WebSocket>();//用户连接池

        public int type = 0;
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            Util.log("开始", "测试socket.txt");
            if (context.IsWebSocketRequest)
            {
                Util.log("context.IsWebSocketRequest", "测试socket.txt");
                context.AcceptWebSocketRequest(ProcessChat);
            }

        }
        /// <summary>
        /// 检查连接状态 如果连接异常 删除连接池
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ljc"></param>
        /// <returns></returns>
        private bool closedsocket(WebSocket socket, string ljc)
        {
            try
            {
                if (socket.State != WebSocketState.Open)//连接关闭
                {
                    if (CONNECT_POOL.ContainsKey(ljc))
                    {

                        CONNECT_POOL.Remove(ljc);//删除连接池
                                                 //  MechineType.Remove(ljc);
                    }

                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch
            {

                return false;
            }
        }

        public static ArraySegment<byte> sendmsg(string msg)
        {
            ArraySegment<byte> buffer1 = new ArraySegment<byte>(new byte[2048]);
            buffer1 = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
            return buffer1;
        }
        private async Task ProcessChat(AspNetWebSocketContext context)
        {
            Util.log("ProcessChat", "测试socket.txt");
            WebSocket socket = context.WebSocket;
            string msg = string.Empty;
            string mechineID = context.QueryString["mechineID"].ToString();

            StringBuilder sb = new StringBuilder();
            try
            {
                //第一次open时，添加到连接池中

                //第一次open时，添加到连接池中
                if (!CONNECT_POOL.ContainsKey(mechineID))
                {
                    Util.log("第一次", "测试socket.txt");
                    log("添加机器mechineID=" + mechineID, "连接信息.txt");
                    MechineUtil.updateMechineStatus(mechineID, "0");
                    CONNECT_POOL.Add(mechineID, socket);//不存在，添加
                }
                else
                {
                    if (socket != CONNECT_POOL[mechineID])//当前对象不一致，更新
                    {
                        CONNECT_POOL[mechineID] = socket;
                    }
                }
                while (true)
                {
                    Util.log("循环", "测试socket.txt");
                    ArraySegment<byte> buffer3 = new ArraySegment<byte>(new byte[2048]);
                    WebSocketReceiveResult result2 = await socket.ReceiveAsync(buffer3, CancellationToken.None);
                    try
                    {
                        Util.log("循环1", "测试socket.txt");
                        sb.Clear();
                        #region 关闭Socket处理，删除连接池
                        if (socket.State != WebSocketState.Open)//连接关闭
                        {
                            if (CONNECT_POOL.ContainsKey(mechineID))
                            {
                                log("移除机器mechineID=" + mechineID, "连接信息.txt");
                                MechineUtil.updateMechineStatus(mechineID, "1");
                                CONNECT_POOL.Remove(mechineID);//删除连接池
                            }
                            break;
                        }
                        Util.log("循环2", "测试socket.txt");
                        if (!CONNECT_POOL.ContainsKey(mechineID))
                        {
                            Util.log("循环2.1", "测试socket.txt");
                            log("发消息添加机器mechineID=" + mechineID, "连接信息.txt");
                            MechineUtil.updateMechineStatus(mechineID, "0");
                            CONNECT_POOL.Add(mechineID, socket);//不存在，添加
                            Util.log("循环2.2", "测试socket.txt");
                        }
                        else
                        {
                            if (socket != CONNECT_POOL[mechineID])//当前对象不一致，更新
                            {
                                Util.log("循环2.3", "测试socket.txt");
                                CONNECT_POOL[mechineID] = socket;
                            }
                        }
                        Util.log("循环3", "测试socket.txt");
                        #endregion
                        string userMsg = Encoding.UTF8.GetString(buffer3.Array, 0, result2.Count);//发送过来的消息
                        Util.log("循环4", "测试socket.txt");
                        
                        if (string.IsNullOrEmpty(userMsg)) return;
                        Util.log(userMsg, "测试socket.txt");
                        JObject jo = JObject.Parse(userMsg);
                        string cmd = jo["cmd"].ToString();
                        string mechine_ID = jo["mechineID"].ToString();
                        Util.log("jo"+jo.ToString(), "测试socket.txt");

                        //新增调试
                        //////////////////////////////////////////////////////////////////////////////////////
                        //eventWait是发送消息后的“等待”集合，客户端发来消息后首先看是否有正在等待此客户端发的此消息的线程在跑，有则取出来
                        //if (BlockService.eventWait.ContainsKey(mechine_ID))
                        //{
                        //    Dictionary<string, EventWaitHandle> callMap = BlockService.eventWait[mechine_ID];
                           

                        //    if (callMap.ContainsKey(jo["msgid"].ToString()))
                        //    {
                        //        //在此处已经取出等待线程
                        //        EventWaitHandle callBack = callMap[jo["msgid"].ToString()];

                        //        Util.log("callBack" + callBack.ToString(), "测试socket.txt");
                        //        //将此次传回来的消息存到集合中
                        //        //首先判断将此次传回来的消息是否已经存在，存在更新掉，不存在创建
                        //        if (BlockService.msgList.ContainsKey(mechine_ID))
                        //        {
                                    
                        //            BlockService.msgList[mechine_ID].Add(jo["msgid"].ToString(), jo["cmd"].ToString());
                                    
                                    
                        //        }
                        //        else
                        //        {

                        //            Dictionary<string, string> msginfo = new Dictionary<string, string>();
                        //            msginfo.Add(jo["msgid"].ToString(), jo["cmd"].ToString());
                        //            BlockService.msgList.Add(mechine_ID, msginfo);

                        //        }
                        //        //放开此消息的等待方法
                        //        callBack.Set();
                        //        //在等待集合中移除此等待
                        //        callMap.Remove(jo["msgid"].ToString());

                        //    }
                        //}



                        ////////////////////////////////////////////////////////////////////////////////////////


                        try
                        {
                            
                            //log("ceshikaishi=" , "_" + mechine_ID + "_心跳.txt");
                            //MsgDTO msgDTO = new MsgDTO();
                            //msgDTO.Createtime = Util.GetTimeStamp();
                            //msgDTO.Statusvalue = jo["samtype"].ToString();
                            //log("ceshikaishi=" + jo["samtype"].ToString(), "_" + mechine_ID + "_心跳.txt");
                            //string msgString = JsonConvert.SerializeObject(msgDTO);
                            //log("ceshikaishi=" + msgString, "_" + mechine_ID + "_心跳.txt");
                            //RedisHelper.SetRedisModel<string>(mechineID + "tem", msgString, new TimeSpan(12, 0, 0));
                            //log("ceshijieshu=", "_" + mechine_ID + "_心跳.txt");
                            //处理机器长时间没有正常工作的异常
                            await Util.samtype(mechine_ID, jo["samtype"].ToString());
                        }
                        catch (Exception e )
                        {
                            log("ceshikaishi=" + e.ToString(), "_" + mechine_ID + "_心跳.txt");
                            throw;
                        }




                        if (cmd != "memory")
                        {
                            log("IP:" + context.UserHostAddress + ";userMsg=" + userMsg, "_" + mechineID + "_所有指令.txt");
                        }
                        if (cmd == "app.heartbeat")
                        {
                           
                            sb.Append(GetJson("cmd", "app.heartbeat"));
                            sb.Append(GetJson("msg", "心跳"));
                            msg = ReturnJson(context, sb);
                            log("userMsg=" + userMsg, "_" + mechine_ID + "_心跳.txt");
                            try
                            {
                                string GKJStatus = jo["GKJStatus"].ToString();
                                string status = RedisHelper.GetRedisModel<string>(mechineID + "_gkjStatus");
                                if (int.Parse(GKJStatus) >= 10)
                                {
                                    if (status != "10")
                                    {
                                        RedisHelper.SetRedisModel<string>(mechineID + "_gkjStatus", "10", new TimeSpan(12, 0, 0));
                                        //机器下位脱机
                                        log("机器下位脱机=脱机", "_" + mechine_ID + "_心跳.txt");
                                    }

                                }
                                else
                                {
                                    if (status != "0")
                                    {
                                        RedisHelper.SetRedisModel<string>(mechineID + "_gkjStatus", "0", new TimeSpan(12, 0, 0));
                                        //机器下位脱机
                                        log("机器下位脱机=重连", "_" + mechine_ID + "_心跳.txt");
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                        if (cmd == "lx")
                        {
                            log("userMsg=" + userMsg, "_" + mechineID + "_指令.txt");
                        }
                        if (cmd == "all")
                        {
                            log("userMsg=" + userMsg, "_" + mechineID + "_app端接收的指令.txt");
                        }
                        if (cmd == "video")
                        {
                            string videoID = jo["videoID"].ToString();
                            //{"cmd":"video","mechineID":"51","videoID":"175","times":1,"time":"2019-08-06 00:00:05"}
                            //到redis用video+mechineID获取该机器的今日播放数据
                            //不存在就创建今日播放数据

                            //取出播放记录的视频ID，到redis今日播放数据里面取播放量，不存在，则增加该视频ID到redis记录里，存在则数据加1，更新redis
                            //播放数据的日期是一小时前更新播放数据到数据库并且清除该redis
                            //用这个类似写法写 await socket.SendAsync(sendmsg(msg), WebSocketMessageType.Text, true, CancellationToken.None);
                            await Util.playCount(mechineID, videoID);
                            logVideo(userMsg, mechineID + "_视频播放记录.txt");
                        }
                        if (cmd == "tem")
                        {
                            try
                            {
                                logVideo(userMsg, mechineID + "_温度记录.txt");
                                string temperature = jo["value"].ToString();
                                if (double.Parse(temperature) > 10)
                                {
                                    MechineUtil.AddTemBrokenRecord(mechineID, temperature);
                                }
                            }
                            catch
                            {

                            }
                        }
                        if (cmd == "log")
                        {
                            log("userMsg=" + userMsg, "_" + mechineID + "_log.txt");
                        }
                        if (cmd == "record")//上传操作日志
                        {
                            log("userMsg=" + userMsg, "_" + mechineID + "record.txt");
                            try
                            {
                                //{"cmd":"record","companyID":"14","mechineID":"25","indexCount":5,"smCount":1,"productCount":2,"endCount":0,"productStr":"283,290","version":"1.0.2"}
                                string companyID = jo["companyID"].ToString();
                                string indexCount = jo["indexCount"].ToString();//首页次数
                                string smCount = jo["smCount"].ToString();//扫码页面次数
                                string productCount = jo["productCount"].ToString();//支付页面次数
                                string endCount = jo["endCount"].ToString();//支付完成
                                string productStr = jo["productStr"].ToString();//选择的产品ID
                                string billno = jo["billno"].ToString();//零售时候传
                                string type = jo["type"].ToString();//1 订购2零售
                                string memberID = jo["memberID"].ToString();//订购时传
                                string memberStr = "";
                                if (type == "2")
                                {
                                    string sql1 = "select acct from  asm_pay_info  where trxid='" + billno + "'";
                                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                                    if (d1.Rows.Count > 0)
                                    {
                                        string sqlp = "select id from asm_member where openID ='" + d1.Rows[0]["acct"].ToString() + "'";
                                        DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                                        if (dp.Rows.Count > 0)
                                        {
                                            memberID = dp.Rows[0]["id"].ToString();
                                        }
                                        else
                                        {
                                            memberStr = d1.Rows[0]["acct"].ToString();
                                        }
                                    }

                                }
                                if (!string.IsNullOrEmpty(productStr))
                                {
                                    string sqlp = "SELECT STUFF((SELECT ','+proName FROM  asm_product  where productID in(" + productStr + ") for xml path('')),1,1,'') proName";
                                    DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                                    if (dp.Rows.Count > 0)
                                    {
                                        productStr = dp.Rows[0]["proName"].ToString();
                                    }
                                }


                                string sql = "insert into asm_mechineRecord(companyID,mechineID,indexCount,smCount,productCount,endCount,productStr,timeStr,memberStr,type,memberID)"
                                    + " values(" + companyID + "," + mechineID + "," + indexCount + "," + smCount + "," + productCount + "," + endCount + ",'" + productStr + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + memberStr + "','" + type + "','" + memberID + "')";
                                log("sql=" + sql, "_" + mechineID + "record.txt");
                                DbHelperSQL.ExecuteSql(sql);
                            }
                            catch
                            {
                            }

                        }
                        if (cmd == "chBillno")
                        {
                            string billno = jo["billno"].ToString();
                            log("billno=" + billno, "_" + mechineID + "LSCH.txt");
                            RedisHelper.SetRedisModel<string>("CH" + billno, null, new TimeSpan(0, 0, 0));
                        }
                        if (cmd == "chforeach")
                        {
                            string content = jo["content"].ToString();
                            log("content=" + content, "_" + mechineID + "chforeach.txt");

                        }
                        if (cmd == "upSellRecordList")
                        {
                            string recordList = jo["recordList"].ToString();
                            log("recordList=" + recordList, "_" + mechineID + "upSellRecordList.txt");

                        }
                        if (cmd == "memory")
                        {
                            log("userMsg=" + userMsg, "_" + mechineID + "_memory.txt");
                        }
                        await socket.SendAsync(sendmsg(msg), WebSocketMessageType.Text, true, CancellationToken.None);
                        //获取机器状态


                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            catch (Exception ex)
            {
                log("2userMsg=" + ex.Message, "_异常.txt");
                //整体异常处理
                if (CONNECT_POOL.ContainsKey(mechineID))
                {
                    log("出现异常移除机器mechineID=" + mechineID, "连接信息.txt");
                    MechineUtil.updateMechineStatus(mechineID, "1");
                    CONNECT_POOL.Remove(mechineID);

                }
            }
        }

        public static T JSONToObject<T>(string jsonText)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

        public bool IsReusable
        {
            get
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
        public static void logVideo(string log, string logname = "_Debuglog.txt")
        {
            if (Directory.Exists(HttpRuntime.AppDomainAppPath.ToString() + "log/video") == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(HttpRuntime.AppDomainAppPath.ToString() + "log/video");
            }
            try
            {
                StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "log/video/" + (DateTime.Now.ToString("yyyyMMdd") + "_" + logname));
                writer.WriteLine(log);
                writer.Close();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception)
            {
            }
        }
        public static string GetJson(string JsonName, string JsonValue)
        {
            return "\"" + JsonName + "\":\"" + JsonValue + "\",";
        }
        private string ReturnJson(AspNetWebSocketContext context, StringBuilder sb)
        {
            string json = "{" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "}";
            return json;
        }

    }
}