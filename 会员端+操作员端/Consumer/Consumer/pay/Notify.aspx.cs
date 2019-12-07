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

namespace uniondemo
{
    public partial class Notify : System.Web.UI.Page
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
                Util.Debuglog("key=" + Request.Form.Keys[i] + ";param="+ Request.Form[i].ToString(), "_充值日志.txt");

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
                Util.Debuglog("支付信息sql=" + sql, "_充值日志.txt");
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    //获取openid查询这个openid的companyID 的appkey
                    string sql12 = "select * from asm_member where openID='"+dt.Rows[0]["acct"].ToString()+"'";
                 
                    DataTable d1 = DbHelperSQL.Query(sql12).Tables[0];
                    string sql2 = "select * from asm_company where id="+d1.Rows[0]["companyID"].ToString();
                  
                    DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                    //AppUtil.validSign(reqParams, d2.Rows[0]["tl_APPKEY"].ToString(),d1.Rows[0]["companyID"].ToString())
                    Util.Debuglog("支付信息状态=" + dt.Rows[0]["statu"].ToString(), "_充值日志.txt");
                    if (1==1&&dt.Rows[0]["statu"].ToString()=="0")//验签成功
                    {
                        //验签成功后,进行业务处理,处理完毕返回成功
                        string trxdate = Request.Form["trxdate"];
                        string paytime = Request.Form["paytime"];
                        string acct = Request.Form["acct"];
                        string chnltrxid = Request.Form["chnltrxid"];
                        double trxamtY = double.Parse(Request.Form["trxamt"])/100;
                        trxamtY = double.Parse(dt.Rows[0]["trxamt"].ToString())/100;
                        //支付成功向asm_pay 表 更新记录
                        string updateSQL = "update asm_pay_info set paytime='"+paytime+"',statu='1',trxdate='"+trxdate+"',chnltrxid='"+chnltrxid+"' where trxid='" + jo["trxid"].ToString() + "'";
                        int a=DbHelperSQL.ExecuteSql(updateSQL);

                        if (a>0)
                        {
                            ////更新会员余额
                            //string update = "update asm_member set AvailableMoney=AvailableMoney+" + dt.Rows[0]["dzMoney"].ToString() + ",sumRecharge=sumRecharge+" + dt.Rows[0]["dzMoney"].ToString() + ",point=point+" + trxamtY + " where openID='" + dt.Rows[0]["acct"].ToString() + "'";
                            //Util.Debuglog("充值更新sql" + update, "_充值日志.txt");
                            //DbHelperSQL.ExecuteSql(update);
                            //string sql12 = "select * from asm_member where openID='" + dt.Rows[0]["acct"].ToString() + "'";
                            //DataTable dd = DbHelperSQL.Query(sql12).Tables[0];
                            //wxHelper wx = new wxHelper(d1.Rows[0]["companyID"].ToString());
                            //string data = TemplateMessage.success_cz(dt.Rows[0]["acct"].ToString(), OperUtil.getMessageID(d1.Rows[0]["companyID"].ToString(), "OPENTM410481462"), "充值成功通知", dt.Rows[0]["dzMoney"].ToString(), dd.Rows[0]["AvailableMoney"].ToString(), "充值时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            //TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(d1.Rows[0]["companyID"].ToString()), data);
                            ////插入记录
                            //Util.insertNotice(dd.Rows[0]["id"].ToString(), "充值成功通知", "您于" + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + "充值金额:" + trxamtY + ";到账金额:" + dt.Rows[0]["dzMoney"].ToString());
                            //Util.moneyChange(dd.Rows[0]["id"].ToString(), trxamtY.ToString(), dd.Rows[0]["AvailableMoney"].ToString(), "会员充值", "1", "");
                            //Util.moneyChange(dd.Rows[0]["id"].ToString(), trxamtY.ToString(), dd.Rows[0]["point"].ToString(), "积分变动", "5", "");

                            
                            //更新会员余额
                            string update = "update asm_member set AvailableMoney=AvailableMoney+" + dt.Rows[0]["dzMoney"].ToString() + ",sumRecharge=sumRecharge+" + dt.Rows[0]["dzMoney"].ToString() + ",point=point+" + trxamtY + " where openID='" + dt.Rows[0]["acct"].ToString() + "'";
                            Util.Debuglog("充值更新update1" + update, "_充值日志.txt");
                            DbHelperSQL.ExecuteSql(update);
                            string sql1 = "select * from asm_member where openID='" + dt.Rows[0]["acct"].ToString() + "'";
                            string shouchongMoney = "0";
                            Util.Debuglog("充值更新sql1" + sql1, "_充值日志.txt");
                            DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
                            shouchongMoney = dd.Rows[0]["hongbaoF"].ToString();
                            //判断系统是否开启首次充值赠送金额活动0不开启
                            string sqlC = "select * from asm_company where id=" + d1.Rows[0]["companyID"].ToString();
                            Util.Debuglog("充值更新sqlC" + sqlC+ ";shouchongMoney="+ shouchongMoney, "_充值日志.txt");
                            DataTable dc = DbHelperSQL.Query(sqlC).Tables[0];
                            if (dc.Rows.Count > 0)
                            {
                                if (dc.Rows[0]["p4"].ToString() != "" && dc.Rows[0]["p4"].ToString() != null && dc.Rows[0]["p4"].ToString() != "0")
                                {
                                    //判断是否是在时间段内
                                    DateTime ze = Convert.ToDateTime(dd.Rows[0]["createDate"].ToString());
                                    if (ze.AddDays(int.Parse(dc.Rows[0]["p12"].ToString())) > DateTime.Now)
                                    {
                                        Util.Debuglog("p11" + dc.Rows[0]["p11"].ToString()+ ";trxamtY"+ trxamtY, "_充值日志.txt");
                                        //并且大于激活下限
                                        if (double.Parse(shouchongMoney) > 0 && double.Parse(dc.Rows[0]["p11"].ToString()) <=trxamtY)
                                        {
                                            //给发红包
                                            string update1 = "update asm_member set AvailableMoney=AvailableMoney+" + shouchongMoney + ",hongbaoF=0 where openID='" + dt.Rows[0]["acct"].ToString() + "'";
                                            Util.Debuglog("充值更新update" + update1, "_充值日志.txt");
                                            DbHelperSQL.ExecuteSql(update1);
                                            dd = DbHelperSQL.Query(sql1).Tables[0];
                                            Util.moneyChange(dd.Rows[0]["id"].ToString(), shouchongMoney, dd.Rows[0]["AvailableMoney"].ToString(), "首冲红包", "7", "");
                                        }
                                    }
                                }
                            }
                            wxHelper wx = new wxHelper(d1.Rows[0]["companyID"].ToString());
                            string data = TemplateMessage.success_cz(dt.Rows[0]["acct"].ToString(), OperUtil.getMessageID(d1.Rows[0]["companyID"].ToString(), "OPENTM410481462"), "充值成功通知", trxamtY.ToString(), dd.Rows[0]["AvailableMoney"].ToString(), "充值时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(d1.Rows[0]["companyID"].ToString()), data);
                            //插入记录
                            Util.insertNotice(dd.Rows[0]["id"].ToString(), "充值成功通知", "您于" + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + "充值金额:" + trxamtY + ";到账金额:" + dt.Rows[0]["dzMoney"].ToString(),"");
                            Util.moneyChange(dd.Rows[0]["id"].ToString(), trxamtY.ToString(), dd.Rows[0]["AvailableMoney"].ToString(), "会员充值", "1", "");
                            Util.moneyChange(dd.Rows[0]["id"].ToString(), trxamtY.ToString(), dd.Rows[0]["point"].ToString(), "积分变动", "5", "");
                        }
                      
                    }
                   
                }
                else {
                    Response.Write("error");
                    return;
                }
            }
            else {
                Response.Write("error");
                return;
            }
          
        }
    }
}