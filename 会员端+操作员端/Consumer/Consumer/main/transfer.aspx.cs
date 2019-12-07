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

namespace Consumer.main
{
    public partial class transfer : System.Web.UI.Page
    {
        public string availableMpney = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                this.memberID.Value = Util.getMemberID();
                string sql = "select * from asm_member where id="+this.memberID.Value;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                availableMpney = dt.Rows[0]["AvailableMoney"].ToString();
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
        public static string transfer1(string bh,string money,string member_ID)
        {
            //先判断手机号是否存在
            string sql1 = "select * from asm_member where phone='"+bh+"'";
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count<=0)
            {
                return "1";//该编号不存在
            }
            //判断当前会员是否绑定手机

            string sql2 = "select * from asm_member where id="+member_ID;
            DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
            if (string.IsNullOrEmpty(dt2.Rows[0]["phone"].ToString()))
            {
                return "4";//当前没有绑定手机号
            }
            if (double.Parse(dt2.Rows[0]["AvailableMoney"].ToString())-double.Parse(money)<0)
            {
                return "2";//当前余额不足
            }
            //更新两个人的余额
            //本人
            //

            string sql3 = "update asm_member set AvailableMoney=AvailableMoney-" +money+ " where id="+ member_ID;
            DbHelperSQL.ExecuteSql(sql3);
            Util.insertRecord(member_ID, "-"+money,dt2.Rows[0]["AvailableMoney"].ToString(), "会员转账");
            dt2 = DbHelperSQL.Query(sql2).Tables[0];
            Util.moneyChange(member_ID,money, dt2.Rows[0]["AvailableMoney"].ToString(), "会员转账支出","3", dt.Rows[0]["id"].ToString());
            Util.insertNotice(member_ID, "会员转账通知", "您给手机号:"+bh+" 转账:"+money+",当前可用余额:"+ dt2.Rows[0]["AvailableMoney"].ToString(),"");
            //收款人
            string sql4 = "update asm_member set AvailableMoney=AvailableMoney+" + money + " where id=" + dt.Rows[0]["id"].ToString();
            Util.insertRecord(dt.Rows[0]["id"].ToString(), money, dt.Rows[0]["AvailableMoney"].ToString(), "会员转账");
            dt = DbHelperSQL.Query(sql1).Tables[0];
            Util.moneyChange(dt.Rows[0]["id"].ToString(), money, (double.Parse(dt.Rows[0]["AvailableMoney"].ToString())+double.Parse(money)).ToString(), "会员转账收入", "3", member_ID);
            Util.insertNotice(dt.Rows[0]["id"].ToString(), "收款通知", "收到手机号:" + dt2.Rows[0]["phone"].ToString() + " 转账:" + money + ",当前可用余额:" + dt2.Rows[0]["AvailableMoney"].ToString(),"");
            DbHelperSQL.ExecuteSql(sql4);
           
            return "3";
        }
    }
}