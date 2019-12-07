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

namespace Consumer.main
{
    public partial class getWXInfoNew : System.Web.UI.Page
    {
        protected string sum = "0";
        protected string headImg = "";
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
                    Util.Debuglog("code="+ code+ ";state="+ Request.QueryString["state"], "获取参数.txt");
                    string[] param = Request.QueryString["state"].ToString().Split('|');
                    if (param.Length!=4)
                    {
                        //Response.Write("<span style='color:#FF0000;font-size:20px'>" + "参数不全请重试" + "</span>");
                    }
                    //9|14|43|334|0|1
                    //string money = param[0];//9
                    string companyID = param[0];//14
                    //string mechineID = param[2];//43
                   // string productID = param[3];//334
                   // string dgOrderDetailID = param[4];//0
                   // string type = param[5];//1
                    //string sftj = param[6];//1 是特价
                    string reqsn = param[1];
                    //reqsn= reqsn.Replace("reqsn", "");
                    string productID = "";//334
                    string mechineID = "";//334
                    string dgOrderDetailID = "0";//0
                    string type = "2";//1
                    string sftj = "0";//1 是特价
                    string asm_product_picksql = "select * from asm_product_pick  where reqsnNo='" + reqsn + "' ";
                    DataTable asm_product_pickdt = DbHelperSQL.Query(asm_product_picksql).Tables[0];
                    if (asm_product_pickdt.Rows.Count > 0)
                    {
                        productID = asm_product_pickdt.Rows[0]["productID"].ToString();
                        mechineID = asm_product_pickdt.Rows[0]["mechineID"].ToString();
                        dgOrderDetailID = asm_product_pickdt.Rows[0]["dgOrderDetailID"].ToString();
                        type = asm_product_pickdt.Rows[0]["type"].ToString();
                        sftj = asm_product_pickdt.Rows[0]["sftj"].ToString();
                    }
                    else {
                        return;
                    }
                    string sql2 = "select * from asm_company where id=" + companyID;
                    DataTable d1 = DbHelperSQL.Query(sql2).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        appid = d1.Rows[0]["appId"].ToString();
                        appsecret = d1.Rows[0]["wx_appsecret"].ToString();
                    }
                   
