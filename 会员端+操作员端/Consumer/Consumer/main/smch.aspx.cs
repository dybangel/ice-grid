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
    public partial class smch : System.Web.UI.Page
    {
        public string companyID;
        public string mechineID;
        public string memberID;
        public string url="";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //查询该会员是否设置支付密码没有的话弹出提示框设置
                //companyID = Request.QueryString["companyID"].ToString();
                mechineID = Request.QueryString["mechineID"].ToString();
                string sqlCC = "select companyID from asm_mechine where id='"+mechineID+"'";
                DataTable dcc = DbHelperSQL.Query(sqlCC).Tables[0];
                companyID = dcc.Rows[0]["companyID"].ToString();
                this._companyID.Value = companyID;
                this._mechineID.Value = mechineID;
                this._memberID.Value = Util.getMemberID();
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
                        else {
                            //没关注
                            string sql1 = "select * from asm_company where id=" + companyID;
                            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                            url = @"https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=" + dt.Rows[0]["biz"].ToString() + "#wechat_redirect";
                            Response.Redirect(url);
                        }
                       
                    }
                    else
                    {
                        //正常已经关注的
                    }
                }
                else
                {
                    Response.Redirect("WXCallback.aspx?companyID=" + this._companyID.Value);
                }
            }
            catch
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "参数有误，请重试" + "</span>");
            }
            //this._mechineID.Value = "25";
            //this._memberID.Value = "37";
        }
        [WebMethod]
        public static string getProductToday(string memberID, string mechineID)
        {
            string sql = "select *,(select proName from asm_product ap where ap.productID=ao.productID) name from asm_orderDetail ao where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and mechineID=" + mechineID + " and memberID=" + memberID + " and zt=4 and ldNO!='' ";
            Util.Debuglog("会员扫码获取该会员的商品sql=" + sql, "_获取会员扫码取货的商品.txt");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            return "1";
        }
        [WebMethod]
        public static string okCH(string productID, string code, string memberID, string mechineID)
        {
            //插入到asm_waitCH表 等待机器端获取需要出的货
            //先查询该表里边是否有该会员该机器的今天记录
            //循环
            string[] productIDArr = productID.Split(',');
            string[] codeArr = code.Split(',');
            Util.Debuglog("会员点击需要出货的商品productID=" + productID + ";code=" + code, "_获取会员扫码取货的商品.txt");
            if (productIDArr.Length > 0)
            {
                for (int i = 0; i < productIDArr.Length; i++)
                {
                    string sql2 = "select * from asm_waitCH where memberID=" + memberID + " and mechineID=" + mechineID + " and createTime like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%' and statu=0";
                    Util.Debuglog("查询出货指令表sql2" + sql2, "_获取会员扫码取货的商品.txt");
                    DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                    if (dt2.Rows.Count <= 0)
                    {
                        string sql1 = @"INSERT INTO [dbo].[asm_waitCH](
                                        [memberID],
                                        [mechineID],
                                        [productID],
                                        [zt],
                                        [createTime],[code])
                                        VALUES(" + memberID + "," + mechineID + "," + productIDArr[i] + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + codeArr[i] + "')";
                        Util.Debuglog("插入出货指令表sql1" + sql1, "_获取会员扫码取货的商品.txt");
                        DbHelperSQL.ExecuteSql(sql1);
                        //此时可以更新asm_orderDetail表的zt=1 为完成状态吗？
                    }
                }
            }
            return "1";
        }
    }
}