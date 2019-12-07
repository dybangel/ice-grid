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

namespace Consumer.pay
{
    public partial class czNotify : System.Web.UI.Page
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
                    //获取openid查询这个openid的companyID 的appkey
                    string sql12 = "select * from asm_member where minOpenID='" + dt.Rows[0]["acct"].ToString() + "'";
                    DataTable d1 = DbHelperSQL.Query(sql12).Tables[0];
                   
                    if (1 == 1 && dt.Rows[0]["statu"].ToString() == "0")//验签成功
                    {
                        //验签成功后,进行业务处理,处理完毕返回成功
                        string trxdate = Request.Form["trxdate"];
                        string paytime = Request.Form["paytime"];
                        string acct = Request.Form["acct"];
                        string chnltrxid = Request.Form["chnltrxid"];
                        double trxamtY = double.Parse(Request.Form["trxamt"]) / 100;
                        trxamtY = double.Parse(dt.Rows[0]["trxamt"].ToString()) / 100;
                        //支付成功向asm_pay 表 更新记录
                        string updateSQL = "update asm_pay_info set paytime='" + paytime + "',statu='1',trxdate='" + trxdate + "',chnltrxid='" + chnltrxid + "',afterMoney="+(double.Parse(d1.Rows[0]["AvailableMoney"].ToString())+ trxamtY) +" where trxid='" + jo["trxid"].ToString() + "'";
                        int a = DbHelperSQL.ExecuteSql(updateSQL);

                        if (a > 0)
                        {
                             
                            //更新会员余额
                            string update = "update asm_member set AvailableMoney=AvailableMoney+" + dt.Rows[0]["dzMoney"].ToString() + ",sumRecharge=sumRecharge+" + dt.Rows[0]["dzMoney"].ToString() + ",point=point+" + trxamtY + " where minOpenID='" + dt.Rows[0]["acct"].ToString() + "'";
                            Util.Debuglog("update=" + update, "会员充值.txt");
                            DbHelperSQL.ExecuteSql(update);
                           
                            Util.chgMoney(d1.Rows[0]["id"].ToString(), dt.Rows[0]["dzMoney"].ToString(),"会员充值", "充值：" + double.Parse(trxamtY.ToString()).ToString("f2") + "元；实际到账：" + double.Parse(dt.Rows[0]["dzMoney"].ToString()).ToString("f2") + "元","1");
                            string tqID=Util.growUpMemberBYCZ(dt.Rows[0]["acct"].ToString(), trxamtY.ToString(), dt.Rows[0]["companyID"].ToString());//充值升级
                            //插入参加的活动记录
                            string activityID = dt.Rows[0]["activityID"].ToString();
                            if (!string.IsNullOrEmpty(activityID))
                            {
                                string sqlAc = "select * from asm_pay_activity where status=1 and id='" + activityID + "'";
                                DataTable dac = DbHelperSQL.Query(sqlAc).Tables[0];
                                if (dac.Rows.Count>0)
                                {
                                    string zsType = dac.Rows[0]["type"].ToString();
                                    string status = "1";
                                    string type = "";
                                    if (zsType == "2")
                                    {
                                        type = "5";
                                    } else if (zsType=="1")
                                    {
                                        type = "4";
                                    }
                                    if (dac.Rows[0]["type"].ToString() == "2")
                                    {
                                        status = "0";//手动处理的时候发通知
                                    }
                                    else {
                                        try
                                        {
                                            //发通知
                                            if (!string.IsNullOrEmpty(d1.Rows[0]["openID"].ToString()))
                                            {

                                                string companyID = d1.Rows[0]["companyID"].ToString();
                                                string openID = d1.Rows[0]["openID"].ToString();
                                                wxHelper wx = new wxHelper(companyID);
                                                string data = TemplateMessage.getPrize(openID, "hPFDCcfuANnDAGaIaAjsAnDKfgFXK-Y0SYGK12iIsAM", "活动奖励通知", dac.Rows[0]["activityContent"].ToString(), dac.Rows[0]["activityName"].ToString(), "请尽快到小程序查看奖励");
                                                TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(d1.Rows[0]["companyID"].ToString()), data);
                                            }
                                        }
                                        catch { }
                                    }
                                    string sqlActivity = "insert into asm_partActivity(memberID,partTime,type,activityContent,companyID,zsType,status,activityName,totalMoney,tqID) values('" + d1.Rows[0]["id"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',2,'"+dac.Rows[0]["tag"].ToString()+"','"+dac.Rows[0]["companyID"].ToString()+"','"+type+"',"+status+",'"+dac.Rows[0]["payName"].ToString()+"',"+trxamtY+",'"+tqID+"')";
                                    DbHelperSQL.ExecuteSql(sqlActivity);
                                }
                            }
                            Util.insertNotice(d1.Rows[0]["id"].ToString(), "充值到账通知", "充值成功到账金额：" + double.Parse(dt.Rows[0]["dzMoney"].ToString()).ToString("f2") + "元", "");
                            //发送模板消息
                            if (!string.IsNullOrEmpty(d1.Rows[0]["openID"].ToString()))
                            {
                               
                                string companyID = d1.Rows[0]["companyID"].ToString();
                                string openID= d1.Rows[0]["openID"].ToString();
                                Util.Debuglog("发送模板消息=companyID=" + companyID+ ";openID="+ openID, "会员充值.txt");
                                wxHelper wx = new wxHelper(companyID);
                                string data = TemplateMessage.success_cz(openID, "Tmin60E6DJtBO962B_5BEzVRC3Rbdv1JrKQNuzoY0Gw", 
                                    "充值成功通知", trxamtY.ToString(),d1.Rows[0]["AvailableMoney"].ToString(), "充值时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(d1.Rows[0]["companyID"].ToString()), data);
                            }
                        }
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