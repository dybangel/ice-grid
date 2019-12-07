using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using OpenPlatForm.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using WeiXin.Lib.Core.Consts;
using WeiXin.Lib.Core.Helper;
using WeiXin.Lib.Core.Models;
using WeiXin.Lib.Core.Models.Message;

namespace autosell_center.util
{
    public static class OperUtil
    {
        public static string DataTableToJsonWithJsonNet(DataTable table)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.None);
            return jsonString;
        }
        public static void Add(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 30;
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
        //public static string getCompanyID()
        //{
        //    string sql = "select * from asm_member where id="+Util.getMemberID();
        //    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        return dt.Rows[0]["companyID"].ToString();
        //    }
        //    else {
        //        return "0";
        //    }
        //}
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

        #region 生成二维码
   
           /// <summary>
           /// 获取Ticket
           /// </summary>
           /// <returns></returns>
           private static string CreateTicket(string scene_id)
           {
               var token = AccessToken.Instance;
 
              if (string.IsNullOrEmpty(token.Access_Token))
                  throw new ArgumentNullException("Access_Token");
  
              string url = string.Format(WeiXinConst.WeiXin_Ticket_CreateUrl, token.Access_Token);
              string postData = WeiXinConst.WeiXin_QrCodeTicket_Create_JsonString.Replace("{0}", scene_id);
  
              string result = HttpClientHelper.PostResponse(url, postData);
              Ticket ticket = JsonConvert.DeserializeObject<Ticket>(result); //HttpClientHelper.PostResponse<Ticket>(url, postData);
  
              if (ticket == null || string.IsNullOrEmpty(ticket.ticket))
             {
                  ErrorMessage msg = JsonConvert.DeserializeObject<ErrorMessage>(result);
                 if (msg.TokenExpired)
                     return CreateTicketByNewAccessToken(scene_id);
              }
              return ticket.ticket;
          }
  
          /// <summary>
          /// 当AccessToken过期时 调用此方法
          /// </summary>
          /// <param name="scene_id"></param>
          /// <returns></returns>
          private static string CreateTicketByNewAccessToken(string scene_id)
          {
              var token = AccessToken.NewInstance;
  
              if (string.IsNullOrEmpty(token.Access_Token))
                  throw new ArgumentNullException("Access_Token");
  
              string url = string.Format(WeiXinConst.WeiXin_Ticket_CreateUrl, token.Access_Token);
              string postData = WeiXinConst.WeiXin_QrCodeTicket_Create_JsonString.Replace("{0}", scene_id);
  
              Ticket ticket = HttpClientHelper.PostResponse<Ticket>(url, postData);
  
              if (ticket == null || string.IsNullOrEmpty(ticket.ticket))
                  throw new Exception("Ticket为Null，AccessToken:" + token.Access_Token);
              return ticket.ticket;
          }
  
  
          /// <summary>
          /// 根据Ticket获取二维码图片保存在本地
          /// </summary>
          /// <param name="scene_id">二维码场景id</param>
          /// <param name="imgPath">图片存储路径</param>
          public static void SaveQrCodeImage(string scene_id, string imgPath)
          {
              string Ticket = CreateTicket(scene_id);
  
              if (Ticket == null)
                  throw new ArgumentNullException("Ticket");
  
              //ticket需 urlEncode
              string stUrl = string.Format(WeiXinConst.WeiXin_QrCode_GetUrl, HttpUtility.UrlEncode(Ticket));
  
              HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);
  
              req.Method = "GET";
  
              using (WebResponse wr = req.GetResponse())
              {
                  HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                  string strpath = myResponse.ResponseUri.ToString();
  
                  WebClient mywebclient = new WebClient();
  
                  try
                  {
                      mywebclient.DownloadFile(strpath, imgPath);
                  }
                  catch (Exception)
                  {
                      throw new Exception("获取二维码图片失败！");
                  }
              }
          }
  
          /// <summary>
          /// 根据SceneId 获取 二维码图片流
          /// </summary>
          /// <param name="scene_id"></param>
          /// <returns></returns>
          public static Stream GetQrCodeImageStream(string scene_id)
          {
              string Ticket = CreateTicket(scene_id);
  
              if (Ticket == null)
                  throw new ArgumentNullException("Ticket");
 
             //ticket需 urlEncode
             string strUrl = string.Format(WeiXinConst.WeiXin_QrCode_GetUrl, HttpUtility.UrlEncode(Ticket));
             try
             {
                 System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                 Byte[] bytes = client.GetByteArrayAsync(strUrl).Result;
                 return new MemoryStream(bytes);
             }
             catch
             {
                 throw new Exception("获取二维码图片失败！");
             }
         }

        #endregion
        /// <summary>
        /// 生成并保存二维码图片的方法
        /// </summary>
        /// <param name="str">输入的内容</param>
        public static void CreateQRImg(string str)
        {
            Bitmap bt;
            string enCodeString = str;
            //生成设置编码实例
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置二维码的规模，默认4
            qrCodeEncoder.QRCodeScale = 2;
            //设置二维码的版本，默认7
            qrCodeEncoder.QRCodeVersion = 0;
            //设置错误校验级别，默认中等
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //生成二维码图片
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);

            //二维码图片的名称
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss");
            //保存二维码图片在photos路径下
            //bt.Save(Server.MapPath("../qr/") + filename + ".jpg");
            //bt.Save("../qr/"+filename+".jpg");
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"qr\" + str + ".jpg";
            bt.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

        }
        public static string ModelMessageSend(string data, string companyID)
        {
            wxHelper wx = new wxHelper(companyID);
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + wx.IsExistAccess_Token(companyID);
            string result = HttpPost(url, data);
            if (result.Contains("ok"))
            {
                return "OK";
            }
            else
            {
                return "消息推送失败,具体错误为:" + result;
            }
        }
        public  static string HttpPost(string url, string postData)
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.KeepAlive = true;
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();

            request.Abort();
            response.Close();
            reader.Dispose();
            stream.Close();
            stream.Dispose();

            return content;
        }
        //会员充值模板
        public static string cz(string receiveID,string modelID)
        {
            string str = "{\"touser\":\"" + receiveID + "\",\"template_id\":\""+modelID+"\",\"url\":\"\",\"topcolor\":\"#FF0000\",\"data\":{\"first\": {\"value\":\"恭喜,您已成功充值\",\"color\":\"#000000\"},\"accountType\":{\"value\":\"会员卡号\",\"color\":\"#000000\"},\"account\":{\"value\":\"8888\",\"color\":\"#000000\"},\"amount\":{\"value\":\"99\",\"color\":\"#000000\"},\"result\":{\"value\":\"充值成功\",\"color\":\"#000000\"},\"remark\":{\"value\":\"欢迎下次充值\",\"color\":\"#000000\"}}}";
            return str;
        }
        //获取微信消息模板的ID
        public static string getMessageID1(string companyID, string templateBH)
        {
            string sql = "select * from asm_company where id=" + companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            wxHelper wx = new wxHelper(companyID);
            string token = Util.GetTokenInfo(OpenPFConfig.Appid, dt.Rows[0]["appId"].ToString(), Util.getComAccessToken(), dt.Rows[0]["refresh_token"].ToString()).authorizer_access_token;
            string templageID = wx.getTemplateId(token, templateBH);
            Util.Debuglog("templageID=" + templageID, "_获取模板ID");
            if (templageID != "")
            {
                return templageID;
            }
            return "0";
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
        public static void insertNotice(string memberID, string title, string con)
        {
            string sql = "insert into asm_notice (memberID,title,con,time) values('" + memberID + "','" + title + "','" + con + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            DbHelperSQL.ExecuteSql(sql);
        }
        public static string getOperaID()
        {
            string openID = OperUtil.getCooki("vshop_openID").ToString();
            string sql = "select * from asm_opera where id='" + openID + "'";
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
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static string getOrderNO(string mechineID)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return mechineID+ret;
           
        }
        /// <summary>
        /// 获取配送日期
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="psMode">1 隔一天 2 隔两天 3 隔三天 4周一到周五 5 周末</param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public static string getSelDate(string totalDays, string psMode,string startTime)
        {
            string str="";
            string zq = totalDays;
            if (psMode == "1")
            {
                str=getDataTimeDay(psMode,zq, startTime);
            } else if (psMode == "2")
            {
                str=getDataTimeDay(psMode, zq, startTime);
            } else if (psMode=="3")
            {
                str=getDataTimeDay(psMode, zq, startTime);
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
                    t = DateTime.Parse(qsDate).AddDays(N -2).ToString("yyyy-MM-dd");
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
    }
}