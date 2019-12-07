using autosell_center.util;
using Consumer.cls;
using Consumer.lib;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenPlatForm.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center
{
    public partial class getWXToAppCall : System.Web.UI.Page
    {

        protected string sum = "0";
        protected string headImg = "";
        protected string id = "";
        protected string name = "";
        public string reurl = "";
        private string appid = "";
        private string appsecret = "";
        private string money = "0";
        private string unionID = "";
        private CommonMethod.RefreshToken tokenModel = new CommonMethod.RefreshToken();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string code = "";
                if (Request.QueryString["code"] != null && Request.QueryString["code"] != "")
                {
                    code = Request.QueryString["code"].ToString();
                    Util.Debuglog("ceshicode=" + code, "公众号绑定openid.txt");
                    string[] param = Request.QueryString["state"].ToString().Split('|');

                    string companyID = param[0];//14
                    Util.Debuglog("companyID=" + companyID, "公众号绑定openid.txt");


                    string sql2 = "select * from asm_company where id=" + companyID;
                    DataTable d1 = DbHelperSQL.Query(sql2).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        appid = d1.Rows[0]["appId"].ToString();
                        appsecret = d1.Rows[0]["wx_appsecret"].ToString();
                    }

                    OAuth_Token Model = Get_token(code, companyID);
                    Util.Debuglog("Model=" + Model, "公众号绑定openid.txt");
                    Util.Debuglog("Model.access_token=" + Model.access_token, "公众号绑定openid.txt");
                    Util.Debuglog("Model.openid=" + Model.openid, "公众号绑定openid.txt");
                    OAuthUser OAuthUser_Model = Get_UserInfo(Model.access_token, Model.openid);
                    Util.Debuglog("OAuthUser_Model=" + OAuthUser_Model.openid, "公众号绑定openid.txt");
                    if (OAuthUser_Model.openid != null && OAuthUser_Model.openid != "")  //已获取得openid及其他信息
                    {
                        Util.Debuglog("进入=" + OAuthUser_Model.openid, "公众号绑定openid.txt");
                        headImg = OAuthUser_Model.headimgurl.ToString();//头像图片
                        name = OAuthUser_Model.nickname;//昵称
                        id = OAuthUser_Model.openid;//opendid
                        unionID = OAuthUser_Model.unionid;
                        string province = OAuthUser_Model.province;
                        string city = OAuthUser_Model.city;
                        string country = OAuthUser_Model.city;
                        string gender = OAuthUser_Model.sex == "1" ? "男" : "女";
                       
                       
                        Util.Debuglog("id=" + id, "公众号绑定openid.txt");//o1_mf1aL2bduKZnTzG1irrfvN0x8

                        string sql = "select * from asm_member where unionID='" + unionID + "'";
                        Util.Debuglog("sql=" + sql, "公众号绑定openid.txt");
                        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                        if (dt.Rows.Count <= 0)
                        {
                            string insert = "insert into asm_member(name,phone,province,city,country,AvailableMoney,sumConsume,sumRecharge,createDate,companyID,headurl,nickname,sex,unionID,openID,consumeCount)"
                                + " values(N'" + name + "','','" + province + "','" + city + "','" + country + "',0,0,0,'" + DateTime.Now + "','" + companyID + "','" + headImg + "',N'" + name + "','" + gender + "','" + unionID + "','" + id + "',0)";
                            Util.Debuglog(insert, "公众号绑定openid.txt");
                            DbHelperSQL.ExecuteSql(insert);
                            Response.Write("<span style='color:#FF0000;font-size:200px'>" + "已绑定成功！" + "</span>");
                        }
                        else{
                        

                            string update = "update asm_member set openID='" + id + "' where unionID='" + unionID + "'";
                            Util.Debuglog("更新" + update, "公众号绑定openid.txt");
                            DbHelperSQL.ExecuteSql(update);
                            
                             Response.Write("<div style='width:100%;height:100%;font-family:楷体;font-size:160px;text-align:center;color:#FF0000;'> 已绑定成功! </ div >");
                        }
                        

                    }
                }
            }

        }
        protected OAuth_Token Get_token(string Code, string companyID)
        {

            string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/component/access_token?appid=" + appid + "&code=" + Code + "&grant_type=authorization_code&component_appid=" + OpenPFConfig.Appid + "&component_access_token=" + Util.getComAccessToken() + "");
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
            Util.Debuglog(companyID + "服务器token=" + Str, "获取token.txt");
            return Oauth_Token_Model;

        }
         protected OAuthUser Get_UserInfo(string REFRESH_TOKEN, string OPENID)
        {
            Util.Debuglog("Get_UserInfo=1", "公众号绑定openid.txt");
            string userinfo = RedisHelper.GetRedisModel<string>(OPENID);
            Util.Debuglog("userinfo="+ userinfo, "公众号绑定openid.txt");
            if (string.IsNullOrEmpty(userinfo))
            {
                Util.Debuglog("userinfo1=" + userinfo, "公众号绑定openid.txt");
                string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID + "&lang=zh_CN");
                Util.Debuglog("Str=" + Str, "公众号绑定openid.txt");
                Util.Debuglog("REFRESH_TOKEN=" + REFRESH_TOKEN + ";OPENID=" + OPENID, "公众号绑定openid.txt");
                OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(Str);
                Util.Debuglog("out=" + OAuthUser_Model, "公众号绑定openid.txt");
                return OAuthUser_Model;
            }
            else {
                Util.Debuglog("else" , "公众号绑定openid.txt");
                OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(userinfo);
                Util.Debuglog("else=" + OAuthUser_Model, "公众号绑定openid.txt");
                return OAuthUser_Model;
            }
          
        }
        public class OAuth_Token
        {
            public OAuth_Token() { }
            public string _access_token;
            public string _expires_in;
            public string _refresh_token;
            public string _openid;
            public string _scope;

            public string access_token
            {
                set { _access_token = value; }
                get { return _access_token; }
            }
            public string expires_in
            {
                set { _expires_in = value; }
                get { return _expires_in; }
            }

            public string refresh_token
            {
                set { _refresh_token = value; }
                get { return _refresh_token; }
            }
            public string openid
            {
                set { _openid = value; }
                get { return _openid; }
            }
            public string scope
            {
                set { _scope = value; }
                get { return _scope; }
            }

        }
        public class OAuthUser
        {
            public OAuthUser() { }

            private string _openID;
            private string _searchText;
            private string _nickname;
            private string _sex;
            private string _province;
            private string _city;
            private string _country;
            private string _headimgUrl;
            private string _privilege;
            private string _unionid;

            public string unionid
            {
                set { _unionid = value; }
                get { return _unionid; }
            }
            public string openid
            {
                set { _openID = value; }
                get { return _openID; }
            }

            public string SearchText
            {
                set { _searchText = value; }
                get { return _searchText; }
            }

            public string nickname
            {
                set { _nickname = value; }
                get { return _nickname; }
            }

            public string sex
            {
                set { _sex = value; }
                get { return _sex; }
            }

            public string province
            {
                set { _province = value; }
                get { return _province; }
            }

            public string city
            {
                set { _city = value; }
                get { return _city; }
            }

            public string country
            {
                set { _country = value; }
                get { return _country; }
            }

            public string headimgurl
            {
                set { _headimgUrl = value; }
                get { return _headimgUrl; }
            }

            public string privilege
            {
                set { _privilege = value; }
                get { return _privilege; }
            }

        }
        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);

            if (returnText.Contains("errcode"))
            {

            }
            return returnText;
        }
        public class JsonHelper
        {

            public static string GetJson<T>(T obj)
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, obj);
                    string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
                }
            }

            public static T ParseFromJson<T>(string szJson)
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
}