using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.ascx
{
    public partial class CheckboxListControl : System.Web.UI.UserControl
    {
        public string cbostr { get; set; }

        public string cboID { get; set; }//复选框的value值

        public string cboName { get; set; } // 复选框的Text值

        public DataSet dtDataList { get; set; }//数据源

        public string cboselectedValue { get; set; }//选中的字符串值
        protected void Page_Load(object sender, EventArgs e)
        {
            hdscbo.Value = cboID;
            BindData();

        }

        protected void BindData()
        {
            string liststr = "";
            DataTable dt = new DataTable();            
            if (CheckDataSet(dtDataList, out dt))
            {
                for (int i = 0; i<dt.Rows.Count; i++)
                {
                    string chkchecked = "";
                    if (!string.IsNullOrEmpty(cboselectedValue))
                    {
                        string[] arrstr = cboselectedValue.Split(',');
                        if (arrstr != null)
                        {
                            for (int c = 0; c<arrstr.Length; c++)
                            {
                                if (arrstr[c] == dt.Rows[i]["sCode"].ToString())
                                {
                                    chkchecked = "checked=\"checked\"";
                                    cboID += dt.Rows[i]["sCode"] + ",";
                                    cboName += dt.Rows[i]["sName"] + ",";
                                }
                            }
                        }
                    }
                    liststr += "<div><input type=\"checkbox\"   " + chkchecked + " name=\"subBox\"    onclick=\"changeinfo()\" value=\"" + dt.Rows[i]["sCode"] + "\" />" + dt.Rows[i]["sName"] + "</div>";
                }
            }
           cbostr = liststr;
         }
        public static bool CheckDataSet(DataSet ds, out DataTable dt)
        {
            if (ds!=null&&ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    return true;
                }
                else
                {
                    dt = null;
                    return false;
                }           
            }
            else
            {
                dt = null;
                return false;
            }
        }

    }
}