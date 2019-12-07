
using Consumer.cls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Timers;
using System.Web;
using DBUtility;
using System.Threading;
using autosell_center.api;
using autosell_center.cls;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Net.WebSockets;
using System.Globalization;
using System.Collections;

namespace autosell_center.main.jiqi
{
    public class JiQi
    {
        private String name;
        private String id;
        private String status = "3";

        private String netStatus = "3";
        private long lastTime = 0;
        
        //心跳使用5秒一次，安卓判断socket断开是10秒
        private System.Timers.Timer myTimer = new System.Timers.Timer(1000 * 5);


        private System.Timers.Timer minTimer_1 = new System.Timers.Timer(1000 * 60 * 1);

        //：目前写定时查询大屏产品信息是否有变化的
        private System.Timers.Timer minTimer_2 = new System.Timers.Timer(1000 * 60 * 2);
        //ch超时没有回值信息的，定时处理
        private System.Timers.Timer minTimer_5 = new System.Timers.Timer(1000 * 60 * 5);
        //定时更新产品类别
        private System.Timers.Timer minTimer_20 = new System.Timers.Timer(1000 * 60 * 20);
        //30分钟执行一次的

        private System.Timers.Timer minTimer_30 = new System.Timers.Timer(1000 * 60 * 30);

        
       
        public static int minTime = 20;
        public static int maxTime = 120;
        public static readonly string reqUrl = "http://websocket.cs.suqiangkeji.com:88";
        //public static readonly string reqUrl = "http://192.168.2.143:37465";


        public JiQi()
        {

        }

        public JiQi(string id, string name, string status)
        {
            this.id = id;
            this.status = status;
            this.name = name;
            this.dingshi();
        }
      
        //发送指令并等待返回值
        public static BlockModel sendMessageToAndroidWaitReturn( BlockModel msgmodel )
        {
           

            try
            {
                BlockService blockService = new BlockService(msgmodel);
                msgmodel = blockService.getMsg();
             
                return msgmodel;
            }
            catch (Exception e)
            {
               
                msgmodel.Status = 2;
                return msgmodel;

            }
        }
        //发指令到指定设备
        public static ArraySegment<byte> sendmsg(string msg)
        {
            ArraySegment<byte> buffer1 = new ArraySegment<byte>(new byte[2048]);
            buffer1 = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
            return buffer1;
        }
        //发送不需要等待返回值的命令
        public static void sendMessageToAndroid(BlockModel msgmodel)
        {
            if (webSocket.CONNECT_POOL.ContainsKey(msgmodel.ID))//判断客户端是否在线
            {
                WebSocket destSocket = webSocket.CONNECT_POOL[msgmodel.ID];//目的客户端   


                if (destSocket != null && destSocket.State == WebSocketState.Open)
                {
                    destSocket.SendAsync(sendmsg(JsonConvert.SerializeObject(msgmodel)), WebSocketMessageType.Text, true, CancellationToken.None);

                }
                else//连接关闭
                {
                    MechineUtil.updateMechineStatus(msgmodel.ID, "1");
                    webSocket.CONNECT_POOL.Remove(msgmodel.ID);
                }
            }
            else
            {

            }
            
        }
        private void dingshi()
        {


            myTimer.Elapsed += new ElapsedEventHandler(myTimer_ElapsedT);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;
           
          
            minTimer_1.Elapsed += new ElapsedEventHandler(minTimer_1_ElapsedT);
            minTimer_1.Enabled = true;
            minTimer_1.AutoReset = true;
            minTimer_2.Elapsed += new ElapsedEventHandler(minTimer_2_ElapsedT);
            minTimer_2.Enabled = true;
            minTimer_2.AutoReset = true;
            minTimer_5.Elapsed += new ElapsedEventHandler(minTimer_5_ElapsedT);
            minTimer_5.Enabled = true;
            minTimer_5.AutoReset = true;
            minTimer_20.Elapsed += new ElapsedEventHandler(minTimer_20_ElapsedT);
            minTimer_20.Enabled = true;
            minTimer_20.AutoReset = true;
            minTimer_30.Elapsed += new ElapsedEventHandler(minTimer_30_ElapsedT);
            minTimer_30.Enabled = true;
            minTimer_30.AutoReset = true;


        }
        public string execute(string sendMsg)
        {
            Util.log("执行execute", id + "心跳流程.txt");
            BlockModel msgmodel = new BlockModel();
            msgmodel.ID = id;
            msgmodel.MsgId = Util.GetTimeStamp();
            msgmodel.cmd = "heartbeat";
            msgmodel.SendMsg = "";
            sendMessageToAndroid(msgmodel);
            
            return "";
        }

