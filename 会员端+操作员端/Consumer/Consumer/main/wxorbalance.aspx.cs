﻿using autosell_center.util;
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
    public partial class wxorbalance : System.Web.UI.Page
    {
        public DataTable dt1=new DataTable();
        public DataTable dt2 = new DataTable();
        public string djName = "";
        public string price = "0";
        public string dgOrderDetailID = "";//如果是半价出售的才会有值 且不是0
        public string type = "";//1订购 2零售 3半价
        public string sftj = "";//是否特价
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    this._mechineID.Value = Request.QueryString["mechineID"].ToString();
                    this._companyID.Value = Request.QueryString["companyID"].ToString();
                    this._unionID.Value = Request.QueryString["unionID"].ToString();
                    this._money.Value = Request.QueryString["money"].ToString();
                    this._productID.Value = Request.QueryString["productID"].ToString();
                    this._openID.Value = Request.QueryString["openID"].ToString();
                    this._dgOrderID.Value = Request.QueryString["dgOrderDetailID"].ToString();
                    this._type.Value = Request.QueryString["type"].ToString();
                    this._sftj.Value = Request.QueryString["sftj"].ToString();//是否特价

                    //this._mechineID.Value = "25";
                    //this._companyID.Value = "14";
                    //this._unionID.Value = "owhCR0Yh6wTJwWWmeNihP4_7VEPU";
                    //this._money.Value = "10";
                    //this._productID.Value = "309";
                    //this._openID.Value = "o1_mf1fobBa_nGmNo0DIVY58TRKE";
                    //this._dgOrderID.Value = "";
                    //this._type.Value = "2";
                    //this._sftj.Value = "";
                    string sql1 = "select * from asm_product where productID="+ _productID.Value;
                    dt1 = DbHelperSQL.Query(sql1).Tables[0];
                    string sql2 = "select * from asm_member where unionID='"+this._unionID.Value+"'";
                    dt2 = DbHelperSQL.Query(sql2).Tables[0];

                    string sql3 = "select * from asm_tqlist where companyID=" + this._companyID.Value;
                    DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];
                    if (d3.Rows.Count > 0 && d3.Rows[0]["memberprice"].ToString() == "1")
                    {
                        //1普通会员2白银会员3黄金会员
                        if (dt2.Rows[0]["dj"].ToString() == "0")
                        {
                            djName = "游客";
                            

                        }
                        else if (dt2.Rows[0]["dj"].ToString() == "1")
                        {
                            djName = "普通会员";
                            
                        }
                        else if (dt2.Rows[0]["dj"].ToString() == "2")
                        {
                            djName = "白银会员";
                           
                        }
                        else if (dt2.Rows[0]["dj"].ToString() == "3")
                        {
                            djName = "黄金会员"; 
                           
                        }
                        price =Util.getNewProductPrice(this._productID.Value, this._mechineID.Value, dt2.Rows[0]["dj"].ToString());
                    }
                    else {
                        price = dt1.Rows[0]["price0"].ToString();
                    }
                   
                    this._money.Value = price;
                    if (dt1.Rows.Count<=0||dt2.Rows.Count<=0)
                    {
                        Response.Write("<span style='color:#FF0000;font-size:20px'>" + "信息读取失败" + "</span>");
                        return;
                    }
                }
                catch {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "参数不全" + "</span>");
                    return;
                }
               
            }
        }
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        [WebMethod]
        public static string yzPwd(string unionID, string money, string companyID, string productID,string mechineID,string type,string dgOrderDetailID,string sftj)
        {
            string ldno = Util.getLDNO(mechineID, productID);
            if (string.IsNullOrEmpty(ldno))
            {
                return "5";//当前机器库存不足，请等待配送员上货

            }
            Random rd = new Random();
            int rand = rd.Next(10000, 99999);
            string sql2 = "select * from asm_company where id='" + companyID + "'";
            DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
            string trxid = ConvertDateTimeToInt(DateTime.Now).ToString() + rand;
            string insertSQL = "insert into asm_pay_info(trxid,statu,type,payType,trxamt,createTime,appid,unionID,mechineID,productID,companyID,orderType,dgOrderDetailID,sftj) values('" + trxid + "',0,2,4," + double.Parse(money) * 100 + ",'" + DateTime.Now + "','" + d2.Rows[0]["tl_APPID"].ToString() + "','" + unionID+ "','"+ mechineID + "','"+ productID + "','"+ companyID + "','"+type+"','"+dgOrderDetailID+"','"+ sftj + "')";
            Util.Debuglog("insertSQL="+ insertSQL, "余额支付.txt");
            int a = DbHelperSQL.ExecuteSql(insertSQL);
    

            //验证密码是否正确
            string sql = "select * from asm_member where unionID='" + unionID + "'";
            Util.Debuglog("sql=" + sql, "余额支付.txt");
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];

           
            string openID = dd.Rows[0]["openID"].ToString();
            //判断该订单支付状态
            string sql1 = "select * from asm_pay_info where trxid='" + trxid + "'";
            Util.Debuglog("sql1=" + sql1, "余额支付.txt");
            DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
            if (d1.Rows.Count > 0 && d1.Rows[0]["statu"].ToString() == "1")
            {
                return "4";//已经支付完成无需重复支付
            }
            //判断余额
            if (double.Parse(dd.Rows[0]["AvailableMoney"].ToString()) - double.Parse(money)<0)
            {
                return "2";//余额不足
            }
            //更新余额
            string update = "update asm_member set AvailableMoney=AvailableMoney-" + money + ",consumeCount=consumeCount+1,sumConsume=sumConsume+" + money + ",LastTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where openID='" + openID + "'";
            Util.Debuglog("update=" + update, "余额支付.txt");
            DbHelperSQL.ExecuteSql(update);
            string sqlM = "select addres,id,mechineName from asm_mechine where id in("+mechineID+")";
            Util.Debuglog("sqlM=" + sqlM, "余额支付.txt");
            DataTable dM = DbHelperSQL.Query(sqlM).Tables[0];
            string address = "";
            if (dM.Rows.Count > 0)
            {
                address = dM.Rows[0]["addres"].ToString();
            }
            //发送消息模板

            ////插入记录
            //Util.insertNotice(dd.Rows[0]["id"].ToString(),"余额变动提醒", "您于"+DateTime.Now.ToString("yyyy/MM/dd HH:mm")+"购物消费："+money+"元；余额："+ (double.Parse(dd.Rows[0]["AvailableMoney"].ToString())-double.Parse(money)));
            //Util.moneyChange(dd.Rows[0]["id"].ToString(),money,(double.Parse(dd.Rows[0]["AvailableMoney"].ToString()) - double.Parse(money)).ToString(), "会员消费", "2", "");
            //接着更新订单的状态
           
            string update1 = "update asm_pay_info set chLdNo='"+ldno+"', trxdate='" + DateTime.Now.ToString("yyyyMMdd")+"',statu=1,paytime='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',acct='" + openID + "',afterMoney="+(double.Parse(dd.Rows[0]["AvailableMoney"].ToString())-double.Parse(money))+" where trxid='" + trxid + "' and unionID='" + unionID + "'";
            Util.Debuglog("update1=" + update1+";料道编号="+ ldno, "余额支付.txt");
            DbHelperSQL.ExecuteSql(update1);
            Util.ch(ldno, dM.Rows[0]["id"].ToString(), trxid, "3", productID, money);
            string sqlP = "SELECT * FROM asm_product where productID=" + productID;
            Util.Debuglog("sqlP=" + sqlP + ";料道编号=" + ldno, "余额支付.txt");
            DataTable dp = DbHelperSQL.Query(sqlP).Tables[0];
            string updateM = "update asm_member set mechineID="+dM.Rows[0]["id"].ToString()+" where id="+dd.Rows[0]["id"].ToString();
            DbHelperSQL.ExecuteSql(updateM);
            Util.chgMoney(dd.Rows[0]["id"].ToString(), money, "会员消费", "购买" + dp.Rows[0]["proName"].ToString(), "2");
            Util.Debuglog("111111", "余额支付.txt");
             
            wxHelper wx = new wxHelper(companyID);
            string data = TemplateMessage.comsume(openID, OperUtil.getMessageID(companyID, "OPENTM401313503"), "亲，你的购买的商品信息如下",
                "" + dp.Rows[0]["proName"].ToString() + "", money, trxid, dM.Rows[0]["mechineName"].ToString(), "“机器已出货，请尽快推开机器左下方推板取出奶品，超过1分钟未取视为丢弃奶品，推板将关闭");
            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
            return "3";
        }

    }
}