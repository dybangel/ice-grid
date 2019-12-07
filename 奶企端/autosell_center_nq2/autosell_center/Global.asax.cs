using autosell_center.api;
using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;
using WZHY.Common.DEncrypt;

namespace autosell_center
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {

            

            //myTimer_Elapsed(null, null);
            System.Timers.Timer myTimer = new System.Timers.Timer(1000 * 60 * 5);
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;
            System.Timers.Timer myTimerT = new System.Timers.Timer(1000 * 60 * 30);
            myTimerT.Elapsed += new ElapsedEventHandler(myTimer_ElapsedT);
            myTimerT.Enabled = true;
            myTimerT.AutoReset = true;
            System.Timers.Timer myTimerM = new System.Timers.Timer(1000 * 60);//定时执行更新会员等级
            myTimerM.Elapsed += new ElapsedEventHandler(myTimer_ElapsedM);
            myTimerM.Enabled = true;
            myTimerM.AutoReset = true;



            //判断机器是否过期
            string sql5 = "update asm_mechine set zt=3 where DATEDIFF(dd,GETDATE(),convert(datetime,validateTime))<0 and zt=2";

            DbHelperSQL.ExecuteSql(sql5);
            

            System.Timers.Timer myTimer1 = new System.Timers.Timer(1000 * 60 * 60 * 6);//6小时更新一次
            myTimer1.Elapsed += new ElapsedEventHandler(myTimer1_Elapsed);
            myTimer1.Enabled = true;
            myTimer1.AutoReset = true;
            

        }


        public void clearTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            if (DateTime.Now.ToString("HH:mm") == "00:00")
            {
                Util.ClearRedisProductInfo();
                string sql = "update asm_ldInfo set ld_productNum=0 ,productID='' where type=1";
                DbHelperSQL.ExecuteSql(sql);
            }

        }
        /// <summary>
        /// 2019-09-04发送会员公众号通知
        /// </summary>
        public void getProductbg()
        {
            string sqlc = "select * from asm_company";
            DataTable dc1 = DbHelperSQL.Query(sqlc).Tables[0];
            if (dc1.Rows.Count > 0)
            {
                String time1 = DateTime.Now.ToString("HH:mm");

                for (int k = 0; k < dc1.Rows.Count; k++)
                {
                    if (time1 == dc1.Rows[k]["p3"].ToString())
                    {

                        
                        string sql6 = "select b.mechineName,a.* from asm_orderlistDetail a left join asm_mechine b on a.mechineid = b.id where a.createTime = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and a.zt = 4 and a.companyID = " + dc1.Rows[k]["id"].ToString();
                        OperUtil.Debuglog("定时执行正在执行" + sql6, "_tongzhi.txt");
                        DataTable dd6 = DbHelperSQL.Query(sql6).Tables[0];
                        if (dd6.Rows.Count > 0)
                        {
                            for (int i = 0; i < dd6.Rows.Count; i++)
                            {
                                string sqlM = "select * from asm_member where id=" + dd6.Rows[i]["memberID"].ToString();
                                OperUtil.Debuglog("定时执行正在执行" + sqlM, "_tongzhi.txt");
                                DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];

                                
                                if (dm.Rows.Count > 0)
                                {
                                    wxHelper wx = new wxHelper(dd6.Rows[i]["companyID"].ToString());
                                    string data = TemplateMessage.getProduct(dm.Rows[0]["openID"].ToString(), OperUtil.getMessageID(dd6.Rows[i]["companyID"].ToString(), "OPENTM407685552"), "亲爱的会员，您今日订购的商品还未取货", "" + dd6.Rows[i]["code"].ToString() + "", "" + dd6.Rows[i]["mechineName"].ToString() + "", "请及时取件,否则第二天会自动失效处理");
                                    TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dd6.Rows[i]["companyID"].ToString()), data);
                                    OperUtil.insertNotice(dm.Rows[0]["id"].ToString(), "待取货通知", "您今日订购的商品还未取货,请您及时取货否则第二天作失效处理，请前往指定的售卖机取货,取货码:" + dd6.Rows[i]["code"].ToString());
                                }
                            }
                        }
                    }
                    //定时检测限时特价
                    // clearXSTJ(dc1.Rows[k]["id"].ToString());
                }
            }
        }
        public void getProduct()
        {
            string sqlc = "select * from asm_company";
            DataTable dc1 = DbHelperSQL.Query(sqlc).Tables[0];
            if (dc1.Rows.Count > 0)
            {
                for (int k = 0; k < dc1.Rows.Count; k++)
                {
                    if (DateTime.Now.ToString("HH:mm") == dc1.Rows[k]["p3"].ToString())
                    {
                        string sql6 = "select * from asm_orderDetail where createTime like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%' and zt=4 and ldNO!='' and mechineID in(select id from  asm_mechine where companyID=" + dc1.Rows[k]["id"].ToString() + ")";
                        OperUtil.Debuglog("定时执行正在执行" + sql6, "_tongzhi.txt");
                        DataTable dd6 = DbHelperSQL.Query(sql6).Tables[0];
                        if (dd6.Rows.Count > 0)
                        {
                            for (int i = 0; i < dd6.Rows.Count; i++)
                            {
                                string sqlM = "select * from asm_member where id=" + dd6.Rows[i]["memberID"].ToString();
                                OperUtil.Debuglog("定时执行正在执行" + sqlM, "_tongzhi.txt");
                                DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
                                string sqlC = "select companyID,mechineName from asm_mechine where id in(select mechineID from asm_orderDetail where code='" + dd6.Rows[i]["code"].ToString() + "') ";
                                OperUtil.Debuglog("定时执行正在执行" + sqlC, "_tongzhi.txt");
                                DataTable dc = DbHelperSQL.Query(sqlC).Tables[0];

                                if (dm.Rows.Count > 0)
                                {
                                    wxHelper wx = new wxHelper(dc.Rows[0]["companyID"].ToString());
                                    string data = TemplateMessage.getProduct(dm.Rows[0]["openID"].ToString(), OperUtil.getMessageID(dc.Rows[0]["companyID"].ToString(), "OPENTM407685552"), "亲爱的会员，您今日订购的商品还未取货", "" + dd6.Rows[i]["code"].ToString() + "", "" + dc.Rows[0]["mechineName"].ToString() + "", "请及时取件,否则第二天会自动失效处理");
                                    TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dc.Rows[0]["companyID"].ToString()), data);
                                    OperUtil.insertNotice(dm.Rows[0]["id"].ToString(), "待取货通知", "您今日订购的商品还未取货,请您及时取货否则第二天作失效处理，请前往指定的售卖机取货,取货码:" + dd6.Rows[i]["code"].ToString());
                                }
                            }
                        }
                    }
                    //定时检测限时特价
                    // clearXSTJ(dc1.Rows[k]["id"].ToString());
                }
            }
        }
        public void myTimer1_Elapsed(object source, ElapsedEventArgs e)
        {
            
            Util.ClearRedisProductInfo();//清空redis
            List<string> list = new List<string>();
            //更新剩余订单数量
            string sql = "UPDATE asm_order  SET   syNum=totalNum-A.num    FROM (select COUNT(*) num,orderNO from asm_orderDetail where zt in (1,2,3,6,7)  group by orderNO having COUNT(*) >0) A,asm_order   WHERE A.orderNO = asm_order.orderNO";
            list.Add(sql);
            string sql1 = "update asm_orderDetail set zt=2 where  DATEDIFF(dd,GETDATE(),convert(datetime,createTime))<0 and zt in(4)";
            list.Add(sql1);
            string sql2 = "update asm_order set zt=3 where syNum<=0 and zt!=4";
            list.Add(sql2);
            string sql3 = "UPDATE asm_orderlist  SET   syNum=totalNum-A.num    FROM (select COUNT(*) num,orderNO from asm_orderlistDetail where zt in (1,2,3,6,7)  group by orderNO having COUNT(*) >0) A,asm_orderlist   WHERE A.orderNO = asm_orderlist.orderNO";
            list.Add(sql3);
            string sql4 = "update asm_orderlistDetail set zt=2 where  DATEDIFF(dd,GETDATE(),convert(datetime,createTime))<0 and zt in(4,5)";
            list.Add(sql4);
            string sql5 = "update asm_orderlist set orderZT=3 where syNum<=0 and orderZT!=4";
            list.Add(sql5);
            DbHelperSQL.ExecuteSqlTran(list);

        }
        public void myTimer_ElapsedT(object source, ElapsedEventArgs e)
        {
            
            //发送温度异常短信 //
            string sqlT3 = "select * from asm_mechine where sendT=1 and lastReqTime is not null and  id not in (68,69)";
            DataTable dt3 = DbHelperSQL.Query(sqlT3).Tables[0];
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    try
                    {
                        string sql21 = "select linkphone from asm_opera where id='" + dt3.Rows[i]["operaID"].ToString() + "'";
                        string sql22 = "select linkphone from asm_company where id='" + dt3.Rows[i]["companyID"].ToString() + "'";
                        DataTable d21 = DbHelperSQL.Query(sql21).Tables[0];
                        DataTable d22 = DbHelperSQL.Query(sql22).Tables[0];
                        if (d21.Rows.Count > 0 && d21.Rows[0]["linkphone"].ToString() != "")
                        {
                            OperUtil.sendMessage3(d21.Rows[0]["linkphone"].ToString(), dt3.Rows[i]["mechineName"].ToString(), dt3.Rows[i]["lastReqTime"].ToString().Substring(11, 5), dt3.Rows[i]["temperture"].ToString());
                        }
                        if (d22.Rows.Count > 0 && d22.Rows[0]["linkphone"].ToString() != "")
                        {
                            OperUtil.sendMessage3(d22.Rows[0]["linkphone"].ToString(), dt3.Rows[i]["mechineName"].ToString(), dt3.Rows[i]["lastReqTime"].ToString().Substring(11, 5), dt3.Rows[i]["temperture"].ToString());
                        }
                        string sqlupdate = "update asm_mechine set sendT=0 where id=" + dt3.Rows[i]["id"].ToString();
                        DbHelperSQL.ExecuteSql(sqlupdate);
                    }
                    catch (Exception ex)
                    {
                        OperUtil.Debuglog("cuowu=" + ex.Message, "短信_.txt");
                    }
                }
            }
            //更新活动有效期 是否启用
            string update1 = "update asm_activity set statu=0 where  startTime is not null and endTime is not null and endTime<GETDATE()";
            string update2 = "update asm_activity set statu=1 where  startTime is not null and endTime is not null and GETDATE()>startTime and GETDATE()<endTime";
            string update3 = "update asm_pay_activity set statu=0 where  startTime is not null and endTime is not null and endTime<GETDATE()";
            string update4 = "update asm_pay_activity set statu=1 where  startTime is not null and endTime is not null and GETDATE()>startTime and GETDATE()<endTime";

            string update5 = "update asm_zfbhb set status=0 where  startTime is not null and endTime is not null and endTime<GETDATE()";
            string update6 = "update asm_zfbhb set status=1 where  startTime is not null and endTime is not null and GETDATE()>startTime and GETDATE()<endTime";


            List<string> list = new List<string>();
            list.Add(update1);
            list.Add(update2);
            list.Add(update3);
            list.Add(update4);
            list.Add(update5);
            list.Add(update6);
            DbHelperSQL.ExecuteSqlTran(list);

            string start = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            calInComeTJ(start, end);
        }
        /// <summary>
        /// 定时更新会员等级 降级 升级是在会员购物的时候判断
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void myTimer_ElapsedM(object source, ElapsedEventArgs e)
        {

            Util.Debuglog(DateTime.Now.ToString("HH:mm") + "===" + (DateTime.Now.ToString("HH:mm") == "00:00"), "定时任务.txt");
            if (DateTime.Now.ToString("HH:mm") == "00:00") //更新会员等级
            {
                string sqlM = "select * from  asm_member where hjhyDays=1 and phone!=''";
                DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
                string sql = "update asm_member set hjhyDays=hjhyDays-1 where hjhyDays>0";
                int a = DbHelperSQL.ExecuteSql(sql);
                if (a > 0)
                {
                    string sql1 = "update asm_member set dj=1 where hjhyDays=0 and phone!=''";
                    DbHelperSQL.ExecuteSql(sql1);
                    string sql2 = "update asm_member set dj=0 where phone='' or phone is null";
                    DbHelperSQL.ExecuteSql(sql2);
                    
                }
                growUpMember();
            }
           
            //修改订单
            if (DateTime.Now.ToString("HH:mm") == "00:01")//修改订购订单状态
            {
                updateOrder();
            }
            if (DateTime.Now.ToString("HH:mm") == "00:10")//计算前一天的收入统计
            {
                string start = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
                string end = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
                calInComeTJ(start, end);
            }
           
            if (DateTime.Now.ToString("HH:mm") == "00:02")//删除一些历史记录
            {
                //DbHelperSQL.ExecuteSql(sql3);
                string sql4 = "delete from asm_videoRecord where DATEDIFF(MM,convert(datetime,time),GETDATE())>3";
                DbHelperSQL.ExecuteSql(sql4);
                //更新广告期限状态
                string sql = "update asm_videoAddMechine set zt=1 where ((tfType=1 and times>=tfcs) or (tfType=2 and GETDATE()>=valiDate)) ";
                DbHelperSQL.ExecuteSql(sql);
                //删除温度记录
                string sql5 = "delete from asm_temperature where time<'" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                DbHelperSQL.ExecuteSql(sql5);
            }
            if (DateTime.Now.ToString("HH:mm") == "00:03")//清空订购料道信息
            {
                Util.ClearRedisProductInfo();
                string sql = "update asm_ldInfo set ld_productNum=0 ,productID='' where type=1";
                DbHelperSQL.ExecuteSql(sql);
            }
            if (DateTime.Now.ToString("HH:mm")=="11:30")
            {
                sendKongyLDMsg();// 如果订购料道不足提前一天发送短信
            }
            //新增每天将第二天的去奶码生成
            if (DateTime.Now.ToString("HH:mm") == "23:59") {
                
                string sql = "update asm_orderlistDetail set code = floor(100000 + rand(checksum(newid())) * 899999) where createtime = CONVERT(VARCHAR(10), DATEADD(DAY, 1, GETDATE()), 120) and(code is null or code = '')";
                DbHelperSQL.ExecuteSql(sql);
                
            }
            

            getProduct();//迎春乐订购订单取货提醒
            getProductbg();//冰格订购订单取货提醒
        }
        public void sendKongyLDMsg()
        {
            string sql1 = "SELECT o.mechineID,o.productID,COUNT(*) as needNum,(SELECT proName from asm_product p WHERE p.productID=o.productID) proName"
                          + "  FROM asm_orderlistDetail o"
                          +"  where o.createTime = '"+DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")+"' and o.zt = 5  GROUP BY mechineID,productID";
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count>0)
            {
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    string sql2 = "SELECT isnull(SUM((ldNum - csldNum)),0) as tgNum FROM asm_ldInfo WHERE mechineID ="+dt.Rows[i]["mechineID"].ToString()+" AND productID ="+ dt.Rows[i]["productID"].ToString();
                    DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                    
                    double num = double.Parse(dt.Rows[i]["needNum"].ToString());
                    double sy = double.Parse(d2.Rows[0]["tgNum"].ToString());
                    if (num>sy)
                    {
                        string sql3 = "SELECT m.bh,m.mechineName,o.linkphone FROM asm_mechine m left JOIN asm_opera o ON m.dls = o.id where m.id ="+ dt.Rows[i]["mechineID"].ToString();
                        DataTable d3 = DbHelperSQL.Query(sql3).Tables[0];
                        //发送短信
                        OperUtil.sendMessage6(d3.Rows[0]["linkphone"].ToString(), dt.Rows[i]["proName"].ToString(), d3.Rows[0]["bh"].ToString());
                    }
                }
            }
        }
        public void updateOrder()
        {
            Util.ClearRedisProductInfo();//清空redis
     //   Util.orderDelay();
            //更新每日的asm_orderDetail的状态
            string sql = "update asm_orderDetail set zt=4 where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and zt=5";
            DbHelperSQL.ExecuteSql(sql);
            sql = "update asm_orderlistDetail set zt=4 where createTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and zt=5";
            Util.Debuglog("sql=" + sql, "订单暂停.txt");
            DbHelperSQL.ExecuteSql(sql);            //更新asm_order表 的订单状态zt由0变2为1
            string sql3 = "update asm_order set zt=1 where zt=0 and  DATEDIFF(dd,GETDATE(),convert(datetime,createTime))<0";
            DbHelperSQL.ExecuteSql(sql3);
            sql3 = "update asm_orderlist set orderZT=1 where fkzt=1 and orderZT=0 and  DATEDIFF(dd,GETDATE(),convert(datetime,createTime))<0";
            Util.Debuglog("sql3=" + sql3, "订单暂停.txt");
            DbHelperSQL.ExecuteSql(sql3);
        }
        public void growUpMember()
        {
             //降级
            string sql1 = "select * from asm_company";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string companyID = dt1.Rows[i]["id"].ToString();
                    string sql2 = "select * from asm_dj where companyID=" + dt1.Rows[i]["id"].ToString() + " order by djID";
                    DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                    if (dt2.Rows.Count > 0)
                    {
                        string day1 = dt2.Rows[0]["consumeDay"].ToString();
                        string day2 = dt2.Rows[1]["consumeDay"].ToString();
                        string day3 = dt2.Rows[2]["consumeDay"].ToString();
                        string sql = "select * from  [dbo].[View_member_consumeCount30] where companyID="+companyID+ " and num>="+day1+" and num<"+day2+ " and phone!='' and phone is not null and hjhyDays=0";
                        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                        List<string> list = new List<string>();
                        for (int a=0;a<dt.Rows.Count;a++)
                        {
                            if (dt.Rows[a]["dj"].ToString()!="1")
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[a]["openID"].ToString()))
                                {
                                    try
                                    {
                                        wxHelper wx = new wxHelper(dt1.Rows[0]["id"].ToString());
                                        string data = TemplateMessage.getDJChange(dt.Rows[a]["openID"].ToString(),
                                            OperUtil.getMessageID(dt1.Rows[0]["id"].ToString(), "OPENTM406811407"),
                                            "尊敬的会员，您的会员等级发生变更", "" + dt.Rows[a]["dj"].ToString() + "", "1",
                                            "如有疑问，请拨打会员服务热线" + dt1.Rows[i]["customerPhone"].ToString());
                                        string result=TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dt1.Rows[i]["id"].ToString()), data);
                                        Util.Debuglog("发送消息模板="+ dt.Rows[a]["openID"].ToString()+";result="+result, "会员等级消息模板.txt");
                                    }
                                    catch (Exception e)
                                    {
                                        Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                                    }
                                }
                            }
                            list.Add("update asm_member set dj=1 where id="+dt.Rows[a]["id"].ToString());
                        }
                        DbHelperSQL.ExecuteSqlTran(list);
                        sql = "select * from  [dbo].[View_member_consumeCount30] where companyID=" + companyID + " and num>=" + day2 + " and num<" + day3 + " and phone!='' and phone is not null and hjhyDays=0";
                        DataTable d2 = DbHelperSQL.Query(sql).Tables[0];
                        List<string> list2 = new List<string>();
                        for (int b = 0; b < d2.Rows.Count; b++)
                        {
                            if (d2.Rows[b]["dj"].ToString()!="2")
                            {
                                if (!string.IsNullOrEmpty(d2.Rows[b]["openID"].ToString()))
                                {
                                    try
                                    {
                                        wxHelper wx = new wxHelper(dt1.Rows[0]["id"].ToString());
                                        string data = TemplateMessage.getDJChange(d2.Rows[b]["openID"].ToString(),
                                            OperUtil.getMessageID(dt1.Rows[0]["id"].ToString(), "OPENTM406811407"),
                                            "尊敬的会员，您的会员等级发生变更", "" + d2.Rows[b]["dj"].ToString() + "", "2",
                                            "如有疑问，请拨打会员服务热线" + dt1.Rows[i]["customerPhone"].ToString());
                                        string result=TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dt1.Rows[i]["id"].ToString()), data);
                                        Util.Debuglog("发送消息模板=" + d2.Rows[b]["openID"].ToString() + ";result=" + result, "会员等级消息模板.txt");
                                    }
                                    catch (Exception e)
                                    {
                                        Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                                    }
                                }
                            }
                            list2.Add("update asm_member set dj=2 where id=" + d2.Rows[b]["id"].ToString());
                        }
                        DbHelperSQL.ExecuteSqlTran(list2);
                        sql = "select * from  [dbo].[View_member_consumeCount30] where companyID=" + companyID + " and num>=" + day3 + "  and phone!='' and phone is not null and hjhyDays=0";
                        DataTable d3 = DbHelperSQL.Query(sql).Tables[0];
                        List<string> list3 = new List<string>();
                        string aaa = "";
                        for (int c = 0; c < d3.Rows.Count; c++)
                        {
                            if (d3.Rows[c]["dj"].ToString()!="3"&&!string.IsNullOrEmpty(d3.Rows[c]["openID"].ToString()))
                            {
                                try
                                {
                                    wxHelper wx = new wxHelper(dt1.Rows[0]["id"].ToString());
                                    string data = TemplateMessage.getDJChange(d3.Rows[c]["openID"].ToString(),
                                        OperUtil.getMessageID(dt1.Rows[0]["id"].ToString(), "OPENTM406811407"),
                                        "尊敬的会员，您的会员等级发生变更", "" + d3.Rows[c]["dj"].ToString() + "", "3",
                                        "如有疑问，请拨打会员服务热线" + dt1.Rows[i]["customerPhone"].ToString());
                                   string result= TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dt1.Rows[i]["id"].ToString()), data);
                                   Util.Debuglog("发送消息模板=" + d3.Rows[c]["openID"].ToString() + ";result=" + result, "会员等级消息模板.txt");
                                }
                                catch (Exception e)
                                {
                                    Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                                }
                            }
                            list3.Add("update asm_member set dj=3 where id=" + d3.Rows[c]["id"].ToString());
                            aaa = aaa + "update asm_member set dj=3 where id=" + d3.Rows[c]["id"].ToString();
                        }
                        Util.Debuglog("list3=" + aaa, "会员等级消息模板.txt");
                        DbHelperSQL.ExecuteSqlTran(list3);

                    }
                }
               
            }
        }
         
        public void myTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            checkSellDetail();
            checkOrderDetail();

            //发送离线短信
            string sql2 = "select *,DATEDIFF(MI,convert(datetime,lastReqTime),getdate()) last from  asm_mechine where DATEDIFF(MI,convert(datetime,lastReqTime),getdate())>5 and statu=0 and sendF!=1";
            DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
            string sql1 = "update asm_mechine set statu=1,brokenTime='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ "' where DATEDIFF(MI,convert(datetime,lastReqTime),getdate())>5 and statu=0";
            DbHelperSQL.ExecuteSql(sql1);
            if (d2.Rows.Count>0)
            {
                for (int i=0;i<d2.Rows.Count;i++)
                {
                    string sql3 = "insert into asm_mechine_statu (mechineID,breakID,time,statu) values(" + d2.Rows[i]["id"].ToString() + ",2,'" + DateTime.Now + "',0)";
                    DbHelperSQL.ExecuteSql(sql3);
                    //发短信
                    try
                    {
                        string sql21 = "select linkphone from asm_opera where id='"+d2.Rows[i]["dls"].ToString() +"'";
                        string sql22 = "select linkphone from asm_company where id='"+ d2.Rows[i]["companyID"].ToString() + "'";
                        DataTable d21 = DbHelperSQL.Query(sql21).Tables[0];
                        DataTable d22 = DbHelperSQL.Query(sql22).Tables[0];
                       
                        if (d21.Rows.Count>0&&d21.Rows[0]["linkphone"].ToString()!="")
                        {
                            OperUtil.Debuglog("d21.Rows.Count=" + d21.Rows.Count + ";phone=" + d21.Rows[0]["linkphone"].ToString(), "短信_.txt");
                            OperUtil.sendMessage1(d21.Rows[0]["linkphone"].ToString(), d2.Rows[i]["mechineName"].ToString(),d2.Rows[i]["last"].ToString());
                        }
                        if (d22.Rows.Count > 0 && d22.Rows[0]["linkphone"].ToString() != "")
                        {
                            OperUtil.sendMessage1(d22.Rows[0]["linkphone"].ToString(), d2.Rows[i]["mechineName"].ToString(),d2.Rows[i]["last"].ToString());
                        }
                        string sqlupdate = "update asm_mechine set sendF=1 where id=" + d2.Rows[i]["id"].ToString();
                        DbHelperSQL.ExecuteSql(sqlupdate);

                    }
                    catch(Exception ex) {
                        OperUtil.Debuglog("cuowu="+ex.Message, "短信_.txt");
                    }
                    
                }
            }
           
        }
        public void checkOrderDetail()
        {
            //退款距离当前时间 5分钟之前到40分钟之前的
            string sql = "select p.* from asm_orderlistDetail p where statu = 1 and zt = 4 and DATEDIFF(MI, sellTime, GETDATE())> 30 AND not exists( select 1 from asm_sellDetail s where p.code = s.code and p.mechineID = s.mechineID and p.memberID = s.memberID and p.ldNO = s.proLD )  ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                    long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位 
                    string payType = "3";
                   

                    string productID = dt.Rows[i]["productID"].ToString();
                    string money = "0.00";
                    string sql1 = "select price from asm_orderlist where orderNO='" + dt.Rows[i]["orderNO"].ToString() + "' ";
                    DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        money = dt1.Rows[0]["price"].ToString();
                    }
                    string ldNO = dt.Rows[i]["ldNO"].ToString();
                    string type = "2";
                   // string bill = dt.Rows[i]["orderNO"].ToString();
                    string mechineID = dt.Rows[i]["mechineID"].ToString();

                    Util.Debuglog("payType=" + payType + ";productID=" + productID + ";money=" + money + ";ldNO=" + ldNO + ";bill=" + t + ";mechineID=" + mechineID, "检查支付成功未出货订单.txt");
                    if (string.IsNullOrEmpty(productID) || string.IsNullOrEmpty(t.ToString()) || string.IsNullOrEmpty(mechineID))
                    {
                        continue;
                    }

                    string memberID = dt.Rows[i]["memberID"].ToString();
                   
                    Hashtable ht = new Hashtable();
                    ht.Add("payType", payType);
                    ht.Add("productID", productID);
                    ht.Add("orderTime", dt.Rows[i]["sellTime"].ToString());
                    ht.Add("num", 1);
                    ht.Add("totalMoney", money);
                    if (string.IsNullOrEmpty(ldNO))
                    {
                        ht.Add("proLD", "0");
                    }
                    else
                    {
                        ht.Add("proLD", ldNO);
                    }

                    ht.Add("type", type);
                    ht.Add("orderNO", t);
                   
                    ht.Add("bz", "出货失败");
                    

                    ht.Add("code", dt.Rows[i]["code"].ToString());
                    ht.Add("billno", t);
                    ht.Add("mechineID", mechineID);
                    ht.Add("memberID", memberID);
                    Hashtable ht1 = new Hashtable();
                    ht1.Add("recordList", "[" + Util.HashtableToWxJson(ht) + "]");
                    Util.Debuglog("[" + Util.HashtableToWxJson(ht) + "]", "订购未出货订单1.txt");
                    XmlDocument xx = WebSvcCaller.QuerySoapWebService("http://nq.bingoseller.com/api/mechineService.asmx", "upSellRecord", ht1);
                    string result = xx.OuterXml;
                    Util.Debuglog("result=" + result, "订购未出货订单.txt");
                }
            }

        }
        public void checkSellDetail()
        { 
            //退款距离当前时间 5分钟之前到40分钟之前的
            string sql = "select DATEDIFF(MI,createTime,GETDATE()) sjc,"
                       + " p.appid,p.trxid,p.acct,p.type,p.payType,p.trxamt,p.createTime,p.chLdNo,p.unionID,p.mechineID,p.productID,p.companyID,p.chzt,s.mechineID,s.billno,s.orderTime from  asm_pay_info p left join asm_sellDetail s on p.trxid = s.billno"
                       + " where p.type = 2 and p.statu = 1 and p.payType in(1,2,4) "
                       + " and DATEDIFF(MI, createTime, GETDATE())>= 5 and DATEDIFF(MI, createTime, GETDATE())<= 120 and p.companyID=14  and s.billno is null"
                       + " order by p.id desc"; 
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                    long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位 
                    string payType = "";
                    if (dt.Rows[i]["payType"].ToString() == "4")
                    {
                        payType = "3";
                    }
                    else {
                        payType = dt.Rows[i]["payType"].ToString();
                    }
                    
                    string productID = dt.Rows[i]["productID"].ToString();
                    string money = (double.Parse(dt.Rows[i]["trxamt"].ToString())/100).ToString("f2");
                    string ldNO = dt.Rows[i]["chLdNo"].ToString();
                    string type = "2";
                    string bill = dt.Rows[i]["trxid"].ToString();
                    string mechineID = dt.Rows[i]["mechineID"].ToString();

                    Util.Debuglog("payType=" + payType + ";productID="+productID+ ";money="+ money+ ";ldNO="+ldNO+ ";bill="+ bill+ ";mechineID="+ mechineID, "检查支付成功未出货订单.txt");
                    if (string.IsNullOrEmpty(productID)||string.IsNullOrEmpty(bill)||string.IsNullOrEmpty(mechineID))
                    {
                        continue;
                    }
                   
                    string memberID = "0";
                    string sqlM = "select * from asm_member where openID='"+dt.Rows[i]["acct"].ToString()+"' ";
                    DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
                    if (dm.Rows.Count>0)
                    {
                        memberID = dm.Rows[0]["id"].ToString();
                    }
                    Hashtable ht = new Hashtable();
                    ht.Add("payType", payType);
                    ht.Add("productID", productID);
                    ht.Add("orderTime", dt.Rows[i]["createTime"].ToString());
                    ht.Add("num", 1);
                    ht.Add("totalMoney", money);
                    if (string.IsNullOrEmpty(ldNO))
                    {
                        ht.Add("proLD", "0");
                    }
                    else {
                        ht.Add("proLD", ldNO);
                    }
                    
                    ht.Add("type", type);
                    ht.Add("orderNO", t);
                    if (dt.Rows[i]["chzt"].ToString() == "1")
                    {
                        ht.Add("bz", "交易成功");
                    }
                    else {
                        ht.Add("bz", "出货失败");
                    }
                   
                    ht.Add("code", "");
                    ht.Add("billno", bill);
                    ht.Add("mechineID", mechineID);
                    ht.Add("memberID", memberID);
                    Hashtable ht1 = new Hashtable();
                    ht1.Add("recordList", "[" + Util.HashtableToWxJson(ht) + "]");
                    Util.Debuglog("[" + Util.HashtableToWxJson(ht) + "]", "检查支付成功未出货订单1.txt");
                    XmlDocument xx = WebSvcCaller.QuerySoapWebService("http://nq.bingoseller.com/api/mechineService.asmx", "upSellRecord", ht1);
                    string result = xx.OuterXml;
                    Util.Debuglog("result=" + result, "检查支付成功未出货订单.txt");
                }
            }

        }
        public void calInComeTJ(string start,string end)
        {
            string sqlC = "select * from asm_company";
            DataTable dmc = DbHelperSQL.Query(sqlC).Tables[0];
            if (dmc.Rows.Count>0)
            {
                for (int j=0;j< dmc.Rows.Count;j++)
                {
                   
                    string companyID = dmc.Rows[j]["id"].ToString();
                    string sql = " and 1=1 ";
                    string sql12 = " and 1=1 ";
                    if (!string.IsNullOrEmpty(start))
                    {
                        sql += " and paytime>='" + start.Replace("-", "").Replace(":", "").Replace(" ", "") + "'";
                        sql12 += " and time>='" + start.Substring(0,10) + "'";
                    }
                    if (!string.IsNullOrEmpty(end))
                    {
                        sql += " and paytime<='" + end.Replace("-", "").Replace(":", "").Replace(" ", "") + "'";
                        sql12 += " and time<='" + end.Substring(0,10) + "'";
                    }


                    sql += " and appid='" + dmc.Rows[j]["tl_APPID"].ToString() + "'";
                    string sql1 = "select A.time1,cast(round(isnull(A.lsMoney,0)/100.0,2)  as  decimal(18,2)) lsMoney,"
                                  + " cast(round(isnull(A.lsMoneyAvai,0)/100.0,2)  as  decimal(18,2)) lsMoneyAvai,"
                                  + "  cast(round(isnull(A.dgMoney, 0) / 100.0, 2) as decimal(18, 2)) dgMoney,"
                                  + "  cast(round(isnull(A.czMoney, 0) / 100.0, 2) as decimal(18, 2)) czMoney,"
                                  + "  cast(round(isnull(A.tkMoney, 0) / 100.0, 2) as decimal(18, 2)) tkMoney,"
                                  + "  cast(round(isnull(A.lsMoney, 0) / 100.0, 2)+round(isnull(A.lsMoneyAvai, 0) / 100.0, 2) + round(isnull(A.dgMoney, 0) / 100.0, 2) + round(isnull(A.czMoney, 0) / 100.0, 2) as decimal(18, 2)) totalMoney"
                                  + "  from(select time1,"
                                  + " (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where payType in(1,2) and statu=1 and type=2  and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1 " + sql + " group by SUBSTRING(paytime, 0, 9)) lsMoney,"
                                  + " (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where payType in(4) and statu=1 and type=2  and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1 " + sql + " group by SUBSTRING(paytime, 0, 9)) lsMoneyAvai,"
                                  + "  (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where payType in(3) and statu=1 and type = 3 and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1 " + sql + " group by SUBSTRING(paytime, 0, 9)) dgMoney,"
                                  + "  (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where type in(1) and statu=1 and payType in(3,5) and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1 " + sql + " group by SUBSTRING(paytime, 0, 9)) czMoney,"
                                  + "  (select SUM(ISNULL(trxamt, 0)) from asm_pay_info where type in(2) and statu=2  and paytime is not null and SUBSTRING(paytime,0, 9)= t.time1 " + sql + " group by SUBSTRING(paytime, 0, 9)) tkMoney"
                                  + "  from asm_time t where 1=1 " + sql12 + ") A ";
                    
                    DataTable da = DbHelperSQL.Query(sql1).Tables[0];
                    if (da.Rows.Count>0)
                    {
                        string time = da.Rows[0]["time1"].ToString();
                        string lsMoney = da.Rows[0]["lsMoney"].ToString();
                        string lsMoneyAvai = da.Rows[0]["lsMoneyAvai"].ToString();
                        string dgMoney = da.Rows[0]["dgMoney"].ToString();
                        string czMoney = da.Rows[0]["czMoney"].ToString();
                        string tkMoney = da.Rows[0]["tkMoney"].ToString();
                        string totalMoney = da.Rows[0]["totalMoney"].ToString();
                        string sql2 = "select * from asm_inComeTJ where companyID="+companyID+" and time='"+ DateTime.Parse(start).ToString("yyyyMMdd")+ "'";
                        DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                        if (d2.Rows.Count <= 0)
                        {
                            string insert = "insert into asm_inComeTJ(companyID,time,lsMoney,dgMoney,czMoney,tkMoney,totalMoney,lsMoneyAvai) " +
                             "values(" + companyID + ",'" + time + "'," + lsMoney + "," + dgMoney + "," + czMoney + "," + tkMoney + "," + totalMoney + ","+ lsMoneyAvai + ")";
                            Util.Debuglog(insert, "calInComeTJ.txt");
                            DbHelperSQL.ExecuteSql(insert);
                        }
                        else {
                            string update = "update asm_inComeTJ set lsMoney="+lsMoney+ ",lsMoneyAvai="+ lsMoneyAvai + ",dgMoney=" + dgMoney+ ",czMoney="+ czMoney+ ",tkMoney="+ tkMoney+ ",totalMoney="+ totalMoney+" where companyID="+companyID+" and time='"+ DateTime.Parse(start).ToString("yyyyMMdd") + "'";
                            Util.Debuglog(update, "calInComeTJ.txt");
                            DbHelperSQL.ExecuteSql(update);
                        }
                    }
                }

            }

          
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            
            //WebForm1.html = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":Application End!";

            //下面的代码是关键，可解决IIS应用程序池自动回收的问题  
            Thread.Sleep(1000);
            //这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start  
            //string url = HttpContext.Current.Request.Url.Host+"/WebForm1.aspx";
            //HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            //Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流  
        }

       
      



    }
}