using autosell_center.api;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using ThoughtWorks.QRCode.Codec;
using WZHY.Common.DEncrypt;

namespace autosell_center
{
    public class OperUtil
    {
        public static string AK = "NUhf3DfhMuF97uz15QjH3ykOEB1YURoi";
        public static string DataTableToJsonWithJsonNet(DataTable table)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.None);
            return jsonString;
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        public static void Add(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 240;
        }
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static string Get(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            return HttpContext.Current.Session[strSessionName].ToString();
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
            catch (ThreadAbortException)
            {
            }
            catch (Exception)
            {
            }
        }
        public static void sendMessage1(string member_phone,string bh,string count)
        {
            Debuglog("member_phone="+ member_phone+";bh="+bh+";count="+count,"短信_.txt");
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
                 
                String registration = "异常提醒 ｛bh｝售卖机 离线 ｛count｝分钟  请及时处理【冰格售卖】";
                //registration = registration.Replace("@SMSign", SMSign);
                registration = registration.Replace("｛bh｝", bh);  //卡号
                registration = registration.Replace("｛count｝", count);


                Hashtable ht = new Hashtable();
                ht.Add("U", "bingge");
                ht.Add("p", DESEncrypt.MD5Encrypt("123456"));
                ht.Add("N", member_phone);
                ht.Add("M", registration);
                ht.Add("T", "2");
                XmlDocument xmlDoc = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SendMes", ht);
                Debuglog("registration=" + registration, "短信_.txt");
                String status = xmlDoc.InnerText;
                string insert = "insert into asm_dgmsg(phone,sendTime,msg,type) values('" + member_phone + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + registration + "',2)";
                DbHelperSQL.ExecuteSql(insert);
            }

        }
        public static void sendMessage2(string member_phone, string bh, string count)
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


                String registration = "解除提醒 ｛bh｝售卖机 ｛date｝分恢复网络 【冰格售卖】";
                //registration = registration.Replace("@SMSign", SMSign);
                registration = registration.Replace("｛bh｝", bh);  //卡号
                registration = registration.Replace("｛date｝", count);


                Hashtable ht = new Hashtable();
                ht.Add("U", "bingge");
                ht.Add("p", DESEncrypt.MD5Encrypt("123456"));
                ht.Add("N", member_phone);
                ht.Add("M", registration);
                ht.Add("T", "2");
                XmlDocument xmlDoc = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SendMes", ht);
                String status = xmlDoc.InnerText;
            }

        }
        public static void sendMessage3(string member_phone, string bh,string date,string T)
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
                
                String registration = "温度异常提醒 ｛bh｝售卖机 ｛date｝分温度异常 {T}℃ 【冰格售卖】";
                //registration = registration.Replace("@SMSign", SMSign);
                registration = registration.Replace("｛bh｝", bh);  //卡号
                registration = registration.Replace("｛date｝", date);
                registration = registration.Replace("{T}", T);
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
        /// <summary>
        /// 出货异常短信通知
        /// </summary>
        /// <param name="member_phone"></param>
        /// <param name="bh"></param>
        /// <param name="date"></param>
        /// <param name="T"></param>
        public static void sendMessage4(string member_phone, string bh, string date,string no)
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
                String registration = "出货异常提醒 ｛bh｝售卖机 ｛date｝分出货异常料道号{NO} 【冰格售卖】";
                //registration = registration.Replace("@SMSign", SMSign);
                registration = registration.Replace("｛bh｝", bh);  //卡号
                registration = registration.Replace("｛date｝", date);
                registration = registration.Replace("{NO}", no);
                registration = registration.Replace("#", "※");
                Hashtable ht = new Hashtable();
                ht.Add("U", "bingge");
                ht.Add("p", DESEncrypt.MD5Encrypt("123456"));
                ht.Add("N", member_phone);
                ht.Add("M", registration);
                ht.Add("T", "2");
                try
                {
                    XmlDocument xmlDoc = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SendMes", ht);
                    String status = xmlDoc.InnerText;
                    string insert = "insert into asm_dgmsg(phone,sendTime,msg,type) values('" + member_phone + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + registration + "',3)";
                    DbHelperSQL.ExecuteSql(insert);
                }
                catch (Exception e)
                {

                    throw;
                }
                
               
            }
        }

        public static string sendMessage5(string member_phone, string code)
        {
            if (string.IsNullOrEmpty(member_phone))
            {
                return "手机号不正确";
            }
            if (!Regex.IsMatch(member_phone, @"^1\d{10}$"))//判断手机号
            {
                return "手机号不正确";
            }
            if (Util.CheckServeStatus("sms.ruizhiwei.net"))
            {
                //查询签名
                Hashtable ht1 = new Hashtable();
                ht1.Add("U", "bingge");
                ht1.Add("p", DESEncrypt.MD5Encrypt("123456"));

                XmlDocument xmlDoc1 = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SignShow", ht1);
                String SMSign = xmlDoc1.InnerText;

                String registration = "【生鲜时逐】您的订奶码{code},微信搜“生鲜时逐”→订奶→我的→线下订奶码兑换→选好首送日期,天天享受初产24h鲜奶!";
                registration = registration.Replace("{code}", code);  //卡号
                Hashtable ht = new Hashtable();
                ht.Add("U", "bingge");
                ht.Add("p", DESEncrypt.MD5Encrypt("123456"));
                ht.Add("N", member_phone);
                ht.Add("M", registration);
                ht.Add("T", "2");
                XmlDocument xmlDoc = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SendMes", ht);
                String status = xmlDoc.InnerText;
                string insert = "insert into asm_dgmsg(phone,sendTime,msg,type) values('"+member_phone+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+registration+"',4)";
                DbHelperSQL.ExecuteSql(insert);
                return status;
            }
            return "服务器异常";
        }
        public static string sendMessage6(string member_phone, string proName,string bh)
        {
            if (string.IsNullOrEmpty(member_phone))
            {
                return "手机号不正确";
            }
            if (!Regex.IsMatch(member_phone, @"^1\d{10}$"))//判断手机号
            {
                return "手机号不正确";
            }
            if (Util.CheckServeStatus("sms.ruizhiwei.net"))
            {
                //查询签名
                Hashtable ht1 = new Hashtable();
                ht1.Add("U", "bingge");
                ht1.Add("p", DESEncrypt.MD5Encrypt("123456"));

                XmlDocument xmlDoc1 = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SignShow", ht1);
                String SMSign = xmlDoc1.InnerText;

                String registration = "【生鲜时逐】机器编号为{bh},产品名称{proName}明日订购料道数量不足";
                registration = registration.Replace("{bh}", bh);  //卡号
                registration = registration.Replace("{proName}", proName);  //卡号
                Hashtable ht = new Hashtable();
                ht.Add("U", "bingge");
                ht.Add("p", DESEncrypt.MD5Encrypt("123456"));
                ht.Add("N", member_phone);
                ht.Add("M", registration);
                ht.Add("T", "2");
                XmlDocument xmlDoc = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SendMes", ht);
                String status = xmlDoc.InnerText;
                string insert = "insert into asm_dgmsg(phone,sendTime,msg,type) values('" + member_phone + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + registration + "',4)";
                DbHelperSQL.ExecuteSql(insert);
                return status;
            }
            return "服务器异常";
        }
        public static void setCooki(string key, string val)
        {
            System.Web.HttpCookie httpCookie = new System.Web.HttpCookie(key);
            httpCookie.Value = val;
            httpCookie.Expires = System.DateTime.Now.AddYears(1);
            System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);

        }
        public static string getCooki(string key)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies.Get(key);
            string result;
            if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
            {
                result = "0";
            }
            else
            {
                result = httpCookie.Value;
            }
            return result;
        }

        #region 生成二维码

       



    
        #endregion
        /// <summary>
        /// 生成并保存二维码图片的方法
        /// </summary>
        /// <param name="str">输入的内容</param>
        public  static void CreateQRImg(string content,string str)
        {
            Bitmap bt;
            string enCodeString = content;
            //生成设置编码实例
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置二维码的规模，默认4
            qrCodeEncoder.QRCodeScale =6;
            //设置二维码的版本，默认7
            qrCodeEncoder.QRCodeVersion =0;
            //设置错误校验级别，默认中等
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //生成二维码图片
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            
            //二维码图片的名称
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss");
            //保存二维码图片在photos路径下
            //bt.Save(Server.MapPath("../qr/") + filename + ".jpg");
            //bt.Save("../qr/"+filename+".jpg");
            string path= System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+@"qr\1"+str+".jpg";
            bt.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
           
        }
        public static int DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            //dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return ts.Minutes;
        }


       // {"status":0,"result":{"location":{"lng":115.95845796638,"lat":28.696117043877},"formatted_address":"江西省南昌市青山湖区创新路1号","business":"高新开发区,火炬广场,发展路","addressComponent":{"city":"南昌市","district":"青山湖区","province":"江西省","street":"创新路","street_number":"1号"},"cityCode":163}}
        public static String getCity(String lat, String lng)
        {
            JObject jo = getLocationInfo(lat, lng);
            JObject j= (JObject)JsonConvert.DeserializeObject(jo["result"].ToString());
            JObject l= (JObject)JsonConvert.DeserializeObject(j["addressComponent"].ToString());
            
            return j["addressComponent"].ToString();
        }

        public static JObject getLocationInfo(String lat, String lng)
        {
            String url = "http://api.map.baidu.com/geocoder/v2/?location=" + lat + ","+ lng + "&output=json&ak=" + AK + "&pois=0";
 
            JObject jo = (JObject)JsonConvert.DeserializeObject(GetHttpResponse(url, 5000));
            return jo;
        }

        ///
        /// Get请求
        /// 
        /// 
        /// 字符串
        public static string GetHttpResponse(string url, int Timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = Timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        public static void insertNotice(string memberID, string title, string con)
        {
            string sql = "insert into asm_notice (memberID,title,con,time) values('" + memberID + "','" + title + "','" + con + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            DbHelperSQL.ExecuteSql(sql);
        }

         /// <summary>
          /// 将字典类型序列化为json字符串
          /// </summary>
          /// <typeparam name="TKey">字典key</typeparam>
          /// <typeparam name="TValue">字典value</typeparam>
          /// <param name="dict">要序列化的字典数据</param>
          /// <returns>json字符串</returns>
         public static string SerializeDictionaryToJsonString<TKey, TValue>(Dictionary<TKey, TValue> dict)
         {
             if (dict.Count == 0)
                 return "";
 
             string jsonStr = JsonConvert.SerializeObject(dict);
             return jsonStr;
         }
         /// <summary>
         /// 将json字符串反序列化为字典类型
         /// </summary>
         /// <typeparam name="TKey">字典key</typeparam>
         /// <typeparam name="TValue">字典value</typeparam>
         /// <param name="jsonStr">json字符串</param>
         /// <returns>字典数据</returns>
         public static Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
         {
             if (string.IsNullOrEmpty(jsonStr))
                 return new Dictionary<TKey, TValue>();
 
             Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);
 
             return jsonDict;
 
         }
        //获取微信消息模板的ID
        public static string getMessageID(string companyID, string templateBH)
        {
            string sql = "select * from [dbo].[asm_wxTemplate] where companyID=" + companyID + " and templateBH='" + templateBH + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["templateID"].ToString();
            }
            return "0";
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

            string sql1 = "insert into asm_moneyChange(payTime,money,AvaiilabMOney,memberID,type,bz,skID) values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + money + "," + avaMoney + "," + memberID + "," + type + ",'" + bz + "'," + skID + ")";
            DbHelperSQL.ExecuteSql(sql1);
        }
        /// <summary>
        /// 获取采购单数据
        /// </summary>
        /// <param name="mechineList"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static DataTable getCGD(string mechineList,string productID,string startTime,string endTime,string time) {
            if (!string.IsNullOrEmpty(mechineList))
            {
                string sql = "";
                string[] mechineIDArr = mechineList.Split(',');
                for (int i=0;i<mechineIDArr.Length;i++)
                {
                    string proID = "";
                    string sqlLd = "SELECT STUFF((SELECT ','+productID FROM  asm_ldinfo where mechineID in (" + mechineIDArr[i] + ")"
                        + " and productID  in(" + productID + ") and productID!='' and productID is not null order by productID  for xml path('')),1,1,'') productID";
                    DataTable dd = DbHelperSQL.Query(sqlLd).Tables[0];
                    if (dd.Rows.Count > 0)
                    {
                        proID = dd.Rows[0]["productID"].ToString();
                    }
                    sql += " SELECT *,"
                     + " isnull((select COUNT(*) totalDgNum from asm_orderlistDetail o where zt < 7 and o.productID = k.productID and createTime >= '" + startTime+"'and createTime <= '"+endTime+"' and mechineID="+mechineIDArr[i]+" group by productID),0) as totalDG"
                     +" FROM asm_kcDetail k WHERE(id IN(SELECT MAX([id])   FROM asm_kcDetail  where  mechineID = "+mechineIDArr[i]+ " and dateTime<'" + time + "' and imbalance>=0 and productID in (" + proID + ") group by productID))  union";
                }
                sql = sql.Substring(0,sql.Length-5);

                string sqlTotal = "select productID,(select bh from asm_product p where p.productID=A.productID) bh,"
                    + " (SELECT STUFF((SELECT ','+ldNO FROM  asm_ldinfo where productID=A.productID and mechineID in("+mechineList+") for xml path('')),1,1,'')  ldno) ldno,"
                    + "(select proname from asm_product p where p.productID=A.productID) proname,(select progg from asm_product p where p.productID=A.productID) progg,"
                    +" (select sluid from asm_product p where p.productID = A.productID) sluid,"
                    +" (select brandName from  asm_brand where id in(select brandID from asm_product where productID = A.productID))  brandName,"
                      + "sum(dgNum)dgNum,sum(lsNum)lsNum,sum(totalLsNum)totalLsNum,sum(imbalance)imbalance,sum(totalDgNum)totalDgNum,sum(totalDG)totalDG from (" + sql+ ") A where productID in ("+productID+ ") group by productID";
                string sql1 = "select * from (" + sqlTotal + ") B where B.ldno is not null";
                return DbHelperSQL.Query(sql1).Tables[0];
               

            }
            return null;
        }
        public static DataTable getJHD(string mechineList,string productID, string startTime, string endTime, string time)
        {
            if (!string.IsNullOrEmpty(mechineList))
            {
                string sql = "";
                string[] mechineIDArr = mechineList.Split(',');
                for (int i = 0; i < mechineIDArr.Length; i++)
                {
                    string proID = "";
                    string sqlLd = "SELECT STUFF((SELECT ','+productID FROM  asm_ldinfo where mechineID in (" + mechineIDArr[i] + ")"
                        + " and productID  in(" + productID + ") and productID!='' and productID is not null order by productID for xml path('')),1,1,'') productID";
                    DataTable dd = DbHelperSQL.Query(sqlLd).Tables[0];
                    if (dd.Rows.Count > 0)
                    {
                        proID = dd.Rows[0]["productID"].ToString();
                    }

                    sql += " SELECT *,"
                     + " isnull((select COUNT(*) totalDgNum from asm_orderlistDetail o where zt <7 and o.productID = k.productID and createTime >= '" + startTime + "'and createTime <= '" + endTime + "' and mechineID=" + mechineIDArr[i] + " group by productID),0) as totalDG"
                     + " FROM asm_kcDetail k WHERE(id IN(SELECT MAX([id])   FROM asm_kcDetail  where  mechineID = " + mechineIDArr[i] + " and dateTime<'" + time + "' and imbalance>=0 and productID in(" + proID + ") group by productID))  union";
                }
                sql = sql.Substring(0, sql.Length - 5);

                string sqlTotal = "select productID,(select bh from asm_product p where p.productID=A.productID) bh,"
                     + " (SELECT STUFF((SELECT ','+ldNO FROM  asm_ldinfo where productID=A.productID and mechineID in(" + mechineList + ") for xml path('')),1,1,'')  ldno) ldno,"
                    + " (select typeName from  asm_protype where productTypeID in (select  protype from asm_product where productID=A.productID)) typeName,"
                    + "(select proname from asm_product p where p.productID=A.productID) proname,(select progg from asm_product p where p.productID=A.productID) progg,"
                    + " (select sluid from asm_product p where p.productID = A.productID) sluid,"
                    + " (select brandName from  asm_brand where id in(select brandID from asm_product where productID = A.productID))  brandName,"
                    + "sum(dgNum)dgNum,sum(lsNum)lsNum,sum(totalLsNum)totalLsNum,sum(imbalance)imbalance,sum(totalDgNum)totalDgNum,sum(totalDG)totalDG from (" + sql + ") A where  mechineID in ("+ mechineList + ") group by productID";
                string sql1 = "select * from (" + sqlTotal + ") B where B.ldno is not null";
                return DbHelperSQL.Query(sql1).Tables[0];

            }
            return null;
        }
        public static DataTable getSHD(string mechineList, string productID, string startTime, string endTime, string time)
        {
            if (!string.IsNullOrEmpty(mechineList))
            {
                string sql = "";
                string[] mechineIDArr = mechineList.Split(',');
                for (int i = 0; i < mechineIDArr.Length; i++)
                {
                    sql += " SELECT *,"
                     + " isnull((select COUNT(*) totalDgNum from asm_orderlistDetail o where zt < 7 and o.productID = k.productID and createTime >= '" + startTime + "'and createTime <= '" + endTime + "' and mechineID=" + mechineIDArr[i] + " group by productID),0) as totalDG"
                     + " FROM asm_kcDetail k WHERE(id IN(SELECT MAX([id])   FROM asm_kcDetail  where  mechineID = " + mechineIDArr[i] + " and dateTime<'" + time + "' and productID in (" + productID + ") group by productID))  union";
                }
                sql = sql.Substring(0, sql.Length - 5);

                string sqlTotal = "select productID,(select bh from asm_product p where p.productID=A.productID) bh,"
                    + " (SELECT STUFF((SELECT ','+ldNO FROM  asm_ldinfo where productID=A.productID and mechineID in("+mechineList+") for xml path('')),1,1,'')  ldno) ldno,"
                    + " (select typeName from  asm_protype where productTypeID in (select  protype from asm_product where productID=A.productID)) typeName,"
                    + " (select proname from asm_product p where p.productID=A.productID) proname,(select progg from asm_product p where p.productID=A.productID) progg,"
                    + " (select sluid from asm_product p where p.productID = A.productID) sluid,"
                    + " (select brandName from  asm_brand where id in(select brandID from asm_product where productID = A.productID))  brandName,"
                    + "sum(dgNum)dgNum,sum(lsNum)lsNum,sum(totalLsNum)totalLsNum,sum(imbalance)imbalance,sum(totalDgNum)totalDgNum,sum(totalDG)totalDG from (" + sql + ") A where productID in (" + productID + ") and mechineID in (" + mechineList + ") group by productID";
                string sql1 = "select * from ("+sqlTotal+ ") B where B.ldno is not null order by B.ldno";
                return DbHelperSQL.Query(sql1).Tables[0];

            }
            return null;
        }
    }
}