        private void myTimer_ElapsedT(object source, ElapsedEventArgs e)
        {
            //首先 知道当前机器是否禁用
            TimerUpdateStatus();
            //如果是2 正常状态 
            if (status == "2")
            {
                
                //心跳监测
                jiQiOutTime();
                //获取该机器最新产品配置信息    产品规则,产品登记信息(库存,料道),   计算获得 产品展示信息    俩个redis计算  值和其中一个里面比对 不同就替换

            }
        }
        private void minTimer_1_ElapsedT(object source, ElapsedEventArgs e)
        {
            if (DateTime.Now.ToString("HH:mm") == "00:02")//删除一些历史记录
            {
                
                //更新广告期限状态
                string sql = "update asm_videoAddMechine set zt=1 where ((tfType=1 and times>=tfcs) or (tfType=2 and GETDATE()>=valiDate)) and mechineID='" +id+"' ";
                Util.log("asm_videoAddMechine:sql=" + sql, id+"taskday.txt");
                DbHelperSQL.ExecuteSql(sql);
                RedisHelper.Remove(id+ "_VideoAddMechine");
                
            }
            if (DateTime.Now.ToString("HH:mm") == "00:03")//清空订购料道信息
            {
                string sql = "update asm_ldInfo set ld_productNum=0 ,productID='' where type=1 and mechineID='" + id + "' ";
                Util.log("asm_ldInfo:sql=" + sql, id + "taskday.txt");
                DbHelperSQL.ExecuteSql(sql);
                RedisHelper.Remove(id + "_LDList");
            }
            if (DateTime.Now.ToString("HH:mm") == "00:10")//给安卓发命令让安卓打包日志
            {
               
                BlockModel msgmodel = new BlockModel();
                msgmodel.ID = id;
                msgmodel.MsgId = Util.GetTimeStamp();
                msgmodel.cmd = "pack";
                msgmodel.SendMsg = "{\"packTime\":\"" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "\"}";
                Util.log("安卓日志打包："+ msgmodel.SendMsg, id + "taskday.txt");
                sendMessageToAndroid(msgmodel);
            }
            
            MechineUtil.readZTMechine2(id);
            
        }
      
        private void minTimer_5_ElapsedT(object source, ElapsedEventArgs e)
        {
            //updateSoft();
            outTimeChFail();
            updategetVideoList(id);
        }
        
      
        private void minTimer_2_ElapsedT(object source, ElapsedEventArgs e)
        {
            updateSoft();
            updateProductView(id);
        }
        private void minTimer_20_ElapsedT(object source, ElapsedEventArgs e)
        {
            updateProductType(id);
        }

        //30分钟查询是否有温度异常没发短信的，有则发短信
        private void minTimer_30_ElapsedT(object source, ElapsedEventArgs e)
        {
            //发送温度异常短信 //
            string sqlT3 = "select * from asm_mechine where sendT=1 and lastReqTime is not null and  id ="+id;
            DataTable dt3 = DbHelperSQL.Query(sqlT3).Tables[0];
            if (dt3.Rows.Count > 0)
            {
                
                try
                {
                    string sql21 = "select linkphone from asm_opera where id='" + dt3.Rows[0]["operaID"].ToString() + "'";
                    string sql22 = "select linkphone from asm_company where id='" + dt3.Rows[0]["companyID"].ToString() + "'";
                    DataTable d21 = DbHelperSQL.Query(sql21).Tables[0];
                    DataTable d22 = DbHelperSQL.Query(sql22).Tables[0];
                    if (d21.Rows.Count > 0 && d21.Rows[0]["linkphone"].ToString() != "")
                    {
                        OperUtil.sendMessage3(d21.Rows[0]["linkphone"].ToString(), dt3.Rows[0]["mechineName"].ToString(), dt3.Rows[0]["lastReqTime"].ToString().Substring(11, 5), dt3.Rows[0]["temperture"].ToString());
                    }
                    if (d22.Rows.Count > 0 && d22.Rows[0]["linkphone"].ToString() != "")
                    {
                        OperUtil.sendMessage3(d22.Rows[0]["linkphone"].ToString(), dt3.Rows[0]["mechineName"].ToString(), dt3.Rows[0]["lastReqTime"].ToString().Substring(11, 5), dt3.Rows[0]["temperture"].ToString());
                    }
                    string sqlupdate = "update asm_mechine set sendT=0 where id=" + dt3.Rows[0]["id"].ToString();
                    DbHelperSQL.ExecuteSql(sqlupdate);
                }
                catch (Exception ex)
                {
                    OperUtil.Debuglog("cuowu=" + ex.Message, "短信_.txt");
                }
               
            }


        }
        //首先 知道当前机器是否禁用
        private void TimerUpdateStatus()
        {
            string _mechineInfo = RedisUtil.getMechine(id);
            JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);

