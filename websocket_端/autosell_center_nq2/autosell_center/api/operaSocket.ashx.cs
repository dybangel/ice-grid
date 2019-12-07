using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.WebSockets;

namespace autosell_center.api
{
    /// <summary>
    /// operaSocket 的摘要说明
    /// </summary>
    public class operaSocket : IHttpHandler
    {

        public static Dictionary<string, List<StringBuilder>> msglist = new Dictionary<string, List<StringBuilder>>();


        public static Dictionary<string, WebSocket> CONNECT_POOL = new Dictionary<string, WebSocket>();//用户连接池
        //private static Dictionary<string, List<MessageInfo>> MESSAGE_POOL = new Dictionary<string, List<MessageInfo>>();//离线消息池
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(ProcessChat);
            }
        }
        /// <summary>
        /// 检查连接状态 如果连接异常 删除连接池
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ljc"></param>
        /// <returns></returns>
        //private bool closedsocket(WebSocket socket, string ljc)
        //{
        //    try
        //    {
        //        if (socket.State != WebSocketState.Open)//连接关闭
        //        {
        //            if (CONNECT_POOL.ContainsKey(ljc))
        //            {
        //                log("ljc=" + ljc + "移除", "连接消息.txt");
        //                CONNECT_POOL.Remove(ljc);//删除连接池
        //            } 

        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }

        //    }
        //    catch
        //    {

        //        return false;
        //    }
            
        //}
        public  static ArraySegment<byte> sendmsg(string msg)
        {
            log(msg,"给客户端发消息.txt");
            ArraySegment<byte> buffer1 = new ArraySegment<byte>(new byte[2048]);
            buffer1 = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
            return buffer1;
        }
        private async Task ProcessChat(AspNetWebSocketContext context)
        {

            WebSocket socket = context.WebSocket;
            string msg = string.Empty;
            string userid = context.QueryString["user"].ToString();
            bool shouquan = true;
            string jiqiID = string.Empty;
            string ljc = string.Empty;
            string url2 = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                #region 非授权处理
                // bool shouquan = util.Yanzhengshanghu(context, out msg,out jiqiID);
                ljc = userid;

                if (!shouquan)
                {
                    msg = "账号密码错误";
                    sb.Append(GetJson("cmd", "app.heartbeat"));
                    sb.Append(GetJson("msg", msg + "loginerror"));
                    msg = ReturnJson(context, sb);
                    ljc = "c" + ljc;
                }
                #region 用户添加连接池
                //第一次open时，添加到连接池中
                if (!CONNECT_POOL.ContainsKey(ljc))
                {
                    log("ljc="+ ljc+"添加", "连接消息.txt");
                    CONNECT_POOL.Add(ljc, socket);//不存在，添加
                }

                else {
                    if (socket != CONNECT_POOL[ljc])//当前对象不一致，更新
                        CONNECT_POOL[ljc] = socket;
                }
                  
                #endregion
                string userMsg = "";
                

                #endregion
                #region    循环监听

                //  util.setOnline(ljc, "1");

                while (shouquan)
                {
                    string gjc = "";
                    try
                    {
                        ArraySegment<byte> buffer3 = new ArraySegment<byte>(new byte[2048]);
                        WebSocketReceiveResult result2 = await socket.ReceiveAsync(buffer3, CancellationToken.None);

                        #region 关闭Socket处理，删除连接池
                        if (socket.State != WebSocketState.Open)//连接关闭
                        {
                            log("ljc=" + ljc + "移除", "连接消息.txt");
                            if (CONNECT_POOL.ContainsKey(ljc)) CONNECT_POOL.Remove(ljc);//删除连接池
                            break;
                        }
                        #endregion
                        userMsg = Encoding.UTF8.GetString(buffer3.Array, 0, result2.Count);//发送过来的消息
                        sb.Clear();
                        JObject jo = JObject.Parse(userMsg);
                        gjc = jo["cmd"].ToString();
                    }
                    catch (Exception e)
                    {
                        log("1异常信息="+e.Message, "连接消息.txt");
                    }
                    sb.Append(GetJson("cmd", "app.heartbeat"));
                    sb.Append(GetJson("msg", "连接正常"));
                    msg = ReturnJson(context, sb);
                    await socket.SendAsync(sendmsg(msg), WebSocketMessageType.Text, true, CancellationToken.None);
                    log("收到信息" + userMsg + "发送" + msg, jiqiID + "收发");


                    //如果有历史未发记录  发送过去
                    if (msglist.ContainsKey(ljc))
                    {
                        List<StringBuilder> mssb = msglist[ljc];

                        for (int i = 0; i < mssb.Count; i++)
                        {
                            await socket.SendAsync(sendmsg(mssb[i].ToString()), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        msglist.Remove(ljc);

                    }
                }
                 
            }
            catch (Exception ex)
            {
                //if (CONNECT_POOL.ContainsKey(ljc)) CONNECT_POOL.Remove(ljc);
                log("2异常信息=" + ex.Message, "连接消息.txt");
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

            logname += "_Debuglog.txt";
            
            try
            {
                StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "operalog/" + (DateTime.Now.ToString("yyyyMMdd") + logname));
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
        public static string GetJson(string JsonName, string JsonValue)
        {
            return "\"" + JsonName + "\":\"" + JsonValue + "\",";
        }
        private string ReturnJson(AspNetWebSocketContext context, StringBuilder sb)
        {
            string json = "{" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "}";

            try
            {

                //string user = context.QueryString["user"].ToString();

                //string jiqiID = context.QueryString["jiqi"].ToString();




                log(" 请求数据 返回json：-----------" + json, "json");
            }
            catch
            {

            }


            return json;
        }
        #endregion
    }
}