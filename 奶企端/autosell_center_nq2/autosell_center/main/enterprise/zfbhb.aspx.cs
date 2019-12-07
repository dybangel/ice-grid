using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.enterprise
{
    public partial class zfbhb : System.Web.UI.Page
    {
        public string comID = "";
        public string id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                id = Request.QueryString["id"].ToString();
                this.picID.Value = id;
                if (!string.IsNullOrEmpty(id))
                {
                    string sql = "select * from asm_zfbhb where id="+id;
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count>0) {
                        this.des.Value = dt.Rows[0]["des"].ToString();
                        this.pic1.Src = dt.Rows[0]["path"].ToString();
                        this.url.Value = dt.Rows[0]["url"].ToString();
                        this.start.Value = dt.Rows[0]["startTime"].ToString();
                        this.end.Value = dt.Rows[0]["endTime"].ToString();
                        this.typeList.SelectedValue= dt.Rows[0]["type"].ToString();
                    }
                }
                comID = OperUtil.Get("companyID");
                if (string.IsNullOrEmpty(comID))
                {
                    Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                    return;
                }
                this.companyID.Value = comID;
            }
            catch {
                comID = OperUtil.Get("companyID");
                if (string.IsNullOrEmpty(comID))
                {
                    Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                    return;
                }
                this.companyID.Value = comID;
            }
          
        }
    }
}