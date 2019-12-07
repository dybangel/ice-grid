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

            for (int i = 0; i < Request.Form.Count; i++)
            {
                reqParams.Add(Request.Form.Keys[i], Request.Form[i].ToString());
                Util.Debuglog("key=" + Request.Form.Keys[i] + ";value=" + Request.Form[i].ToString(), "_聚合支付回调参数.txt");

            }
            if (!reqParams.ContainsKey("sign"))//如果不包含sign,则不进行处理
            {

                Response.Write("error");
                return;
            }
            if (reqParams.ContainsKey("trxid"))
            {

                string json = (new JavaScriptSerializer()).Serialize(reqParams);
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                string sql = "select * from asm_pay_info where trxid='" + jo["trxid"].ToString() + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    //获取appid查询这个appid的 的appkey
                    string sql2 = "select * from asm_company where tl_APPID='" + dt.Rows[0]["appid"].ToString() + "'";

                    DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                    //AppUtil.validSign(reqParams, d2.Rows[0]["tl_APPKEY"].ToString(), d2.Rows[0]["id"].ToString())
                    if (dt.Rows[0]["statu"].ToString() == "0")//验签成功
                    {
                        //验签成功后,进行业务处理,处理完毕返回成功
                        string trxdate = Request.Form["trxdate"];
                        string paytime = Request.Form["paytime"];
                        string acct = Request.Form["acct"];
                        string chnltrxid = Request.Form["chnltrxid"];
                        double trxamtY = double.Parse(Request.Form["trxamt"]) / 100;
                        //支付成功向asm_pay 表 更新记录
                        string updateSQL = "update asm_pay_info set paytime='" + paytime + "',statu='1',trxdate='" + trxdate + "',acct='" + acct + "',chnltrxid='" + chnltrxid + "' where trxid='" + jo["trxid"].ToString() + "'";
                        DbHelperSQL.ExecuteSql(updateSQL);
                        //需要更新会员的消费信息  此处如果是支付宝扫码的话没法更新
                        string update = "update asm_member set sumConsume=sumConsume+" + trxamtY + ",LastTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',point=point+" + trxamtY + " where openID='" + acct + "'";
                        DbHelperSQL.ExecuteSql(update);
                        //发送出货指令
                        Util.ch("23", "25");
                    }
                }
                else
                {
                    Response.Write("error");
                    return;
                }
            }
            else
            {
                Response.Write("error");
                return;
            }

        }
    }
}