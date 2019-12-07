using autosell_center.util;
using Consumer.cls;
using DBUtility;
using OpenPlatForm.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WZHY.Common.DEncrypt;

namespace Consumer.main
{
    public partial class homeIndex : System.Web.UI.Page
    {
        public DataTable dt;
        public string comid;
        public string randstr = "";
        public string signstr = "";
        public string time = "";
        public string app_id = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                comid = Request.QueryString["companyID"].ToString();
                //comid = "13";
                OperUtil.setCooki("companyID", comid);
                this.companyID.Value = comid;
                Response.Cache.SetNoStore();
                if (OperUtil.getCooki("vshop_openID") != "0")
                {
                    string sql3 = "select * from asm_member where openID='" + OperUtil.getCooki("vshop_openID") + "' and companyID=" + companyID.Value;
                    DataTable dd = DbHelperSQL.Query(sql3).Tables[0];
                    if (dd.Rows.Count <= 0)
                    {
                        Response.Redirect("WXCallback.aspx?companyID=" + this.companyID.Value);
                        return;
                    }
                    else
                    {
                        string sql4 = "update asm_member set LastTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where id=" + dd.Rows[0]["id"].ToString();
                        DbHelperSQL.ExecuteSql(sql4);
                    }
                    //获取公众号的appid
                    string sql = "select * from asm_company where id=" + comid;
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["appId"].ToString()))
                    {
                        this._jlc.Value = dt.Rows[0]["jlc"].ToString();
                        this.appID.Value = dt.Rows[0]["appId"].ToString();
                        app_id = dt.Rows[0]["appId"].ToString();
                        //app_id = OpenPFConfig.Appid;
                        if (dt.Rows[0]["p8"].ToString() != "")
                        {
                            this._url1.Value = "1";
                            this.url1.Src = dt.Rows[0]["p8"].ToString();
                        }
                        if (dt.Rows[0]["p9"].ToString() != "")
                        {
                            this._url2.Value = "1";
                            this.url2.Src = dt.Rows[0]["p9"].ToString();
                        }
                        if (dt.Rows[0]["p10"].ToString() != "")
                        {
                            this._url3.Value = "1";
                            this.url3.Src = dt.Rows[0]["p10"].ToString();
                        }
                    }
                    this.memberID.Value = Util.getMemberID();
                    this._openID.Value = OperUtil.getCooki("vshop_openID");
                }
                else
                {
                    Response.Redirect("WXCallback.aspx?companyID=" + this.companyID.Value);
                }
                wxHelper wx = new wxHelper(comid);
                string[] str = wx.GetWXInfo(this.Request.Url.ToString(), comid).Split(',');
                
                //this.ur.Value = this.Request.Url.ToString();
                //this.ticket.Value = wx.GetTicketPlat(comid);
                //this.te.Value = wx.IsExistAccess_Token(comid);
                time = str[0];
                randstr = str[1];
                signstr = str[2];
                Util.Debuglog("time=" +time+";randstr="+randstr+";signstr="+signstr+";url="+ this.Request.Url.ToString(), "微信参数.txt");

            }
            catch
            {
                comid = OperUtil.getCooki("companyID");
                this.companyID.Value = comid;
                Response.Cache.SetNoStore();
                if (OperUtil.getCooki("vshop_openID") != "0")
                {
                    string sql3 = "select * from asm_member where openID='" + OperUtil.getCooki("vshop_openID") + "' and companyID=" + companyID.Value;
                    DataTable dd = DbHelperSQL.Query(sql3).Tables[0];
                    if (dd.Rows.Count <= 0)
                    {
                        Response.Redirect("WXCallback.aspx?companyID=" + this.companyID.Value);
                        return;
                    }
                    //获取公众号的appid
                    string sql = "select * from asm_company where id=" + OperUtil.getCooki("companyID");
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["appId"].ToString()))
                    {
                        this.appID.Value = dt.Rows[0]["appId"].ToString();
                        app_id = dt.Rows[0]["appId"].ToString();
                        //p_id = OpenPFConfig.Appid;
                        if (dt.Rows[0]["p8"].ToString() != "")
                        {
                            this._url1.Value = "1";
                            this.url1.Src = dt.Rows[0]["p8"].ToString();
                        }
                        if (dt.Rows[0]["p9"].ToString() != "")
                        {
                            this._url2.Value = "1";
                            this.url2.Src = dt.Rows[0]["p9"].ToString();
                        }
                        if (dt.Rows[0]["p10"].ToString() != "")
                        {
                            this._url3.Value = "1";
                            this.url3.Src = dt.Rows[0]["p10"].ToString();
                        }
                    }
                    this.memberID.Value = Util.getMemberID();
                    this._openID.Value = OperUtil.getCooki("vshop_openID");
                }
                else
                {
                    Response.Redirect("WXCallback.aspx?companyID=" + this.companyID.Value);
                }
                Response.Cache.SetNoStore();
                wxHelper wx = new wxHelper(OperUtil.getCooki("companyID"));
                this.companyID.Value = OperUtil.getCooki("companyID"); ;
                string[] str = wx.GetWXInfo(this.Request.Url.ToString(), OperUtil.getCooki("companyID")).Split(',');
                //this.ur.Value = this.Request.Url.ToString();
                //this.ticket.Value = wx.GetTicketPlat(OperUtil.getCooki("companyID"));
                //this.te.Value = wx.IsExistAccess_Token(OperUtil.getCooki("companyID"));
                time = str[0];
                randstr = str[1];
                signstr = str[2];
                Util.Debuglog("2time=" + time + ";randstr=" + randstr + ";signstr=" + signstr+";url="+ this.Request.Url.ToString(), "微信参数.txt");
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
        public static string payPwd(string openID)
        {

            string sql = "select * from asm_member where openID='" + openID + "' and pwd is not null";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count > 0)
            {
                return "1";
            }
            else
            {
                return "2";
            }
        }
        [WebMethod]
        public static string getMechineList(string companyID)
        {

            string sql = "select * from asm_mechine where companyID=" + companyID;
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
        [WebMethod]
        public static string del(string memberID, string productID, string code)
        {
            string sql = "select * from asm_orderDetail where code='" + code + "' and zt=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        [WebMethod]
        public static string accetHB(string memberID)
        {
            string sql = "";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return "1";
            }
            return "2";
        }
    }
}