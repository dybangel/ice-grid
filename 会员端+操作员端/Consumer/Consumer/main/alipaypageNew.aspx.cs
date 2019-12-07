using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using uniondemo.com.allinpay.syb;

namespace Consumer.main
{
    public partial class alipaypageNew : System.Web.UI.Page
    {


        public string companyID;
        public string money = "0";
        public string productID = "";
        public string mechineID = "";
        public string dgOrderDetailID = "";
        public string type = "";
        public string reqsn = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    reqsn = Request.QueryString["reqsn"].ToString();
                   
                    string asm_pay_infosql = "select 1 from asm_pay_info  where reqsn='" + reqsn + "' ";
                    DataTable asm_pay_infodt = DbHelperSQL.Query(asm_pay_infosql).Tables[0];
                    if (asm_pay_infodt.Rows.Count > 0)
                    {
                        return;
                    }
                    string asm_product_picksql = "select * from asm_product_pick  where reqsnNo='" + reqsn + "' ";
                    DataTable asm_product_pickdt = DbHelperSQL.Query(asm_product_picksql).Tables[0];
                    if (asm_product_pickdt.Rows.Count > 0)
                    {
                        companyID = asm_product_pickdt.Rows[0]["companyID"].ToString();
                        productID = asm_product_pickdt.Rows[0]["productID"].ToString();
                        mechineID = asm_product_pickdt.Rows[0]["mechineID"].ToString();
                        dgOrderDetailID = asm_product_pickdt.Rows[0]["dgOrderDetailID"].ToString();
                        type = asm_product_pickdt.Rows[0]["type"].ToString();
                        //转售金额固定
                       
                    }
                  
                   // money = "1";
                    Util.Debuglog("companyID=" + companyID+ ";money="+ money+ ";productID="+ productID+ ";mechineID="+mechineID+ ";dgOrderDetailID="+ dgOrderDetailID+ ";type="+ type, "_聚合支付支付宝.txt");
                    SybWxPayService sybService = new SybWxPayService(companyID);
                    string url = "https://wx.bingoseller.com/pay/JHNotifyNew.aspx";
                    string data = "";
                    string sql = "select *from asm_product where productID="+productID;
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    
                    if (dgOrderDetailID != "0" && !string.IsNullOrEmpty(dgOrderDetailID))
                    {
                        string sqlDetail = "select * from asm_orderlistDetail where id=" + dgOrderDetailID;
                        DataTable dtDetail = DbHelperSQL.Query(sqlDetail).Tables[0];
                        if (dtDetail.Rows.Count > 0)
                        {

                            money = dtDetail.Rows[0]["sellPrice"].ToString();
                           
                        }
                    }
                    else {
                        money = Util.getNewProductPrice(productID, mechineID, "0");
                    }
                    Dictionary<String, String> rsp = sybService.pay(long.Parse((double.Parse(money)*100).ToString()), reqsn, "A01", dt.Rows[0]["proName"].ToString(), "商品消费", "", "", url, "");
                    data = OperUtil.SerializeDictionaryToJsonString(rsp);
                    string json = (new JavaScriptSerializer()).Serialize(rsp);
                    Util.Debuglog("json=" + json, "_聚合支付支付宝.txt");
                    JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                    if (jo["retcode"].ToString() == "SUCCESS")
                    {
                        string pickupdate = "update  asm_product_pick set payStatus=2,sacntype='2',paytype='2',startPayTime='"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where reqsnNo='" + reqsn + "' ";
                        Util.Debuglog("sqlInsert=" + pickupdate, "获取预生成订单号.txt");
                        DbHelperSQL.ExecuteSql(pickupdate);
                        string appid = jo["appid"].ToString();
                        string cusid = jo["cusid"].ToString();
                        string trxid = jo["trxid"].ToString();
                        RedisHelper.SetRedisModel<string>(trxid, trxid, new TimeSpan(0, 2, 0));
                        string reqsnNew = jo["reqsn"].ToString();
                        string payinfoUrl = jo["payinfo"].ToString();
                        Util.Debuglog("payinfoUrl=" + payinfoUrl, "_聚合支付支付宝.txt");
                        string insertSQL = @"insert into asm_pay_info(appid,cusid,trxid,statu,reqsn,type,payType,trxamt,createTime,mechineID,productID,companyID,dgOrderDetailID,orderType)
                                            values('" + appid + "','" + cusid + "','" + trxid + "','0','" + reqsnNew + "',2,2," + double.Parse(money) * 100 + ",'" + DateTime.Now + "','"+mechineID+"','"+productID +"','"+ companyID + "','"+dgOrderDetailID+"','"+type+"')";
                        Util.Debuglog("insertSQL="+ insertSQL, "_聚合支付支付宝.txt");
                        DbHelperSQL.ExecuteSql(insertSQL);
                        Response.Write("<script>window.location.href='" + payinfoUrl + "';</script>");
                        // //插入预订单信息


                    }
                }
                catch (Exception ex)
                {
                    Util.Debuglog("Exception=" + ex.Message, "_聚合支付支付宝.txt");
                }

            }
        }
    }
}