using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace autosell_center.main.datastatistics
{
    public partial class Statisticalquery : System.Web.UI.Page
    {
     
        public string totalCount = "0";
        public string totalMoney = "0";
        public string totalOrder = "0";
        public string time = "";
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                //string sql = "select * from asm_company";
                //DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                //this.companyList.DataTextField = "name";
                //this.companyList.DataValueField = "id";
                //this.companyList.DataSource = dt;
                //this.companyList.DataBind();
                //this.companyList.Items.Insert(0, new ListItem("全部", "0")); //添加项
                time = this.end.Value;
                string sqlme = "select  id sCode,bh sName from  asm_mechine ";
                DataSet dd = DbHelperSQL.Query(sqlme);
                this.cbosDeparentment.dtDataList = dd;
            }
        }
        [WebMethod]
        public static string getCompanyList()
        {
            string sqlme = "select '0' id,'全部' name union all select id, name from asm_company";
            DataTable  dd = DbHelperSQL.Query(sqlme).Tables[0];
            if (dd.Rows.Count > 0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dd);
            }
            else {
                return "1";
            }
        }
        [WebMethod]
        public static string getProductList(string companyID)
        {
            string sql1 = "";
            if (companyID!="0"&&companyID!=null)
            {
                sql1 += " and companyID="+companyID;
            }
            string sqlme = " select '0' id,'全部' name union all select productID,proName from asm_product  where 1=1 " + sql1;
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
        [WebMethod]
        public static string getOperaList(string companyID)
        {
            if (companyID != "0")
            {
                string sql = "select * from asm_opera where companyID="+companyID+" and id IN(select operaID from asm_mechine where companyID=" + companyID + ")";
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    return OperUtil.DataTableToJsonWithJsonNet(dd);
                }
                else
                {
                    return "1";
                }
            }
            else {
                string sql = "select * from asm_opera where companyID!=0";
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    return OperUtil.DataTableToJsonWithJsonNet(dd);
                }
                else
                {
                    return "1";
                }
            }
           
        }
        [WebMethod]
        public static string getMechineList(string companyID,string operaID)
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
            else {
                if (operaID != "0")
                {
                    string sqlme = "select  id sCode,bh sName from  asm_mechine where companyID=" + companyID+" and operaID="+operaID;
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
                else {
                    string sqlme = "select  id sCode,bh sName from  asm_mechine where companyID=" + companyID;
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
               
            }
        }
        [WebMethod]
        public static string getData(string province, string city, string country, string start, string end, string type, string companyID,string mechineIDStr, string memberKey,string productId,string price1,string price2,string operaID)
        {
            string totalCount = "0";
            string totalMoney = "0";
            string totalOrder = "0";
            string dgNum = "0";
            string lsNum = "0";
            string sql = " 1=1 ";
            string sq = " 1=1 ";
            string sqq = " 1=1 ";
            if (!string.IsNullOrEmpty(productId)&&productId!="null"&&productId!="0")
            {
                sqq += " and productID='"+productId+"'";
            }
            if (!string.IsNullOrEmpty(price1))
            {
                sqq += " and price2>"+price1;
            }
            if (!string.IsNullOrEmpty(price2))
            {
                sqq += " and price2<"+price2;
            }
            string sqlP = "select productID from asm_product where "+sqq;

            if (operaID != "0")
            {
                //查询操作员下边的所有机器
                string sql1 = "SELECT isnull(STUFF((SELECT ','+convert(varchar,id) FROM asm_mechine where operaID='" + operaID + "'  for xml path('')),1,1,''),0) id";
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                if (mechineIDStr == "")
                {
                    mechineIDStr = d1.Rows[0]["id"].ToString();
                }

            }
            if (companyID != "0"&&companyID!="null")
            {
                sql += " and companyID=" + companyID;
                sq += " and companyID=" + companyID;
                string sqlkc = "select isnull(sum(ld_productNum),0) num,type from [dbo].[asm_ldInfo] where mechineID in(select id from asm_mechine where companyID =" + companyID + ") and productID in ("+sqlP+") and type=1 group by type";
                DataTable dd = DbHelperSQL.Query(sqlkc).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    dgNum = dd.Rows[0]["num"].ToString();

                }
                else {
                    dgNum = "0";
                }
                string sqlkc1 = "select isnull(sum(ld_productNum),0) num,type from [dbo].[asm_ldInfo] where mechineID in(select id from asm_mechine where companyID =" + companyID + ") and productID in (" + sqlP + ") and type=2 group by type";
                DataTable dd1 = DbHelperSQL.Query(sqlkc1).Tables[0];
                if (dd1.Rows.Count > 0)
                {
                    lsNum = dd1.Rows[0]["num"].ToString();

                }
                else {
                    lsNum = "0";
                }
                
            }
            else {
                string sqlkc = "select isnull(sum(ld_productNum),0) num,type from [dbo].[asm_ldInfo] where  productID in (" + sqlP + ") and type=1 group by type";
                DataTable dd = DbHelperSQL.Query(sqlkc).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    dgNum = dd.Rows[0]["num"].ToString();

                } else {
                    dgNum = "0";
                }
                string sqlkc1 = "select isnull(sum(ld_productNum),0) num,type from [dbo].[asm_ldInfo] where  productID in (" + sqlP + ") and type=2 group by type";
                DataTable dd1 = DbHelperSQL.Query(sqlkc1).Tables[0];
                if (dd1.Rows.Count > 0)
                {
                    lsNum = dd1.Rows[0]["num"].ToString();

                }
                else
                {
                    lsNum = "0";
                }
                
            }
            if (province != "省份")
            {
                sql += " and province like '%" + province + "%' ";
                sq += " and province like '%" + province + "%' ";
            }
            if (city != "地级市")
            {
                sql += " and city like '%" + city + "%' ";
                sq += " and city like '%" + city + "%' ";
            }
            if (country != "市、县级市")
            {
                sql += " and country like '%" + country + "%' ";
                sq += " and country like '%" + country + "%' ";
            }
            if (!string.IsNullOrEmpty(start))
            {
                sql += " and CONVERT(varchar(100), convert(datetime,orderTime), 120)>'" + start + "' ";
                sq += " and createTime>'" + start + "' ";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += " and CONVERT(varchar(100), convert(datetime,orderTime), 120)<'" + end + "'";
                sq += " and createTime<'" + end + "'";
            }
            if (!string.IsNullOrEmpty(mechineIDStr)&&mechineIDStr!="0")
            {
                sql += " and mechineID in ("+mechineIDStr+")";
                sq += " and  mechineID in ("+mechineIDStr+")";
                string sqlkc = "select isnull(sum(ld_productNum),0) num,type from [dbo].[asm_ldInfo] where mechineID in("+mechineIDStr+ ") and  productID in (" + sqlP + ") and  type=1 group by type";
                DataTable dd = DbHelperSQL.Query(sqlkc).Tables[0];
                if (dd.Rows.Count>0)
                {
                    dgNum = dd.Rows[0]["num"].ToString();

                }
                string sqlkc1 = "select isnull(sum(ld_productNum),0) num,type from [dbo].[asm_ldInfo] where mechineID in(" + mechineIDStr + ") and  productID in (" + sqlP + ") and  type=2 group by type";
                DataTable dd1 = DbHelperSQL.Query(sqlkc1).Tables[0];
                if (dd1.Rows.Count > 0)
                {
                    lsNum = dd1.Rows[0]["num"].ToString();

                }
                else {
                    lsNum = "0";
                }
                
            }
            if (!string.IsNullOrEmpty(memberKey))
            {
                string sqlM = "SELECT isnull(STUFF((SELECT ','+CONVERT(varchar,id) FROM  asm_member where  phone like '%" + memberKey + "%' or name like '%" + memberKey + "%' or nickname like '%" + memberKey + "%'  for xml path('')),1,1,''),0) id";
                DataTable dd = DbHelperSQL.Query(sqlM).Tables[0];
                sql += " and memberID in ("+dd.Rows[0]["id"].ToString()+")";
                sq += " and memberID in ("+dd.Rows[0]["id"].ToString()+")";
            }
            if (type == "1")
            {
                string sql1 = "select count(*) num from asm_sellDetail asd left join asm_mechine am on asd.mechineID=am.id where type=1 and bz!='退款成功' and  productID in (" + sqlP + ") and " + sql;
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                totalCount = dt.Rows[0]["num"].ToString();
                string sql2 = "select count(*) num,isnull(sum(CONVERT(float,totalMoney)),0) totalMoney from asm_order ao left join asm_mechine am on ao.mechineID=am.id where   ao.productID in (" + sqlP + ") and" + sq;
                DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                totalOrder = d2.Rows[0]["num"].ToString();
                totalMoney = d2.Rows[0]["totalMoney"].ToString();
            }
            else if (type == "2")
            {
                string sql1 = "select count(*) num,isnull(sum(CONVERT(float,totalMoney)),0) totalMoney from asm_sellDetail asd left join asm_mechine am on asd.mechineID=am.id where type=2 and bz!='退款成功' and asd.productID in (" + sqlP + ") and " + sql;
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                totalCount = dt.Rows[0]["num"].ToString();
                totalOrder = totalCount;
                totalMoney = dt.Rows[0]["totalMoney"].ToString();
            }
            else
            {
                string sql1 = "select count(*) num,isnull(sum(CONVERT(float,totalMoney)),0) totalMoney from asm_sellDetail asd left join asm_mechine am on asd.mechineID=am.id where  type=2 and  bz!='退款成功' and  asd.productID in (" + sqlP + ") and " + sql;
                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
                string sql3 = "select count(*) num,isnull(sum(CONVERT(float,totalMoney)),0) totalMoney from asm_sellDetail asd left join asm_mechine am on asd.mechineID=am.id where  type=1 and bz!='退款成功' and  asd.productID in (" + sqlP + ") and " + sql;
                DataTable dt3 = DbHelperSQL.Query(sql3).Tables[0];
                totalCount = (int.Parse(dt3.Rows[0]["num"].ToString()) + int.Parse(dt.Rows[0]["num"].ToString())).ToString();

                string sql2 = "select count(*) num,isnull(sum(CONVERT(float,totalMoney)),0) totalMoney from asm_order ao left join asm_mechine am on ao.mechineID=am.id where ao.productID in (" + sqlP + ") and " + sq;
                DataTable d2 = DbHelperSQL.Query(sql2).Tables[0];
                totalOrder = (int.Parse(d2.Rows[0]["num"].ToString())).ToString();
                totalMoney = (double.Parse(dt.Rows[0]["totalMoney"].ToString()) + double.Parse(d2.Rows[0]["totalMoney"].ToString())).ToString("f2");
            }
            return totalCount + "|" + totalMoney + "|" + totalOrder+"|"+dgNum+"|"+lsNum;
        }

        
    }
}