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

namespace autosell_center.main.product
{
    public partial class updateProduct : System.Web.UI.Page
    {
        public string productID = "";
        private string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            productID = Request.QueryString["productID"].ToString();
            this.product_id.Value = productID;
            comID = OperUtil.Get("companyID");
            this.companyID.Value = comID;
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
          
            string sql1 = "select * from asm_protype ";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            this.typeLB.DataTextField = "typeName";
            this.typeLB.DataValueField = "productTypeID";
            this.typeLB.DataSource = dt1;
            this.typeLB.DataBind();

            string sql2 = "select * from asm_brand where companyID=" + this.companyID.Value;
            DataTable dts = DbHelperSQL.Query(sql2).Tables[0];
            this.brandlist.DataTextField = "brandName";
            this.brandlist.DataValueField = "id";
            this.brandlist.DataSource = dts;
            this.brandlist.DataBind();
            if (productID!="")
            {
                string sql = "select * from asm_product where productID="+productID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count>0)
                {
                    this.name.Value = dt.Rows[0]["proName"].ToString();
                    this.typeLB.SelectedValue = dt.Rows[0]["protype"].ToString();
                    this.price0.Value= dt.Rows[0]["price0"].ToString();
                    this.price1.Value = dt.Rows[0]["price1"].ToString();
                    this.price2.Value = dt.Rows[0]["price2"].ToString();
                    this.price3.Value = dt.Rows[0]["price3"].ToString();
                    this.bzqDay.Value = dt.Rows[0]["bzq"].ToString();
                    this.progg.Value = dt.Rows[0]["progg"].ToString();
                    this.description.Value= dt.Rows[0]["description"].ToString();
                    this.shortname.Value = dt.Rows[0]["shortName"].ToString();
                    this.bh.Value= dt.Rows[0]["bh"].ToString();
                    this.tag.Value= dt.Rows[0]["tag"].ToString();
                    this.ztList.SelectedValue = dt.Rows[0]["sluid"].ToString();
                    this.type.SelectedValue = dt.Rows[0]["dstype"].ToString();
                    this.startSend.Value= dt.Rows[0]["startSend"].ToString();
                    this.weight.Value= dt.Rows[0]["weight"].ToString();
                    if (dt.Rows[0]["path"].ToString() != "")
                    {
                        this.pic.Src = "~"+ dt.Rows[0]["path"].ToString();
                    }
                    else {
                        this.pic.Src = "/main/public/images/addimg.png";
                    }
                    this.brandlist.SelectedValue = dt.Rows[0]["brandID"].ToString();
                }
            }
        }
    }
}