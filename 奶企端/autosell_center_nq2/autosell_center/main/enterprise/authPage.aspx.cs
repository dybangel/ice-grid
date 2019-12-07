using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenPlatForm.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Services;
using WebService = OpenPlatForm.Common.WebService;

namespace OpenPlatForm.api
{
    public partial class authPage : System.Web.UI.Page
    {
        //授权公众号详细信息
        public CommonMethod.RootObjectDetail authInfoModel = new CommonMethod.RootObjectDetail();
        public string PhoneAuthPageUrl = "";
        public string authAppid;
        public string refreshToken;
        public string comID = "";
        public DataTable DC = new DataTable();//每个企业公众号信息
        public DataTable DT = new DataTable();//平台配置
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {


                comID = OperUtil.Get("companyID");
                this._operaID.Value = OperUtil.Get("operaID");
                if (string.IsNullOrEmpty(comID))
                {
                    Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                    return;
                }
                this._companyID.Value = comID;
                Session.Timeout = 360;

                if (!IsPostBack)
                {
                    //初始化页面的时候回去数据库校验当前登录用户（企业账号）是否“授权过”，是-展示企业公众号信息，否-展示授权二维码
                    //1-获取登录用户信息
                    string sql = "select * from asm_company where id=" + this._companyID.Value;

                    DC = DbHelperSQL.Query(sql).Tables[0];
                    DT = DbHelperSQL.Query("select * from asm_platformInfo").Tables[0];
                    this.HF_userInfoId.Value = comID;
                    //获取授权成功后回调的参数：
                    string auth_code = Request.QueryString["auth_code"];
                    string expires_in = Request.QueryString["expires_in"];
                    Util.Debuglog("auth_code=" + auth_code + ";expires_in=" + expires_in, "_授权.txt");
                    if (!string.IsNullOrEmpty(auth_code))
                    {
                        //授权后的回调
                        auth_code = auth_code.Split(new string[] { "@@@" }, StringSplitOptions.RemoveEmptyEntries)[1];
                        string comToken = Util.getComToken();
                        //获取微信公众号接口“调用凭据authorizer_access_token”,用这个参数来调用微信公众平台接口
                        CommonMethod.RootObject authModel = GetAuthToken(OpenPFConfig.Appid, auth_code, comToken);
                        //将authorizer_refresh_token（这个是不会改变的）存入数据库，这是当token过期的时候用来刷新token的。如果authorizer_refresh_token丢失了，则需要重新授权
                        //获取“授权公众号详细信息”
                        authInfoModel = GetAuthInfo(OpenPFConfig.Appid, authModel.authorization_info.authorizer_appid, comToken);
                        string funcinfoIdStr = "";
                        //更新authorizer_refresh_token到userinfo表中
                        if (authInfoModel != null && authInfoModel.authorization_info != null && authInfoModel.authorization_info.func_info != null && authInfoModel.authorization_info.func_info.Count > 0)
                        {
                            UpdateUserInfo(authInfoModel, authModel.authorization_info.authorizer_refresh_token, DC.Rows[0]["id"].ToString());
                            //设置行业为消费品
                            string sqlC = "select * from asm_company where id=" + comID;
                            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                            wxHelper wx = new wxHelper(comID);
                            string token = Util.GetTokenInfo(OpenPFConfig.Appid, dt.Rows[0]["appId"].ToString(), Util.getComToken(), dt.Rows[0]["refresh_token"].ToString()).authorizer_access_token;
                            string result = wx.setIndustry(token);
                            JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                            string errcode = jo["errcode"].ToString();
                        }
                        authAppid = authModel.authorization_info.authorizer_appid;
                        refreshToken = authModel.authorization_info.authorizer_refresh_token;
                    }
                    else
                    {
                        //用户主动点击进来的（用户对象中含有Refresh_token属性）
                        if (DC != null && !string.IsNullOrEmpty(DC.Rows[0]["refresh_token"].ToString()))
                        {
                            //有这个值证明已经授权过，且在数据库中有信息
                            authAppid = DC.Rows[0]["appId"].ToString();
                            refreshToken = DC.Rows[0]["refresh_token"].ToString();
                        }
                        else
                        {
                            //从Redis中取token
                            string comToken = Util.getComToken();
                            if (string.IsNullOrEmpty(comToken))
                            {
                                string ticket = DT.Rows[0]["ticket"].ToString();
                                comToken = GetToken(OpenPFConfig.Appid, OpenPFConfig.Appsecret, ticket);
                            }
                            //获取预授权码
                            string pac = GetPre_Auth_Code(OpenPFConfig.Appid, comToken);
                            //这里要判断下pac是否有值，无值证明comToken过期了，要重新获取
                            if (string.IsNullOrEmpty(pac))
                            {
                                comToken = GetToken(OpenPFConfig.Appid, OpenPFConfig.Appsecret, DT.Rows[0]["ticket"].ToString());
                                pac = GetPre_Auth_Code(OpenPFConfig.Appid, comToken);
                            }
                            pac = pac.Split(new string[] { "@@@" }, StringSplitOptions.RemoveEmptyEntries)[1];
                            //拼接授权页面地址（用户扫描后跳转的扫码页面，前端必须用一个连接去进行跳转）
                            PhoneAuthPageUrl = "https://mp.weixin.qq.com/cgi-bin/componentloginpage?component_appid=" + OpenPFConfig.Appid + "&pre_auth_code=" + pac + "&redirect_uri=http://nq.bingoseller.com/main/enterprise/authPage.aspx&auth_type=3";
                        }
                    }
                }

            }
            catch (Exception ee)
            {

                Util.Debuglog("auth ee=" + ee.ToString(), "_授权.txt");
            }
        }
        [WebMethod]
        public static object judge(string operaID, string menuID)
        {
            Boolean b = Util.judge(operaID, menuID);
            if (b)
            {
                return new { code = 200 };
            }
            else
            {
                return new { code = 500 };
            }
        }
        #region 授权相关方法
        //post方法调用接口获取token
        public static string GetToken(string appid, string secret, string ticket)
        {
            var obj = new
            {
                component_appid = appid,
                component_appsecret = secret,
                component_verify_ticket = ticket
            };
            string responseStr =
                WebService.PostFunction("https://api.weixin.qq.com/cgi-bin/component/api_component_token", obj);

            CommonMethod.Token tokenModel = CommonMethod.JsonHelper.ParseFromJson<CommonMethod.Token>(responseStr);
            if (tokenModel != null && !string.IsNullOrEmpty(tokenModel.component_access_token))
            {
                return tokenModel.component_access_token;
            }
            else
            {
                return "";
            }
        }

