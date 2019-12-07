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

namespace autosell_center.main.Adminlist
{
    public partial class adminlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "select * from asm_role where companyID=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            this.roleList.DataTextField = "name";
            this.roleList.DataValueField = "id";
            this.roleList.DataSource = dt;
            this.roleList.DataBind();
            this.DropDownList1.DataTextField = "name";
            this.DropDownList1.DataValueField = "id";
            this.DropDownList1.DataSource = dt;
            this.DropDownList1.DataBind();
        }
        [WebMethod]
        public static string sear(string id)
        {
            return "";
        }
        [WebMethod]
        public static string add(string bh,string name,string phone,string pwd,string qx)
        {
            //判断编号是否重复
            string sql = "select * from asm_opera where name='"+bh+"'";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count > 0)
            {
                return "1";
            }
            else {
                string sql1 = "insert into asm_opera(name,nickName,linkphone,pwd,companyID,createTime,statu,qx) values('"+bh+"','"+name+"','"+phone+"','"+pwd+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',1,"+qx+")";
                DbHelperSQL.ExecuteSql(sql1);
            }
            return "2";
        }
        [WebMethod]
        public static string update(string bh, string name, string phone, string pwd, string qx,string id)
        {
            //判断编号是否重复
            if (pwd.Trim() != "")
            {
                string sql1 = "update asm_opera set name='" + bh + "',linkphone='" + phone + "',pwd='" + pwd + "',qx='" + qx + "',nickName='"+name+"' where id=" + id;
                DbHelperSQL.ExecuteSql(sql1);
                return "1";
            }
            else
            {
                string sql1 = "update asm_opera set name='" + bh + "',linkphone='" + phone + "',qx='" + qx + "',nickName='"+name+"' where id=" + id;
                DbHelperSQL.ExecuteSql(sql1);
                return "1";
            }
          
           
        }
        [WebMethod]
        public static string sear(string name,string phone)
        {
            string sql1 = "";
            if (name.Trim()!="")
            {
                sql1 += " and (name like '%"+name+"%' or nickName like '%"+name+"%')";
            }
            if (phone.Trim() != "")
            {
                sql1 += " and  linkphone like '%"+phone+"%'";
            }
            string sql = "select *,(select name from asm_role  ar where ar.id=ao.qx) roleName from asm_opera ao where ao.companyid=0 "+sql1;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dd);
            }
            return "1";
        }
        [WebMethod]
        public static string getInfo(string operaID)
        {
            string sql = "select *,(select name from asm_role  ar where ar.id=ao.qx) roleName from asm_opera ao where ao.companyid=0 and id="+operaID;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dd);
            }
            return "1";
        }
        [WebMethod]
        public static string adminDel(string id)
        {
            string sql = "delete from asm_opera where id="+id;
            int a=DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return "1";
            }
            else {
                return "2";
            }
            
        }
    }
}