using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;

namespace Consumer.api
{
    /// <summary>
    /// asm 的摘要说明
    /// </summary>
    public class asm : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "createOrder":  //奶企登录
                    this.createOrder(context);
                    return;
                case "update":
                    this.update(context);
                    return;
              
            }
        }
        public void update(HttpContext context)
        {
            string sql = "SELECT * from asm_orderlist";
            DataTable d1 = DbHelperSQL.Query(sql).Tables[0];
            for (int i=0; i<d1.Rows.Count;i++)
            {
                string sql2 = "SELECT * from  asm_orderlistDetail where orderno in ('"+d1.Rows[i]["orderNO"].ToString()+"') ORDER BY createTime DESC";
                DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                if (d2.Rows.Count>0)
                {
                    string update = "UPDATE asm_orderlist set endTime='" + d2.Rows[0]["createTime"].ToString() + "' WHERE orderno in ('" + d1.Rows[i]["orderNO"].ToString() + "')";
                    DbHelperSQL.ExecuteSql(update);

                }
               
            }
        }
        public void createOrder(HttpContext context)
        {
            string mechine_id = context.Request["mechine_id"].ToString();
            Util.Debuglog("mechine_id=" + mechine_id, "_.参数.txt");
            string product_id = context.Request["product_id"].ToString();
            Util.Debuglog("product_id=" + product_id, "_.参数.txt");
          
            string _orderNO = context.Request["_orderNO"].ToString();
            Util.Debuglog("_orderNO=" + _orderNO, "_.参数.txt");
            
            string _totalMoney = context.Request["_totalMoney"].ToString();
            Util.Debuglog("_totalMoney=" + _totalMoney, "_.参数.txt");
           
            string _trxID = context.Request["_trxID"].ToString();
            Util.Debuglog("_trxID=" + _trxID, "_.参数.txt");
         
            string memberID = Util.getMemberID();
            if (memberID=="0")
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"3\""); //当前没有登录
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                
                return;
            }
            //根据机器id获取机器地址
            string sql1 = "select * from asm_mechine where id="+mechine_id;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
         
            string sql12 = "select * from asm_order where orderNO='"+_orderNO+"'";
            
            DataTable d12 = DbHelperSQL.Query(sql12).Tables[0];
            if (d12.Rows.Count <= 0)
            {
               
            }
            else {
                //判断当前的优惠方式如果是赠送天数的话需要更新totalNum
                if (d12.Rows[0]["yhfs"].ToString().Contains("赠送"))
                {
                    string num = d12.Rows[0]["yhfs"].ToString();
                    num = num.Replace("赠送","").Replace("天","");
                    string update = "update asm_order set fkzt=1,trxID='" + _trxID + "',totalNum=totalNum+"+num+ ",syNum=syNum+" + num + " where id=" + d12.Rows[0]["id"].ToString();
                    DbHelperSQL.ExecuteSql(update);
                }
                else {
                    string update = "update asm_order set fkzt=1,trxID='" + _trxID + "' where id=" + d12.Rows[0]["id"].ToString();
                    DbHelperSQL.ExecuteSql(update);
                }
                //更新状态
            }
            if (1==1)
            {
                d12 = DbHelperSQL.Query(sql12).Tables[0];
                //更新商品销售数量
                string ss = "update asm_product set ljxs=CONVERT(float,ISNULL(ljxs,0))+1 where productID="+product_id;
                DbHelperSQL.ExecuteSql(ss);
                //string[] selDate = _selDate.Split(',');
               
                string[] selDate = insertIntoOrderDetail(d12.Rows[0]["psfs"].ToString(), d12.Rows[0]["psStr"].ToString(), d12.Rows[0]["totalNum"].ToString(), d12.Rows[0]["qsDate"].ToString()).Split(',');
                
                Util.Debuglog(insertIntoOrderDetail(d12.Rows[0]["psfs"].ToString(), d12.Rows[0]["psStr"].ToString(), d12.Rows[0]["totalNum"].ToString(), d12.Rows[0]["qsDate"].ToString()),"时间格式.txt");
                if (selDate.Length>0)
                {
                    string sql14 = "select * from asm_orderDetail where id=0";
                    DataTable dtNew = DbHelperSQL.Query(sql14).Tables[0];
                    for (int i=0;i<selDate.Length;i++)
                    {
                        int code = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                        //zt   1-已完成；2-已失效；3-已转售；4-待取货；5-待配送
                        DataRow dr = dtNew.NewRow();
                        dr["mechineID"] = mechine_id; //通过索引赋值
                        dr["productID"] = product_id;
                        dr["createTime"] = delTime(selDate[i]);//
                        dr["code"] = code;//
                        dr["memberID"] = memberID; //通过索引赋值
                        dr["zt"] = "5";
                        dr["ldNO"] = "";//
                        dr["orderNO"] = _orderNO;//
                        dr["statu"] = "0"; //通过索引赋值
                        dr["sellPrice"] = 0.0;
                        dr["sellTime"] = "";
                        dr["bz"] = "";
                        dtNew.Rows.Add(dr);
                    }
                    DbHelperSQL.BatchInsertBySqlBulkCopy(dtNew, "[dbo].[asm_orderDetail]");
                }
                //给会员绑定机器
                string sql4 = "update asm_member set mechineID="+ mechine_id + ",LastTime='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ "',consumeCount=consumeCount+1,sumConsume=sumConsume+"+ _totalMoney + " where id=" + memberID;
                Util.Debuglog("会员绑定机器=" + sql4, "_.插入订购订单.txt");
                DbHelperSQL.ExecuteSql(sql4);
                string sql5 = "select * from asm_product where productID="+product_id;
                DataTable dd5 = DbHelperSQL.Query(sql5).Tables[0];
                Thread threadB = new Thread(
                 () =>
                 {
                     wxHelper wx = new wxHelper(OperUtil.getCooki("companyID"));
                     string data = TemplateMessage.comsume(OperUtil.getCooki("vshop_openID"), "ti4Dkcm1ELNqaskSYsCYMzqL87nPqapNeOgwhvSci_Q", "亲，你的购买的商品信息如下", "" + dd5.Rows[0]["proName"].ToString() + "", _totalMoney, _orderNO, dt.Rows[0]["bh"].ToString(), "欢迎惠顾");
                     TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(OperUtil.getCooki("companyID")), data);
                 });
                threadB.Start();
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"4\""); //订单提交成功
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }
            else {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append("\"result\":\"2\""); //订单提交失败
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
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
            else {
                return time;
            }
           
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public static string insertIntoOrderDetail(string psfs, string psStr, string zq,string qsDate)
        {
            string result = "";
            if (psfs == "1")//按天派送
            {
                if (psStr.IndexOf("每天配送") > -1)
                {
                    result = getDataTimeDay("0", zq,qsDate);
                }
                else if (psStr.IndexOf("隔一天") > -1)
                {
                    result = getDataTimeDay("1", zq,qsDate);
                }
                else if (psStr.IndexOf("隔两天") > -1)
                {
                    result = getDataTimeDay("2", zq,qsDate);
                }
                else if (psStr.IndexOf("隔三天") > -1)
                {
                    result = getDataTimeDay("3", zq,qsDate);
                }
            }
            else if (psfs == "2")
            {
                //自定义派送
                result = getDataTimeWeek(psStr, int.Parse(zq));
            }
            //创建订单
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="D"></param>
        /// <param name="zq"></param>
        /// <returns></returns>
        public static string getDataTimeDay(string D, string zq,string qsDate)
        {
            string result = "";
            string day = D;
            int m = 0, n = int.Parse(zq);
            if (day == "1")//隔天派送
            {
                m = n * 2 - 1;
            }
            else if (day == "2")//隔2天派送
            {
                m = n * 3 - 2;
            }
            else if (day == "3")//隔3天派送
            {
                m = n * 4 - 3;
            }
            else
            {
                m = n * 1;//每天派送
            }
            //获取应该配送的日期 应该循环m
            var N = 0;//自增变量
            while (N < m)
            {

                if (day == "1")
                {
                    N = N + 2;
                }
                else if (day == "2")
                {
                    N = N + 3;
                }
                else if (day == "3")
                {
                    N = N + 4;
                }
                else if (day == "0")
                {
                    N = N + 1;
                }
                //var t = DateTime.Now.AddDays(N).ToString("yyyy-MM-dd");
                var t = DateTime.Parse(qsDate).AddDays(N - 1).ToString("yyyy-MM-dd");
                result += t + ",";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
        public static string getDataTimeWeek(string psStr, int zq)
        {
            int count = 0;
            int n = zq;
            string result = "";
            for (int i = 1; i < 1000000; i++)
            {
                var time = getDate(i);
                var week = Week(time);
                if (psStr.IndexOf(week) > -1)
                {
                    result += time + ",";
                    count++;
                }
                if (count == n)
                {
                    break;
                }
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
        public static string getDate(int D)
        {
            return DateTime.Now.AddDays(D).ToString("yyyy-MM-dd");
        }
        public static string Week(string time)
        {
            string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            //string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];
            string week = weekdays[Convert.ToInt32(DateTime.Parse(time).DayOfWeek)];
            return week;
        }
    }
}