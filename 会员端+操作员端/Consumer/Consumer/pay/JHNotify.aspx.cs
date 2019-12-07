using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.pay
{
    public partial class JHNotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<String, String> reqParams = new Dictionary<String, String>();
            /**
             * 此处注意,因为通联收银宝以后可能增加字段,所以,这里一定要动态遍历获取所有的请求参数
             * 
             * */
            Util.Debuglog("key=回调开始", "_聚合支付回调参数.txt");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Request.Form.Count; i++)
            {
                reqParams.Add(Request.Form.Keys[i], Request.Form[i].ToString());
                sb.Append("key=" + Request.Form.Keys[i] + ";value=" + Request.Form[i].ToString());
                

            }
            Util.Debuglog(sb.ToString(), "_聚合支付回调参数.txt");
            if (!reqParams.ContainsKey("sign"))//如果不包含sign,则不进行处理
            {

                Response.Write("error");
                return;
            }
            if (reqParams.ContainsKey("trxid"))
            {
                string trxid = reqParams["trxid"];
                Util.Debuglog("keytrxid:" + trxid, "_聚合支付回调参数.txt");
                Task.Run(() =>
                {
                    paycall(reqParams);
                });
               
                Util.Debuglog("返回通联successtrxid:" + trxid, "_聚合支付回调参数.txt");
                Response.Write("SUCCESS");
                return;
            }
            else
            {
                Util.Debuglog("错误keyelse=回调结束", "_聚合支付回调参数.txt");
                Response.Write("error");
                return;

            }
            
        }
        public static async System.Threading.Tasks.Task paycall(Dictionary<String, String> reqParams) {
            string json = (new JavaScriptSerializer()).Serialize(reqParams);
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            string sql = "select * from asm_pay_info where statu=0 and trxid='" + jo["trxid"].ToString() + "'";
            Util.Debuglog("sql=" + sql, "出货信息.txt");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //获取appid查询这个appid的 的appkey
                string sql2 = "select * from asm_company where tl_APPID='" + dt.Rows[0]["appid"].ToString() + "'";
                Util.Debuglog("sql2=" + sql2, "出货信息.txt");
                DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                //AppUtil.validSign(reqParams, d2.Rows[0]["tl_APPKEY"].ToString(), d2.Rows[0]["id"].ToString())
                if (dt.Rows[0]["statu"].ToString() == "0")//验签成功
                {
                    //验签成功后,进行业务处理,处理完毕返回成功
                    string trxdate = jo["trxdate"].ToString();
                    string paytime = jo["paytime"].ToString(); 
                    string acct = jo["acct"].ToString(); 
                    string chnltrxid = jo["chnltrxid"].ToString(); 
                    double trxamtY = double.Parse(jo["trxamt"].ToString()) / 100;

                    //发送出货指令
                    string ldno = "";
                    //继续查找
                    bool b = true;
                    int num = 3;
                    while (b && num > 0)
                    {
                        num--;
                        ldno = Util.getLDNO(dt.Rows[0]["mechineID"].ToString(), dt.Rows[0]["productID"].ToString());
                        if (!string.IsNullOrEmpty(ldno))
                        {
                            b = false;
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    Util.Debuglog("mechineID=" + dt.Rows[0]["mechineID"].ToString() + ";productID=" + dt.Rows[0]["productID"].ToString() + ";LDNO=" + ldno, "出货信息.txt");
                    if (!string.IsNullOrEmpty(ldno))
                    {
                        Util.ch(ldno, dt.Rows[0]["mechineID"].ToString(), jo["trxid"].ToString(), dt.Rows[0]["payType"].ToString(), dt.Rows[0]["productID"].ToString(), trxamtY.ToString());
                    }

                    Util.Debuglog("出货指令发送完成mechineID=" + dt.Rows[0]["mechineID"].ToString() + ";productID=" + dt.Rows[0]["productID"].ToString() + ";LDNO=" + ldno, "出货信息.txt");

                    //支付成功向asm_pay 表 更新记录
                    if (dt.Rows[0]["payType"].ToString() == "2")
                    {
                        string updateSQL = "update asm_pay_info set chLdNo='" + ldno + "', acct='" + acct + "', paytime='" + paytime + "',statu='1',trxdate='" + trxdate + "',chnltrxid='" + chnltrxid + "' where trxid='" + jo["trxid"].ToString() + "'";
                        Util.Debuglog("updateSQL=" + updateSQL, "出货信息.txt");
                        DbHelperSQL.ExecuteSql(updateSQL);
                    }
                    else
                    {
                        string updateSQL = "update asm_pay_info set chLdNo='" + ldno + "', paytime='" + paytime + "',statu='1',trxdate='" + trxdate + "',chnltrxid='" + chnltrxid + "' where trxid='" + jo["trxid"].ToString() + "'";
                        Util.Debuglog("updateSQL=" + updateSQL, "出货信息.txt");
                        DbHelperSQL.ExecuteSql(updateSQL);
                    }

                    //需要更新会员的消费信息  此处如果是支付宝扫码的话没法更新
                    if (!string.IsNullOrEmpty(dt.Rows[0]["unionID"].ToString()))
                    {
                        string update = "update asm_member set sumConsume=sumConsume+" + trxamtY + ",consumeCount=consumeCount+1,LastTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',point=point+" + trxamtY + ",mechineID='" + dt.Rows[0]["mechineID"].ToString() + "' where unionID='" + dt.Rows[0]["unionID"].ToString() + "'";
                        Util.Debuglog("update=" + update, "更新会员余额消费信息.txt");
                        DbHelperSQL.ExecuteSql(update);
                    }

                    string sqlM = "select * from asm_member where unionID='" + dt.Rows[0]["unionID"].ToString() + "'";
                    DataTable dtM = DbHelperSQL.Query(sqlM).Tables[0];

                    //此处判断如果orderType=3是半价的需要根据dgOrderDetailID修改zt为售卖完成 并且给出售人加余额
                    if (dt.Rows[0]["orderType"].ToString() == "3" && dt.Rows[0]["dgOrderDetailID"].ToString() != "0" && !string.IsNullOrEmpty(dt.Rows[0]["dgOrderDetailID"].ToString()))
                    {
                        string sqlDetail = "select * from asm_orderlistDetail where id=" + dt.Rows[0]["dgOrderDetailID"].ToString();
                        DataTable dtDetail = DbHelperSQL.Query(sqlDetail).Tables[0];
                        if (dtDetail.Rows.Count > 0 && dtM.Rows.Count > 0)
                        {
                            string id = dt.Rows[0]["dgOrderDetailID"].ToString();
                            string memberID = dtM.Rows[0]["id"].ToString();
                            string productID = dt.Rows[0]["productID"].ToString();
                            string updateSql = "update asm_orderlistDetail set zt=6 where id=" + id;
                            int a = DbHelperSQL.ExecuteSql(updateSql);
                            if (a > 0)
                            {
                                RedisHelper.Remove(dt.Rows[0]["mechineID"].ToString() + "_SellOrderInfo");
                                //更新余额
                                string sqlPro = "select * from asm_product where productID=" + productID;
                                DataTable dpro = DbHelperSQL.Query(sqlPro).Tables[0];
                                double price = double.Parse(dpro.Rows[0]["price0"].ToString()) / 2;
                                string updateMember = "update asm_member set AvailableMoney=AvailableMoney+" + price + " where id=" + memberID;
                                DbHelperSQL.ExecuteSql(updateMember);
                                Util.chgMoney(memberID, price.ToString(), "售卖", "出售" + dtDetail.Rows[0]["createTime"].ToString() + "日产品", "5");
                            }
                        }
                    }
                    //给会员升级
                    Util.growUpMember(dt.Rows[0]["unionID"].ToString(), "");
                    if (dtM.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dtM.Rows[0]["openID"].ToString()))
                        {
                            string companyID = dtM.Rows[0]["companyID"].ToString();
                            string openID = dtM.Rows[0]["openID"].ToString();
                            string sqlp = "select * from asm_product where productID=" + dt.Rows[0]["productID"].ToString();
                            DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                            string sqlMechine = "select * from asm_mechine where id=" + dt.Rows[0]["mechineID"].ToString();
                            DataTable dMechine = DbHelperSQL.Query(sqlMechine).Tables[0];
                            wxHelper wx = new wxHelper(companyID);
                            string data = TemplateMessage.comsume(openID, OperUtil.getMessageID(companyID, "OPENTM401313503"), "亲，你的购买的商品信息如下",
                                "" + dp.Rows[0]["proName"].ToString() + "", trxamtY.ToString(), jo["trxid"].ToString(), dMechine.Rows[0]["mechineName"].ToString(), "“机器已出货，请尽快推开机器左下方推板取出奶品，超过1分钟未取视为丢弃奶品，推板将关闭");
                            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
                        }
                    }

                }

            }
           
            Util.Debuglog("keyif=回调结束", "_聚合支付回调参数.txt");
        }
    }
}