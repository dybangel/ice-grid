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
    public partial class pwdsetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                this.member_ID.Value = Util.getMemberID();
                this.companyID.Value = OperUtil.getCooki("companyID");
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

        }
        [WebMethod]
        public static string save(string memberID,string yzm,string newpwd,string companyID)
        {
            string sql = "select * from asm_member where id="+memberID+"";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count<=0)
            {
                return "2";
            }
            string update = "update asm_member set pwd='"+newpwd+"' where id="+memberID;
            int a= DbHelperSQL.ExecuteSql(update);
            if (a > 0)
            {
                try
                {
                    wxHelper wx = new wxHelper(companyID);
                    string data = TemplateMessage.modify_pwd(dt.Rows[0]["openID"].ToString(), OperUtil.getMessageID(dt.Rows[0]["companyID"].ToString(), "OPENTM406259604"), " 尊敬的会员，您通过手机端修改了【交易密码】", "会员昵称：" + dt.Rows[0]["nickname"].ToString(), "会员姓名：" + dt.Rows[0]["name"].ToString(), "重置密码：1006201");
                    TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
                }
                catch {

                }
                return "1";
            }
            else {
                return "4";
            }
        }
        [WebMethod]
        public static string sendMess(string phone, string yzm, string memberID)
        {
            return sendMessage(phone, yzm);
        }
        public static string sendMessage(string member_phone, string yzm)
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
            else
            {
                return "3";//服务器异常
            }

        }
    }
}