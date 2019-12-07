using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json.Linq;
using OpenPlatForm.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXin.Lib.Core.Helper;
using WZHY.Common.DEncrypt;

namespace autosell_center.main.enterprise
{
    public partial class Thepublicjb : System.Web.UI.Page
    {
        public DataTable dt4;
        public DataTable dt5;
        public DataTable dt6;
        public DataTable dt7;
        public DataTable dt;
        public string Token = "";//DAE3FF0A834D17E2
        public string appid = "";//wx5fa5622ada06cfe3
        public string appsecret = "";//f00ca380481727f4c50293ba6a121c3c
        public string comID = "";

        private CommonMethod.RefreshToken tokenModel = new CommonMethod.RefreshToken();
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this._operaID.Value = OperUtil.Get("operaID");
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            this.com_id.Value = comID;
            //查询公司信息
           
            string sql1 = "select * from asm_company where id="+comID;
            DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
            Token = dd.Rows[0]["wx_token"].ToString();
            appid = dd.Rows[0]["wx_appid"].ToString();
            appsecret = dd.Rows[0]["wx_appsecret"].ToString();
            string sql = "SELECT * from asm_menu where pid='0' and companyID="+comID;
            dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //if (dt.Rows.Count == 1) {
                //二级菜单的个数
                string sql7 = "SELECT * from asm_menu where pid!='0' and pid='" + dt.Rows[0]["id"].ToString() + "' and companyID="+comID;
                dt7 = DbHelperSQL.Query(sql7).Tables[0];
            }
            //string sqlP = "select * from asm_platformInfo ";
            //DataTable dp = DbHelperSQL.Query(sqlP).Tables[0];
            //string comToken = Util.getComToken();

            //if (string.IsNullOrEmpty(comToken))
            //{
            //    string ticket = dp.Rows[0]["ticket"].ToString();
            //    comToken = GetToken(OpenPFConfig.Appid, OpenPFConfig.Appsecret, ticket);
            //}
            //tokenModel = GetTokenInfo(OpenPFConfig.Appid,dd.Rows[0]["appId"].ToString(), comToken, dd.Rows[0]["refresh_token"].ToString());

            //if (tokenModel == null)
            //{
            //    string ticket = dp.Rows[0]["ticket"].ToString();
            //    comToken = GetToken(OpenPFConfig.Appid, OpenPFConfig.Appsecret, ticket);
            //    tokenModel = GetTokenInfo(OpenPFConfig.Appid, dd.Rows[0]["appId"].ToString(), comToken, dd.Rows[0]["refresh_token"].ToString());
            //}

