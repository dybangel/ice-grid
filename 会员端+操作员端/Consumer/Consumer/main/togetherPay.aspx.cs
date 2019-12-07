using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Consumer.main
{
    public partial class togetherPay : System.Web.UI.Page
    {
        public String companyID = "";
        public String mechineID = "";
        public String product = "";
        public double money = 0;
        public String productName = "";
        public string productID = "";
        public string dgOrderDetailID = "";//如果是半价出售的才会有值 且不是0
        public string type = "";// 2零售 3半价
        public string sftj = "";//是否是参与了限时特价 1参加了  0未参加
        public string timespan = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                  
                    timespan = Request.QueryString["timespan"].ToString();
                    companyID = Request.QueryString["companyID"].ToString();
                    mechineID = Request.QueryString["mechineID"].ToString();
                    sftj = Request.QueryString["sftj"].ToString();
                    string sql1 = "select * from asm_mechine where id='" + mechineID + "'";
                    DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                    DateTime time = Util.GetTime(timespan);
                    long l1 = (DateTime.Now.Ticks - time.Ticks) / 10000000 / 60;
                    Util.Debuglog("time=" + time + ";l1=" + l1 + ";timespan=" + timespan + ";now="+ DateTime.Now.Ticks, "订单超时.txt");
                    if (l1>=30)
                    {
                        Response.Write("<script>alert('订单超时')</script>");
                        return;
                    }
                    if (mechineID!="25")
                    {
                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["openStatus"].ToString() == "1")
                            {
                                Response.Write("<script>window.location.href='pause.aspx';</script>");
                                return;
                            }
                        }
                        if (Util.chStatus("0", mechineID) != "200")
                        {
                            Response.Write("<script>window.location.href='pause.aspx';</script>");
                            return;
                        }
                    }
                    try
                    {
                        string url = Request.Url.ToString();
                        string IP = Request.UserHostAddress;//获取远程客户端的IP 避免同一个二维码多个用户扫描
                        string res = "{\"ip\":\"" + IP + "\",\"timespan\":\""+timespan+"\"}";
                        Util.Debuglog("mechineID="+ IP, "获取支付链接.txt");
                        string result = RedisHelper.GetRedisModel<string>(mechineID + "_payurl");
                        Util.Debuglog("result=" + result, "获取支付链接.txt");
                        if (string.IsNullOrEmpty(result))
                        {
                            RedisHelper.SetRedisModel<string>(mechineID + "_payurl", res, new TimeSpan(0, 0, 30));
                        }
                        else
                        {
                            //判断redis值和实际值是否一致
                            JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                            string ip_redis = jo["ip"].ToString();
                            string timespan_redis = jo["timespan"].ToString();
                            Util.Debuglog("ip_redis="+ ip_redis, "获取支付链接.txt");
                            if (ip_redis != IP&&timespan==timespan_redis)
                            {
                                //弹出页面提示稍等付款
                                Response.Write("<script>alert('同一个二维码不允许多个用户支付');</script>");
                                return;
                            }
                        }
                    }
                    catch (Exception ex){
                        Util.Debuglog("ex="+ex.Message, "获取支付链接.txt");
                    }
                   
                    product = Request.QueryString["product"].ToString();
                    dgOrderDetailID = Request.QueryString["dgOrderDetailID"].ToString();
                    type = Request.QueryString["type"].ToString();
                    Util.Debuglog("toget="+"companyID=" + companyID + ";mechineID=" + mechineID + ";money=" + money + ";product=" + product + ";dgOrderDetailID=" + dgOrderDetailID + ";type=" + type+ ";sftj=", "1111.txt");
                    JArray jArray = (JArray)JsonConvert.DeserializeObject(product);//jsonArrayText必须是带[]数组格式字符串
                    if (jArray.Count > 0)
                    {
                        for (int i = 0; i < jArray.Count; i++)
                        {
                            productID = jArray[i]["productID"].ToString();
                            string sql = "select productID,proName,price0,price1,price2,price3 from asm_product where productID=" + productID;
                            int num = int.Parse(jArray[i]["num"].ToString());
                            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                //money += double.Parse(dt.Rows[i]["price0"].ToString()) * num;
                                money += double.Parse(Util.getNewProductPrice(productID,mechineID,"0")) * num;
                                productName = HttpUtility.UrlEncode(dt.Rows[0]["proName"].ToString());

                               string ldno= Util.getLDNO(mechineID, productID);
                                if (string.IsNullOrEmpty(ldno))
                                {
                                    Response.Write("<script>alert('未获取到出货料道编号，请重试');</script>");
                                    return;
                                }
                            }
                            else
                            {
                                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "产品不存在" + "</span>");
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                   
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "参数不全或金额不正确请重试" + "</span>");
                }

            }
        }
    }
}