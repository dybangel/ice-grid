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

namespace Consumer.pay
{
    public partial class wxOrderMany : System.Web.UI.Page
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
                string sql = "select p.trxid,p.activityID,o.productID,p.unionID,p.companyID,o.activityID acID,o.psMode,o.startTime,o.mechineID,o.memberID,o.orderNO,o.companyID,o.totalNum,o.source  from asm_pay_info p left join asm_orderlist o on p.trxid=o.trxid"
                            + " where p.trxid = '"+jo["trxid"]+"' and o.trxid = '"+jo["trxid"] +"' and statu = 0";
                DataTable dtInfo = DbHelperSQL.Query(sql).Tables[0];
                if (dtInfo.Rows.Count>0)
                {
                    string sql111 = "select top 1 * from asm_duihuan  where newOrderNo='"+ dtInfo.Rows[0]["orderNO"].ToString() + "'    order by dhTime desc ";
                    DataTable asm_duihuanInfo = DbHelperSQL.Query(sql111).Tables[0];
                    if (asm_duihuanInfo.Rows.Count > 0 && (!string.IsNullOrEmpty(asm_duihuanInfo.Rows[0]["oldOrderNo"].ToString()))) {
                        Util.Debuglog("sql111=" + sql111, "dhProduct.txt");
                        List<string> list = new List<string>();
                        string sql2 = "update asm_orderlist set orderZT=4 where orderNO='" + asm_duihuanInfo.Rows[0]["oldOrderNo"].ToString() + "'";
                        string sql3 = "update asm_orderlistDetail set zt=7 where  zt=5 and orderNO='" + asm_duihuanInfo.Rows[0]["oldOrderNo"].ToString() + "'";
                        list.Add(sql2);
                        list.Add(sql3);
                        Util.Debuglog("list=" + sql2+ sql3, "dhProduct.txt");
                        int a = DbHelperSQL.ExecuteSqlTran(list);

                    }
                    string trxdate = Request.Form["trxdate"];
                    string paytime = Request.Form["paytime"];
                    string acct = Request.Form["acct"];
                    string chnltrxid = Request.Form["chnltrxid"];
                    double trxamtY = double.Parse(Request.Form["trxamt"]) / 100;
                    //更新asm_pay_info asm_orderlist支付状态 
                    string update1 = "update asm_pay_info set statu=1,paytime='"+paytime+"',productID='"+ dtInfo.Rows[0]["productID"].ToString() + "',mechineID='"+ dtInfo.Rows[0]["mechineID"].ToString() + "' where trxid='" + jo["trxid"].ToString()+"'";
                    DbHelperSQL.ExecuteSql(update1);
                    string update2 = "update asm_orderlist set fkzt=1  where trxid='"+jo["trxid"].ToString()+"'";
                    DbHelperSQL.ExecuteSql(update2);
                    //需要更新会员的消费信息  
                    string update = "update asm_member set sumConsume=sumConsume+" + trxamtY + ",LastTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',mechineID='"+ dtInfo.Rows[0]["mechineID"].ToString() + "' where minOpenID='" + acct + "'";
                    DbHelperSQL.ExecuteSql(update);

                    string sqlActivity = "select * from asm_activity where status=1 and id=" + dtInfo.Rows[0]["activityID"].ToString();
                    //DataTable dactivity = DbHelperSQL.Query(sqlActivity).Tables[0];
                    


                    string[] selDate = OperUtil.getSelDate(dtInfo.Rows[0]["totalNum"].ToString(), dtInfo.Rows[0]["psMode"].ToString(), dtInfo.Rows[0]["startTime"].ToString()).Split(',');
                    if (selDate.Length > 0)
                    {

                        DataTable dtNew;
                        for (int j=0; j< dtInfo.Rows.Count;j++) {
                            string sql14 = "select * from asm_orderlistDetail where id=0";
                            dtNew = DbHelperSQL.Query(sql14).Tables[0];

                            for (int i = 0; i < selDate.Length; i++)
                            {
                                int code = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                                //zt   1-已完成；2-已失效；3-已转售；4-待取货；5-待配送
                                DataRow dr = dtNew.NewRow();
                                dr["mechineID"] = dtInfo.Rows[j]["mechineID"].ToString(); //通过索引赋值
                                dr["productID"] = dtInfo.Rows[j]["productID"].ToString();
                                dr["createTime"] = delTime(selDate[i]);//
                                //dr["code"] = code;//
                                dr["memberID"] = dtInfo.Rows[j]["memberID"].ToString(); //通过索引赋值
                                if (delTime(selDate[i]) == DateTime.Now.ToString("yyyy-MM-dd"))
                                {
                                    dr["zt"] = "4";
                                    dr["code"] = code;//
                                }
                                else
                                {
                                    dr["zt"] = "5";
                                }

                                dr["ldNO"] = "";//
                                dr["orderNO"] = dtInfo.Rows[j]["orderNO"].ToString();//
                                dr["statu"] = "0"; //通过索引赋值
                                dr["sellPrice"] = 0.0;
                                dr["sellTime"] = "";
                                dr["bz"] = "";
                                dr["companyID"] = dtInfo.Rows[j]["companyID"].ToString();
                                dtNew.Rows.Add(dr);

                            }
                            DbHelperSQL.BatchInsertBySqlBulkCopy(dtNew, "[dbo].[asm_orderlistDetail]");
                            string sql1 = "select * from asm_orderlistDetail where orderNO in ('" + dtInfo.Rows[j]["orderNO"].ToString() + "') ORDER BY createTime DESC ";
                            DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                            if (d1.Rows.Count > 0)
                            {
                                string update12 = "UPDATE asm_orderlist set endTime='" + d1.Rows[0]["createTime"].ToString() + "' WHERE orderNO in ('" + dtInfo.Rows[j]["orderNO"].ToString() + "')";
                                DbHelperSQL.ExecuteSql(update12);
                            }
                            if (dtInfo.Rows[j]["startTime"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                string updagte = "UPDATE asm_orderlist set orderzt=1 where orderno='" + dtInfo.Rows[j]["orderNO"].ToString() + "'";
                                DbHelperSQL.ExecuteSql(updagte);
                            }
                        }
                        
                    }

                   
                    if (dtInfo.Rows[0]["source"].ToString() != "1") {
                        Util.growUpMemberBYDG(acct, int.Parse(dtInfo.Rows[0]["totalNum"].ToString()) * dtInfo.Rows.Count, dtInfo.Rows[0]["companyID"].ToString());
                    }
                    
                    //发送模板消息
                    string sqlM = "select * from asm_member where minOpenID='" + acct + "'";
                    DataTable dM = DbHelperSQL.Query(sqlM).Tables[0];
                    //插入参加的活动记录
                    string activityID = dtInfo.Rows[0]["activityID"].ToString();
                    //兑换的产品不在继续参加活动source!=1
                    if (!string.IsNullOrEmpty(activityID)&& dtInfo.Rows[0]["source"].ToString()!="1")
                    {
                        string sqlAc = "select * from asm_activity where statu=1 and id='" + activityID + "'";
                        DataTable dac = DbHelperSQL.Query(sqlAc).Tables[0];
                        if (dac.Rows.Count > 0)
                        {
                            string status = "1";
                            if (dac.Rows[0]["type"].ToString() == "3")
                            {
                                status = "0";
                            }
                            else {
                                try
                                {
                                    //发通知
                                    if (!string.IsNullOrEmpty(dM.Rows[0]["openID"].ToString()))
                                    {

                                        string companyID = dM.Rows[0]["companyID"].ToString();
                                        string openID = dM.Rows[0]["openID"].ToString();
                                        wxHelper wx = new wxHelper(companyID);
                                        string data = TemplateMessage.getPrize(openID, "hPFDCcfuANnDAGaIaAjsAnDKfgFXK-Y0SYGK12iIsAM", "活动奖励通知", dac.Rows[0]["payName"].ToString(), dac.Rows[0]["tag"].ToString(), "请尽快到小程序查看奖励");
                                        TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dM.Rows[0]["companyID"].ToString()), data);
                                    }
                                }
                                catch { }
                            }
                            string zsType = dac.Rows[0]["type"].ToString();
                            string sqlActivityIn = "insert into asm_partActivity(memberID,partTime,type,activityContent,companyID,zsType,status,activityName,totalMoney) values('" + dtInfo.Rows[0]["memberID"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',1,'" + dac.Rows[0]["activitytag"].ToString() + "','" + dac.Rows[0]["companyID"].ToString() + "',"+zsType+ ","+status+",'" + dac.Rows[0]["activityname"].ToString() + "',"+ trxamtY + ")";
                            DbHelperSQL.ExecuteSql(sqlActivityIn);
                        }
                         
                    }
                  
                    if (dM.Rows.Count>0)
                    {
                        if (!string.IsNullOrEmpty(dM.Rows[0]["openID"].ToString()))
                        {
                            string openID = dM.Rows[0]["openID"].ToString();
                            string companyID = dM.Rows[0]["companyID"].ToString();
                            string sqlp = "select o.*,p.proName from asm_orderlist o left join asm_product p on o.productID=p.productID where trxid='" + jo["trxid"].ToString() + "'";
                            DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                            wxHelper wx = new wxHelper(companyID);
                            string data = TemplateMessage.comsume(openID, OperUtil.getMessageID(companyID, "OPENTM401313503"), "亲，你的购买的商品信息如下", 
                                "" + dp.Rows[0]["proName"].ToString() + "", dp.Rows[0]["totalMoney"].ToString(), dp.Rows[0]["orderNO"].ToString(), dp.Rows[0]["mechineName"].ToString(), "“生鲜时逐”订奶订单已生成，鲜活即将配送到家");
                            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(OperUtil.getCooki("companyID")), data);
                        }
                    }
                  
                }
            }
        }
        //处理时间
        public string delTime(string time)
        {
            string[] timeArr = time.Split('-');
            if (timeArr.Length == 3)
            {
                timeArr[1] = timeArr[1].PadLeft(2, '0');
                timeArr[2] = timeArr[2].PadLeft(2, '0');
                return timeArr[0] + "-" + timeArr[1] + "-" + timeArr[2];
            }
            else
            {
                return time;
            }

        }
    }
}