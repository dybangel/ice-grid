using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Xml;
using Consumer.cls;
using DBUtility;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

/// <summary>
/// wxHelper 的摘要说明
/// </summary>
public partial class wxHelper
{
    /// <summary>
    /// 签名生成时间
    /// </summary>
    public  string dtime = "";
    /// <summary>
    /// 签名提交url地址
    /// </summary>
    public  string url = "";
    /// <summary>
    /// 生成签名的时间戳
    /// </summary>
    public  string time = "";
    /// <summary>
    /// 生成签名的随机串
    /// </summary>
    public  string randstr = "";
    /// <summary>
    /// 签名
    /// </summary>
    public  string signstr = "";
    /// <summary>
    /// appid
    /// </summary>
    private   string appid = "";
    /// <summary>
    /// secret
    /// </summary>
    private   string secret ="";
    public wxHelper(string companyID)
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
      
        string sql = "select * from asm_company where id="+companyID;
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if (dt.Rows.Count>0)
        {
            appid = dt.Rows[0]["wx_appid"].ToString();
            secret = dt.Rows[0]["wx_appsecret"].ToString();
        }
       
    }
    /// <summary>
    /// 设置行业 31 消费品行业
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public string setIndustry(string accessToken)
    {

        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        sb.Append("\"industry_id1\":\"31\",");
        sb.Append("\"industry_id2\":\"41\"");
        sb.Append("}");
        string url = string.Format("https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token={0}", accessToken);
        HttpWebRequest hwr = WebRequest.Create(url) as HttpWebRequest;
        hwr.Method = "POST";
        hwr.ContentType = "application/x-www-form-urlencoded";
        byte[] payload;
        payload = System.Text.Encoding.UTF8.GetBytes(sb.ToString()); //通过UTF-8编码  
        hwr.ContentLength = payload.Length;
        Stream writer = hwr.GetRequestStream();
        writer.Write(payload, 0, payload.Length);
        writer.Close();
        var result = hwr.GetResponse() as HttpWebResponse; //此句是获得上面URl返回的数据  
        string strMsg = WebResponseGet(result);
        return strMsg;
    }
    /// <summary>
    /// 获取行业
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public string getIndustry(string accessToken)
    {
        string jsonresult = "";
        string ticketUrl = "https://api.weixin.qq.com/cgi-bin/template/get_industry?access_token=" + accessToken;
        jsonresult = HttpGet(ticketUrl, "UTF-8");
        return jsonresult;
    }
    /// <summary>
    /// 获得模板ID
    /// </summary>
    /// <param name="accessToken"></param>
    /// template_id_short 
    /// <returns></returns> 
    public string getTemplateId(string accessToken, string template_id_short)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        sb.Append("\"template_id_short\":\"" + template_id_short + "\",");
        sb.Append("}");
        string url = string.Format("https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}", accessToken);
        HttpWebRequest hwr = WebRequest.Create(url) as HttpWebRequest;
        hwr.Method = "POST";
        hwr.ContentType = "application/x-www-form-urlencoded";
        byte[] payload;
        payload = System.Text.Encoding.UTF8.GetBytes(sb.ToString()); //通过UTF-8编码  
        hwr.ContentLength = payload.Length;
        Stream writer = hwr.GetRequestStream();
        writer.Write(payload, 0, payload.Length);
        writer.Close();
        var result = hwr.GetResponse() as HttpWebResponse; //此句是获得上面URl返回的数据  
        string strMsg = WebResponseGet(result);

        JObject jo = (JObject)JsonConvert.DeserializeObject(strMsg);
        string errcode = jo["errcode"].ToString();
        string errmsg = jo["errmsg"].ToString();
        string template_id = "";
        if (errmsg == "ok" && errcode == "0")
        {
            template_id = jo["template_id"].ToString();
            return template_id;
        }
        return template_id;
    }
    public static string WebResponseGet(HttpWebResponse webResponse)
    {
        StreamReader responseReader = null;
        string responseData = "";
        try
        {
            responseReader = new StreamReader(webResponse.GetResponseStream());
            responseData = responseReader.ReadToEnd();
        }
        catch
        {
            throw;
        }
        finally
        {
            webResponse.GetResponseStream().Close();
            responseReader.Close();
            responseReader = null;
        }
        return responseData;
    }
    /// <summary>
    /// 获得accesstoken
    /// </summary>
    /// <returns></returns>
    /// 
    /// <summary>  
    /// HttpGET请求  
    /// </summary>  
    /// <param name="url">请求地址</param>  
    /// <param name="encode">编码方式：GB2312/UTF-8</param>  
    /// <returns>字符串</returns>  
    private string HttpGet(string url, string encode)
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
    public class WX_Ticket
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public string expires_in { get; set; }
    }
    public string GetTicket(string companyID)
    {
        string jsonresult = "";
        string token = IsExistAccess_Token(companyID);
      
        string ticketUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token + "&type=jsapi";
        jsonresult = HttpGet(ticketUrl, "UTF-8");
        WX_Ticket wxTicket = JsonDeserialize<WX_Ticket>(jsonresult);
        return wxTicket.ticket;
    }
    /// <summary>  
    /// JSON反序列化  
    /// </summary>  
    /// <typeparam name="T">实体类</typeparam>  
    /// <param name="jsonString">JSON</param>  
    /// <returns>实体类</returns>  
    private T JsonDeserialize<T>(string jsonString)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)ser.ReadObject(ms);
        return obj;
    }
    public  string AccessToken()
    {
        return SendRequest("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret, Encoding.UTF8);
    }

/// <summary>
/// 根据accesstoken获得ticket
/// </summary>
/// <returns></returns>
//public  string GetTicket1(string companyID)
//{
//        //string access_token = AccessToken();
//        //string url1 = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + access_token.Substring(access_token.IndexOf(':') + 2, access_token.IndexOf(',') - 3 - access_token.IndexOf(':')) + "&type=jsapi";
//        //string requstStr = SendRequest(url1, Encoding.UTF8);
//        string requstStr = IsExistAccess_Token(companyID);
//        Util.Debuglog("requstStrt=" + requstStr + "---", "_Debuglog.txt");
//        string ticket = requstStr.Substring(requstStr.IndexOf("ticket") + 9, requstStr.LastIndexOf(',') - 1 - requstStr.IndexOf("ticket") - 9);// 获得json参数没搞，懂的自己优化
//        Util.Debuglog("ticketticketticketticket=" + ticket + "---", "_Debuglog.txt");
//        return ticket;
//}
/// <summary>
/// 获取jssdk所需签名
/// </summary>
/// <returns></returns>
public  string GetSignature(string link, string noncestr, int timestamp,string companyID)
{
    DateTime dti = DateTime.Now;
    dtime = dti.ToString("yyyy-MM-dd HH:mm:ss");
    string a = IsExistAccess_Token(companyID);
    wxHelper wx = new wxHelper(companyID);
    string ticket = wx.GetTicket(companyID);
    time = "1510124527";
    randstr = noncestr;
    string string1 = "jsapi_ticket=" + ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + link;
    url = string1;
    string signature = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1");
    return signature.ToLower(); // 生成后一定转换为小写
}
/// <summary>   
/// Get方式获取url地址输出内容   
/// </summary> /// <param name="url">url</param>   
/// <param name="encoding">返回内容编码方式，例如：Encoding.UTF8</param>   
public  string SendRequest(string url, Encoding encoding)
{
    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
    webRequest.Method = "GET";
    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
    StreamReader sr = new StreamReader(webResponse.GetResponseStream(), encoding);
    string str = sr.ReadToEnd();
    return str;
}
/// <summary>
/// 获得微信权限信息，格式：时间戳,随机数,签名
/// </summary>
/// <param name="link"></param>
/// <returns></returns>
public  string GetWXInfo(string link,string companyID)
{
    DateTime dti = DateTime.Now;
    dtime = dti.ToString("yyyy-MM-dd HH:mm:ss");
    string noncestr = dti.ToString("yyyyMMddHHmmss");
    int timestamp = 1510124527;
    string ticket = IsExistAccess_Token(companyID);
    time = "1510124527";
    randstr = noncestr;
    signstr = GetSignature(link, noncestr, timestamp, companyID);
    return time + "," + randstr + "," + signstr;
}


public class Access_token
{
    public Access_token()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    string _access_token;
    string _expires_in;

    /// <summary>
    /// 获取到的凭证 
    /// </summary>
    public string access_token
    {
        get { return _access_token; }
        set { _access_token = value; }
    }

    /// <summary>
    /// 凭证有效时间，单位：秒
    /// </summary>
    public string expires_in
    {
        get { return _expires_in; }
        set { _expires_in = value; }
    }
}
    public Access_token GetAccess_token()
    {
       
        string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
        Access_token mode = new Access_token();
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
        req.Method = "GET";
        using (WebResponse wr = req.GetResponse())
        {
            HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();
            //Response.Write(content);
            //在这里对Access_token 赋值
            Access_token token = new Access_token();
            JsonHelper jh = new JsonHelper();
            token = jh.ParseFromJson<Access_token>(content);
            mode.access_token = token.access_token;
            mode.expires_in = token.expires_in;

        }
        return mode;
    }
    /// <summary>
    /// 根据当前日期 判断Access_Token 是否超期 如果超期返回新的Access_Token 否则返回之前的Access_Token
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    public string IsExistAccess_Token(string companyID)
    {
        string Token = RedisHelper.GetRedisModel<string>(companyID+"_accessToken");
        if (string.IsNullOrEmpty(Token))
        {
            Token = GetAccess_token().access_token;
            RedisHelper.SetRedisModel<string>(companyID + "_accessToken", Token, new TimeSpan(1, 55, 0));
        }
        else {
        }
        return Token;

        //string Token = string.Empty;
        //DateTime YouXRQ;
        //// 读取XML文件中的数据，并显示出来 ，注意文件路径
        //string sql = "select * from asm_company where id=" + companyID;
        //DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        //if (dt.Rows.Count > 0)
        //{
        //    Token = dt.Rows[0]["Access_Token"].ToString();
        //    if (Token == "")
        //    {
        //        Token = GetAccess_token().access_token;
        //    }

        //    YouXRQ = Convert.ToDateTime(dt.Rows[0]["yxq"].ToString());
        //    if (DateTime.Now > YouXRQ)
        //    {
        //        DateTime _youxrq = DateTime.Now;
        //        Access_token mode = GetAccess_token();
        //        _youxrq = _youxrq.AddSeconds(1000*60*120);
        //        string sql1 = "update asm_company set Access_Token='" + mode.access_token + "',yxq='" + _youxrq.ToString() + "' where id=" + companyID;
        //        DbHelperSQL.ExecuteSql(sql1);
        //        Token = mode.access_token;
        //    }
        //    return Token;
        //}
        //else
        //{
        //    return "";
        //}
    }
    public class JsonHelper
{
    /// <summary>
    /// 生成Json格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public  string GetJson<T>(T obj)
    {
        DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
        using (MemoryStream stream = new MemoryStream())
        {
            json.WriteObject(stream, obj);
            string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
        }
    }
    /// <summary>
    /// 获取Json的Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="szJson"></param>
    /// <returns></returns>
    public  T ParseFromJson<T>(string szJson)
    {
        T obj = Activator.CreateInstance<T>();
        using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            return (T)serializer.ReadObject(ms);
        }
    }
       
    }
}