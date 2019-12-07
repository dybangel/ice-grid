using autosell_center.util;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            string id = OperUtil.Get("comOperID");
            this.operaID.Value = id;
            this.OPID.Value = OperUtil.Get("operaID");
        }
        [WebMethod]
        public static string update(string id, string oldpwd, string newpwd,string OPID)
        {
           
            if (OPID == "0")
            {
                //说明是企业总管理员
                string update = "select * from asm_company where pwd='" + oldpwd + "' and  id="+id;
                DataTable dt = DbHelperSQL.Query(update).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string up = "update asm_company set pwd='" + newpwd + "' where id="+id;
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
            else
            {
                string sql = "select * from asm_opera where  pwd='" + oldpwd + "' and id=" + id;
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    string up = "update asm_opera set pwd='" + newpwd + "' where id=" + id;
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

        }
        [WebMethod]
        public static string update1(string id, string oldpwd, string newpwd, string OPID)
        {

            if (OPID == "0")
            {
                //说明是企业总管理员
                string update = "select * from asm_company where pwd2='" + oldpwd + "' and  id=" + id;
                DataTable dt = DbHelperSQL.Query(update).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string up = "update asm_company set pwd2='" + newpwd + "' where id=" + id;
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
            else
            {
                string sql = "select * from asm_opera where  pwd='" + oldpwd + "' and id=" + id;
                DataTable dd = DbHelperSQL.Query(sql).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    string up = "update asm_opera set pwd='" + newpwd + "' where id=" + id;
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

        }
    }
}