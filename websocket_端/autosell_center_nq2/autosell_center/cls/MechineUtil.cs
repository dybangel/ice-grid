using autosell_center.api;
using autosell_center.main.jiqi;
using Consumer.cls;
using DBUtility;
using Maticsoft.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.WebSockets;
using System.Xml;
using uniondemo.com.allinpay.syb;
using WZHY.Common.DEncrypt;

namespace autosell_center.cls
{
    public class MechineUtil
    {
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
        /// 更新机器网络状态
        /// </summary>
        /// <param name="mechineID"></param>
        /// <param name="netStatus">0正常 1异常</param>
        public static object updateMechineStatus(string mechineID,string netStatus)
        {
            operaSocket.log("mechineID="+ mechineID+ ";netStatus="+ netStatus, "updateMechineStatus.txt");
            try
            {
                if (string.IsNullOrEmpty(mechineID)||string.IsNullOrEmpty(netStatus))
                {
                    return new { code=500,msg="参数不全"};
                }
                log("进入UpdateSocketStatus" + mechineID + netStatus, "测试socket111111.txt");
                if (mechineID=="68" || mechineID == "69") {
                    string mechineInfo = RedisUtil.getMechine(mechineID);
                    JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                    string socketStatus = jay[0]["netStatus"].ToString();

                    if (!netStatus.Equals(socketStatus))
                    {

                        jay[0]["netStatus"] = netStatus;
                        RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", JsonConvert.SerializeObject(jay));
                    }
                }
               
                string sql = "update asm_mechine set netStatus="+netStatus+" where id="+mechineID;
                operaSocket.log("sql=" + sql, "updateMechineStatus.txt");
                int a= DbHelperSQL.ExecuteSql(sql);
                operaSocket.log("a=" + a, "updateMechineStatus.txt");
                if (a>0)
                {
                    //如果netstatus=1 并且最后一条断网记录是已处理的 需要插入断网异常激烈
                    if (netStatus == "1")
                    {
                        string sql1 = "select top 1 * from asm_mechine_statu where mechineID=" + mechineID + " and breakID=2  order by id desc";
                        operaSocket.log("sql1=" + sql1, "updateMechineStatus.txt");
                        DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                        if (dt.Rows.Count > 0&&dt.Rows[0]["statu"].ToString()=="1")
                        {
                            
                            string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu) values(" + mechineID + ",2,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0)";
                            operaSocket.log("1insert=" + insert, "updateMechineStatus.txt");
                            DbHelperSQL.ExecuteSql(insert);

                            sendErrorMsg(mechineID, "2", "","");
                            return new { code = 200, msg = "" };
                        }
                        if (dt.Rows.Count==0)
                        {
                            string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu) values(" + mechineID + ",2,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0)";
                            operaSocket.log("1insert=" + insert, "updateMechineStatus.txt");
                            DbHelperSQL.ExecuteSql(insert);
                            sendErrorMsg(mechineID, "2", "","");
                            return new { code = 200, msg = "" };
                        }

                    } else if (netStatus=="0")
                    {
                        //需要更新机器联网状态 且 查看如果最后一条断网记录是异常的给自动置成已处理
                        string sql1 = "select top 1 * from asm_mechine_statu where mechineID=" + mechineID + " and breakID=2  order by id desc";
                        log("1sql1=" + sql1, "updateMechineStatus.txt");
                        DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                        if (dt.Rows.Count > 0 && dt.Rows[0]["statu"].ToString() == "0")
                        {
                            string update = "update asm_mechine_statu set statu=1 where id="+dt.Rows[0]["id"].ToString();
                            operaSocket.log("update=" + update, "updateMechineStatus.txt");
                            DbHelperSQL.ExecuteSql(update);
                            sendErrorMsg(mechineID, "2", "","");
                            return new { code = 200, msg = "" };
                        }
                    }
                }
                return new { code = 500, msg = "系统异常" };
            }
            catch (Exception ex){
                log("e="+ex.Message, "updateMechineStatus.txt");
                return new { code = 500, msg = "系统异常" };
            }
        }
        /// <summary>
        /// 之前的更新机器网络状态，没有更新redis
        /// </summary>
        /// <param name="mechineID"></param>
        /// <param name="netStatus">0正常 1异常</param>
        public static object updateMechineStatusOld(string mechineID, string netStatus)
        {
            operaSocket.log("mechineID=" + mechineID + ";netStatus=" + netStatus, "updateMechineStatus.txt");
            try
            {
                if (string.IsNullOrEmpty(mechineID) || string.IsNullOrEmpty(netStatus))
                {
                    return new { code = 500, msg = "参数不全" };
                }
                log("进入UpdateSocketStatus" + mechineID + netStatus, "测试socket111111.txt");
                string sql = "update asm_mechine set netStatus=" + netStatus + " where id=" + mechineID;
                operaSocket.log("sql=" + sql, "updateMechineStatus.txt");
                int a = DbHelperSQL.ExecuteSql(sql);
                operaSocket.log("a=" + a, "updateMechineStatus.txt");
                if (a > 0)
                {
                    //如果netstatus=1 并且最后一条断网记录是已处理的 需要插入断网异常激烈
                    if (netStatus == "1")
                    {
                        string sql1 = "select top 1 * from asm_mechine_statu where mechineID=" + mechineID + " and breakID=2  order by id desc";
                        operaSocket.log("sql1=" + sql1, "updateMechineStatus.txt");
                        DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                        if (dt.Rows.Count > 0 && dt.Rows[0]["statu"].ToString() == "1")
                        {

                            string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu) values(" + mechineID + ",2,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0)";
                            operaSocket.log("1insert=" + insert, "updateMechineStatus.txt");
                            DbHelperSQL.ExecuteSql(insert);

                            sendErrorMsg(mechineID, "2", "", "");
                            return new { code = 200, msg = "" };
                        }
                        if (dt.Rows.Count == 0)
                        {
                            string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu) values(" + mechineID + ",2,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0)";
                            operaSocket.log("1insert=" + insert, "updateMechineStatus.txt");
                            DbHelperSQL.ExecuteSql(insert);
                            sendErrorMsg(mechineID, "2", "", "");
                            return new { code = 200, msg = "" };
                        }

                    }
                    else if (netStatus == "0")
                    {
                        //需要更新机器联网状态 且 查看如果最后一条断网记录是异常的给自动置成已处理
                        string sql1 = "select top 1 * from asm_mechine_statu where mechineID=" + mechineID + " and breakID=2  order by id desc";
                        log("1sql1=" + sql1, "updateMechineStatus.txt");
                        DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                        if (dt.Rows.Count > 0 && dt.Rows[0]["statu"].ToString() == "1")
                        {
                            string update = "update asm_mechine_statu set statu=0 where id=" + dt.Rows[0]["id"].ToString();
                            operaSocket.log("update=" + update, "updateMechineStatus.txt");
                            DbHelperSQL.ExecuteSql(update);
                            sendErrorMsg(mechineID, "2", "", "");
                            return new { code = 200, msg = "" };
                        }
                    }
                }
                return new { code = 500, msg = "系统异常" };
            }
            catch (Exception ex)
            {
                log("e=" + ex.Message, "updateMechineStatus.txt");
                return new { code = 500, msg = "系统异常" };
            }
        }
        /// <summary>
        /// 添加温度异常记录 10度以上添加
        /// </summary>
        /// <param name="mechineID"></param>
        /// <param name="temperature"></param>
        public static object AddTemBrokenRecord(string mechineID,string temperature)
        {
            try
            {
                if (string.IsNullOrEmpty(mechineID) || string.IsNullOrEmpty(temperature))
                {
                    return new { code = 500, msg = "参数不全" };
                }

                if (double.Parse(temperature)<getMinTem(mechineID)||double.Parse(temperature)>getMaxTem(mechineID))
                {
                    //取上一条未处理的温度异常的记录 如果仍然打印10度则不再添加记录
                    string sql = "select top 1 * from asm_mechine_statu where mechineID=" + mechineID + " and breakID=4  order by id desc";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0 && dt.Rows[0]["statu"].ToString() == "1")
                    {
                        string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu,bz) values(" + mechineID + ",4,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0," + temperature + ")";
                        DbHelperSQL.ExecuteSql(insert);


                        sendErrorMsg(mechineID, "4", temperature,"");
                        return new { code = 200, msg = "" };
                    }
                    if (dt.Rows.Count == 0)
                    {
                        string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu,bz) values(" + mechineID + ",4,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0," + temperature + ")";
                        DbHelperSQL.ExecuteSql(insert);
                        sendErrorMsg(mechineID, "4", temperature,"");
                        return new { code = 200, msg = "" };
                    }
                }
              
                return new { code = 500, msg = "系统异常" };
            }
            catch {
                return new { code = 500, msg = "系统异常" };
            }
        }
        public static void sendErrorMsg(string mechineID,string type,string temperature,string ldNO)
        {
            string sqlOp = "select * from asm_mechine where id=" + mechineID;
            DataTable dop = DbHelperSQL.Query(sqlOp).Tables[0];
            
            string sql1 = "select * from asm_opera where id='"+dop.Rows[0]["operaID"].ToString()+"' or id='"+ dop.Rows[0]["dls"].ToString() + "'";
            DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
            String[] opArray = new String[d1.Rows.Count];
            for (int i=0;i < d1.Rows.Count;i++) {
                opArray[i]=d1.Rows[i]["name"].ToString();
            }
            
            
           
           
            StringBuilder sb = new StringBuilder();
            if (type == "4")
            {
                string msg = "温度异常：位于" + dop.Rows[0]["addres"].ToString() + "机器出现温度异常" + temperature + "°C，请前去处理";
                sb.Append("{\"cmd\":\"app.error\",\"msg\":\"" + msg + "\",\"bh\":\"" + dop.Rows[0]["id"].ToString() + "\"}");
            }
            else if (type == "2") {
                string msg = "网络异常：位于" + dop.Rows[0]["addres"].ToString() + "机器出现网络异常，请前去处理";
                sb.Append("{\"cmd\":\"app.error\",\"msg\":\"" + msg + "\",\"bh\":\"" + dop.Rows[0]["bh"].ToString() + "\"}");
            } else if (type=="1")
            {
                string msg = "料道异常：位于" + dop.Rows[0]["addres"].ToString() + "机器出现料道异常，编号:"+ldNO+"请前去处理";
                sb.Append("{\"cmd\":\"app.error\",\"msg\":\"" + msg + "\",\"bh\":\"" + dop.Rows[0]["bh"].ToString() + "\"}");
            }else if (type == "5")
            {
                if (mechineID=="68" || mechineID == "69") {
                    string msg = "位于" + dop.Rows[0]["addres"].ToString() + temperature;
                    sb.Append("{\"cmd\":\"app.error\",\"msg\":\"" + msg + "\",\"bh\":\"" + dop.Rows[0]["id"].ToString() + "\"}");
                } else {
                    //这里给管理员添加短信息

                    string msg = "位于" + dop.Rows[0]["addres"].ToString() + temperature;

                    string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu,bz) values(" + mechineID + ",5,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + msg + "')";
                    DbHelperSQL.ExecuteSql(insert);
                    sb.Append("{\"cmd\":\"app.error\",\"msg\":\"" + msg + "\",\"bh\":\"" + dop.Rows[0]["id"].ToString() + "\"}");
                    //机器状态长时间没有 进入轮询和恢复轮询
                    sendMessage(d1.Rows[0]["linkphone"].ToString(), msg + "【冰格售卖】");
                }
                
            }
            else if (type == "6")
            {
                if (mechineID == "68" || mechineID == "69")
                {
                    string msg = "位于" + dop.Rows[0]["addres"].ToString() + temperature;
                    sb.Append("{\"cmd\":\"app.error\",\"msg\":\"" + msg + "\",\"bh\":\"" + dop.Rows[0]["id"].ToString() + "\"}");
                }
                else
                {
                    //这里给管理员添加短信息

                    string msg = "位于" + dop.Rows[0]["addres"].ToString() + temperature;

                    string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu,bz) values(" + mechineID + ",6,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + msg + "')";
                    DbHelperSQL.ExecuteSql(insert);

                    //string up1 = "update asm_mechine set ldStatus=1 where id=" + mechineID;
                    //DbHelperSQL.ExecuteSql(up1);
                    sb.Append("{\"cmd\":\"app.error\",\"msg\":\"" + msg + "\",\"bh\":\"" + dop.Rows[0]["id"].ToString() + "\"}");
                    //机器状态长时间没有 进入轮询和恢复轮询
                    sendMessage(d1.Rows[0]["linkphone"].ToString(), msg + "【冰格售卖】");
                }
                   
            }

            operaSocket.log("发送消息=" + sb.ToString(), "收发");


            Util.faxiaoxi(opArray, sb);
        }
        //1异常，0正常
        public static void updateMechineStatus(string mechineID,string type,string status,string msg) {
            string sqlOp = "select * from asm_mechine where id=" + mechineID;
            DataTable dop = DbHelperSQL.Query(sqlOp).Tables[0];

            string asm_operasql = "select * from asm_opera where id='" + dop.Rows[0]["operaID"].ToString() + "' or id='" + dop.Rows[0]["dls"].ToString() + "'";
            DataTable asm_operad1 = DbHelperSQL.Query(asm_operasql).Tables[0];
            msg = "位于" + dop.Rows[0]["addres"].ToString() + msg;
            if (status == "1")
            {
                string sql1 = "select top 1 * from asm_mechine_statu where mechineID=" + mechineID + " and breakID="+type+"  order by id desc";
                operaSocket.log("sql1=" + sql1, "updateMechineStatus.txt");
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                if (dt.Rows.Count > 0 && dt.Rows[0]["statu"].ToString() == "0")
                {

                    string insert = "update  asm_mechine_statu set bz='" + msg + "'  where id=" + dt.Rows[0]["id"].ToString();
                    operaSocket.log("1insert=" + insert, "updateMechineStatus.txt");
                    DbHelperSQL.ExecuteSql(insert);
                    sendMessage(asm_operad1.Rows[0]["linkphone"].ToString(), msg + "【冰格售卖】");


                }
                else {
                    string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu,bz) values(" + mechineID + ","+type+",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + msg + "')";
                    DbHelperSQL.ExecuteSql(insert);
                    sendMessage(asm_operad1.Rows[0]["linkphone"].ToString(), msg + "【冰格售卖】");
                }
               

            }
            else if (status == "0")
            {
                string sql1 = "select top 1 * from asm_mechine_statu where mechineID=" + mechineID + " and breakID=" + type + "  order by id desc";
                log("1sql1=" + sql1, "updateMechineStatus.txt");
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                if (dt.Rows.Count > 0 && dt.Rows[0]["statu"].ToString() == "0")
                {
                    string update = "update asm_mechine_statu set statu=1 where id=" + dt.Rows[0]["id"].ToString();
                    
                    DbHelperSQL.ExecuteSql(update);
                    sendMessage(asm_operad1.Rows[0]["linkphone"].ToString(), msg + "【冰格售卖】");
                }
            }
        }
        public static void sendMessage(string member_phone, string msg)
        {
            if (string.IsNullOrEmpty(member_phone))
            {
                return;
            }
            if (!Regex.IsMatch(member_phone, @"^1\d{10}$"))//判断手机号
            {
                return;
            }
            if (Util.CheckServeStatus("sms.ruizhiwei.net"))
            {
                //查询签名
                Hashtable ht1 = new Hashtable();
                ht1.Add("U", "bingge");
                ht1.Add("p", DESEncrypt.MD5Encrypt("123456"));

                XmlDocument xmlDoc1 = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SignShow", ht1);
                String SMSign = xmlDoc1.InnerText;

                String registration = msg;
                 
                Hashtable ht = new Hashtable();
                ht.Add("U", "bingge");
                ht.Add("p", DESEncrypt.MD5Encrypt("123456"));
                ht.Add("N", member_phone);
                ht.Add("M", registration);
                ht.Add("T", "2");
                XmlDocument xmlDoc = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SendMes", ht);
                String status = xmlDoc.InnerText;
                string insert = "insert into asm_dgmsg(phone,sendTime,msg,type) values('" + member_phone + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + registration + "',1)";
                DbHelperSQL.ExecuteSql(insert);
            }
        }
        public static string GetJson(string JsonName, string JsonValue)
        {
            return "\"" + JsonName + "\":\"" + JsonValue + "\",";
        }
        private static string ReturnJson(AspNetWebSocketContext context, StringBuilder sb)
        {
            string json = "{" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "}";
             
            return json;
        }
        /// <summary>
        /// redis 读取该机器的最低温度报警信息
        /// </summary>
        /// <param name="mechineID"></param>
        /// <returns></returns>
        public static  double getMinTem(string mechineID)
        {
            string sql = "select * from  asm_mechine where id="+mechineID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];

            if (dt.Rows.Count>0)
            {
                string minTem = RedisHelper.GetRedisModel<string>("min_"+mechineID);
                if (string.IsNullOrEmpty(minTem))
                {
                    string[] setTem = dt.Rows[0]["setTem"].ToString().Split('-');
                    if (setTem.Length == 2)
                    {
                        double min = double.Parse(setTem[0]);
                        RedisHelper.SetRedisModel("min_" + mechineID, min.ToString(), new TimeSpan(1, 0, 0));//1小时过期
                        return min;
                    }
                }
                else {
                    return double.Parse(minTem);
                }
            }
            return 0;
        }
        public static double getMaxTem(string mechineID)
        {
            string sql = "select * from  asm_mechine where id=" + mechineID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];

            if (dt.Rows.Count > 0)
            {
                string maxTem = RedisHelper.GetRedisModel<string>("max_" + mechineID);
                if (string.IsNullOrEmpty(maxTem))
                {
                    string[] setTem = dt.Rows[0]["setTem"].ToString().Split('-');
                    if (setTem.Length == 2)
                    {
                        double max = double.Parse(setTem[1]);
                        RedisHelper.SetRedisModel("max_" + mechineID, max.ToString(), new TimeSpan(1, 0, 0));//1小时过期
                        return max;
                    }
                }
                else
                {
                    return double.Parse(maxTem);
                }
            }
            return 0;
        }
        //更新零售
        public static string uploadSellDetail(string recordList)
        {
            try
            {
                log("Util开始recordList=" + recordList, "upSellRecord1.txt");
                List<asm_sellDetail> twoList = JsonConvert.DeserializeObject<List<asm_sellDetail>>(recordList);
                foreach (asm_sellDetail stu in twoList)
                {
                    log("Util进入recordList=" + recordList, "upSellRecord1.txt");
                    //清空redis 
                    string mechineChInfoString = RedisUtil.getMechineChString(stu.mechineID.ToString()); //出货信息缓存
                    JObject mechineIDCHObj = JObject.Parse(mechineChInfoString);
                    if (!string.IsNullOrEmpty(stu.code))
                    {
                        mechineIDCHObj.Remove("CH" + stu.memberID.ToString() + stu.code.ToString());
                    }
                    else {
                        mechineIDCHObj.Remove("CH" + stu.billno.ToString());
                    }
                    
                    RedisHelper.SetRedisModel<string>("CH" + stu.mechineID, mechineIDCHObj.ToString());
                    log("清理redis之后", "upSellRecord1.txt");
                    string sql12 = "select * from asm_sellDetail where billno='" + stu.billno + "'";
                    log("sql12："+ sql12, "upSellRecord1.txt");
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
                        //Util.Debuglog("sql=" + sql, "插入售卖记录.txt");
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
                            + " VALUES( '" + dp.Rows[0]["companyID"].ToString() + "','" + dp.Rows[0]["proName"].ToString() + "'," + stu.productID + "," + stu.num + "," + stu.totalMoney + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + stu.proLD + "'," + type + ",'" + stu.orderNO + "','" + memberID + "','" + stu.code + "'," + payType + "," + stu.mechineID + ",'" + stu.bz + "','" + stu.billno + "','" + dp.Rows[0]["price0"].ToString() + "')";
                                 Util.log("零售sql=" + sql, "插入售卖记录.txt");

                                int a = DbHelperSQL.ExecuteSql(sql);
                                //////////////////////////////////////////
                                //此处调整，之前是成功扣库存，现在改为出货前减库存失败加库存
                                if (stu.bz == "出货失败")
                                {
                                    Util.asm_ld_change(stu.mechineID.ToString(), stu.productID.ToString(), stu.proLD.ToString(), 1, "2", 1);//库存变动记录
                                }
                                else
                                {
                                   
                                }
                                //////////////////////////////////////////
                                //此处更新会员等级
                                Util.updateMemberDJ(memberID, stu.mechineID.ToString());
                            }
                        }
                        else if (type == "1")
                        {
                            //////////////////////////////////////////
                            //此处调整，之前是成功扣库存，现在改为出货前减库存失败加库存
                            if (stu.bz == "出货失败")
                            {
                                log("Util进入出货失败=", "upSellRecord1.txt");
                                Util.asm_ld_change(stu.mechineID.ToString(), stu.productID.ToString(), stu.proLD.ToString(), 1, "3", 1);//库存变动记录
                            }
                            else
                            {
                              
                            }
                            //////////////////////////////////////////
                            string sqlp = "select * from asm_product where productID='" + stu.productID + "'";
                            DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                            string sql = "INSERT INTO [dbo].[asm_sellDetail](companyID,productname,[productID],[num] ,[totalMoney],[orderTime],[proLD],[type],[orderNO],[memberID],[code] ,[payType],[mechineID],[bz],[billno])"
                        + " VALUES( '" + dp.Rows[0]["companyID"].ToString() + "','" + dp.Rows[0]["proName"].ToString() + "'," + stu.productID + "," + stu.num + "," + stu.totalMoney + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + stu.proLD + "'," + type + ",'" + stu.orderNO + "','" + stu.memberID + "','" + stu.code + "'," + payType + "," + stu.mechineID + ",'" + stu.bz + "','" + stu.billno + "')";
                            Util.log("订购sql=" + sql, "插入售卖记录.txt");
                            int a = DbHelperSQL.ExecuteSql(sql);
                        }

                    }
                    string sqlM = "select * from asm_company where id in(select companyID from asm_mechine where id=" + stu.mechineID + ")";
                    DataTable dM = DbHelperSQL.Query(sqlM).Tables[0];
                    if (stu.bz == "交易成功" && dM.Rows.Count > 0)
                    {

                        //这几个机器是通过设备app端调的更新库存接口更新的库存
                        //if (stu.mechineID != 33 && stu.mechineID != 34 && stu.mechineID != 35 && stu.mechineID != 36 && stu.mechineID != 37)
                       // {
                            ///////////////////////////////
                            //此处料道库存增加
                            //Util.update_reduceLDKC(stu.mechineID.ToString(), stu.proLD.ToString());
                            ////////////////////////
                            //update_KC1(stu.mechineID.ToString(), stu.proLD.ToString());
                        //}
                        //update_KC(stu.mechineID.ToString(), dM.Rows[0]["id"].ToString(), stu.productID.ToString());
                    }
                    #region 出货错误发送短信
                    //
                    if (stu.bz == "料道错误" || stu.bz == "交易序列号相同" || stu.bz == "料道故障" || stu.bz == "校验错误" || stu.bz == "出货失败")
                    {

                        ///////////////////////////////
                        //这几个机器是通过设备app端调的更新库存接口更新的库存
                        if (stu.mechineID != 33 && stu.mechineID != 34 && stu.mechineID != 35 && stu.mechineID != 36 && stu.mechineID != 37)
                        {
                          
                            //此处料道库存增加
                            Util.update_addLDKC(stu.mechineID.ToString(), stu.proLD.ToString());
                           
                        }
                        Util.update_KCDetail(stu.mechineID.ToString(), stu.productID.ToString());
                        ///////////////////////////////
                        Util.sendMsgWhenLDError(stu.mechineID.ToString(), stu.proLD.ToString());//发送短信
                        //更新错误料道
                        string update = "update asm_ldInfo set zt='1',lastUpTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ldNO=" + stu.proLD + " and mechineID=" + stu.mechineID;
                        log("更新错误料道" + update, "upSellRecord1.txt");
                        DbHelperSQL.ExecuteSql(update);




                        //////////////////////////////////////////////////////
                        RedisHelper.Remove(stu.mechineID + "_LDList");
                       



                        ////////////////////////////////////////////////////////



                        //ClearRedisProductInfoByMechineID(stu.mechineID.ToString());//清除该设备的产品信息缓存
                        //插入异常记录
                        //string sqlM1 = "SELECT phone FROM asm_member where unionID in (SELECT unionID FROM asm_pay_info  WHERE trxid='"+stu.billno+"')";
                        string sqlM1 = "select b.phone from asm_sellDetail a left join asm_member b  on a.memberID=b.id  where orderno='" + stu.orderNO + "'";


                        log("插入异常记录" + sqlM1, "upSellRecord1.txt");
                        DataTable dm1 = DbHelperSQL.Query(sqlM1).Tables[0];
                        string phone = "";
                        if (dm1.Rows.Count > 0 && !string.IsNullOrEmpty(dm1.Rows[0]["phone"].ToString()))
                        {
                            phone = "购买人手机号:" + dm1.Rows[0]["phone"].ToString();
                        }
                        log("料道错误" + sqlM1, "upSellRecord1.txt");
                        string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu,bz) values(" + stu.mechineID + ",3,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'出错料道号" + stu.proLD + phone + "')";
                        DbHelperSQL.ExecuteSql(insert);
                        string up1 = "update asm_mechine set ldStatus=1 where id=" + stu.mechineID;
                        DbHelperSQL.ExecuteSql(up1);
                        if (stu.mechineID==68 || stu.mechineID == 69) {
                            string _mechineInfo = RedisUtil.getMechine(""+stu.mechineID);
                            JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);
                            if (_mechineJArray != null)
                            {
                                _mechineJArray[0]["ldStatus"] = 1;
                                RedisHelper.SetRedisModel<string>(stu.mechineID + "_mechineInfoSet", _mechineJArray.ToString());
                            }
                        }
                        sendErrorMsg( stu.mechineID.ToString(), "1", "", stu.proLD.ToString());//给操作员发送错误
                    }
                    #endregion
                    log("产品列表前" , "upSellRecord1.txt");
                    /////////////////////
                    //此处写出货流程结束，给安卓发信息让更新页面到产品列表
                    JiQi.updateProductView(stu.mechineID.ToString());
                    BlockModel msgmodel = new BlockModel();
                    msgmodel.ID = stu.mechineID.ToString();
                    msgmodel.MsgId = Util.GetTimeStamp();
                    msgmodel.cmd = "chEnd";
                    msgmodel.SendMsg = "{}";
                    JiQi.sendMessageToAndroid(msgmodel);
                    log("产品列表后", "upSellRecord1.txt");
                    /////////////////////
                    //////
                    //此处别忘了还有退款操作，为了测试给减去了
                    /////
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
                                Util.log("查询sql=" + sql, "退款记录.txt");
                                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                                if (dd.Rows.Count <= 0)
                                {
                                    //更新会员钱包 并插入资金变动记录
                                    string update = "update asm_member set  AvailableMoney=AvailableMoney+" + stu.totalMoney.ToString() + ",sumConsume=sumConsume-" + stu.totalMoney.ToString() + " where id=" + stu.memberID.ToString();
                                    Util.log("变动余额sql=" + sql, "退款记录.txt");
                                    int a = DbHelperSQL.ExecuteSql(update);
                                    string sqlm = "select * from asm_member where id=" + stu.memberID.ToString();
                                    DataTable dt = DbHelperSQL.Query(sqlm).Tables[0];
                                    if (a > 0)
                                    {
                                        string sqlu = "update asm_sellDetail set bz='退款成功' where billno='" + stu.billno + "'";
                                        Util.log("更改状态sql=" + sql, "退款记录.txt");
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
                                                Util.log("e=" + e.Message, "会员等级消息模板.txt");
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
                                                    Util.log("e=" + e.Message, "会员等级消息模板.txt");
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
                                        string reqsn = Util.ConvertDateTimeToInt(DateTime.Now).ToString() + rand;
                                        string oldtrxid = dd.Rows[0]["trxid"].ToString();
                                        string oldreqsn = dd.Rows[0]["reqsn"].ToString();
                                        Dictionary<String, String> rsp = sybService.cancel(fen, reqsn, oldtrxid, oldreqsn);
                                        string data = OperUtil.SerializeDictionaryToJsonString(rsp);

                                        Util.log("微信支付链接data=" + data, "_退款.txt");
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
                                                            Util.log("e=" + e.Message, "会员等级消息模板.txt");
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
       //上传温度2
        public static  void readZTMechine2(string mechineID)
        {
            try
            {
                BlockModel msgmodel = new BlockModel();
                msgmodel.ID = mechineID;
                msgmodel.MsgId = Util.GetTimeStamp();
                msgmodel.cmd = "searchTem";
                msgmodel.SendMsg = "{}";
                Util.log("查询温度：", mechineID + "查询温度.txt");
                msgmodel =JiQi.sendMessageToAndroidWaitReturn(msgmodel);
                Util.log("查询温度："+ msgmodel.Status, mechineID + "查询温度.txt");
                if (msgmodel.Status == 0) {
                    JObject samtype = JObject.Parse(msgmodel.samtype);
                    if (samtype!=null) {
                        string temperature = samtype["temperature"].ToString();
                        string versions = samtype["version"].ToString();
                        Util.log("samtype：" + msgmodel.samtype, mechineID + "查询温度.txt");
                        string result=RedisUtil.getMechine(mechineID);
                        string tem = "";
                        if (!string.IsNullOrEmpty(result))
                        {

                            JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
                            tem = jArray[0]["setTem"].ToString();
                            Util.log(mechineID + "缓存读取=" + tem, mechineID + "查询温度.txt");
                        }
                        
                       
                        //向asm_info 表插入一条消息 用于判断机器是否脱机
                        //更新机器温度

                        double min = 0;
                        double max = 0;
                        try
                        {
                            string[] arr = tem.Split('-');
                            if (arr.Length == 2)
                            {
                                min = double.Parse(arr[0]);
                                max = double.Parse(arr[1]);
                            }
                        }
                        catch
                        {

                        }
                        if (double.Parse(temperature) < min || double.Parse(temperature) > max)//温度大于10度显示异常
                        {

                            string sqlUpdate = "update asm_mechine set softversion='" + versions + "',temperture='" + double.Parse(temperature) + "',statu=2,temStatus=1,brokenTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',lastReqTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',sendF=0,sendT=1 where id='" + mechineID + "'";
                            Util.log("1sqlUpdate=" + sqlUpdate, mechineID + "查询温度.txt");
                            DbHelperSQL.ExecuteSql(sqlUpdate);
                            if (mechineID == "68" || mechineID == "69")
                            {
                                string _mechineInfo = RedisUtil.getMechine(mechineID);
                                JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);
                                if (_mechineJArray != null)
                                {
                                    _mechineJArray[0]["temperture"] = double.Parse(temperature).ToString();
                                    _mechineJArray[0]["statu"] = 2;
                                    _mechineJArray[0]["temStatus"] = 1;
                                    _mechineJArray[0]["brokenTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    _mechineJArray[0]["sendF"] = 0;
                                    _mechineJArray[0]["sendT"] = 1;
                                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", _mechineJArray.ToString());
                                }
                            }
                        }
                        else
                        {
                            string sqlUpdate = "update asm_mechine set softversion='" + versions + "',temperture='" + double.Parse(temperature) + "',lastReqTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',statu=0,sendF=0,sendT=0,temStatus=0 where id='" + mechineID + "'";
                            Util.log("2sqlUpdate=" + sqlUpdate, mechineID + "查询温度.txt");
                            DbHelperSQL.ExecuteSql(sqlUpdate);
                            if (mechineID == "68" || mechineID == "69")
                            {
                                string _mechineInfo = RedisUtil.getMechine(mechineID);
                                JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);
                                if (_mechineJArray != null)
                                {
                                    _mechineJArray[0]["temperture"] = double.Parse(temperature).ToString();
                                    _mechineJArray[0]["statu"] = 0;
                                    _mechineJArray[0]["temStatus"] = 0;
                                    _mechineJArray[0]["sendF"] = 0;
                                    _mechineJArray[0]["sendT"] = 0;
                                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", _mechineJArray.ToString());
                                }
                            }
                        }
                        //插入温度记录
                        if (double.Parse(temperature) > -20)
                        {
                            string sqlInsert = "insert into asm_temperature (mechineID,temperature,time) values('" + mechineID + "','" + temperature + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')";
                            DbHelperSQL.ExecuteSql(sqlInsert);
                        }
                        if (double.Parse(temperature) < getMinTem(mechineID) || double.Parse(temperature) > getMaxTem(mechineID))
                        {
                            string sql1 = "select top 1 * from asm_mechine_statu where mechineID=" + mechineID + " and breakID=" + 4 + "  order by id desc";
                            operaSocket.log("sql1=" + sql1, "updateMechineStatus.txt");
                            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                            if (dt.Rows.Count > 0 && dt.Rows[0]["statu"].ToString() == "0")
                            {

                                string insert = "update  asm_mechine_statu set bz='" + "机器温度异常：" + temperature + "℃请及时处理" + "'  where id=" + dt.Rows[0]["id"].ToString();
                                operaSocket.log("1insert=" + insert, "updateMechineStatus.txt");
                                DbHelperSQL.ExecuteSql(insert);


                            }
                            else
                            {
                                string insert = "insert into asm_mechine_statu(mechineID,breakID,time,statu,bz) values(" + mechineID + "," + 4 + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + "机器温度异常：" + temperature + "℃请及时处理" + "')";
                                DbHelperSQL.ExecuteSql(insert);
                            }

                         
                        }
                        else {
                            //取上一条未处理的温度异常的记录 如果仍然打印10度则不再添加记录
                            string sql = "select top 1 * from asm_mechine_statu where mechineID=" + mechineID + " and breakID=4  order by id desc";
                            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                            if (dt.Rows.Count > 0 && dt.Rows[0]["statu"].ToString() == "0")
                            {
                                string msg = "机器温度已恢复正常:" + temperature + "℃";
                                string insert = "update  asm_mechine_statu set statu=1,bz='" + "机器温度已恢复正常: " + temperature + "℃" + "'  where id=" + dt.Rows[0]["id"].ToString();
                                operaSocket.log("1insert=" + insert, "updateMechineStatus.txt");
                                DbHelperSQL.ExecuteSql(insert);

                            }
                               
                        }


                    }
                }


               
            }
            catch (Exception e)
            {
                Util.log("error=" + e.Message, "上传温度.txt");
            }

        }
    }
}