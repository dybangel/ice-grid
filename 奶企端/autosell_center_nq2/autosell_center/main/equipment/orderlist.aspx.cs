using autosell_center.cls;
using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using uniondemo.com.allinpay.syb;

namespace autosell_center.main.equipment
{
    public partial class orderlist : System.Web.UI.Page
    {
        public string comID = "";
        public string operaID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this.agentID.Value = OperUtil.Get("operaID");
            operaID = this.agentID.Value;
            if (string.IsNullOrEmpty(comID)||string.IsNullOrEmpty(operaID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            this.companyID.Value = comID;
            
           
        }
        [WebMethod]
        public static object judge(string operaID, string menuID)
        {
            Boolean b = Util.judge(operaID, menuID);
            if (b)
            {
                return new { code = 200 };
            }
            else
            {
                return new { code = 500 };
            }
        }
        [WebMethod]
        public static Object ok(string billno,string pwd,string companyID)
        {
            //首先验证二级退款密码是否正确
            string sql12 = "select * from asm_company where id="+companyID;
            DataTable dd1 = DbHelperSQL.Query(sql12).Tables[0];
            if (dd1.Rows.Count>0)
            {
                if (pwd == dd1.Rows[0]["pwd2"].ToString())
                {
                    //查询该笔订单信息
                    //string sql1 = "select * from asm_pay_info where trxid='" + billno + "'";
                    //DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                    //if (dt1.Rows.Count > 0)
                    //{
                        //查询订单信息
                        string sqlOrder = "select * from  asm_sellDetail where billno='"+billno+"'";
                        DataTable Dorder = DbHelperSQL.Query(sqlOrder).Tables[0];
                        if (Dorder.Rows.Count<=0)
                        {
                            return new { result = 0, msg = "该笔订单查询失败"};
                        }
                        //判断出货状态如果是料道错误或者是交易序列号相同给退款 零售的怎么来怎么退 订购的退到钱包
                        if (!string.IsNullOrEmpty(Dorder.Rows[0]["code"].ToString()))
                        {
                            //订购的
                            if (Dorder.Rows[0]["bz"].ToString() == "料道错误" || Dorder.Rows[0]["bz"].ToString() == "交易序列号相同"|| Dorder.Rows[0]["bz"].ToString() == "料道故障"|| Dorder.Rows[0]["bz"].ToString() == "校验错误" || Dorder.Rows[0]["bz"].ToString() == "出货失败")
                            {
                                string sql = "select * from asm_sellDetail where billno='" + billno + "'";
                                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                                if (dd.Rows.Count > 0)
                                {
                                    //更新会员钱包 并插入资金变动记录
                                    string update = "update asm_member set  AvailableMoney=AvailableMoney+" + dd.Rows[0]["totalMoney"].ToString() + ",sumConsume=sumConsume-"+ dd.Rows[0]["totalMoney"].ToString() + " where id=" + dd.Rows[0]["memberID"].ToString();
                                    int a = DbHelperSQL.ExecuteSql(update);
                                    string sqlm = "select * from asm_member where id=" + dd.Rows[0]["memberID"].ToString();
                                    DataTable dt = DbHelperSQL.Query(sqlm).Tables[0];
                                    if (a > 0)
                                    {
                                        string sqlu = "update asm_sellDetail set bz='退款成功' where billno='" + billno + "'";
                                        DbHelperSQL.ExecuteSql(sqlu);
                                        Util.insertNotice(dt.Rows[0]["id"].ToString(), "出货异常退款", "您于" + Dorder.Rows[0]["orderTime"].ToString() + "取货异常退还金额:" + dd.Rows[0]["totalMoney"].ToString() + ";请查收钱包");
                                        Util.moneyChange(dt.Rows[0]["id"].ToString(), dd.Rows[0]["totalMoney"].ToString(), dt.Rows[0]["AvailableMoney"].ToString(), "退款通知", "7", "", dd.Rows[0]["totalMoney"].ToString());

                                        if (!string.IsNullOrEmpty(dt.Rows[0]["openID"].ToString()))
                                        {
                                            try
                                            {
                                                string company = Util.getCompany(dt.Rows[0]["companyID"].ToString()); 
                                                wxHelper wx = new wxHelper(dt.Rows[0]["companyID"].ToString());
                                                string data = TemplateMessage.tk(dt.Rows[0]["openID"].ToString(), OperUtil.getMessageID(dt.Rows[0]["companyID"].ToString(), "OPENTM410089600"), "退款通知", dd.Rows[0]["totalMoney"].ToString(), "您购买的商品没有出货成功，钱已退还到账户");
                                                TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dt.Rows[0]["companyID"].ToString()), data);
                                            }
                                            catch (Exception e)
                                            {
                                                Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                                            }
                                        }
                                        return new { result = 0, msg = "退款成功，成功到会员钱包中" };

                                    }
                                }
                            }
                        }
                        else
                        {
                            //零售的
                            if (Dorder.Rows[0]["bz"].ToString() == "料道错误" || Dorder.Rows[0]["bz"].ToString() == "交易序列号相同" || Dorder.Rows[0]["bz"].ToString() == "料道故障" || Dorder.Rows[0]["bz"].ToString() == "校验错误" || Dorder.Rows[0]["bz"].ToString() == "出货失败")
                            {
                                Util.Debuglog("billno=" + billno, "_手动退款.txt");
                                if (Dorder.Rows[0]["payType"].ToString() == "3")
                                {
                                    //退到钱包
                                    string sql = "select * from asm_sellDetail where billno='" + billno + "'";
                                    DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                                    if (dd.Rows.Count > 0)
                                    {
                                        //更新会员钱包 并插入资金变动记录
                                        string update = "update asm_member set  AvailableMoney=AvailableMoney+" + dd.Rows[0]["totalMoney"].ToString() + ",sumConsume=sumConsume-"+ dd.Rows[0]["totalMoney"].ToString() + " where id=" + dd.Rows[0]["memberID"].ToString();
                                        int a = DbHelperSQL.ExecuteSql(update);
                                        string sqlm = "select * from asm_member where id=" + dd.Rows[0]["memberID"].ToString();
                                        DataTable dt = DbHelperSQL.Query(sqlm).Tables[0];
                                        if (a > 0)
                                        {
                                            string update1 = "update asm_pay_info set statu=2,fintime='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' where trxid='" + billno + "'";
                                            DbHelperSQL.ExecuteSql(update1);
                                            string sqlu = "update asm_sellDetail set bz='退款成功' where billno='" + billno + "'";
                                            DbHelperSQL.ExecuteSql(sqlu);
                                            Util.insertNotice(dt.Rows[0]["id"].ToString(), "出货异常退款", "您于" + Dorder.Rows[0]["orderTime"].ToString() + "购买商品出货异常退还金额:" + dd.Rows[0]["totalMoney"].ToString() + ";请查收钱包");
                                            Util.moneyChange(dt.Rows[0]["id"].ToString(), dd.Rows[0]["totalMoney"].ToString(), dt.Rows[0]["AvailableMoney"].ToString(), "退款通知", "7", "", "取货异常退还金额:" + dd.Rows[0]["totalMoney"].ToString());
                                            if (!string.IsNullOrEmpty(dt.Rows[0]["openID"].ToString()))
                                            {
                                                try
                                                {
                                                    string company = Util.getCompany(dt.Rows[0]["companyID"].ToString());

                                                    wxHelper wx = new wxHelper(dt.Rows[0]["companyID"].ToString());
                                                    string data = TemplateMessage.tk(dt.Rows[0]["openID"].ToString(), OperUtil.getMessageID(dt.Rows[0]["companyID"].ToString(), "OPENTM410089600"), "退款通知", dd.Rows[0]["totalMoney"].ToString(), "您购买的商品没有出货成功，钱已退还到账户");
                                                    TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dt.Rows[0]["companyID"].ToString()), data);
                                                }
                                                catch (Exception e)
                                                {
                                                    Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                                                }
                                            }
                                        return new { result = 0, msg = "退款成功，成功到会员钱包中" };
                                        }
                                    }
                                }
                                else if (Dorder.Rows[0]["payType"].ToString() == "1" || Dorder.Rows[0]["payType"].ToString() == "2")
                                {
                                    //退到1微信或者2支付宝
                                    string sql = "select * from asm_pay_info where trxid='" +billno + "'";
                                    DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                                    if (dd.Rows.Count > 0 && dd.Rows[0]["statu"].ToString() == "1")
                                    {
                                        SybWxPayService sybService = new SybWxPayService(Dorder.Rows[0]["mechineID"].ToString());
                                        long fen = long.Parse(dd.Rows[0]["trxamt"].ToString());
                                        Random rd = new Random();
                                        int rand = rd.Next(10000, 99999);
                                        string reqsn = ConvertDateTimeToInt(DateTime.Now).ToString() + rand;
                                        string oldtrxid = dd.Rows[0]["trxid"].ToString();
                                        string oldreqsn = dd.Rows[0]["reqsn"].ToString();
                                        Dictionary<String, String> rsp = sybService.cancel(fen, reqsn, oldtrxid, oldreqsn);
                                        string data = OperUtil.SerializeDictionaryToJsonString(rsp);
                                        Util.Debuglog("oldtrxid="+ oldtrxid + ";微信支付链接data=" + data, "_手动退款.txt");
                                        //插入预处理订单信息
                                        string json = (new JavaScriptSerializer()).Serialize(rsp);
                                        Util.Debuglog("微信支付链接json=" + json, "_手动退款.txt");
                                        JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                                        if (jo["retcode"].ToString() == "SUCCESS")
                                        {
                                            try
                                            {
                                                string msg = jo["errmsg"].ToString();
                                                return new { result = 0, msg = msg };
                                            }
                                            catch {

                                            }
                                            string cusid = jo["cusid"].ToString();//商户号
                                            string trxid = jo["trxid"].ToString();//交易单号
                                            string fintime = jo["fintime"].ToString();//交易完成时间
                                            string reqsn1 = jo["reqsn"].ToString();//商户订单号
                                            string trxstatus = jo["trxstatus"].ToString();//交易状态
                                            //更新asm_pay_info
                                            if (trxstatus == "0000")
                                            {    
                                                string update = "update asm_pay_info set statu=2,tkreqsn='" + reqsn1 + "',fintime='" + fintime + "',trxstatus='"+trxstatus+ "',errmsg='' where trxid='" + oldtrxid + "'";
                                                DbHelperSQL.ExecuteSql(update);
                                                string sqlu = "update asm_sellDetail set bz='退款成功' where billno='" + billno + "'";
                                                DbHelperSQL.ExecuteSql(sqlu);
                                                string update1 = "update asm_member set  sumConsume=sumConsume-" + (double.Parse(dd.Rows[0]["trxamt"].ToString()) / 100) + " where openID='" + dd.Rows[0]["acct"].ToString() + "'";
                                                int a = DbHelperSQL.ExecuteSql(update1);

                                                string sqlm = "select * from asm_member where openID='"+dd.Rows[0]["acct"].ToString()+"'";
                                                DataTable dt = DbHelperSQL.Query(sqlm).Tables[0];
                                                if (!string.IsNullOrEmpty(dt.Rows[0]["openID"].ToString()))
                                                {
                                                    try
                                                    {
                                                        string sqlPayInfo = "select * from asm_pay_info where trxid='" + billno + "'";
                                                        DataTable ddpayInfo = DbHelperSQL.Query(sqlPayInfo).Tables[0];
                                                        string company = Util.getCompany(dt.Rows[0]["companyID"].ToString()); 
                                                        wxHelper wx = new wxHelper(dt.Rows[0]["companyID"].ToString());
                                                        data = TemplateMessage.tk(dt.Rows[0]["openID"].ToString(), OperUtil.getMessageID(dt.Rows[0]["companyID"].ToString(), "OPENTM410089600"), "退款通知", (double.Parse(ddpayInfo.Rows[0]["trxamt"].ToString()) / 100).ToString("f2"), "您购买的商品没有出货成功，钱已退还到账户");
                                                        TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(dt.Rows[0]["companyID"].ToString()), data);
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        Util.Debuglog("e=" + e.Message, "会员等级消息模板.txt");
                                                    }
                                                }
                                                return new { result = 0, msg = "退款成功，已经退到会员的微信或支付宝" };
                                            }
                                            else {
                                                string errmsg = jo["errmsg"].ToString();//交易失败信息
                                                string update = "update asm_pay_info set tkreqsn='" + reqsn1 + "',fintime='" + fintime + "',trxstatus='" + trxstatus + "',errmsg='" + errmsg + "' where trxid='" + oldtrxid + "'";
                                                DbHelperSQL.ExecuteSql(update);
                                                if (trxstatus == "3008")
                                                {
                                                    return new { result = 0, msg = errmsg };
                                                }
                                                else {
                                                    return new { result = 0, msg = "退款失败" };
                                                }
                                            }
                                        }
                                        else {
                                            return new { result = 0, msg = "退款失败" };
                                        }
                                    }
                                }
                            }
                        }
                    //}
                    //else {
                       // return new { result = 0, msg = "该笔订单查询失败" };
                   // }
                }
                else {
                    return new { result = 0, msg = "退款密码错误" };//密码错误
                }
            }
            return new { result = 0, msg = "退款失败" };
        }
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        [WebMethod]
        public static string getOrderList(string mechineID,string companyID,string start,string end, string pageCurrentCount,string ztlist,string brandList,string selType,string productName,string idORphone,string orderNO)
        {
           
            string sql = " 1=1 and companyID="+companyID;
            if (!string.IsNullOrEmpty(mechineID) && mechineID != "0")
            {
                sql += " and C.mechineID in("+mechineID+")";
            }
            if (!string.IsNullOrEmpty(start))
            {
                sql += " and orderTime>convert(datetime,'"+start+"')";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += " and orderTime<convert(datetime,'" + end + "')";
            }
            if (ztlist!="全部")
            {
                sql += " and bz='"+ztlist+"'";
            }
            if (!string.IsNullOrEmpty(brandList))
            {
                string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                         + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + companyID + " and brandID in(" + brandList + ") GROUP BY brandID; ";
                DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
                if (brandDt.Rows.Count > 0)
                {
                    sql += " and productID in("+brandDt.Rows[0]["value"].ToString()+")";
                }
               
            }
            if (!string.IsNullOrEmpty(productName))
            {
                sql += " and productID in (select productID from asm_product where proName like '%"+productName+"%' or bh like '%"+productName+ "%' or shortName like '%" + productName+"%')";
            }
            if (selType!="0")
            {
                sql += " and type="+selType;
            }
            if (!string.IsNullOrEmpty(orderNO))
            {
                sql += " and (orderNO like '%"+orderNO+"%' or acct like '%"+orderNO+ "%' or billno like '%"+orderNO+"%')";
            }
            if (!string.IsNullOrEmpty(idORphone))
            {
                string sqlM = "SELECT STUFF((SELECT ','+convert(varchar,id) FROM  asm_member where id='"+idORphone+"' or phone= '"+idORphone+ "' for xml path('')),1,1,'') id";
                DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
                if (dm.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dm.Rows[0]["id"].ToString()))
                    {
                        sql += " and  memberID in(" + dm.Rows[0]["id"].ToString() + ")";
                    }
                    else
                    {
                        sql += " and  memberID in(-1)";//证明没有这个会员
                    }
                }
                else {
                    sql += " and  memberID in(-1)";//证明没有这个会员
                }
            }
            string sql1 = "select * from(select A.*,B.bh,(select payType from asm_pay_info api where api.trxid=A.billno) payType1,(select acct from asm_pay_info where trxid=billno) acct,case A.type when '1' then '订购' when '2' then '零售' else '其他' end stu from (select asd.*,ap.proname from asm_sellDetail asd left join asm_product ap on asd.productID=ap.productID) A left join asm_mechine B on A.mechineID=B.id) C where 1=1 and " + sql;
          
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql1, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {

                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@" + OperUtil.DataTableToJsonWithJsonNet(dt);

                
                return ss;
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]
        public static object getMechineList(string companyID)
        {
            try
            {
              
                string sql = "select * from asm_mechine where  mechineName is not null and companyID=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
                }
                return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
            }
            catch
            {
                return new { code = 500, msg = "系统异常" };
            }
        }
        [WebMethod]
        public static object getBrandList(string companyID)
        {
            try
            {
                string sql = "select * from asm_brand where companyID=" + companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
                }
                return new { code = 300, msg = "暂无记录" };
            }
            catch
            {
                return new { code = 500, msg = "系统异常" };
            }
        }
        protected void excel_Click(object sender, EventArgs e)
        {
            string sql = " 1=1 ";
            if (!string.IsNullOrEmpty(this._mechineList.Value) && this._mechineList.Value != "0")
            {
                sql += " and mechineID in("+this._mechineList.Value+")";
            }
            if (!string.IsNullOrEmpty(this.start.Value))
            {
                sql += " and orderTime>convert(datetime,'" + this.start.Value + "')";
            }
            if (!string.IsNullOrEmpty(this.end.Value))
            {
                sql += " and orderTime<convert(datetime,'" + this.end.Value + "')";
            }
            if (ztlist.SelectedValue!="0")
            {
                sql += " and bz='"+ztlist.SelectedItem.Text+"'";
            }
            if (!string.IsNullOrEmpty(this._brandList.Value))
            {
                string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                         + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + this.companyID.Value + " and brandID in(" + this._brandList.Value + ") GROUP BY brandID; ";
                DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
                if (brandDt.Rows.Count > 0)
                {
                    sql += " and productID in(" + brandDt.Rows[0]["value"].ToString() + ")";
                }
            }
            if (!string.IsNullOrEmpty(productName.Value))
            {
                sql += " and productID in (select productID from asm_product where proName like '%" + productName.Value + "%' or bh like '%" + productName.Value + "%' or shortName like '%" + productName.Value + "%')";
            }
            if (selType.SelectedValue != "0")
            {
                sql += " and type=" + selType.SelectedValue;
            }
            
            if (!string.IsNullOrEmpty(orderNO.Value))
            {
                sql += " and (orderNO like '%" + orderNO.Value + "%' or acct like '%" + orderNO.Value + "%' or billno like '%" + orderNO.Value + "%')";
            }
            if (!string.IsNullOrEmpty(idORphone.Value))
            {
                string sqlM = "SELECT STUFF((SELECT ','+convert(varchar,id) FROM  asm_member where id='" + idORphone.Value + "' or phone= '" + idORphone.Value + "' for xml path('')),1,1,'') id";
                DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
                if (dm.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dm.Rows[0]["id"].ToString()))
                    {
                        sql += " and  memberID in(" + dm.Rows[0]["id"].ToString() + ")";
                    }
                    else
                    {
                        sql += " and  memberID in(-1)";//证明没有这个会员
                    }
                }
                else
                {
                    sql += " and  memberID in(-1)";//证明没有这个会员
                }
            }

            string sql1 = "select B.memberID,B.orderNO,B.proName,B.totalMoney,B.oldPrice,B.proLD,B.orderTime,B.orderDate,B.bz,B.bh,B.probh,B.stu,B.acct,B.pay,B.billno, case when B.dj=1 then '普通会员' when B.dj=2 then '白银会员' when B.dj=3 then '黄金会员' when B.dj=0 then '游客' else '支付宝' end djName from (select A.memberID,A.mechineID,A.productID,A.type,(select dj from asm_member where id=A.memberID) dj,A.orderNO,A.proname,A.totalMoney,A.oldPrice,A.proLD,A.orderTime,SUBSTRING(A.orderTime,0,12) orderDate,A.bz, B.bh,A.probh,case A.type when '1' then '订购' when '2' then '零售' else '其他' end stu,(select acct from asm_pay_info where trxid=billno) acct,"
               + " case A.payType when '0' then '' when '1' then '微信扫码支付' when '2' then '支付宝扫码' when '3' then '余额支付'  else '其他' end pay, A.billno from(select asd.*,ap.proname,ap.bh probh from asm_sellDetail asd left join asm_product ap on asd.productID=ap.productID) A left join asm_mechine B on A.mechineID=B.id where  B.companyID="+this.companyID.Value+") B where 1=1 and " + sql;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            ExportToSpreadsheet(dt, DateTime.Now.ToString("yyyyMMdd"));
        }
        public static void ExportToSpreadsheet(DataTable table, string name)
        {
            Random r = new Random();
            string rf = "";
            for (int j = 0; j < 10; j++)
            {
                rf = r.Next(int.MaxValue).ToString();
            }
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.ContentType = "application/ms-excel";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + rf + ".xls");
            context.Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            foreach (DataColumn column in table.Columns)
            {
                if (column.ColumnName == "memberID")
                {
                    context.Response.Write("会员ID" + ",");
                }
                if (column.ColumnName == "djName")
                {
                    context.Response.Write("等级" + ",");
                }
                if (column.ColumnName == "acct")
                {
                    context.Response.Write("支付ID" + ",");
                }
                if (column.ColumnName == "orderNO")
                {
                    context.Response.Write("订单编号" + ",");
                }
                
                if (column.ColumnName == "probh")
                {
                    context.Response.Write("商品条码" + ",");
                }
                if (column.ColumnName == "proName")
                {
                    context.Response.Write("商品名称" + ",");
                }
                if (column.ColumnName == "totalMoney")
                {
                    context.Response.Write("单价" + ",");
                }
                if (column.ColumnName == "oldPrice")
                {
                    context.Response.Write("原价" + ",");
                }
                if (column.ColumnName == "stu")
                {
                    context.Response.Write("类型" + ",");
                }
                if (column.ColumnName == "proLD")
                {
                    context.Response.Write("出货料道" + ",");
                }
                if (column.ColumnName == "orderTime")
                {
                    context.Response.Write("订单日期" + ",");
                }
                if (column.ColumnName == "orderDate")
                {
                    context.Response.Write("订单日" + ",");
                }
                if (column.ColumnName == "bh")
                {
                    context.Response.Write("机器编号" + ",");
                }
                if (column.ColumnName == "bz")
                {
                    context.Response.Write("备注" + ",");
                }
                if (column.ColumnName == "pay")
                {
                    context.Response.Write("支付方式" + ",");
                }
                if (column.ColumnName == "billno")
                {
                    context.Response.Write("流水号" + ",");
                }
            }
            context.Response.Write(Environment.NewLine);
            double test;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    switch (table.Columns[i].DataType.ToString())
                    {
                        case "System.String":
                            if (double.TryParse(row[i].ToString(), out test)) context.Response.Write("=");
                            context.Response.Write("\"" + row[i].ToString().Replace("\"", "\"\"") + "\",");
                            break;
                        case "System.DateTime":
                            if (row[i].ToString() != "")
                                context.Response.Write("\"" + ((DateTime)row[i]).ToString("yyyy-MM-dd HH:mm:ss") + "\",");
                            else
                                context.Response.Write("\"" + row[i].ToString().Replace("\"", "\"\"") + "\",");
                            break;
                        default:
                            context.Response.Write("\"" + row[i].ToString().Replace("\"", "\"\"") + "\",");
                            break;
                    }
                }
                context.Response.Write(Environment.NewLine);
            }
            context.Response.End();
        }
      
    }
}