using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Configuration;
using WebClient = System.Net.WebClient;
using System.Data;
using Consumer.cls;
using DBUtility;
using System.Runtime.Serialization.Json;
using autosell_center.util;
using OpenPlatForm.Common;
using Consumer.lib;

namespace Mine.Members
{
    public partial class WXinformation : System.Web.UI.Page
    {
        protected string sum = "0";
        protected string tupian = "";
        protected string id = "";
        protected string name = "";
        public string reurl = "";
        private string appid = "";
        private string appsecret = "";
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
                    Util.Debuglog("code=" + code, "_获取会员信息.txt");
                    string companyID = Request.QueryString["companyID"].ToString();
                    string sql2 = "select * from asm_company where id=" + companyID;
                    DataTable d1 = DbHelperSQL.Query(sql2).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        appid = d1.Rows[0]["appId"].ToString();
                        appsecret = d1.Rows[0]["wx_appsecret"].ToString();
                    }
                    OAuth_Token Model = Get_token(code, companyID);
                    OAuthUser OAuthUser_Model = Get_UserInfo(Model.access_token, Model.openid);
                    //OAuthUser OAuthUser_Model = Get_UserInfo(Model.access_token, Model.openid);
                    if (OAuthUser_Model.openid != null && OAuthUser_Model.openid != "")  //已获取得openid及其他信息
                    {
                        tupian = OAuthUser_Model.headimgurl.ToString();//头像图片
                        name = OAuthUser_Model.nickname;//昵称
                        id = OAuthUser_Model.openid;//opendid
                        unionID = OAuthUser_Model.unionid;
                        string country = OAuthUser_Model.country;
                        string province = OAuthUser_Model.province;
                        string city = OAuthUser_Model.city;
                        Util.Debuglog("unionid=" + OAuthUser_Model.unionid, "_获取会员信息.txt");
                        OperUtil.setCooki("vshop_openID", id);
                        string sql = "select * from asm_member where openID='" + id + "'or unionID='"+unionID+"' and companyID=" + companyID;
                        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                        wxHelper wx = new wxHelper(companyID);
                        wx.Get_UserInfo(id);
                        if (dt.Rows.Count <= 0)
                        {
                            //name= System.Web.HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8);
                            //添加会员 会员不一定必须绑定机器  只有会员有订购产品的时候才必须绑定机器
                            string sql1 = "INSERT INTO [dbo].[asm_member]"
                                     + " ([name],[phone],[QQ],[province],[city],[country],[addres],[AvailableMoney],[sumConsume],[sumRecharge],[createDate],[mechineID],[companyID],[age],[LastTime],[memberBH],[consumeCount],[openID],[brithday],[headurl],[nickname],[pwd],[utfName],unionID)"
                                 + " VALUES(N'" + name + "','','','" + province + "','" + city + "','','',0,0,0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','','" + companyID + "','0','','',0,'" + id + "','','" + tupian + "',N'" + name + "','000000','"+ System.Web.HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8) + "','"+unionID+"')";
                            DbHelperSQL.ExecuteSql(sql1);
                            Util.Debuglog("sql1=" + sql1 + ";会员注册", "首页_.txt");
                            string data = TemplateMessage.Member_ZC(id, OperUtil.getMessageID(companyID, "OPENTM203347141"), "恭喜您注册成为会员！", name, "恭喜您注册成为会员，您将享受到会员所有权利！交易密码默认为000000，请前往会员中心修改");
                            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
                            //跳转领取红包判断系统是否开启
                            if (d1.Rows[0]["p4"].ToString() != "" && d1.Rows[0]["p4"].ToString() != null && d1.Rows[0]["p4"].ToString() != "0")
                            {
                                Response.Write("<script>top.location.href='accethb.aspx?companyID=" + companyID + "&openID=" + id + "';</script>");
                            }
                            else
                            {
                                Response.Write("<script>top.location.href='homeIndex.aspx?companyID=" + companyID + "';</script>");
                            }
                        }
                        else
                        {
                            //更新会员信息
                            string sqlupdate=string.Format("update asm_member set name=N'{0}',headurl='{1}',utfName='{3}',unionID='"+unionID+"' where openID='{2}'", name,tupian,id, System.Web.HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8));
                            DbHelperSQL.ExecuteSql(sqlupdate);
                            Response.Write("<script>top.location.href='homeIndex.aspx?companyID=" + companyID + "';</script>");
                        }

                    }
                }
            }
          
        }
        public static void SetDistributorCookie(int distributorid)
        {
            HttpCookie httpCookie = new HttpCookie("syname");//创建并命名新的cookie
            httpCookie.Value = distributorid.ToString();//获取或设置单个Cookie值
            httpCookie.Expires = DateTime.Now.AddYears(1);//获取和设置此cookie的过期日期和时间
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }
        protected OAuth_Token Get_token(string Code,string companyID)
        {
            Util.Debuglog("Code=" + Code, "获取token.txt");
            string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/component/access_token?appid=" + appid + "&code=" + Code + "&grant_type=authorization_code&component_appid=" + OpenPFConfig.Appid + "&component_access_token=" + Util.getComAccessToken() + "");
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
            Util.Debuglog(companyID + "服务器token=" + Str, "获取token.txt");
            return Oauth_Token_Model;
        }
        protected OAuth_Token refresh_token(string REFRESH_TOKEN)
        {
            string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + appid + "&grant_type=refresh_token&refresh_token=" + REFRESH_TOKEN);
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
            return Oauth_Token_Model;
        }

        protected OAuthUser Get_UserInfo(string REFRESH_TOKEN, string OPENID)
        {
            //string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID + "&lang=zh_CN");
            //Util.Debuglog("Str=" + Str, "_获取会员信息.txt");
            //Util.Debuglog("REFRESH_TOKEN=" + REFRESH_TOKEN + ";OPENID=" + OPENID, "_获取会员信息.txt");
            //OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(Str);
            //return OAuthUser_Model;
            string userinfo = RedisHelper.GetRedisModel<string>(OPENID);
            if (string.IsNullOrEmpty(userinfo))
            {
                string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID + "&lang=zh_CN");
                Util.Debuglog("Str=" + Str, "_获取会员信息.txt");
                Util.Debuglog("REFRESH_TOKEN=" + REFRESH_TOKEN + ";OPENID=" + OPENID, "_获取会员信息.txt");
                OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(Str);
                return OAuthUser_Model;
            }
            else
            {
                OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(userinfo);
                return OAuthUser_Model;
            }
        }
        protected OAuthUser Get_UserInfoUnion(string token, string OPENID)
        {
            string Str = GetJson("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + token + "&openid=" + OPENID + "&lang=zh_CN");
            Util.Debuglog("Str=" + Str, "_获取会员信息.txt");
            Util.Debuglog("token=" + token + ";OPENID=" + OPENID, "_获取会员信息.txt");
            OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(Str);
            return OAuthUser_Model;
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