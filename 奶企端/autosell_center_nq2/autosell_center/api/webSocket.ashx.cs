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
    /// 离线消息
    /// </summary>
    public class MessageInfo
    {
        public MessageInfo(DateTime _MsgTime, ArraySegment<byte> _MsgContent)
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
    public class webSocket : IHttpHandler
    {

        public static Dictionary<string, WebSocket> CONNECT_POOL = new Dictionary<string, WebSocket>();//用户连接池
        //private static Dictionary<string, List<MessageInfo>> MESSAGE_POOL = new Dictionary<string, List<MessageInfo>>();//离线消息池
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            if (context.IsWebSocketRequest)
            {
                log("msg=11" , "websocket.txt");
                context.AcceptWebSocketRequest(ProcessChat);
            }
            log("msg=2", "websocket.txt");
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
                    if (CONNECT_POOL.ContainsKey(ljc)) CONNECT_POOL.Remove(ljc);//删除连接池
                    
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
         
        private ArraySegment<byte> sendmsg(string msg)
        {
            ArraySegment<byte> buffer1 = new ArraySegment<byte>(new byte[2048]);
            buffer1 = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
            return buffer1;
        }
        private async Task ProcessChat(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            string msg = string.Empty;
            string mechineID = context.QueryString["mechineID"].ToString();
            
            StringBuilder sb = new StringBuilder();
            try
            {
                 
                #region 非授权处理
               
                #region 用户添加连接池
                //第一次open时，添加到连接池中
                if (!CONNECT_POOL.ContainsKey(mechineID))
                    CONNECT_POOL.Add(mechineID, socket);//不存在，添加
                else
                    if (socket != CONNECT_POOL[mechineID])//当前对象不一致，更新
                    CONNECT_POOL[mechineID] = socket;
                
                #endregion
               
                #endregion
                
                #region    循环监听
                 
                while (true)
                {
                    ArraySegment<byte> buffer3 = new ArraySegment<byte>(new byte[2048]);
                    WebSocketReceiveResult result2 = await socket.ReceiveAsync(buffer3, CancellationToken.None);
                    try
                    {
                        #region 关闭Socket处理，删除连接池
                        if (socket.State != WebSocketState.Open)//连接关闭
                        {
                            if (CONNECT_POOL.ContainsKey(mechineID)) CONNECT_POOL.Remove(mechineID);//删除连接池
                            
                            break;
                        }
                        #endregion
                         
                        string userMsg = Encoding.UTF8.GetString(buffer3.Array, 0, result2.Count);//发送过来的消息
                        JObject jo = JObject.Parse(userMsg);

                        string cmd = jo["cmd"].ToString();
                        if (cmd== "app.heartbeat")
                        {
                            sb.Append(GetJson("cmd", "app.heartbeat"));
                            sb.Append(GetJson("msg", "心跳"));
                            msg = ReturnJson(context, sb);
                        }
                        log("userMsg="+ userMsg,"websocket.txt");
                        sb.Clear();
                        await socket.SendAsync(sendmsg(msg), WebSocketMessageType.Text, true, CancellationToken.None);
                         
                    }
                    catch
                    {
                         
                        if (CONNECT_POOL.ContainsKey(mechineID))
                            CONNECT_POOL.Remove(mechineID);
                    }
                }
                 
            }
            catch (Exception ex)
            {
                //整体异常处理
             
                if (CONNECT_POOL.ContainsKey(mechineID)) CONNECT_POOL.Remove(mechineID);
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
                StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "log/socketlog/" + (DateTime.Now.ToString("yyyyMMdd") + logname));
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