                    OAuth_Token Model = Get_token(code, companyID);
                    OAuthUser OAuthUser_Model = Get_UserInfo(Model.access_token, Model.openid);
                    if (OAuthUser_Model.openid != null && OAuthUser_Model.openid != "")  //已获取得openid及其他信息
                    {
                        headImg = OAuthUser_Model.headimgurl.ToString();//头像图片
                        name = OAuthUser_Model.nickname;//昵称
                        id = OAuthUser_Model.openid;//opendid
                        unionID = OAuthUser_Model.unionid;
                        string province = OAuthUser_Model.province;
                        string city = OAuthUser_Model.city;
                        string country = OAuthUser_Model.city;
                        string gender = OAuthUser_Model.sex == "1" ? "男" : "女";
                        // Util.SetSession("_openID", id);
                        Util.Debuglog("id=" + id, "获取token.txt");//o1_mf1aL2bduKZnTzG1irrfvN0x8
                        string sql = "select * from asm_member where unionID='" + unionID + "'";
                        Util.Debuglog("sql="+ sql, "零售支付用户注册.txt");
                        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                       
                        //判断限购次数
                        if (dt.Rows.Count <= 0)
                        {
                            
                                
                            string insert = "insert into asm_member(name,phone,province,city,country,AvailableMoney,sumConsume,sumRecharge,createDate,companyID,headurl,nickname,sex,unionID,openID,consumeCount)"
                            + " values(N'" + name + "','','" + province + "','" + city + "','" + country + "',0,0,0,'" + DateTime.Now + "','" + companyID + "','" + headImg + "',N'" + name + "','" + gender + "','" + unionID + "','" + id + "',0);select @@IDENTITY";
                            Util.Debuglog(insert, "零售支付用户注册.txt");
                            object obj = DbHelperSQL.GetSingle(insert);
                            if (obj == null)
                            {

                            }
                            else {
                                string pickupdate = "update  asm_product_pick set memberID=" + Convert.ToInt32(obj).ToString() + " where reqsnNo='" + reqsn + "' ";
                                Util.Debuglog("sqlInsert=" + pickupdate, "获取预生成订单号.txt");
                                DbHelperSQL.ExecuteSql(pickupdate);
                               
                            }

                            //发送注册成为会员模板消息
                            wxHelper wx = new wxHelper(companyID);
                            string data = TemplateMessage.Member_ZC(id, OperUtil.getMessageID(companyID, "OPENTM203347141"), "恭喜您注册成为会员！", name, "恭喜您注册成为会员，您将享受到会员所有权利！");
                            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
                        }
                        else {
                            string pickupdate = "update  asm_product_pick set memberID=" + dt.Rows[0]["id"].ToString() + " where reqsnNo='" + reqsn + "' ";
                            Util.Debuglog("sqlInsert=" + pickupdate, "获取预生成订单号.txt");
                            DbHelperSQL.ExecuteSql(pickupdate);
                            //更新
                            string update = "update asm_member set openID='"+ id + "' where unionID='"+unionID+"'";
                            Util.Debuglog("更新"+ update, "是否限购.txt");
                            DbHelperSQL.ExecuteSql(update);
                            //限购判断
                            if (!Util.xgCount(productID,dt.Rows[0]["id"].ToString(),mechineID))
                            {
                                Util.Debuglog("限购"+ unionID, "是否限购.txt");
                                string url13 = "https://wx.bingoseller.com/main/xg.aspx";
                                //限购不让购买
                                Response.Write("<script>window.location.href='" + url13 + "';</script>");
                                return;
                            }
                            if (unionID== "owhCR0esai2hPXH4lYkeLMAcccuE"|| unionID == "owhCR0XLU0NM_GauIWydxGogmHfk"|| unionID == "owhCR0bslgzXtWBLHv-ll7W1Me4c"|| unionID == "owhCR0UPVXSCxYyPhkNy3wejNjNs"|| unionID == "owhCR0dm8yPqHIYZhMnjti_PvA3U") {


                              

                                string url1 = "https://wx.bingoseller.com/main/wxorbalanceNew.aspx?companyID=" + companyID + "&mechineID=" + mechineID + "&unionID=" + unionID + "&openID=" + id + "&productID=" + productID + "&dgOrderDetailID=" + dgOrderDetailID + "&type=" + type + "&sftj=" + sftj + "&reqsn=" + reqsn;
                                Util.Debuglog("url1=" + url1, "微信+余额.txt");
                                Response.Write("<script>window.location.href='" + url1 + "';</script>");
                                return;
                               
                            }
                          

                           
                        }
                        string url12 = "https://wx.bingoseller.com/main/wxorbalance.aspx?companyID=" + companyID + "&mechineID=" + mechineID + "&unionID=" + unionID + "&openID=" + id + "&productID=" + productID + "&dgOrderDetailID=" + dgOrderDetailID + "&type=" + type+ "&sftj="+ sftj;
                        Util.Debuglog("url12=" + url12, "微信+余额.txt");
                        Response.Write("<script>window.location.href='" + url12 + "';</script>");
                        return;
                       
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
            string userinfo = RedisHelper.GetRedisModel<string>(OPENID);
            if (string.IsNullOrEmpty(userinfo))
            {
                string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID + "&lang=zh_CN");
                Util.Debuglog("Str=" + Str, "_获取会员信息.txt");
                Util.Debuglog("REFRESH_TOKEN=" + REFRESH_TOKEN + ";OPENID=" + OPENID, "_获取会员信息.txt");
                OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(Str);
                return OAuthUser_Model;
            }
            else {
                OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(userinfo);
                return OAuthUser_Model;
            }
          
        }
        protected OAuthUser Get_UserInfoUnion(string token, string OPENID)
        {
            Util.Debuglog("token=" + token + ";OPENID=" + OPENID, "_获取会员信息2.txt");
            string Str = GetJson("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + token + "&openid=" + OPENID + "&lang=zh_CN");
            Util.Debuglog("Str=" + Str, "_获取会员信息2.txt");
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