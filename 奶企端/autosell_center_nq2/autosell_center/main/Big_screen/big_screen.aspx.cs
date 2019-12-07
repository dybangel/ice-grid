using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.Big_screen
{
    public partial class big_screen : System.Web.UI.Page
    {
        private string comID = "";
        public string position = "";
        public string ycposttion = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this._companyID.Value = comID;
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
          
            string sqlme = "select  id sCode,bh sName from  asm_mechine where companyID="+comID;
            DataSet ddd = DbHelperSQL.Query(sqlme);
            this.cbosDeparentment.dtDataList = ddd;
            string sql = "select id,bh,zdX,zdY from asm_mechine where zdX!='' and zdY!='' and statu=0 and companyID="+comID;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            string str = "";
            if (dd.Rows.Count > 0)
            {
                for (int i = 0; i < dd.Rows.Count; i++)
                {
                    str += "[" + dd.Rows[i]["zdX"].ToString() + "," + dd.Rows[i]["zdY"].ToString() + "],";
                }
                str = str.Substring(0, str.Length - 1);
                position = str;
            }
            string sql1 = "select id,bh,zdX,zdY from asm_mechine where zdX!='' and zdY!='' and statu!=0 and companyID="+comID;
            DataTable dds = DbHelperSQL.Query(sql1).Tables[0];
            string str2 = "";
            if (dds.Rows.Count > 0)
            {
                for (int i = 0; i < dds.Rows.Count; i++)
                {
                    str2 += "[" + dds.Rows[i]["zdX"].ToString() + "," + dds.Rows[i]["zdY"].ToString() + "],";
                }
                str2 = str2.Substring(0, str2.Length - 1);
                ycposttion = str2;
            }
           
        }
        [WebMethod]
        public static string getTotalNum(string companyID)
        {
           
            string sql = "select count(*) orderNum,sum(totalMoney) totalMoney from asm_sellDetail  where type=2 and bz!='退款成功' and mechineID in (select id from asm_mechine where companyID="+companyID+")";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            string sql1 = "select count(*) orderNum,sum(totalMoney) totalMoney from asm_order where  mechineID in (select id from asm_mechine where companyID="+companyID+")";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
           
            string sql3 = "select count(*) orderNum,isnull(sum(totalMoney),0) totalMoney from asm_order where mechineID in (select id from asm_mechine where companyID=" + companyID+")";
            DataTable dt3 = DbHelperSQL.Query(sql3).Tables[0];

            string sql4 = "select count(*) orderNum,sum(totalMoney) totalMoney from asm_sellDetail where  bz!='退款成功' and mechineID in (select id from asm_mechine where companyID=" + companyID+")";
            DataTable dt4 = DbHelperSQL.Query(sql4).Tables[0];
            int totalDealNum = int.Parse(dt4.Rows[0]["orderNum"].ToString());
            int totalOrderNum = int.Parse(dt1.Rows[0]["orderNum"].ToString());
            double totalMoney = double.Parse(dt.Rows[0]["totalMoney"].ToString()) + double.Parse(dt3.Rows[0]["totalMoney"].ToString());
            return totalDealNum + "|" + totalOrderNum + "|" + totalMoney;
        }
        /// <summary>
        /// 获取今日机器订单信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string getOrderList(string companyID)
        {
           
            string sql = @"select A.*,ac.name,'1' type,''bz from (select am.bh,am.companyID,ao.createTime from asm_order ao left join asm_mechine am on ao.mechineID=am.id  where createTime like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%') A left join asm_company ac on A.companyID=ac.id union all"
                 + "   select B.bh,B.companyID,CONVERT(varchar(100), convert(datetime,B.orderTime), 20),ab.name,'2',bz from (select am.bh,am.companyID,asd.orderTime,asd.bz from asm_sellDetail asd left join asm_mechine am on asd.mechineID=am.id where CONVERT(varchar(100), convert(datetime,orderTime), 20) like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%') B left join asm_company ab on B.companyID=ab.id where B.companyID="+companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            return "1";
        }
        [WebMethod]
        public static string getHotProduct(string name,string companyID)
        {
            SqlConnection conn = null;
            conn = DbHelperSQL.getConnection();
            string sql = "select *,(select typeName from asm_protype app where app.productTypeID=ap.protype) typeName,(select name from asm_company where id=ap.companyID) companyName from asm_product ap where ap.companyID in (select id from asm_company where name like '%" + name + "%') and companyID="+companyID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            conn.Close();
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            return "1";
        }
        [WebMethod]
        public static string getBrokenDownList(string companyID)
        {
           
            string sql = "select top 5 bh,statu,case statu when '1' then '脱机' when '2' then '温度异常' else '' end brokenName,case statu when '1' then '停止运行' when '2' then '运行中' else '' end runStatu,brokenTime from asm_mechine where statu!=0 and companyID="+companyID+" order by brokenTime desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
           
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            return "1";
        }
        [WebMethod]
        public static string getMechineList(string companyID)
        {
          
            if (companyID == "0")
            {
                string sqlme = "select  id sCode,bh sName from  asm_mechine where 1=1";
                DataTable dd = DbHelperSQL.Query(sqlme).Tables[0];
                
                if (dd.Rows.Count > 0)
                {
                    
                    return OperUtil.DataTableToJsonWithJsonNet(dd);
                }
                else
                {
                  
                    return "1";
                }
            }
            else
            {
                string sqlme = "select  id sCode,bh sName from  asm_mechine where companyID=" + companyID;
                DataTable dd = DbHelperSQL.Query(sqlme).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    ;
                    return OperUtil.DataTableToJsonWithJsonNet(dd);
                }
                else
                {
                    
                    return "1";
                }
            }

        }
        [WebMethod]
        public static string ssts(string companyID)
        {
            
            string sql = @"select acct,am.name,am.sumConsume,am.sumRecharge,am.AvailableMoney, SUBSTRING(paytime,0,5)+'-'+SUBSTRING(paytime,5,2)+'-'+SUBSTRING(paytime,7,2)+' ' +SUBSTRING(paytime,9,2)+':'+SUBSTRING(paytime,11,2) time from asm_pay_info,asm_member am where (asm_pay_info.acct = am.openID or asm_pay_info.acct = am.minOpenID) and  1 = 1 and type = 1 and paytime like  '" + DateTime.Now.ToString("yyyyMMdd") + "%' and am.companyID="+companyID+"  order by paytime desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
         
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            return "1";
        }
        /// <summary>
        /// 1 销量 2销售额 3订单
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [WebMethod]
        public static string mechineList(string type,string companyID)
        {
            
            if (type == "1")
            {
                string sql1 = "select mechineID from (select top 5 SUM(num) num,mechineID from asm_sellDetail where type=2  group by mechineID) A order by A.num desc ";
                DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    string id = "";
                    for (int i = 0; i < dd.Rows.Count; i++)
                    {
                        id += dd.Rows[i]["mechineID"].ToString() + ",";
                    }
                    id = id.Substring(0, id.Length - 1);
                    string sql2 = "select (select name from asm_company where asm_company.id=am.companyID) companyName,* from asm_mechine am where id in (" + id + ") and companyID="+companyID;
                    DataTable dt = DbHelperSQL.Query(sql2).Tables[0];
                    
                    if (dt.Rows.Count > 0)
                    {
                       
                        string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                        return ss;
                    }

                }
            }
            else if (type == "2")
            {
                string sql1 = @"
                    select top 5(A.totalMoney + b.totalMoney) money,A.mechineID from
                     (select sum(totalMoney) totalMoney, mechineID from asm_order group by mechineID) A left join
                      (select sum(totalMoney) totalMoney, mechineID from asm_sellDetail where type = 2 group by mechineID) B on A.mechineID = B.mechineID order by (A.totalMoney + b.totalMoney) desc";
                DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    string id = "";
                    for (int i = 0; i < dd.Rows.Count; i++)
                    {
                        id += dd.Rows[i]["mechineID"].ToString() + ",";
                    }
                    id = id.Substring(0, id.Length - 1);
                    string sql2 = "select (select name from asm_company where asm_company.id=am.companyID) companyName,* from asm_mechine am where id in (" + id + ") and companyID=" + companyID;
                    DataTable dt = DbHelperSQL.Query(sql2).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        
                        string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                        return ss;
                    }

                }
            }
            else if (type == "3")
            {
                string sql1 = @" select count(*) num ,mechineID from asm_order group by mechineID order by count(*) desc";
                DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    string id = "";
                    for (int i = 0; i < dd.Rows.Count; i++)
                    {
                        id += dd.Rows[i]["mechineID"].ToString() + ",";
                    }
                    id = id.Substring(0, id.Length - 1);
                    string sql2 = "select (select name from asm_company where asm_company.id=am.companyID) companyName,* from asm_mechine am where id in (" + id + ") and companyID=" + companyID;
                    DataTable dt = DbHelperSQL.Query(sql2).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                       
                        return ss;
                    }

                }
            }
          
            return "1";
        }
        /// <summary>
        /// 显示最新的5分钟之内的数据
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string getBrokenDownListNew(string companyID)
        {
            
            string sql = "select top 1 bh,statu,case statu when '1' then '脱机' when '2' then '温度异常' else '' end brokenName,case statu when '1' then '停止运行' when '2' then '运行中' else '' end runStatu,brokenTime from asm_mechine where statu!=0 and brokenTime!='' and companyID="+companyID+"  and  DATEDIFF(MI,CONVERT(datetime,brokentime),GETDATE())<5 order by brokenTime desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
         
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
           return "1";
        }
        [WebMethod]
        public static string getPath(string id)
        {
            
            string sql = "select * from asm_mechine where id=" + id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = dt.Rows[0]["videoPath"].ToString();
                return ss;
            }
            else
            {
                return "1";
            }
        }
    }
}