            if (_mechineJArray != null)
            {
                //判断机器是否过期

                if (_mechineJArray[0]["zt"].ToString()=="2" && DateTime.Now>= Convert.ToDateTime(_mechineJArray[0]["validateTime"].ToString())) {
                    _mechineJArray[0]["zt"] = "3";
                    string sql5 = "update asm_mechine set zt=3 where id='"+ id + "'";
                    DbHelperSQL.ExecuteSql(sql5);
                    RedisHelper.SetRedisModel<string>(id + "_mechineInfoSet", _mechineJArray.ToString());
                }
               
                string status = _mechineJArray[0]["zt"].ToString();
                string netStatus = _mechineJArray[0]["netStatus"].ToString();
                this.status = status;
                this.netStatus = netStatus;
            }
        }
     

        
        #region 心跳监测
        private void jiQiOutTime()
        {
            int time = getJiQiOut();


            if (netStatus == "0")//正常  已连接
            {
                execute("心跳");
                
                //时间超时 或者 -1 未连接
                if (time >= minTime || time < 0)
                {
                   
                    //execute("心跳");
                }
                if (time > maxTime || time < 0)
                {
                    jiQiOutTime(time);
                }
                string _mechineInfo = RedisUtil.getMechine(id);
                JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);

                if (_mechineJArray != null)
                {
                    if (0 < time && time < maxTime && _mechineJArray[0]["sendF"].ToString() == "1")
                    {
                        Util.log("回复连接", id + "心跳流程.txt");
                        //机器已恢复的短信
                        sendNetOKStatusMsg(time.ToString());
                        _mechineJArray[0]["sendF"] = "0";
                        RedisHelper.SetRedisModel<string>(id + "_mechineInfoSet", _mechineJArray.ToString());
                    }
                }
                    
            }
            else
            {
                Util.log("连接超时", id + "心跳流程.txt");
                jiQiOutTime(time);
            }
            Util.log(id + "time:{" + time + "}", id + "jiQiOutTime.txt");
        }
        private void jiQiOutTime(long time)
        {
            string _mechineInfo = RedisUtil.getMechine(id);
            JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);

            if (_mechineJArray != null)
            {

                if (_mechineJArray[0]["sendF"].ToString() == "0")
                {
                    //机器宕机 没有连websocket
                    sendNetNotStatusMsg(time.ToString());
                    _mechineJArray[0]["sendF"] = "1";
                    RedisHelper.SetRedisModel<string>(id + "_mechineInfoSet", _mechineJArray.ToString());
                }
                else if (time % 120 < 9)
                {
                    //机器已超出多长时间  

                }
            }
        }

        //从redis中获取 最近通信时间
        private int getJiQiOut()
        {

            string _mechineInfo = RedisUtil.getMechine(id);
            JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);

            if (_mechineJArray != null)
            {
                string lastReqTime = _mechineJArray[0]["lastReqTime"].ToString();

                DateTime lastTime = Convert.ToDateTime(lastReqTime);
                DateTime now = DateTime.Now;
                TimeSpan ts = now - lastTime;

                return ts.Seconds;

               
            }
            return -1;
        }
        //根据超时 时间 发送短信给 操作员和公司负责人
        private void sendNetNotStatusMsg(string time)
        {
            MechineUtil.updateMechineStatus(id, "1");
            DataTable da = getJiQiDetail();
            if (da != null && da.Rows.Count > 0)
            {
                string operaID = da.Rows[0]["dls"].ToString();
                string companyID = da.Rows[0]["companyID"].ToString();

                string companyPhone = getCompanyPhone(companyID);
                string operaPhone = getOperaPhone(operaID);
                if (!string.IsNullOrEmpty(companyPhone))
                {
                    Util.log(id + "time:{" + time + "},发送超时短信", id + "jiQiOutTime.txt");
                    OperUtil.sendMessage1(companyPhone, name, ((int)(int.Parse(time)/60)).ToString());
                }
                if (!string.IsNullOrEmpty(operaPhone))
                {
                    Util.log(id + "time:{" + time + "},发送超时短信", id + "jiQiOutTime.txt");
                    OperUtil.sendMessage1(operaPhone, name, ((int)(int.Parse(time) / 60)).ToString());
                }
            }


        }

        //根据超时 时间 发送短信给 操作员和公司负责人
        private void sendNetOKStatusMsg(string time)
        {
            MechineUtil.updateMechineStatus(id, "0");
            DataTable da = getJiQiDetail();
            if (da != null && da.Rows.Count > 0)
            {
                string operaID = da.Rows[0]["dls"].ToString();
                string companyID = da.Rows[0]["companyID"].ToString();

                string companyPhone = getCompanyPhone(companyID);
                string operaPhone = getOperaPhone(operaID);
                if (!string.IsNullOrEmpty(companyPhone))
                {
                    Util.log(id + "time:{" + time + "},发送重连成功短信", id + "jiQiOutTime.txt");
                    OperUtil.sendMessage2(companyPhone, name, ((int)(int.Parse(time) / 60)).ToString());
                }
                if (!string.IsNullOrEmpty(operaPhone))
                {
                    Util.log(id + "time:{" + time + "},发送重连成功短信", id + "jiQiOutTime.txt");
                    OperUtil.sendMessage2(operaPhone, name, ((int)(int.Parse(time) / 60)).ToString());
                }
            }


        }
        #endregion



        //获取该机器 负责人 操作员id  
        private DataTable getJiQiDetail()
        {
            string sql = "select companyID,dls from asm_mechine where id=" + id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }
        //获取该机器 负责人 手机
        private string getCompanyPhone(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = "select linkphone from asm_company where id=" + id;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["linkphone"].ToString();
                }
            }

            return null;
        }
        //获取该机器 操作员 手机
        private string getOperaPhone(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = "select linkphone from asm_opera where id = " + id;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["linkphone"].ToString();
                }
            }
            return null;
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

       



        //以下是一些方法

        //定时更新软件的方法
        private void updateSoft()
        {
            string _mechineInfo = RedisUtil.getMechine(id);
            JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);
            if (_mechineJArray != null)
            {

                DateTime updateTime = DateTime.ParseExact(_mechineJArray[0]["updateTime"].ToString(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                DateTime nowTime = DateTime.Now;

                Util.log("updateTime：" + _mechineJArray[0]["updateTime"].ToString(), id + "软件更新.txt");
                if (updateTime<=nowTime) {
                    Util.log("newVerCode：" + _mechineJArray[0]["newVerCode"].ToString()+ _mechineJArray[0]["downUrl"].ToString(), id + "软件更新.txt");
                    if ((_mechineJArray[0]["newVerCode"].ToString()!= _mechineJArray[0]["verCode"].ToString())&& !string.IsNullOrEmpty(_mechineJArray[0]["downUrl"].ToString()) && !string.IsNullOrEmpty(_mechineJArray[0]["newsoftversion"].ToString()))
                    {
                        _mechineJArray[0]["updateSoftStatus"] = 1;
                        string sqlUpdate = "update asm_mechine set updateSoftStatus=1 where id='" + id + "'";
                        DbHelperSQL.ExecuteSql(sqlUpdate);
                        RedisHelper.SetRedisModel<string>(id + "_mechineInfoSet", _mechineJArray.ToString());
                        BlockModel msgmodel = new BlockModel();
                        msgmodel.ID = id;
                        msgmodel.MsgId = Util.GetTimeStamp();
                        msgmodel.cmd = "updateSoft";
                        msgmodel.SendMsg = "{\"newVerCode\":" + _mechineJArray[0]["newVerCode"].ToString() + ",\"downUrl\":\"" + _mechineJArray[0]["downUrl"].ToString() + "\",\"newsoftversion\":\"" + _mechineJArray[0]["newsoftversion"].ToString() + "\"}";
                        Util.log("SendMsg：" + msgmodel.SendMsg, id + "软件更新.txt");
                        sendMessageToAndroid(msgmodel);
                    }
                }
            }
        }


        //循环遍历缓存中的出货信息数据，超时的走失败流程
        private void outTimeChFail() {
           
            string mechineIDCHString = RedisUtil.getMechineChString(id);  //出货指令缓存
          
            JObject mechineIDCHObj = JObject.Parse(mechineIDCHString);
            Util.log("mechineIDCHObj=" + mechineIDCHObj.ToString(), id+"outTimeChFail.txt");
            IEnumerable<JProperty> properties = mechineIDCHObj.Properties();
            foreach (JProperty item in properties)
            {
               
                string key = item.Name;
                string value = item.Value.ToString();
                Util.log("key:value=" + key+":"+value, id + "outTimeChFail.txt");
                JObject chObject =JObject.Parse(value);

                DateTime createTime=DateTime.ParseExact(chObject["createTime"].ToString(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

                TimeSpan difTime = DateTime.Now - createTime;//结束时间减去开始的时间
                Util.log("difTimeSeconds=" + difTime.TotalSeconds, id + "outTimeChFail.txt");
                if (difTime.TotalSeconds>60*30) {
                    string ldNO = chObject["ldNO"].ToString();
                    string bill = chObject["billno"].ToString();
                    string payType = chObject["payType"].ToString();
                    string productID = chObject["productID"].ToString();
                    string money = chObject["money"].ToString();
                    string type = chObject["type"].ToString();
                    string sql = "select trxid,m.id from  asm_pay_info a left join asm_member m on a.unionID=m.unionID where  a.statu=1 and trxid='" + bill + "'";
                    Util.log("sql=" + sql, id + "outTimeChFail.txt");
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
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
                        ht.Add("mechineID", id);
                        ht.Add("memberID", dt.Rows[0]["id"].ToString());
                        Hashtable ht1 = new Hashtable();
                        Util.log("uploadSellDetail=" + Util.HashtableToWxJson(ht), id + "outTimeChFail.txt");
                        MechineUtil.uploadSellDetail("[" + Util.HashtableToWxJson(ht) + "]");
                        //ht1.Add("recordList", "[" + Util.HashtableToWxJson(ht) + "]");
                        //XmlDocument xx = WebSvcCaller.QuerySoapWebService("http://nq.bingoseller.com/api/mechineService.asmx", "upSellRecord", ht1);
                        //string result = xx.OuterXml;
                        //log("bill:" + bill + ";result=" + result + ";HT:" + ht.ToString(), "LSCH.txt");
                        //return;
                    }
                }
            }
            
            
        }
        
        //和大屏上的视频信息序号比较，判断是否发送更新视频信息的命令
        public static void updategetVideoList(string mechineID)
        {
            Util.log("updategetVideoList=", mechineID + "updategetVideoList.txt");
            string _mechineInfo = RedisUtil.getMechine(mechineID);
            JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);
            if (_mechineJArray != null)
            {
                RedisUtil.getVideoList(mechineID);
                //取出产品redis
                string _videoInfoString = RedisHelper.GetRedisModel<string>(mechineID + "_videoInfoSet");
                JObject _videoInfoStringJObject = (JObject)JsonConvert.DeserializeObject(_videoInfoString);
                if (_videoInfoStringJObject != null)
                {
                    Util.log("zt=" + _mechineJArray[0]["zt"].ToString()+ ";"+_mechineJArray[0]["netStatus"].ToString()+";"+_videoInfoStringJObject["androidNo"].ToString()+";"+_mechineJArray[0]["videoListNo"].ToString(), mechineID + "updategetVideoList.txt");
                    if (_mechineJArray[0]["zt"].ToString() == "2" )
                    {
                        if ((_videoInfoStringJObject["androidNo"].ToString() != _mechineJArray[0]["videoListNo"].ToString()))
                        {
                            
                            BlockModel msgmodel = new BlockModel();
                            msgmodel.ID = mechineID;
                            msgmodel.MsgId = Util.GetTimeStamp();
                            msgmodel.cmd = "updateVideoList";
                            msgmodel.SendMsg = "{\"androidNo\":" + _videoInfoStringJObject["androidNo"].ToString() + ",\"androidInfoDetail\":" + _videoInfoStringJObject["androidInfoDetail"].ToString() + "}";
                            Util.log("SendMsg=" + msgmodel.SendMsg, mechineID + "updategetVideoList.txt");

                            sendMessageToAndroid(msgmodel);

                        }
                    }
                }
            }


        }
        //和大屏上的产品类别序号比较，判断是否发送更新产品类别的命令
        public static void updateProductType(string mechineID)
        {
            Util.log("进入updateProductType=", mechineID + "updateProductType.txt");
            string _mechineInfo = RedisUtil.getMechine(mechineID);
            JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);
            if (_mechineJArray != null)
            {
                RedisUtil.getProductTypeInfo();
                //取出产品redis
                string productTypeString = RedisHelper.GetRedisModel<string>("_productTypeInfoSet");
                JObject _productTypeJObject = (JObject)JsonConvert.DeserializeObject(productTypeString);
                if (_productTypeJObject != null)
                {
                    Util.log("zt=" + _mechineJArray[0]["zt"].ToString() + ";" + _mechineJArray[0]["netStatus"].ToString() + ";" , mechineID + "updateProductType.txt");

                    if (_mechineJArray[0]["zt"].ToString() == "2" )
                    {
                        if ((_productTypeJObject["androidNo"].ToString() != _mechineJArray[0]["productTypeNo"].ToString()) )
                        {
                            Util.log("_productTypeJArray=" + _productTypeJObject.ToString(), mechineID + "updateProductType.txt");
                            BlockModel msgmodel = new BlockModel();
                            msgmodel.ID = mechineID;
                            msgmodel.MsgId = Util.GetTimeStamp();
                            msgmodel.cmd = "updateProductType";
                            msgmodel.SendMsg = "{\"androidNo\":" + _productTypeJObject["androidNo"].ToString() + ",\"androidInfoDetail\":" + _productTypeJObject["androidInfoDetail"].ToString() + "}";
                            Util.log("SendMsg" + msgmodel.SendMsg, mechineID+"updateProductType.txt");
                            sendMessageToAndroid(msgmodel);

                        }
                    }
                }
            }
              

        }
        //和大屏上的产品信息序号和是否开启会员价比较，判断是否发送更新产品信息的命令
        public static  void updateProductView(string mechineID ) {
            Util.log("进入updateProductView", mechineID+"updateProductView.txt");
            string _mechineInfo = RedisUtil.getMechine(mechineID);
            JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);
            string priceSwitch = RedisUtil.getMemberprice(_mechineJArray[0]["companyID"].ToString());
            
            if (!string.IsNullOrEmpty(priceSwitch))
            {
                if (_mechineJArray != null)
                {
                    Util.log("getAPPProductView", mechineID+"updateProductView.txt");
                    //更新成最新的产品redis
                    RedisUtil.getAPPProductView(_mechineJArray[0]["id"].ToString(), _mechineJArray[0]["companyID"].ToString());
                    //以下是更新条件
                    //取出产品redis
                    string productViewString = RedisHelper.GetRedisModel<string>(mechineID + "_androidProductView");
                    JObject _productViewJObject = (JObject)JsonConvert.DeserializeObject(productViewString);
                    //为空则需要添加并通知机器更新
                    if (_productViewJObject != null)
                    {
                        Util.log("campare:" + _mechineJArray[0]["zt"].ToString()+";"+ _mechineJArray[0]["netStatus"].ToString()+";"+_productViewJObject["androidNo"].ToString()+ ";" + _mechineJArray[0]["androidProductNo"].ToString()+ ";" + priceSwitch+ ";" + _mechineJArray[0]["priceSwitch"].ToString(), mechineID+"updateProductView.txt");
                        if (_mechineJArray[0]["zt"].ToString() == "2")
                        {
                            if ((_productViewJObject["androidNo"].ToString() != _mechineJArray[0]["androidProductNo"].ToString()) || (priceSwitch != _mechineJArray[0]["priceSwitch"].ToString()))
                            {
                               
                                BlockModel msgmodel = new BlockModel();
                                msgmodel.ID = mechineID;
                                msgmodel.MsgId = Util.GetTimeStamp();
                                msgmodel.cmd = "updateProductList";
                                msgmodel.SendMsg = "{\"androidNo\":" + _productViewJObject["androidNo"].ToString() + ",\"androidInfoDetail\":" + _productViewJObject["androidInfoDetail"].ToString() + ",\"priceSwitch\":" + priceSwitch + "}";
                                Util.log("SendMsg" + msgmodel.SendMsg, mechineID+"updateProductView.txt");
                                sendMessageToAndroid(msgmodel);
                               
                            }
                        }

                    }
                }
            }

        }
    }
}