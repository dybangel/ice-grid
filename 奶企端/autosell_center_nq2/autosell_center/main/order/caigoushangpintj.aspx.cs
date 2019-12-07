using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.order
{
    public partial class caigoushangpintj : System.Web.UI.Page
    {
        public string comID = "";
        public string operaID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                comID = OperUtil.Get("companyID");
                this.companyID.Value = comID;
                this.agentID.Value = OperUtil.Get("operaID");
                operaID = this.agentID.Value;
                if (string.IsNullOrEmpty(comID) || string.IsNullOrEmpty(operaID))
                {
                    Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                    return;
                }
            }
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
        public static object getBrandList(string companyID)
        {
            try
            {
                string sql = "select * from asm_brand where companyID="+companyID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return new { code=200,db=OperUtil.DataTableToJsonWithJsonNet(dt)};
                }
                return new { code = 300, msg="暂无记录" };
            }
            catch {
                return new { code=500,msg="系统异常"};
            }
        }
        [WebMethod]
        public static object getOperaList(string companyID)
        {
            try
            {
                string sql = "select * from asm_opera where companyID=" + companyID+ " and appQX=2";
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
        
        [WebMethod]
        public static object getMechineList(string companyID,string operalist)
        {
            try
            {
                string sql1 = "";
                if (!string.IsNullOrEmpty(operalist))
                {
                    sql1 += " and operaID in("+operalist+")";
                }
                string sql = "select * from asm_mechine where  mechineName is not null and companyID=" + companyID+sql1;
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
        public static object getOrderList(string lsTime,string dgStart,string dgEnd,string brandList,string operaList,string mechineList,string companyID,string productName)
        {
            try
            {
                if (string.IsNullOrEmpty(operaList))
                {
                    return new { code = 500, msg = "请选择操作员" };
                }
                if (string.IsNullOrEmpty(mechineList))
                {
                    return new { code = 500, msg = "请选择机器" };
                }
                if (string.IsNullOrEmpty(lsTime))
                {
                    return new { code = 500, msg = "请选择零售时间点" };
                }
                if (string.IsNullOrEmpty(dgStart))
                {
                    return new { code = 500, msg = "请选择订购开始时间" };
                }
                if (string.IsNullOrEmpty(dgEnd))
                {
                    return new { code = 500, msg = "请选择订购结束时间" };
                }
                if (string.IsNullOrEmpty(brandList))
                {
                    return new { code = 500, msg = "请选择品牌" };
                }
                string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                         + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + companyID + " and brandID in(" + brandList + ") and is_del=0 GROUP BY brandID; ";
                string productID = "";
                DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
                if (brandDt.Rows.Count > 0)
                {
                    for (int i = 0; i < brandDt.Rows.Count; i++)
                    {
                        productID += brandDt.Rows[i]["value"].ToString() + ",";
                    }
                    productID = productID.Substring(0, productID.Length - 1);
                    //此处目的是为了去掉历史记录里之前有的产品而现在把该产品给下架的产品ID
                    string sqlLd = "SELECT STUFF((SELECT ','+productID FROM  asm_ldinfo where mechineID in (" + mechineList + ")"
                              + " and productID  in(" + productID + ")  for xml path('')),1,1,'') productID";
                    DataTable dd = DbHelperSQL.Query(sqlLd).Tables[0];
                    if (dd.Rows.Count > 0)
                    {
                        productID = dd.Rows[0]["productID"].ToString();
                    }
                    if (!string.IsNullOrEmpty(productName))
                    {
                        string sqlp = "select * from  asm_product where proname like '%" + productName + "%' or bh like '%" + productName + "%' or shortName like '%" + productName + "%'";
                        DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                        if (dp.Rows.Count > 0)
                        {
                            for (int k = 0; k < dp.Rows.Count; k++)
                            {
                                productID += dd.Rows[0]["productID"].ToString() + ",";
                            }
                            productID = productID.Substring(0, productID.Length - 1);
                        }
                    }
                }
                productID=String.Join(",", productID.Split(',').Distinct<string>());
                if (string.IsNullOrEmpty(productID))
                {
                    return new { code = 500, msg = "无需要补货的商品" };
                }
                DataTable dt = OperUtil.getCGD(mechineList, productID, dgStart, dgEnd, lsTime);
                if (dt.Rows.Count > 0)
                {
                    return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
                }
                return new { code = 500, msg = "暂无记录" };
            }
            catch
            {
                return new { code = 500, msg = "系统异常" };
            }

        }
        [WebMethod]
        public static object getFjdList(string operaID, string companyID, string productName,string dgStart,string dgEnd,string lsTime,string brandList,string mechinelist)
        {
         

            try
            {
                if (string.IsNullOrEmpty(mechinelist))
                {
                    return new { code = 500, msg = "请选择机器" };
                }
                
                if (string.IsNullOrEmpty(lsTime))
                {
                    return new { code = 500, msg = "请选择零售时间点" };
                }
                if (string.IsNullOrEmpty(dgStart))
                {
                    return new { code = 500, msg = "请选择订购开始时间" };
                }
                if (string.IsNullOrEmpty(dgEnd))
                {
                    return new { code = 500, msg = "请选择订购结束时间" };
                }
                if (string.IsNullOrEmpty(brandList))
                {
                    return new { code = 500, msg = "请选择品牌" };
                }
                string sqlOpera = "select * from asm_opera where id="+operaID;
                DataTable dOpera = DbHelperSQL.Query(sqlOpera).Tables[0];

                string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                         + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + companyID + " and brandID in(" + brandList + ") and is_del=0 GROUP BY brandID; ";
                string productID = "";
                DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
                //检索机器ID
                if (!string.IsNullOrEmpty(operaID))
                {
                    string sqlMechine = "SELECT  operaID ,value = ( STUFF(( SELECT    ',' + convert(varchar,id) FROM asm_mechine"
                       + " WHERE operaID = Test.operaID and id in ("+mechinelist+") FOR XML PATH('') ), 1, 1, '') )FROM asm_mechine  AS Test where companyID = " + companyID + " and operaID=" + operaID + " GROUP BY operaID";
                    mechinelist = DbHelperSQL.Query(sqlMechine).Tables[0].Rows[0]["value"].ToString();
                }
                if (brandDt.Rows.Count > 0)
                {
                    for (int i = 0; i < brandDt.Rows.Count; i++)
                    {
                        productID += brandDt.Rows[i]["value"].ToString() + ",";
                    }
                    productID = productID.Substring(0, productID.Length - 1);
                    //此处目的是为了去掉历史记录里之前有的产品而现在把该产品给下架的产品ID
                    if (!string.IsNullOrEmpty(mechinelist))
                    {
                        string sqlLd = "SELECT STUFF((SELECT ','+productID FROM  asm_ldinfo where mechineID in (" + mechinelist + ")"
                             + " and productID  in(" + productID + ")  for xml path('')),1,1,'') productID";
                        DataTable dd = DbHelperSQL.Query(sqlLd).Tables[0];
                        if (dd.Rows.Count > 0)
                        {
                            productID = dd.Rows[0]["productID"].ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(productName))
                    {
                        string sqlp = "select * from  asm_product where proname like '%" + productName + "%' or bh like '%" + productName + "%' or shortName like '%" + productName + "%'";
                        DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                        if (dp.Rows.Count > 0)
                        {
                            for (int k = 0; k < dp.Rows.Count; k++)
                            {
                                productID = productID+ ",";
                            }
                            productID = productID.Substring(0, productID.Length - 1);
                        }
                    }
                }
                productID = String.Join(",", productID.Split(',').Distinct<string>());
                if (!string.IsNullOrEmpty(productID)&&!string.IsNullOrEmpty(mechinelist))
                {
                    DataTable dt = OperUtil.getJHD(mechinelist, productID ,dgStart, dgEnd, lsTime);
                    if (dt.Rows.Count > 0)
                    {
                        return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt),name= dOpera.Rows[0]["nickName"].ToString() };
                    }
                }
               
                return new { code = 500, msg = "暂无记录" };
            }
            catch
            {
                return new { code = 500, msg = "系统异常" };
            }
        }

    }
}