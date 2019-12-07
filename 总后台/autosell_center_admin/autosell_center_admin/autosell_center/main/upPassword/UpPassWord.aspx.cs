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

namespace autosell_center.main.upPassword
{
    public partial class UpPassWord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           string id=  OperUtil.Get("AdminOperaID");
            this.operaID.Value = id;
        }
        [WebMethod]
        public static string update(string id,string oldpwd,string newpwd)
        {
            if (id == "0")
            {
                //说明是admin
                string update = "select * from asm_manager where pwd='" + oldpwd + "' and bh='admin'";
                DataTable dt = DbHelperSQL.Query(update).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string up = "update asm_manager set pwd='" + newpwd + "' where bh='admin'";
                    int a = DbHelperSQL.ExecuteSql(up);
                    if (a > 0)
                    {
                        return "修改成功";
                    }
                    else
                    {
                        return "修改失败";
                    }
                }
                else
                {
                    return "原密码错误";
                }
            }
            else {
                string sql = "select * from asm_opera where  pwd='"+oldpwd+"' and id="+id;
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    string up = "update asm_opera set pwd='"+newpwd+"' where id="+id;
                    int a=DbHelperSQL.ExecuteSql(up);
                    if (a > 0)
                    {
                        return "修改成功";
                    }
                    else {
                        return "修改失败";
                    }
                }
                else {
                    return "原密码错误";
                }
            }
             
        }
    }
}