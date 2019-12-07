using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using WZHY.Common.DEncrypt;

namespace Consumer.main
{
    public partial class phonesetup : System.Web.UI.Page
    {
        public string memberID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                this.member_ID.Value = Util.getMemberID();
            }
            else
            {
                string userAgent = Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    Response.Redirect("WXCallback.aspx?companyID=" + OperUtil.getCooki("companyID"));
                    return;
                }
            }
            string sql1 = "select * from asm_member where id='" + this.member_ID.Value + "'";
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.phone.Value = dt.Rows[0]["phone"].ToString();
            }

        }
        [WebMethod]
        public static string setupOk(string phone,string memberID)
        {
            string sql1 = "select * from asm_member where id!='"+memberID+"' and phone='"+phone+ "' and  companyID='"+OperUtil.getCooki("companyID") +"'";
            Util.Debuglog("sql1="+sql1,"_修改手机号.txt");
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count>0)
            {
                return "3";
            }
            string sql = "update asm_member set phone='"+phone+"' where id='"+memberID+"'";
            Util.Debuglog("update=" + sql, "_修改手机号.txt");
            int a=DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                string sql3 = "select * from asm_member where id='" + memberID + "'";
                DataTable dd = DbHelperSQL.Query(sql3).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    if (!dd.Rows[0]["certF"].ToString().Contains("phone"))
                    {
                        string sql2 = "update asm_member set certF=isnull(certF,'')+',phone' where id='" + memberID + "'";
                        DbHelperSQL.ExecuteSql(sql2);
                    }
                }
                return "1";
            }
            else {
                return "2";
            }
           
        }
        [WebMethod]
        public static string sendMess(string phone,string yzm, string memberID)
        {
       
            return sendMessage(phone, yzm);
        }

        public static string sendMessage(string member_phone,string yzm)
        {
            if (string.IsNullOrEmpty(member_phone))
            {
                return "1";//手机号不能为空
            }
            if (!Regex.IsMatch(member_phone, @"^1\d{10}$"))//判断手机号
            {

                return "2";//手机号不正确
            }
            if (Util.CheckServeStatus("sms.ruizhiwei.net"))
            {
                //查询签名
                //Hashtable ht1 = new Hashtable();
                //ht1.Add("U", "bingge");
                //ht1.Add("p", DESEncrypt.MD5Encrypt("123456"));

                //XmlDocument xmlDoc1 = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SignShow", ht1);
                //String SMSign = xmlDoc1.InnerText;

                String registration = "您的验证码是：{yzm}，请不要把验证码泄漏给其他人。【冰格售卖】";
                //registration = registration.Replace("@SMSign", SMSign);
                registration = registration.Replace("{yzm}", yzm);  //卡号


                Hashtable ht = new Hashtable();
                ht.Add("U", "bingge");
                ht.Add("p", DESEncrypt.MD5Encrypt("123456"));
                ht.Add("N", member_phone);
                ht.Add("M", registration);
                ht.Add("T", "2");
                XmlDocument xmlDoc = WZHY.Common.WebService.QueryGetWebService("http://sms.ruizhiwei.cn/API/SMS_Send.asmx", "SendMes", ht);
                String status = xmlDoc.InnerText;
                return "4";//发送成功
            }
            else {
                return "3";//服务器异常
            }

        }
    }
}