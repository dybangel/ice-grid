using autosell_center.cls;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Tencent;
using WeiXin.Lib.Core.Helper;
using WeiXin.Lib.Core.Models;

namespace autosell_center.api
{
    /// <summary>
    /// WXApi 的摘要说明
    /// </summary>
    public class WXApi : IHttpHandler
    {
        string sToken = "3704021990";
        string sAppID = "";
        string sEncodingAESKey = "3704021990041019173704021990041019171234567";
        public static String getUserInfo(String opendID)
        {

            wxHelper wx = new wxHelper("14");
            String GET_USERINFO_URL="https://api.weixin.qq.com/cgi-bin/user/info?access_token="+ wx.IsExistAccess_Token("14")+ "&openid="+ opendID + "&lang=zh_CN";
            Util.Debuglog("GET_USERINFO_URL：" + GET_USERINFO_URL, "wxapi.txt");

            return WXApi.HttpGet(GET_USERINFO_URL, "UTF-8");
        }
        public static string HttpGet(string url, string encode)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=" + encode;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(encode));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        public void ProcessRequest(HttpContext context)
        {
            try
            { 
                context.Request["appID"].ToString().Replace("/","") ;
                sAppID = OpenPFConfig.Appid;//开放平台的Appid
                Stream stream = context.Request.InputStream;
                byte[] byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, (int)stream.Length);
                string postXmlStr = System.Text.Encoding.UTF8.GetString(byteArray);
                Util.Debuglog("微信："+ postXmlStr, "wxapi.txt");
                if (!string.IsNullOrEmpty(postXmlStr))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(postXmlStr);
                    if (!string.IsNullOrWhiteSpace(sAppID))  //没有AppID则不解密(订阅号没有AppID)
                    {
                        //解密 
                        WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
                        string signature = context.Request["msg_signature"];
                        string timestamp = context.Request["timestamp"];
                        string nonce = context.Request["nonce"];
                        string stmp = "";
                        int ret = wxcpt.DecryptMsg(signature, timestamp, nonce, postXmlStr, ref stmp);
                        if (ret == 0)
                        {
                            doc = new XmlDocument();
                            Util.Debuglog("stmp" + stmp, "wxapi.txt");
                            doc.LoadXml(stmp);
                            try
                            {
                                responseMsg(context, doc);
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }
                        else
                        {
                            Util.Debuglog("解密失败，错误码：" + ret, "wxapi.txt");
                        }
                    }
                    else
                    {
                        responseMsg(context, doc);
                    }

                    responseMsg(context, doc);
                }
                else
                {
                    valid(context);
                }
            }
            catch (Exception ex)
            {
                Util.Debuglog("ex" + ex, "wxapi.txt");
                responseMsg(context, null);
                //    FileLogger.WriteErrorLog(context, ex.Message);
            }
             
        }
        public void valid(HttpContext context)
        {
            var echostr = context.Request["echoStr"].ToString();
            if (checkSignature(context) && !string.IsNullOrEmpty(echostr))
            {
                context.Response.Write(echostr);
                context.Response.Flush();//推送...不然微信平台无法验证token
            }
        }
 
        public bool checkSignature(HttpContext context)
        {
            var signature = context.Request["signature"].ToString();
            var timestamp = context.Request["timestamp"].ToString();
            var nonce = context.Request["nonce"].ToString();
            var token = "qweq";
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public void responseMsg(HttpContext context, XmlDocument xmlDoc)
        {
             
            string result = "";
            string msgType = WeiXinXML.GetFromXML(xmlDoc, "MsgType");
            switch (msgType)
            {
                case "event":
                    switch (WeiXinXML.GetFromXML(xmlDoc, "Event"))
                    {
                        case "subscribe": //订阅
                            String retUserInfo = getUserInfo(WeiXinXML.GetFromXML(xmlDoc, "FromUserName"));
                            Util.Debuglog("getUserInfo" + retUserInfo, "wxapi.txt");
                            if (retUserInfo.Contains("errcode"))
                            {

                            }
                            else
                            {
                                WeiXinUserInfo info = JsonConvert.DeserializeObject<WeiXinUserInfo>(retUserInfo);
                                if (!string.IsNullOrEmpty(info.OpenId) && !string.IsNullOrEmpty(info.UnionId))
                                {
                                    Util.Debuglog("info=" + info.ToString(), "wxapi.txt");
                                    string sql = "select * from asm_member where unionID='" + info.UnionId + "'";
                                    Util.Debuglog("sql=" + sql, "wxapi.txt");
                                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                                    if (dt.Rows.Count <= 0)
                                    {
                                        string insert = "insert into asm_member(name,phone,province,city,country,AvailableMoney,sumConsume,sumRecharge,createDate,companyID,headurl,nickname,sex,unionID,openID,consumeCount)"
                                            + " values(N'" + info.NickName + "','','" + info.Province + "','" + info.City + "','" + info.Conuntry + "',0,0,0,'" + DateTime.Now + "',14,'" + info.HeadImgUrl + "',N'" + info.NickName + "','" + info.Sex + "','" + info.UnionId + "','" + info.OpenId + "',0)";
                                        Util.Debuglog(insert, "wxapi.txt");
                                        DbHelperSQL.ExecuteSql(insert);
                                    }
                                    else
                                    {
                                        //更新
                                        string update = "update asm_member set openID='" + info.OpenId + "' where unionID='" + info.UnionId + "'";
                                        Util.Debuglog("更新" + update, "wxapi.txt");
                                        DbHelperSQL.ExecuteSql(update);
                                    }

                                }
                            }
                           
                            string sqlcom = "select * from asm_company where user_name='" + WeiXinXML.GetFromXML(xmlDoc, "ToUserName") + "'";
                            Util.Debuglog("sqlcom=" + sqlcom, "wxapi.txt");
                            DataTable dtcom = DbHelperSQL.Query(sqlcom).Tables[0];
                            string txt ="";
                            if (dtcom.Rows.Count > 0){ 
                                txt = dtcom.Rows[0]["subscribe_info"].ToString();
                            }
                            result = WeiXinXML.CreateTextMsg(xmlDoc,txt);
                            Util.Debuglog("用户关注result="+ result, "wxapi.txt");
                            
                            break;
                        case "unsubscribe": //取消订阅
                            Util.Debuglog("取消订阅", "wxapi.txt");
                            break;
                        case "LOCATION":
                            string Latitude = WeiXinXML.GetFromXML(xmlDoc, "Latitude");
                            string Longitude = WeiXinXML.GetFromXML(xmlDoc, "Longitude");
                            Util.Debuglog("Latitude="+ Latitude+ ";Longitude="+ Longitude, "wxapi.txt");
                            break;
                        default:
                           
                            
                            break;
                    }
                    break;
                 
                default:
                    break;
            }

            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
            string signature = context.Request["msg_signature"];
            string timestamp = context.Request["timestamp"];
            string nonce = context.Request["nonce"];
            string stmp = "";
            int ret = wxcpt.EncryptMsg(result, timestamp, nonce, ref stmp);
            context.Response.Write(stmp);

            context.Response.Flush();
        }
  
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}