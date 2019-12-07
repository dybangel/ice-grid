using autosell_center;
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
    public partial class togetherPayNew : System.Web.UI.Page
    {
        public String companyID = "";
        public String mechineID = "";
        public String product = "";
       // public String productName = "";
        public string productID = "";
        //public string dgOrderDetailID = "";//如果是半价出售的才会有值 且不是0
        //public string type = "";// 2零售 3半价
        //public string sftj = "";//是否是参与了限时特价 1参加了  0未参加
        public string reqsn = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    reqsn = Request.QueryString["reqsn"].ToString();
                    this._reqsn.Value = reqsn;
                    string asm_product_picksql = "select * from asm_product_pick  where reqsnNo='" + reqsn + "' ";
                    DataTable asm_product_pickdt = DbHelperSQL.Query(asm_product_picksql).Tables[0];
                    if (asm_product_pickdt.Rows.Count > 0)
                    {
                       
                        if (!string.IsNullOrEmpty(asm_product_pickdt.Rows[0]["scanProductTime"].ToString()))
                        {
                            Response.Write("<script>alert('该二维码已失效！');</script>");
                            return;
                        }
                    }
                    else {
                        Response.Write("<script>alert('未获取到信息请重试！')</script>");
                        return;
                    }
                    companyID = asm_product_pickdt.Rows[0]["companyID"].ToString();
                    mechineID = asm_product_pickdt.Rows[0]["mechineID"].ToString();
                    this._companyID.Value = companyID;
                    
                    Util.Debuglog(";reqsn=" + reqsn + ";now=" + DateTime.Now.Ticks, "订单超时.txt");
                   

                    string _mechineInfo = RedisUtil.getMechine(mechineID);
                    JArray _mechineJArray = RedisUtil.DeserializeObject(_mechineInfo);
                    if (_mechineJArray != null)
                    {
                        if (mechineID != "25")
                        {

                            if (_mechineJArray[0]["openStatus"].ToString() == "1")
                            {
                                Response.Write("<script>window.location.href='pause.aspx';</script>");
                                return;
                            }
                        }

                        if (_mechineJArray[0]["netStatus"].ToString() == "1" || _mechineJArray[0]["gkjStatus"].ToString() == "1")
                        {
                            Response.Write("<script>window.location.href='pause.aspx';</script>");
                            return;
                        }
                        if (_mechineJArray[0]["updateSoftStatus"].ToString() != "0")
                        {
                            Response.Write("<script>window.location.href='pause.aspx';</script>");
                            return;
                        }
                    }
                    
                    string pickupdate = "update  asm_product_pick set payStatus=1,scanProductTime='"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where reqsnNo='" + reqsn + "' ";
                    Util.Debuglog("sqlInsert=" + pickupdate, "获取预生成订单号.txt");
                    DbHelperSQL.ExecuteSql(pickupdate);
                    product = asm_product_pickdt.Rows[0]["product"].ToString();
                   
                    JArray jArray = (JArray)JsonConvert.DeserializeObject(product);//jsonArrayText必须是带[]数组格式字符串
                    Util.Debuglog("jArray=" + jArray.Count, "获取预生成订单号.txt");
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
                               
                                //productName = HttpUtility.UrlEncode(dt.Rows[0]["proName"].ToString());
                                //Util.Debuglog("sqlInsert=1111", "获取预生成订单号.txt");
                                string ldno= Util.getLDNO(mechineID, productID);

                                Util.Debuglog("ldno=" + ldno, "获取预生成订单号.txt");
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