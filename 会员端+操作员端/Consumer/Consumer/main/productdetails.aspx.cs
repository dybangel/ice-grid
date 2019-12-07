using autosell_center.util;
using DBUtility;
using System;
using System.Data;
using System.Web.Services;

namespace Consumer.main
{
    public partial class productdetails : System.Web.UI.Page
    {
        public string productID = "";
        public string mechineID = "";
        public DataTable dt=new DataTable();
        public string createTime = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            productID = Request.QueryString["productID"].ToString();
            mechineID = Request.QueryString["mechineID"].ToString();

            //productID = "167";
            //mechineID = "24";
            this.product_id.Value = productID;
            this._mechineID.Value = mechineID;
            string sql = "select * from asm_mechine where id="+mechineID;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            this.companyID.Value = dd.Rows[0]["companyID"].ToString();
            createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            initData();
        }
        public void initData()
        {
            string sql = "select * from asm_product where productID="+this.product_id.Value;
            dt = DbHelperSQL.Query(sql).Tables[0];

        }
        [WebMethod]
        public static string getActivityList(string companyID,string productID,string mechineID)
        {
            string sql = "select *,'' str from asm_activity where companyID="+companyID;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
             
            if (dd.Rows.Count > 0)
            {
                for (int i = 0; i < dd.Rows.Count; i++)
                {
                    string sql1 = "select * from asm_activity_detail  where  activityID=" + dd.Rows[i]["id"].ToString() + " and companyID=" + companyID + " and mechineID=" + mechineID + " and productID=" + productID;
                    DataTable dt = DbHelperSQL.Query(sql1).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        string type = dd.Rows[i]["type"].ToString();
                        string num = dd.Rows[i]["num"].ToString();
                        if (type == "1")
                        {
                            dd.Rows[i]["str"] = "打" + num + "折";
                        }
                        else if (type == "2")
                        {
                            dd.Rows[i]["str"] = "赠送" + num + "天";
                        }
                        else
                        {
                            dd.Rows[i]["str"] = "暂无设置";
                        }
                    }
                    else {
                        dd.Rows[i]["str"] = "暂无设置";
                    }
                }
                return OperUtil.DataTableToJsonWithJsonNet(dd);
            }
            else {
                return "1";
            }
        }
    }
}