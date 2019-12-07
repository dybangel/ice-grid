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

using System.Configuration;


namespace autosell_center.main.member
{
    public partial class memberdjcontent : System.Web.UI.Page
    {
        public string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this._operaID.Value = OperUtil.Get("operaID");
            this.companyID.Value = comID;
            
            if (!IsPostBack)
            {
                search(comID,"1");
               

            }


        }
        //保存数据到数据库
        protected void Button1_Click(object sender, EventArgs e)
        {
           
            int count = 0;
            
            string aa = "update   asm_content  set content='" + this.nContent.Text.Trim() + "' where companyID='" + this.companyID.Value + "' and type ='1' ";
            count = DbHelperSQL.ExecuteSql(aa);
          
           
            
            if (count > 0)
            {
                Response.Write("<script>alert('成功！！');</script>");
            }
            else
            {
                Response.Write("<script>alert('添加失败！！');</script>");
            }
           
        }

        protected void search(string companyID, string type)
        {

            string sql = "select * from asm_content where companyID='" + companyID + "' and type ='1' ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.contentID.Value = dt.Rows[0]["id"].ToString();
                this.nContent.Text = dt.Rows[0]["content"].ToString();
            }
            else {
                string aa = "insert into asm_content (type,companyID,content) values ('1','" + this.companyID.Value + "','')";
                DbHelperSQL.ExecuteSql(aa);
            }
            
        }

    }
}