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
    public partial class member : System.Web.UI.Page
    {
        public string name = "";
        public string phone = "";
        public string headUrl = "";
        public string money = "0";
        public string point = "0";
        public string randstr = "";
        public string signstr = "";
        public string time = "";
        public string app_id = "";
        public string company_ID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                company_ID = OperUtil.getCooki("companyID");
                this.companyID.Value = company_ID;
                this.member_ID.Value = Util.getMemberID();
                initData();
            }
            else
            {
                string userAgent = Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    Response.Redirect("WXCallback.aspx?companyID=" + this.companyID.Value);
                    return;
                }
            }
            if (!IsPostBack)
            {
                Response.Cache.SetNoStore();
                wxHelper wx = new wxHelper(this.companyID.Value);
                string[] str = wx.GetWXInfo(this.Request.Url.ToString(), this.companyID.Value).Split(',');
                time = str[0];
                randstr = str[1];
                signstr = str[2];
                this.member_ID.Value = Util.getMemberID();
            }
            try
            {
                if (OperUtil.getCooki("vshop_openID") != "0")
                {
                    //获取公众号的appid
                    string sql = "select * from asm_company where id=" + this.companyID.Value;
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["appId"].ToString()))
                    {
                        this.appID.Value = dt.Rows[0]["appId"].ToString();
                        app_id = dt.Rows[0]["appId"].ToString();
                    }
                    this.member_ID.Value = Util.getMemberID();
                }
                else
                {
                    string userAgent = Request.UserAgent;
                    if (userAgent.ToLower().Contains("micromessenger"))
                    {
                        Response.Redirect("WXCallback.aspx?companyID=" + this.companyID.Value);
                        return;
                    }
                }
            }
            catch
            {
                this.member_ID.Value = Util.getMemberID();
            }


        }
        public void initData()
        {
            string sql = "select * from asm_member where id="+this.member_ID.Value;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                name = dt.Rows[0]["name"].ToString();
               
                phone = dt.Rows[0]["phone"].ToString();
                headUrl = dt.Rows[0]["headurl"].ToString();
                money = dt.Rows[0]["AvailableMoney"].ToString();
                point = dt.Rows[0]["point"].ToString();
            }
        }
        [WebMethod]
        public static string getTodayProductCode(string memberID)
        {
            string sql = "select * from asm_orderDetail  where memberID=" + memberID + " and createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and zt=4 and ldNO!=''";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return "1";
            }
            return "2";
        }
        [WebMethod]
        public static string getProductToday(string memberID, string mechineID)
        {
            string sql = "select *,(select proName from asm_product ap where ap.productID=ao.productID) name from asm_orderDetail ao where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and mechineID=" + mechineID + " and memberID=" + memberID + " and zt=4";
            Util.Debuglog("会员中心会员扫码获取该会员的商品sql=" + sql, "_获取会员扫码取货的商品.txt");
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
            Util.Debuglog("会员中心会员点击需要出货的商品productID=" + productID + ";code=" + code, "_获取会员扫码取货的商品.txt");
            if (productIDArr.Length > 0)
            {
                for (int i = 0; i < productIDArr.Length; i++)
                {
                    string sql2 = "select * from asm_waitCH where memberID=" + memberID + " and mechineID=" + mechineID + " and createTime like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%' and statu=0";
                    Util.Debuglog("会员中心查询出货指令表sql2" + sql2, "_获取会员扫码取货的商品.txt");
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
                        Util.Debuglog("会员中心插入出货指令表sql1" + sql1, "_获取会员扫码取货的商品.txt");
                        DbHelperSQL.ExecuteSql(sql1);
                        //此时可以更新asm_orderDetail表的zt=1 为完成状态吗？
                    }
                }
            }
            return "1";
        }
    }
}