            //this.HF_accesstoken.Value = tokenModel.authorizer_access_token;
           
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
        //post方法获取authorizer_access_token
        public static CommonMethod.RefreshToken GetTokenInfo(string appid, string authAppid, string componentToken, string refresh_token)
        {
            var obj = new
            {
                component_appid = appid,
                authorizer_appid = authAppid,
                authorizer_refresh_token = refresh_token
            };
            string responseStr = OpenPlatForm.Common.WebService.PostFunction("https://api.weixin.qq.com/cgi-bin/component/api_authorizer_token?component_access_token=" + componentToken, obj);
            CommonMethod.RefreshToken authInfo = CommonMethod.JsonHelper.ParseFromJson<CommonMethod.RefreshToken>(responseStr);
            if (authInfo != null)
            {
                return authInfo;
            }
            else
            {
                return new CommonMethod.RefreshToken();
            }
        }
        public static string GetToken(string appid, string secret, string ticket)
        {
            var obj = new
            {
                component_appid = appid,
                component_appsecret = secret,
                component_verify_ticket = ticket
            };
            string responseStr = OpenPlatForm.Common.WebService.PostFunction("https://api.weixin.qq.com/cgi-bin/component/api_component_token", obj);
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
        [WebMethod]
        public static string createQR()
        {

            return "";
        }

        [WebMethod]
        public static string GradeType(String id,String comID)
        {
          
            string sql = "SELECT * from asm_menu where pid = '" + id + "' and companyID="+comID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            else
            {
                return "";
            }
        }

        protected void create_Click(object sender, EventArgs e)
        {
            String str = DESEncrypt.Encrypt("comid=10&mechineID=18");

        }

        /// <summary>
        /// 获得accesstoken
        /// </summary>
        /// <returns></returns>
        public string accesstoken()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret;
            WebClient client = new WebClient();
            string resp = client.DownloadString(url);
            var result = JObject.Parse(resp);
            return result["access_token"].ToString();

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            string atk = post(accesstoken());
            if (atk != "")
            {
                //Response.Write(atk);
            }
        }
        public string post(string accesstoken)
        {
          
            string id = "";
            string name1 = "";
            string url1 = "";
            int j;
            int i;
            //string name3 = "";
            //string url3 = "";
            string sqlLev1 = "SELECT * from asm_menu where pid='0' and companyID=" + this.com_id.Value;
            DataTable dtLev1 = DbHelperSQL.Query(sqlLev1).Tables[0];
            string minAppid = DbHelperSQL.Query("select * from asm_company where id="+this.com_id.Value).Tables[0].Rows[0]["minAppid"].ToString();
            string str = "{\"button\":[";
            string s1 = "", s2 = "";
            if (dtLev1.Rows.Count > 0)
            {
                string ss = "";
                for (int m = 0; m < dtLev1.Rows.Count; m++)
                {
                    id = dtLev1.Rows[m]["id"].ToString();
                    name1 = dtLev1.Rows[m]["name"].ToString();
                    url1 = dtLev1.Rows[m]["url"].ToString();
                    string sqlLev2 = "SELECT * from asm_menu where  pid='" + id + "' and companyID=" + this.com_id.Value;
                    DataTable dtLev2 = DbHelperSQL.Query(sqlLev2).Tables[0];
                    if (dtLev2.Rows.Count > 0)
                    {
                        s2 = "{\"name\":\""+name1+"\",\"sub_button\":[";
                        for (int n = 0; n < dtLev2.Rows.Count; n++)
                        {
                            s2 += "{\"type\":\"view\",\"name\":\"" + dtLev2.Rows[n]["name"].ToString() + "\",\"url\":\"" + dtLev2.Rows[n]["url"].ToString() + "\"},";
                        }
                        ss += s2.Substring(0,s2.Length-1)+"]},";
                    }
                    else
                    {
                        //s1 += "{\"type\":\"view\",\"name\":\"" + name1 + "\",\"url\":\"" + url1 + "\"},";
                        s1 = "{\"type\":\"miniprogram\",\"name\":\""+ name1 + "\",\"url\":\"http://mp.weixin.qq.com\",\"appid\":\""+ minAppid + "\",\"pagepath\":\""+ url1 + "\"},";
                        ss += s1;
                    }
                   
                }
                if (!string.IsNullOrEmpty(ss))
                {
                    str += ss.Substring(0, ss.Length - 1) + "]}";
                }
                Util.Debuglog("str="+str,"自定义菜单.txt");
                string responeJsonStr = str;
                int a = 0, b = 0, c = 0;
                string postData = null;
                System.Net.HttpWebRequest request = default(System.Net.HttpWebRequest);
                System.IO.Stream requestStream = default(System.IO.Stream);
                byte[] postBytes = null;
                //封装参数    
                postData = "a=" + a + "&b=" + b + "&c=" + c;
                string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + accesstoken;
                request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = responeJsonStr.Length;
                request.Timeout = 10000;
                request.Method = "POST";
                request.AllowAutoRedirect = false;
                requestStream = request.GetRequestStream();
                postBytes = System.Text.Encoding.UTF8.GetBytes(responeJsonStr.ToString());
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();
                HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                Stream inStream = res.GetResponseStream();
                StreamReader sr = new StreamReader(inStream, Encoding.UTF8);
                string htmlResult = sr.ReadToEnd();
                Util.Debuglog("htmlResult="+ htmlResult,"_设置菜单.txt");
               
                return htmlResult;
            }
            else {
              
                return "";
            }

        }
        [WebMethod]
        public static string oneGrade(string name, string url,string companyID)
        {
        
            string insert = "INSERT INTO [dbo].[asm_menu] ([name], [pid], [url], [bz], [companyID]) VALUES ('" + name + "', '0', '" + url + "', NULL, '"+companyID+"')";
            int add = DbHelperSQL.ExecuteSql(insert);
            if (add > 0)
            {
             
                return "添加一级菜单成功";
            }
            else
            {
               
                return "添加一级菜单失败";
            }

        }

        [WebMethod]
        public static string updateOne(string id, string name, string url)
        {
            
            string update = "update asm_menu SET name='" + name + "',url='" + url + "' where id=" + id + "";
            int u = DbHelperSQL.ExecuteSql(update);
            if (u > 0)
            {
              
                return "编辑一级菜单成功";
            }
            else
            {
               
                return "编辑一级菜单失败";
            }
        }

        [WebMethod]
        public static string updateTwo(string id, string name, string url)
        {
           
            string update = "update asm_menu SET name='" + name + "',url='" + url + "' where id=" + id + "";
            int u = DbHelperSQL.ExecuteSql(update);
          
            if (u > 0)
            {

                return "编辑二级菜单成功";
            }
            else
            {
                return "编辑二级菜单失败";
            }
        }

        [WebMethod]
        public static string twoGrade(string id, string name, string url,string companyID)
        {
           
            string insert = "INSERT INTO [dbo].[asm_menu] ([name], [pid], [url], [bz], [companyID]) VALUES ('" + name + "', '" + id + "', '" + url + "', NULL, '"+companyID+"')";
            int add = DbHelperSQL.ExecuteSql(insert);
           
            if (add > 0)
            {

                return "添加二级菜单成功";
            }
            else
            {
                return "添加二级菜单失败";
            }
        }


        [WebMethod]
        public static string del(string id)
        {
           
            string del = "DELETE from asm_menu where id='" + id + "'";
            int d = DbHelperSQL.ExecuteSql(del);
          
            if (d > 0)
            {
                return "删除一级菜单成功";
            }
            else
            {
                return "删除一级菜单失败";
            }
        }

        [WebMethod]
        public static string delTwo(string id)
        {
         
            string del = "DELETE from asm_menu where id='" + id + "'";
            int d = DbHelperSQL.ExecuteSql(del);
            
            if (d > 0)
            {
                return "删除二级菜单成功";
            }
            else
            {
                return "删除二级菜单失败";
            }
        }
    }
}