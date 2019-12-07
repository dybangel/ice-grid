using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;

namespace Consumer.main
{
    public partial class paypage : System.Web.UI.Page
    {
        public string trxid = "";
        public string money = "";
        public string companyID = "";
        public string openID = "";
        public string req = "";
        public string headURL = "";
        public string ye = "";//余额
        public string companyName = "";
        public string url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //查询该会员是否设置支付密码没有的话弹出提示框设置
                string str = Request.QueryString["req"].ToString();//trxid=111111&money=2222;
                req = PwdHelper.DecodeDES(str, "bingoseller");
                trxid = req.Split('&')[0].Split('=')[1];
                money = req.Split('&')[1].Split('=')[1];
                companyID = req.Split('&')[2].Split('=')[1];
                this._money.Value = money;
                this._trxid.Value = trxid;
                this._companyID.Value = companyID;

                if (OperUtil.getCooki("vshop_openID") != "0")
                {
                    string sql = "select * from asm_member where openID='" + OperUtil.getCooki("vshop_openID") + "' and companyID=" + companyID;
                    DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                    if (dd.Rows.Count <= 0)
                    {
                        //判断是否关注
                        wxHelper wx = new wxHelper(companyID);
                        if (wx.Get_UserInfo(OperUtil.getCooki("vshop_openID")).subscribe == "1")
                        {
                            Response.Redirect("WXCallback.aspx?companyID=" + this._companyID.Value);
                        }
                        else
                        {
                            //没关注
                            string sql12 = "select * from asm_company where id=" + companyID;
                            DataTable dt = DbHelperSQL.Query(sql12).Tables[0];
                            url = @"https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=" + dt.Rows[0]["biz"].ToString() + "#wechat_redirect";
                            Util.Debuglog("关注链接=" + url, "微信回调_.txt");
                            Response.Redirect(url);
                        }
                    }
                    else
                    {
                        //正常已经关注的
                    }
                    headURL = dd.Rows[0]["headurl"].ToString();
                    ye = dd.Rows[0]["AvailableMoney"].ToString();
                    string sql1 = "select * from asm_company where id=" + companyID;

                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    this._openID.Value = OperUtil.getCooki("vshop_openID");
                    companyName = d1.Rows[0]["name"].ToString();
                }
                else
                {
                    Response.Redirect("weixincallback.aspx?companyID=" + companyID + "&req=" + str);
                }
            }
            catch
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "参数有误，请重试" + "</span>");
            }
        }
        [WebMethod]
        public static string yzPwd(string openID, string pwd, string money, string trxid, string companyID)
        {
            Util.Debuglog("openID=" + openID + ";pwd=" + pwd + ";money=" + money + ";trxid=" + trxid + ";companyID=" + companyID, "_余额支付.txt");
            //验证密码是否正确
            string sql = "select * from asm_member where openID='" + openID + "' and pwd='" + pwd + "'";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count <= 0)
            {
                return "1";//支付密码不正确
            }
            //判断该订单支付状态
            string sql1 = "select * from asm_pay_info where trxid='" + trxid + "'";
            DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
            if (d1.Rows.Count > 0 && d1.Rows[0]["statu"].ToString() == "1")
            {
                return "4";//已经支付完成无需重复支付
            }
            //判断余额
            if (double.Parse(dd.Rows[0]["AvailableMoney"].ToString()) < double.Parse(money))
            {
                return "2";//余额不足
            }
            //更新余额
            string update = "update asm_member set AvailableMoney=AvailableMoney-" + money + ",sumConsume=sumConsume+" + money + ",LastTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where openID='" + openID + "'";
            DbHelperSQL.ExecuteSql(update);
            string sqlM = "select addres from asm_mechine where id in(select mechineID from asm_member where openID='" + openID + "')";
            DataTable dM = DbHelperSQL.Query(sqlM).Tables[0];
            string address = "";
            if (dM.Rows.Count > 0)
            {
                address = dM.Rows[0]["addres"].ToString();
            }
            //发送消息模板
            wxHelper wx = new wxHelper(companyID);
            string data = TemplateMessage.money_bd(openID, OperUtil.getMessageID(companyID, "OPENTM403148135"), "余额变动提醒", "购买产品", money, (double.Parse(dd.Rows[0]["AvailableMoney"].ToString()) - double.Parse(money)).ToString(), address, "支付时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
            //插入记录
            Util.insertNotice(dd.Rows[0]["id"].ToString(), "余额变动提醒", "您于" + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + "购物消费：" + money + "元；余额：" + (double.Parse(dd.Rows[0]["AvailableMoney"].ToString()) - double.Parse(money)),"");
            Util.moneyChange(dd.Rows[0]["id"].ToString(), money, (double.Parse(dd.Rows[0]["AvailableMoney"].ToString()) - double.Parse(money)).ToString(), "会员消费", "2", "");
            //接着更新订单的状态
            string update1 = "update asm_pay_info set statu=1,paytime='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',acct='" + openID + "' where trxid='" + trxid + "'";
            DbHelperSQL.ExecuteSql(update1);
            return "3";
        }
    }
}