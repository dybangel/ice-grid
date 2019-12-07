using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.Big_screen
{
    public partial class echartMain : System.Web.UI.Page
    {
        public string province = "";
        public string city = "";
        public string country = "";
        public string start = "";
        public string end = "";
        public string sxType = "";
        public string companyID = "";
        public string mechineIDStr = "";

        public string time = "";
        public string money = "";
        public string ddNum = "";
        public string xsNUm = "";

        public int totalXL = 0;
        public double totalMoney = 0;
        public int totalDD = 0;
        public int max_dd = 0;
        public int max_xl = 0;
        public double max_money = 0;
        public string agentID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            companyID = OperUtil.Get("companyID");
            this._companyID.Value = companyID;
            agentID = OperUtil.Get("operaID");
            this._agentID.Value = agentID;
            if (string.IsNullOrEmpty(companyID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            try
            {
                province = Request.QueryString["province"].ToString();
                city = Request.QueryString["city"].ToString();
                country = Request.QueryString["country"].ToString();
                start = Request.QueryString["start"].ToString();
                end = Request.QueryString["end"].ToString();
                sxType = Request.QueryString["sxType"].ToString();
                mechineIDStr = Request.QueryString["mechineIDStr"].ToString();
                
                getData(province, city, country, start, end, sxType, companyID, mechineIDStr, agentID);
            }
            catch
            {
                //初始化的时候查全部
                getData(null, null, null, null, null, null, companyID, null,this._agentID.Value);
            }
        }
        public void getData(string province, string city, string country, string start, string end, string type, string companyID, string mechineIDStr,string agentID)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(companyID)&&companyID!="0")
            {
                if (agentID != "0")
                {
                    string sqlM = " SELECT isnull(STUFF((SELECT ','+convert(varchar,id) FROM asm_mechine where dls='"+agentID+"' for xml path('')),1,1,''),0) id";
                    DataTable dd = DbHelperSQL.Query(sqlM).Tables[0];
                    if (string.IsNullOrEmpty(mechineIDStr))
                    {
                        mechineIDStr = dd.Rows[0]["id"].ToString();
                    }
                }
                else {
                    string sqlM = " SELECT isnull(STUFF((SELECT ','+convert(varchar,id) FROM asm_mechine where companyID=" + companyID + "  for xml path('')),1,1,''),0) id";
                    DataTable dd = DbHelperSQL.Query(sqlM).Tables[0];
                    if (string.IsNullOrEmpty(mechineIDStr))
                    {
                        mechineIDStr = dd.Rows[0]["id"].ToString();
                    }
                }
                

            }
            if (province != "省份" && !string.IsNullOrEmpty(province))
            {
                sql += " and province like '%" + province + "%'";
            }
            if (city != "地级市" && !string.IsNullOrEmpty(city))
            {
                sql += " and city like '%" + city + "%'";
            }
            if (country != "市、县级市" && !string.IsNullOrEmpty(country))
            {
                sql += " and country like '%" + country + "%'";
            }
            if (!string.IsNullOrEmpty(start))
            {
                sql += " and createTime >='" + start + "'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += " and createTime<='" + end + "'";
            }
            if (!string.IsNullOrEmpty(mechineIDStr))
            {
                sql += " and mechineID in(" + mechineIDStr + ")";
            }

            if (type == "1")
            {
                string sql1 = "select A.createTime,sum(A.num) dgNum ,sum(A.totalMoney) dgTotalMoney, sum(isnull(B.num,0)) dgSellNum,sum(isnull(B.totalMoney,0)) dgSellMoney from "
               + " (select * from[dbo].[View_dgDD]) A left join(select * from[dbo].[View_dgSell]) B on A.createTime = b.createTime and A.mechineID = B.mechineID where 1=1 " + sql
               + " group by A.createTime";
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    int[] dd_arr = new int[dt.Rows.Count];
                    int[] xl_arr = new int[dt.Rows.Count];
                    double[] money_arr = new double[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        time += "'" + dt.Rows[i]["createTime"].ToString() + "',";
                        money += dt.Rows[i]["dgTotalMoney"].ToString() + ",";
                        ddNum += dt.Rows[i]["dgNum"].ToString() + ",";
                        xsNUm += dt.Rows[i]["dgSellNum"].ToString() + ",";
                        totalDD += int.Parse(dt.Rows[i]["dgNum"].ToString());
                        totalXL += int.Parse(dt.Rows[i]["dgSellNum"].ToString());
                        totalMoney += double.Parse(dt.Rows[i]["dgTotalMoney"].ToString());

                        dd_arr[i] = int.Parse(dt.Rows[i]["dgNum"].ToString());
                        xl_arr[i] = int.Parse(dt.Rows[i]["dgSellNum"].ToString());
                        money_arr[i] = double.Parse(dt.Rows[i]["dgTotalMoney"].ToString());
 
                    }
                    Array.Sort(dd_arr);
                    Array.Sort(xl_arr);
                    Array.Sort(money_arr);
                    max_dd = dd_arr[dd_arr.Length - 1] * 2;
                    max_xl = xl_arr[xl_arr.Length - 1] * 2;
                    max_money = money_arr[money_arr.Length - 1] * 1.5;
                    time = time.Substring(0, time.Length - 1);
                    money = money.Substring(0, money.Length - 1);
                    ddNum = ddNum.Substring(0, ddNum.Length - 1);
                    xsNUm = xsNUm.Substring(0, xsNUm.Length - 1);
                }
            }
            else if (type == "2")
            {
                string sql2 = "select createTime, SUM(num) lsNum,sum(totalMoney) lsTotalmoney from [dbo].[View_lsSell] A where 1=1 " + sql + " group by createTime";
                DataTable dt = DbHelperSQL.Query(sql2).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    int[] dd_arr = new int[dt.Rows.Count];
                    int[] xl_arr = new int[dt.Rows.Count];
                    double[] money_arr = new double[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        time += "'" + dt.Rows[i]["createTime"].ToString() + "',";
                        money += dt.Rows[i]["lsTotalmoney"].ToString() + ",";
                        ddNum += dt.Rows[i]["lsNum"].ToString() + ",";
                        xsNUm += dt.Rows[i]["lsNum"].ToString() + ",";

                        totalDD += int.Parse(dt.Rows[i]["lsNum"].ToString());
                        totalXL += int.Parse(dt.Rows[i]["lsNum"].ToString());
                        totalMoney += double.Parse(dt.Rows[i]["lsTotalmoney"].ToString());

                        dd_arr[i] = int.Parse(dt.Rows[i]["lsNum"].ToString());
                        xl_arr[i] = int.Parse(dt.Rows[i]["lsNum"].ToString());
                        money_arr[i] = double.Parse(dt.Rows[i]["lsTotalmoney"].ToString());
                    }
                    Array.Sort(dd_arr);
                    Array.Sort(xl_arr);
                    Array.Sort(money_arr);
                    max_dd = dd_arr[dd_arr.Length - 1] * 2;
                    max_xl = xl_arr[xl_arr.Length - 1] * 2;
                    max_money = money_arr[money_arr.Length - 1] * 1.5;
                    time = time.Substring(0, time.Length - 1);
                    money = money.Substring(0, money.Length - 1);
                    ddNum = ddNum.Substring(0, ddNum.Length - 1);
                    xsNUm = xsNUm.Substring(0, xsNUm.Length - 1);
                }
            }
            else if (type == "0" || type == null)
            {
                string sql3 = "select createTime,SUM(dgNum) dgNum,sum(dgTotalMoney) dgTotalMoney,sum(dgSellNum) dgSellNum from "
                + "(select A.createTime, sum(A.num) dgNum, sum(A.totalMoney) dgTotalMoney, sum(isnull(B.num, 0)) dgSellNum from "
                + " (select * from[dbo].[View_dgDD]) A left join(select * from[dbo].[View_dgSell]) B on A.createTime = b.createTime and A.mechineID = B.mechineID where 1=1 " + sql
                + " group by A.createTime "
                + " union all "
                + " select createTime, SUM(num) totalNum, sum(totalMoney) lsTotalmoney, SUM(num) lsSellNum from[dbo].[View_lsSell] A where 1 = 1 " + sql + " group by createTime) D"
                + " group by createTime ";
                string sqlQ = "select isnull(AA.createTime,BB.createTime) createTime,isnull(AA.num,0) ddNum,isnull(AA.num1,0)ddsNum,isnull(AA.totalMoney,0)ddMoney,isnull(AA.totalMoney1,0)ddsMoney,BB.num,BB.totalMoney lsMoney,isnull(AA.num1,0)+BB.num totalNum,BB.totalMoney+isnull(AA.totalMoney,0) totalMoney from "
                          + " ((select isnull(A.createTime, B.createTime) createTime, isnull(A.num, 0) num, isnull(A.totalMoney, 0) totalMoney, isnull(B.num, 0) num1, isnull(B.totalMoney, 0)totalMoney1  from "
                          + " (select createTime, sum(num) num, SUM(totalMoney) totalMoney from View_dgDD where 1=1 " + sql + " group by createTime) A "
                          + " full join"
                          + " (select createTime, SUM(num) num, SUM(totalMoney) totalMoney from View_dgSell where 1=1 " + sql + " group by createTime) B"
                          + " on A.createTime = B.createTime)) AA"
                          + " full join"
                          + " (select createTime, SUM(num) num, sum(totalMoney) totalMoney from View_lsSell where 1=1 " + sql + " group by createTime) BB"
                          + " on AA.createTime = BB.createTime";
                DataTable dt = DbHelperSQL.Query(sqlQ).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    int[] dd_arr = new int[dt.Rows.Count];
                    int[] xl_arr = new int[dt.Rows.Count];
                    double[] money_arr = new double[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        time += "'" + dt.Rows[i]["createTime"].ToString() + "',";
                        money += dt.Rows[i]["totalMoney"].ToString() + ",";
                        ddNum += dt.Rows[i]["ddNum"].ToString() + ",";
                        xsNUm += dt.Rows[i]["totalNum"].ToString() + ",";

                        totalDD += int.Parse(dt.Rows[i]["ddNum"].ToString()==""?"0": dt.Rows[i]["ddNum"].ToString());
                        totalXL += int.Parse(dt.Rows[i]["totalNum"].ToString() == "" ? "0" : dt.Rows[i]["totalNum"].ToString());
                        totalMoney += double.Parse(dt.Rows[i]["totalMoney"].ToString() == "" ? "0" : dt.Rows[i]["totalMoney"].ToString());
                        dd_arr[i] = int.Parse(dt.Rows[i]["ddNum"].ToString() == "" ? "0" : dt.Rows[i]["ddNum"].ToString());
                        xl_arr[i] = int.Parse(dt.Rows[i]["totalNum"].ToString() == "" ? "0" : dt.Rows[i]["totalNum"].ToString());
                        money_arr[i] = double.Parse(dt.Rows[i]["totalMoney"].ToString() == "" ? "0" : dt.Rows[i]["totalMoney"].ToString());
                    }
                    Array.Sort(dd_arr);
                    Array.Sort(xl_arr);
                    Array.Sort(money_arr);
                    max_dd = dd_arr[dd_arr.Length - 1] * 2;
                    max_xl = xl_arr[xl_arr.Length - 1] * 2;
                    max_money = money_arr[money_arr.Length - 1] * 1.5;
                    time = time.Substring(0, time.Length - 1);
                    money = money.Substring(0, money.Length - 1);
                    ddNum = ddNum.Substring(0, ddNum.Length - 1);
                    xsNUm = xsNUm.Substring(0, xsNUm.Length - 1);
                }
            }
           
        }
    }
}