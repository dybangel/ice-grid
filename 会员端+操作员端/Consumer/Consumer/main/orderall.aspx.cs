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

namespace Consumer.main
{
    public partial class orderall : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                this.memberID.Value = Util.getMemberID();
            }
            else
            {
                Response.Write("<script>window.location.href=homeIndex.aspx</script>");
            }
            this.companyID.Value = OperUtil.getCooki("companyID");
            
        }
        [WebMethod]
        public static string getOrderList(string memberID,string type)
        {
            if (type == "all")
            {
                string sql = "select ao.*,ap.proName,ap.description,ap.httpImageUrl from asm_order ao left join asm_product ap on ao.productID=ap.productID where memberID=" + memberID+ " and fkzt=1 order by createTime desc";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                    return ss;
                }
                else
                {
                    return "1";
                }
            } else if (type== "pickup")//取货中
            {
                string sql = "select ao.*,ap.proName,ap.description,ap.httpImageUrl from asm_order ao left join asm_product ap on ao.productID=ap.productID where memberID=" + memberID+ " and zt=1 and fkzt=1 order by createTime desc";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                    return ss;
                }
                else
                {
                    return "1";
                }
            }
            else if (type == "payment")//取货完成
            {
                string sql = "select ao.*,ap.proName,ap.description,ap.httpImageUrl from asm_order ao left join asm_product ap on ao.productID=ap.productID where memberID=" + memberID+ " and zt=3  order by createTime desc";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                    return ss;
                }
                else
                {
                    return "1";
                }
            }
            else if (type == "resale")//已经转售
            {
                string sql = "select ao.*,ap.proName,ap.description,ap.httpImageUrl from asm_orderDetail ao left join asm_product ap on ao.productID=ap.productID where memberID=" + memberID+ " and zt=3  order by createTime desc";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                    return ss;
                }
                else
                {
                    return "1";
                }
            }
            else if (type == "invalid")//已经失效
            {
                string sql = "select ao.*,ap.proName,ap.description,ap.httpImageUrl from asm_orderDetail  ao left join asm_product ap on ao.productID=ap.productID  where ao.zt=2 and ao.memberID=" + memberID+ "  order by createTime desc";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                    return ss;
                }
                else
                {
                    return "1";
                }
            }
            else if (type == "dzf")//待支付
            {
                string sql = "select ao.*,ap.proName,ap.description,ap.httpImageUrl from asm_order  ao left join asm_product ap on ao.productID=ap.productID  where  fkzt=0 and ao.memberID=" + memberID + " order by createTime desc";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                    return ss;
                }
                else
                {
                    return "1";
                }
            }
            return "1";
        }
        [WebMethod]
        public static object  del(string id)
        {
            string sql = "select * from asm_order where id="+id+" and fkzt=0";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count > 0)
            {
                string sql1 = "delete from asm_order where id="+id;
                int a= DbHelperSQL.ExecuteSql(sql1);
                if (a>0)
                {
                    return new { code = 0, msg = "删除成功" };
                }
                return new { code = 0, msg = "删除失败" };
            }
            else {
                return new { code = 0, msg = "该笔订单无法删除" };
            }
        }
    }
}