        //post方法调用接口获取pre_auth_code
        public string GetPre_Auth_Code(string appid, string token)
        {
            var obj = new
            {
                component_appid = appid
            };
            string responseStr =
                WebService.PostFunction("https://api.weixin.qq.com/cgi-bin/component/api_create_preauthcode?component_access_token=" + token, obj);

            CommonMethod.Pre_Auth_Code pacModel = CommonMethod.JsonHelper.ParseFromJson<CommonMethod.Pre_Auth_Code>(responseStr);
            if (pacModel != null && !string.IsNullOrEmpty(pacModel.pre_auth_code))
            {
                return pacModel.pre_auth_code;
            }
            else
            {
                return "";
            }
        }

        //post方法调用接口获取token
        public CommonMethod.RootObject GetAuthToken(string appid, string authCode, string componentToken)
        {
            var obj = new
            {
                component_appid = appid,
                authorization_code = authCode
            };
            string responseStr =
                WebService.PostFunction("https://api.weixin.qq.com/cgi-bin/component/api_query_auth?component_access_token=" + componentToken, obj);

            CommonMethod.RootObject authInfoModel = CommonMethod.JsonHelper.ParseFromJson<CommonMethod.RootObject>(responseStr);
            //CommonMethod.RootObject rb = JsonConvert.DeserializeObject<CommonMethod.RootObject>(responseStr);//这种方法也可以解析
            if (authInfoModel != null)
            {
                return authInfoModel;
            }
            else
            {
                return new CommonMethod.RootObject();
            }
        }

        //post方法调用接口获取“授权公众号”详细信息
        public static CommonMethod.RootObjectDetail GetAuthInfo(string appid, string authAppid, string componentToken)
        {
            var obj = new
            {
                component_appid = appid,
                authorizer_appid = authAppid
            };
            string responseStr =
                WebService.PostFunction("https://api.weixin.qq.com/cgi-bin/component/api_get_authorizer_info?component_access_token=" + componentToken, obj);
           
            CommonMethod.RootObjectDetail authInfo = CommonMethod.JsonHelper.ParseFromJson<CommonMethod.RootObjectDetail>(responseStr);
            if (authInfo != null)
            {
                return authInfo;
            }
            else
            {
                return new CommonMethod.RootObjectDetail();
            }
        }

        /// <summary>
        /// post方法调用接口 重新获取token
        /// </summary>
        /// <param name="appid">第三方平台appid</param>
        /// <param name="authAppid">授权方appid</param>
        /// <param name="refreshToken">刷新token</param>
        /// <param name="componentToken">第三方平台token</param>
        /// <returns></returns>
        public static CommonMethod.RefreshToken RefreshToken(string appid, string authAppid, string refreshToken, string componentToken)
        {
            var obj = new
            {
                component_appid = appid,
                authorizer_appid = authAppid,
                authorizer_refresh_token = refreshToken
            };
            string responseStr =
                WebService.PostFunction("https://api.weixin.qq.com/cgi-bin/component/api_authorizer_token?component_access_token=" + componentToken, obj);

            CommonMethod.RefreshToken rToken = CommonMethod.JsonHelper.ParseFromJson<CommonMethod.RefreshToken>(responseStr);
            //CommonMethod.RootObject rb = JsonConvert.DeserializeObject<CommonMethod.RootObject>(responseStr);//这种方法也可以解析
            if (rToken != null)
            {
                return rToken;
            }
            else
            {
                return new CommonMethod.RefreshToken();
            }
        }
        #endregion
        
