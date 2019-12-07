using autosell_center.api;
using autosell_center.cls;
using autosell_center.main.jiqi;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;

namespace autosell_center.api
{
    /// <summary>
    /// api 的摘要说明
    /// </summary>
    public class api : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string text = context.Request["action"];
            switch (text)
            {
                case "ch":   
                    this.ch(context);
                    return;
                //2019-12-02更新
                case "chNew":
                    this.chNew(context);
                    return;
                    
                case "dgch":
                    this.dgch(context);
                    return;
                //2019-12-02更新
                case "dgchNew":
                    this.dgchNew(context);
                    return;

                case "chStatus":
                    this.chStatus(context);
                    return;
                case "Send":
                    this.Send(context);
                    return;
                case "SendLDError":
                    this.SendLDError(context);
                    return;
                case "hell":
                    this.hell(context);
                    return;
                case "ceshisocket":
                    this.ceshisocket(context);
                    return;
                    //此处是添加新机器后将机器添加到机器集合中
                case "addMechineToList":
                    this.addMechineToList(context);
                    return;
                    //用户扫码后返回
                case "codeScanReturn":
                   // this.codeScanReturn(context);
                    return;

            }
        }
        
        public void ceshisocket(HttpContext context)
        {
            Util.log("ceshisocket进入" , "测试socket111111.txt");
            BlockModel msgmodel = new BlockModel();
            try
            {
                context.Response.Write("1111111111111111");
                // string id = context.Request["id"].ToString();
                //string cmd = context.Request["cmd"].ToString();
                //string MsgId = context.Request["MsgId"].ToString();
                //msgmodel.ID = id;
                //msgmodel.MsgId = Util.GetTimeStamp();
                // msgmodel.MsgId = long.Parse(MsgId);
                // msgmodel.cmd = cmd;
                //BlockService blockService = new BlockService(msgmodel);
                // msgmodel = blockService.getMsg();
                //Util.log("结束" , "测试socket111111.txt");
                //context.Response.Write(JsonConvert.SerializeObject(msgmodel));
            }
            catch (Exception e)
            {
                Util.log("cuowu" + e.Message, "测试socket111111.txt");
                msgmodel.Status = 2;
                context.Response.Write(msgmodel);

            }
            


        }
    
        public void hell(HttpContext context)
        {
            //string str = "[{\"payType\":\"1\",\"productID\":\"352\",\"orderTime\":\"2019 - 05 - 20 12:43:03\",\"num\":1,\"totalMoney\":\"5.9\",\"proLD\":\"44\",\"type\":\"2\",\"orderNO\":1558327383210,\"bz\":\"交易成功\",\"code\":\"\",\"billno\":\"111977820000757543\",\"mechineID\":\"55\",\"memberID\":\"\"}]";
            //ht.Add("recordList",str);
            //XmlDocument xx = WebSvcCaller.QuerySoapWebService("http://nq.bingoseller.com/api/mechineService.asmx", "upSellRecord", ht);
            //context.Response.Write(xx.OuterXml);
            
         
            string ldNO = "100";
            string bill = "123456";
            string payType = "1";
            string productID = "352";
            string money = "4.0";
            string type = "2";
            //Thread.Sleep(1000 * 60);
            string sql = "select trxid,m.id from  asm_pay_info a left join asm_member m on a.unionID=m.unionID where  a.statu=1 and trxid='" + bill + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count >=0)
            {
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位 
                Hashtable ht = new Hashtable();
                ht.Add("payType", payType);
                ht.Add("productID", productID);
                ht.Add("orderTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                ht.Add("num", 1);
                ht.Add("totalMoney", money);
                ht.Add("proLD", ldNO);
                ht.Add("type", type);
                ht.Add("orderNO", t);
                ht.Add("bz", "交易失败了");
                ht.Add("code", "");
                ht.Add("billno", bill);
                ht.Add("mechineID", "25");
                ht.Add("memberID", "21212");

                Hashtable ht1 = new Hashtable();
                ht1.Add("recordList","["+Util.HashtableToWxJson(ht)+"]");
                XmlDocument xx = WebSvcCaller.QuerySoapWebService("http://nq.bingoseller.com/api/mechineService.asmx", "upSellRecord", ht1);
                string result = xx.OuterXml;
            }
        }
        public void SendLDError(HttpContext context)
        {
            try
            {
                string ldno = context.Request["ldno"].ToString();
                string mechineID = context.Request["mechineID"].ToString();

                MechineUtil.sendErrorMsg(mechineID,"1","",ldno);
            }
            catch {

            }
        }
        public void Send(HttpContext context)
        {
            if (operaSocket.CONNECT_POOL.ContainsKey("002001"))
            {
                MechineUtil.sendErrorMsg("25", "4", "25","1111");
            }
            
        }
        
        public void addMechineToList(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder sb = new StringBuilder();
            string mechineID = context.Request["mechineID"].ToString();
            JArray jay = RedisUtil.DeserializeObject(RedisUtil.getMechine(mechineID));
            if (jay != null)
            {
                string jiqiName = jay[0]["mechineName"].ToString();
                string status = jay[0]["zt"].ToString();
                JiQi jiqi = new JiQi(mechineID, jiqiName, status);
                JiQiManager.Set(mechineID, jiqi);
            }
        }
        public void chStatus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder sb = new StringBuilder();
            string type = context.Request["type"].ToString();
            string mechineID = context.Request["mechineID"].ToString();
            
            string msg = string.Empty;
            sb.Append(GetJson("cmd", "app.heartbeat"));
            sb.Append(GetJson("msg", "心跳"));
            msg = ReturnJson(null, sb);
            sendorderAsync(mechineID, msg);
            Thread.Sleep(1000);     
            sb.Clear();
            sb.Append(GetJson("cmd", "chStatus"));

            if (!getMechineStatus(mechineID))//判断客户端是否在线
            {
                log("当前机器不在线="+ mechineID, "接收到的指令.txt");
                sb.Append(GetJson("code", "500"));
                sb.Append(GetJson("msg", "当前机器不在线"));
                context.Response.Write("{\"code\":\"500\",\"msg\":\"机器不在线\"}");
                return;
            }
            log("当前机器在线="+ mechineID, "接收到的指令.txt");
            sb.Append(GetJson("code", "200"));
            sb.Append(GetJson("type", type));
            context.Response.Write("{\"code\":\"200\",\"msg\":\"机器在线\"}");
            sendorderAsync(mechineID, ReturnJson(context, sb));
        }
        public void dgch(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder sb = new StringBuilder();
            string ldNO = context.Request["ldNO"].ToString();
            string mechineID = context.Request["mechineID"].ToString();
            string memberID = context.Request["memberID"].ToString();
            string productID = context.Request["productID"].ToString();
            string type = context.Request["type"].ToString();
            string code = context.Request["code"].ToString();
            string money = context.Request["money"].ToString();
            log("ldNO="+ ldNO+ ";mechineID="+ mechineID+ ";memberID="+ memberID+ ";productID="+ productID+ ";type="+ type+";code="+code+ ";money="+ money, "订购出货信息.txt");
            sb.Append(GetJson("cmd", "ch"));
            //11111
          

            //111{}
             
            if (!getMechineStatus(mechineID))//判断客户端是否在线
            {
                log("当前机器不在线", "接收到的指令.txt");
                sb.Append(GetJson("code", "500"));
                sb.Append(GetJson("msg", "当前机器不在线"));
                return;
            }
             
            log("当前机器在线", "订购出货信息.txt");
            sb.Append(GetJson("code", "200"));
            sb.Append(GetJson("ldNO", ldNO));
            sb.Append(GetJson("memberID", memberID));
            sb.Append(GetJson("type", type));
            sb.Append(GetJson("productID", productID));
            sb.Append(GetJson("mechineID", mechineID));
            sb.Append(GetJson("code", code));
            sb.Append(GetJson("money", money));
            log("sb="+ sb, "订购出货信息.txt");
            sendorderAsync(mechineID, ReturnJson(context, sb));
        }
       
        public void ch(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder sb = new StringBuilder();
            string ldNO = context.Request["ldNO"].ToString();//料道
            string mechineID = context.Request["mechineID"].ToString();//机器号
            string billno = context.Request["billno"].ToString();//交易号
            string payType = context.Request["payType"].ToString();
            string productID = context.Request["productID"].ToString();//产品编号
            string money = context.Request["money"].ToString();//支付金额
            string type = context.Request["type"].ToString();
            log("ldNO=" + ldNO + ";mechineID=" + mechineID + ";billno=" + billno + ";payType="+ payType+ ";productID="+ productID+ ";money="+ money, "接收到的指令.txt");
            sb.Append(GetJson("cmd", "ch"));

            sb.Append(GetJson("code", "200"));
            sb.Append(GetJson("ldNO", ldNO));
            sb.Append(GetJson("billno", billno));
            sb.Append(GetJson("payType", payType));
            sb.Append(GetJson("productID", productID));
            sb.Append(GetJson("money", money));
            sb.Append(GetJson("type", type));
            RedisHelper.SetRedisModel<string>("CH" + billno, ReturnJson(context, sb), new TimeSpan(0, 10, 0));  //出货指令缓存
            if (!webSocket.CONNECT_POOL.ContainsKey(mechineID))//判断客户端是否在线
            {
                log("当前机器不在线", "接收到的指令.txt");
                LSCH("CH" + billno, mechineID);
                sb.Clear();
                sb.Append(GetJson("code", "500"));
                sb.Append(GetJson("msg", "当前机器不在线"));
                context.Response.Write("{\"code\":\"500\",\"msg\":\"机器不在线\"}");
                return;
            }
            log("当前机器在线", "接收到的指令.txt");
           
            sendorderAsync(mechineID, ReturnJson(context,sb));

            context.Response.Write("{\"code\":\"200\",\"msg\":\"发送成功\"}");
           // if (mechineID=="25"||mechineID=="50"||mechineID=="46"||mechineID=="47"||mechineID=="48")
            {
                LSCH("CH" + billno, mechineID);
            }
             
            return;
        }
        //调整出货指令，等待返回值的
        public void chNew(HttpContext context)
        {
          

            //库存变动记录
                                                                                                              
            //sendMessageToAndroidWaitReturn
                                                                                                              
            //11ldNO=24;mechineID=68;billno=157412726502334876;payType=3;productID=424;money=0.01   type=2

            context.Response.ContentType = "application/json";
            StringBuilder sb = new StringBuilder();
           
            string ldNO = context.Request["ldNO"].ToString();//料道
            string mechineID = context.Request["mechineID"].ToString();//机器号
            string billno = context.Request["billno"].ToString();//交易号
            string payType = context.Request["payType"].ToString();
            string productID = context.Request["productID"].ToString();//产品编号
            string money = context.Request["money"].ToString();//支付金额
            string type = context.Request["type"].ToString();
            log("ldNO=" + ldNO + ";mechineID=" + mechineID + ";billno=" + billno + ";payType=" + payType + ";productID=" + productID + ";money=" + money, "接收到的指令.txt");
            //此处出货之前需要
            //1、料道变动记录（减）
            //2、预减料道库存
            //3、预减库存detail
            Util.asm_ld_change(mechineID, productID, ldNO, 1, "2", -1);//库存变动记录
                                                                       //此处料道库存增加
            Util.update_reduceLDKC(mechineID, ldNO);
            Util.update_KCDetail(mechineID, productID);
            

            sb.Append(GetJson("cmd", "ch"));

            sb.Append(GetJson("code", "200"));
            sb.Append(GetJson("ldNO", ldNO));
            sb.Append(GetJson("billno", billno));
            sb.Append(GetJson("payType", payType));
            sb.Append(GetJson("productID", productID));
            sb.Append(GetJson("money", money));
            sb.Append(GetJson("type", type));
            StringBuilder sbCH = new StringBuilder();
            sbCH = sb;
            sbCH.Append(GetJson("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            sbCH.Append(GetJson("CHStatus", ""));
            log("mechineIDCHString", "接收到的指令.txt");
            string mechineIDCHString = RedisUtil.getMechineChString(mechineID);  //出货指令缓存
            JObject mechineIDCHObj = JObject.Parse(mechineIDCHString);
            mechineIDCHObj["CH" + billno] = ReturnJson(context, sbCH);
            RedisHelper.SetRedisModel<string>("CH" + mechineID, mechineIDCHObj.ToString());  //出货指令缓存
            log("mechineIDCHString=" + mechineIDCHString, "接收到的指令.txt");
            string _mechineInfo = RedisUtil.getMechine(mechineID);
            JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);

            if (_mechineJArray != null)
            {
                log("_mechineJArray != null", "测试chnew.txt");
                string status = _mechineJArray[0]["zt"].ToString();
                string netStatus = _mechineJArray[0]["netStatus"].ToString();
                log("status=" + status + "netStatus=" + netStatus + "is=" + (webSocket.CONNECT_POOL.ContainsKey(mechineID)?1:0), "测试chnew.txt");
                if (status == "2" && netStatus == "0"&& webSocket.CONNECT_POOL.ContainsKey(mechineID))
                {
                    log("进入", "测试chnew.txt");
                   
                    BlockModel msgmodel = new BlockModel();
                    msgmodel.ID = mechineID;
                    msgmodel.MsgId = Util.GetTimeStamp();
                    msgmodel.cmd = "ch";
                    msgmodel.SendMsg = ReturnJson(context, sb);
                    log("开始循环", "测试chnew.txt");
                    for (int i = 0; i < 3; i++)
                    {
                        
                        msgmodel =JiQi.sendMessageToAndroidWaitReturn(msgmodel);
                        log("i=" + i + ";msgmodel.status=" + msgmodel.Status + ";msgmodel.samtype=" + msgmodel.samtype, "测试chnew.txt");
                        if (msgmodel.Status==0) {
                            context.Response.Write("{\"code\":\"200\",\"msg\":\"发送成功\"}");
                            return;
                        }
                    }
                }
            }
            context.Response.Write("{\"code\":\"500\",\"msg\":\"机器不在线\"}");
            return;
        }
        public void dgchNew(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder sb = new StringBuilder();
            string ldNO = context.Request["ldNO"].ToString();
            string mechineID = context.Request["mechineID"].ToString();
            string memberID = context.Request["memberID"].ToString();
            string productID = context.Request["productID"].ToString();
            string type = context.Request["type"].ToString();
            string code = context.Request["code"].ToString();
            string money = context.Request["money"].ToString();
            log("ldNO=" + ldNO + ";mechineID=" + mechineID + ";memberID=" + memberID + ";productID=" + productID + ";type=" + type + ";code=" + code + ";money=" + money, "订购出货信息.txt");

            Util.asm_ld_change(mechineID, productID, ldNO, 1, "2", -1);//库存变动记录
                                                                       //此处料道库存增加
            Util.update_reduceLDKC(mechineID, ldNO);
            Util.update_KCDetail(mechineID, productID);

            sb.Append(GetJson("cmd", "ch"));
            //11111


            //111{}

           
          
            sb.Append(GetJson("ldNO", ldNO));
            sb.Append(GetJson("memberID", memberID));
            sb.Append(GetJson("type", type));
            sb.Append(GetJson("productID", productID));
            sb.Append(GetJson("mechineID", mechineID));
            sb.Append(GetJson("code", code));
            sb.Append(GetJson("money", money));
            log("sb=" + sb, "订购出货信息.txt");
            StringBuilder sbCH = new StringBuilder();
            sbCH = sb;
            sbCH.Append(GetJson("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            sbCH.Append(GetJson("CHStatus", ""));
            log("mechineIDCHString", "接收到的指令.txt");
            string mechineIDCHString = RedisUtil.getMechineChString(mechineID);  //出货指令缓存
            JObject mechineIDCHObj = JObject.Parse(mechineIDCHString);
            mechineIDCHObj["CH" + memberID+ code] = ReturnJson(context, sbCH);
            RedisHelper.SetRedisModel<string>("CH" + mechineID, mechineIDCHObj.ToString());  //出货指令缓存
            log("mechineIDCHString=" + mechineIDCHString, "接收到的指令.txt");
            string _mechineInfo = RedisUtil.getMechine(mechineID);
            JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);

            if (_mechineJArray != null)
            {
                log("_mechineJArray != null", "测试chnew.txt");
                string status = _mechineJArray[0]["zt"].ToString();
                string netStatus = _mechineJArray[0]["netStatus"].ToString();
                log("status=" + status + "netStatus=" + netStatus + "is=" + (webSocket.CONNECT_POOL.ContainsKey(mechineID) ? 1 : 0), "测试chnew.txt");
                if (status == "2" && netStatus == "0" && webSocket.CONNECT_POOL.ContainsKey(mechineID))
                {
                    log("进入", "测试chnew.txt");

                    BlockModel msgmodel = new BlockModel();
                    msgmodel.ID = mechineID;
                    msgmodel.MsgId = Util.GetTimeStamp();
                    msgmodel.cmd = "ch";
                    msgmodel.SendMsg = ReturnJson(context, sb);
                    msgmodel = JiQi.sendMessageToAndroidWaitReturn(msgmodel);

                    if (msgmodel.Status == 0)
                    {
                        context.Response.Write("{\"code\":\"200\",\"msg\":\"发送成功\"}");
                        return;
                    }
                    else {
                        context.Response.Write("{\"code\":\"500\",\"msg\":\"失败\"}");
                    }
                   
                }
            }
            context.Response.Write("{\"code\":\"500\",\"msg\":\"机器不在线\"}");
            return;
        }
        //发指令到指定设备
        public static ArraySegment<byte> sendmsg(string msg)
        {
            ArraySegment<byte> buffer1 = new ArraySegment<byte>(new byte[2048]);
            buffer1 = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
            return buffer1;
        }

        public Boolean getMechineStatus(string jiqiid)
        {
          
            if (webSocket.CONNECT_POOL.ContainsKey(jiqiid))//判断客户端是否在线
            {
               
                WebSocket destSocket = webSocket.CONNECT_POOL[jiqiid];//目的客户端   
                string status = RedisHelper.GetRedisModel<string>(jiqiid + "_gkjStatus");
                if (status=="10")
                {
                    log("下位机脱机=" + jiqiid, "连接信息.txt");
                    return false;
                }
                if (destSocket != null && destSocket.State == WebSocketState.Open)
                {
                    return true;
                }
                else//连接关闭
                {
                    log("出现异常移除机器mechineID=" + jiqiid, "连接信息.txt");
                    MechineUtil.updateMechineStatus(jiqiid, "1");
                    webSocket.CONNECT_POOL.Remove(jiqiid);
                    return false;

                }
            }
            return false;
        }
 
        public static async System.Threading.Tasks.Task sendorderAsync(string jiqiid, string text)
        {
            log("mechineID=" + jiqiid + ";内容="+text, "给客户端发消息.txt");
            if (webSocket.CONNECT_POOL.ContainsKey(jiqiid))//判断客户端是否在线
            {
                WebSocket destSocket = webSocket.CONNECT_POOL[jiqiid];//目的客户端   


                if (destSocket != null && destSocket.State == WebSocketState.Open)
                {
                    await destSocket.SendAsync(sendmsg(text), WebSocketMessageType.Text, true, CancellationToken.None);
                }else//连接关闭
                {
                    log("出现异常移除机器mechineID=" + jiqiid, "连接信息.txt");
                    MechineUtil.updateMechineStatus(jiqiid, "1");
                    webSocket.CONNECT_POOL.Remove(jiqiid);
                }
            }
            else
            {
                
            }
 
        }
        /// <summary>
        /// 零售出货
        /// </summary>
        /// <param name="jiqiid"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task LSCH(string chBillno,string mechineID)
        {
            log("chBillno=" + chBillno+ ";mechineID"+ mechineID, "LSCH.txt");

            string billno = RedisHelper.GetRedisModel<string>(chBillno);
            int i = 0;
            while (!string.IsNullOrEmpty(billno))
            {
                log(i+"=chBillno=" + chBillno, "LSCH.txt");
                if (i==5)
                {
                    log("执行重发出货指令", "LSCH.txt");
                    sendorderAsync(mechineID, billno);
                }
                if (i==50)
                {
                    try
                    {
                        JObject jo = JObject.Parse(billno);
                        string ldNO = jo["ldNO"].ToString();
                        string bill = jo["billno"].ToString();
                        string payType = jo["payType"].ToString();
                        string productID = jo["productID"].ToString();
                        string money = jo["money"].ToString();
                        string type = jo["type"].ToString();
                        Thread.Sleep(1000 * 60);
                        string sql = "select trxid,m.id from  asm_pay_info a left join asm_member m on a.unionID=m.unionID where  a.statu=1 and trxid='" + bill + "'";
                        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                        if (dt.Rows.Count>0)
                        {
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                            long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位 
                            Hashtable ht = new Hashtable();
                            ht.Add("payType", payType);
                            ht.Add("productID", productID);
                            ht.Add("orderTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            ht.Add("num", 1);
                            ht.Add("totalMoney", money);
                            ht.Add("proLD", ldNO);
                            ht.Add("type", type);
                            ht.Add("orderNO", t);
                            ht.Add("bz", "出货失败");
                            ht.Add("code", "");
                            ht.Add("billno", bill);
                            ht.Add("mechineID", mechineID);
                            ht.Add("memberID", dt.Rows[0]["id"].ToString());
                            Hashtable ht1 = new Hashtable();
                            ht1.Add("recordList", "[" + Util.HashtableToWxJson(ht) + "]");
                            XmlDocument xx = WebSvcCaller.QuerySoapWebService("http://nq.bingoseller.com/api/mechineService.asmx", "upSellRecord", ht1);
                            string result = xx.OuterXml;
                            log("bill:"+ bill + ";result=" + result+";HT:"+ ht.ToString(), "LSCH.txt");
                            return;
                        }
                       
                    }
                    catch (Exception e){
                        log("执行退款e=" + e.Message, "LSCH.txt");
                    }
                    log("执行退款"+ billno, "LSCH.txt");
                    return;
                }
                Thread.Sleep(500);
                billno = RedisHelper.GetRedisModel<string>(chBillno);
               
                i++;
            }
           
        }
        public static string GetJson(string JsonName, string JsonValue)
        {
            return "\"" + JsonName + "\":\"" + JsonValue + "\",";
        }
        private static string ReturnJson(HttpContext context, StringBuilder sb)
        {
            string json = "{" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "}";

            return json;

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
            if (Directory.Exists(HttpRuntime.AppDomainAppPath.ToString() + "log/socket/" + DateTime.Now.ToString("yyyyMMdd")) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(HttpRuntime.AppDomainAppPath.ToString() + "log/socket/" + DateTime.Now.ToString("yyyyMMdd"));
            }
            try
            {
                StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "log/socket/" + DateTime.Now.ToString("yyyyMMdd") + "/" + (DateTime.Now.ToString("HH") + logname));
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
    }
}