        [WebMethod]
        public static object saveSubscribeInfo(string subscribeInfo , string companyID)
        {


            string sql = " update asm_company set subscribe_info='" + subscribeInfo + "'  where id= '" + companyID+"'";
            Util.Debuglog("sql=" + sql, "_修改订阅信息.txt");
            int count = DbHelperSQL.ExecuteSql(sql);
            if (count > 0)
            {
                return new { result = 200, msg = "设置成功" };
            }
            else {
                return new { result = 200, msg = "设置失败" };
            }
            
            
        }
        [WebMethod]
        public static object setIndus(string companyID)
        {
            string sql = "select * from asm_company where id=" + companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            wxHelper wx = new wxHelper(companyID);
            string token = Util.GetTokenInfo(OpenPFConfig.Appid, dt.Rows[0]["appId"].ToString(), Util.getComToken(), dt.Rows[0]["refresh_token"].ToString()).authorizer_access_token;
            string result = wx.setIndustry(token);
            JObject jo = (JObject)JsonConvert.DeserializeObject(result);
            string errcode = jo["errcode"].ToString();
            if (errcode == "0")
            {
                return new { result = 200, msg = "设置成功" };
            }
            else if (errcode == "43100")
            {
                return new { result = 200, msg = "行业一个月只允许修改一次" };
            }
            return new { result = 200, msg = "设置失败" };
        }
        //重新授权
        [WebMethod]
        public static object AuthAgain(string authAppid, string refreshToken, string userId)
        {
           
            string comToken = Util.getComToken();

            if (string.IsNullOrEmpty(comToken))
            {
                DataTable dd= DbHelperSQL.Query("select * from asm_platformInfo").Tables[0];
                string ticket =dd.Rows[0]["ticket"].ToString();
                comToken = GetToken(OpenPFConfig.Appid, OpenPFConfig.Appsecret, ticket);
                
            }
            //获取公众号详细信息+更新到数据库
            CommonMethod.RootObjectDetail authInfoModel = GetAuthInfo(OpenPFConfig.Appid, authAppid, comToken);
            if (authInfoModel != null && authInfoModel.authorization_info != null &&
                authInfoModel.authorization_info.func_info != null &&
                authInfoModel.authorization_info.func_info.Count > 0)
            {
                int count = UpdateUserInfo(authInfoModel, refreshToken, userId);
                if (count > 0)
                {
                    return new { result = 0, msg = "ok" };
                }
                else
                {
                    return new { result = 500, msg = "更新信息失败" };
                }
            }
            else
            {
                return new { result = 500, msg = "信息获取失败" };
            }
        }
        //更新userinfo
        public static int UpdateUserInfo(CommonMethod.RootObjectDetail authInfoModel, string refreshToken, string userId)
        {
           
            string funcinfoIdStr = "";
            List<CommonMethod.func_info> funcinfoList = authInfoModel.authorization_info.func_info;
            for (int i = 0; i < funcinfoList.Count; i++)
            {
                if (funcinfoList[i] != null)
                {
                    funcinfoIdStr += funcinfoList[i].funcscope_category.id + ",";
                }
            }
            string sql = " update asm_company set appId='" + authInfoModel.authorization_info.authorizer_appid + "' ,"
               +" refresh_token='" + refreshToken.Split(new string[] {"@@@"},StringSplitOptions.RemoveEmptyEntries)[1] + "',"
               +" nick_name='" + authInfoModel.authorizer_info.nick_name + "',head_img='" + authInfoModel.authorizer_info.head_img + "', "
              +" service_type_info='" + authInfoModel.authorizer_info.service_type_info.id + "', "
              +" verify_type_info='" + authInfoModel.authorizer_info.verify_type_info.id + "', "
             +" user_name='" + authInfoModel.authorizer_info.user_name + "', principal_name='" + authInfoModel.authorizer_info.principal_name + "', "
            +" qrcode_url='" + authInfoModel.authorizer_info.qrcode_url + "', func_info='" + funcinfoIdStr.Substring(0, funcinfoIdStr.Length - 1) + "',"
            +" alias='"+ authInfoModel.authorizer_info.alias + "' where id= " + userId;
            Util.Debuglog("sql="+ sql, "_授权.txt");
            int count = DbHelperSQL.ExecuteSql(sql);
           
            return count;
        }
       
        
    }
}