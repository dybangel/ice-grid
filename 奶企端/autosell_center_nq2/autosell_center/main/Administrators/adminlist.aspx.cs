using autosell_center.util;
using Consumer.cls;
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

namespace autosell_center.main.Adminlist
{
    public partial class adminlist : System.Web.UI.Page
    {
        private string comID = "";
        private string operaID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this.companyID.Value = comID;
            operaID = OperUtil.Get("operaID");
            if (string.IsNullOrEmpty(comID)||string.IsNullOrEmpty(operaID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
           
            this.operID.Value = operaID;
            string sql = "select * from asm_role where companyID="+comID;
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
        public static string getMaxBH(string companyID)
        {
           
            string sql = "select  count(*) bh from [dbo].[asm_opera] where companyID='"+companyID+"'";
            DataTable d1= DbHelperSQL.Query(sql).Tables[0];
            string sql1 = "select code from asm_company where id='"+companyID+"'";
            DataTable d2= DbHelperSQL.Query(sql1).Tables[0];
            string str = "";
            if (d2.Rows.Count>0&&!string.IsNullOrEmpty(d2.Rows[0]["code"].ToString()))
            {
                str = d2.Rows[0]["code"].ToString();
            }
            str = str + d1.Rows[0]["bh"].ToString().PadLeft(3,'0');
            return str;
        }
        [WebMethod]
        public static string add(string bh,string name,string phone,string pwd,string qx,string companyID,string operaID,string appqx)
        {
            //判断编号是否重复
            string sql = "select * from asm_opera where name='"+bh+"' and companyID='"+companyID+"'";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
           
            if (dd.Rows.Count > 0)
            {
                return "1";
            }
            else {
                string sql1 = "insert into asm_opera(name,nickName,linkphone,pwd,createTime,statu,qx,companyID,agentID,appQX) values('" + bh+"','"+name+"','"+phone+"','"+pwd+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',1,"+qx+",'"+ companyID + "','"+operaID+"','"+appqx+"')";
                DbHelperSQL.ExecuteSql(sql1);

            }
            return "2";
        }
        [WebMethod]
        public static string update(string bh, string name, string phone, string pwd, string qx,string id,string appqx)
        {
          
            //判断编号是否重复
            if (pwd.Trim() != "")
            {
                string sql1 = "update asm_opera set name='" + bh + "',linkphone='" + phone + "',pwd='" + pwd + "',qx='" + qx + "',nickName='"+name+ "',appQX='"+appqx+"' where id=" + id;
                DbHelperSQL.ExecuteSql(sql1);
                
                return "1";
            }
            else
            {
                string sql1 = "update asm_opera set name='" + bh + "',linkphone='" + phone + "',qx='" + qx + "',nickName='"+name+ "',appQX='" + appqx + "'  where id=" + id;
                DbHelperSQL.ExecuteSql(sql1);
            
                return "1";
            }
          
           
        }
        [WebMethod]
        public static string sear(string name,string phone,string companyID,string operaID)
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
            //if (operaID!="0")
            //{
            //    sql1 += " and agentID="+operaID;
            //}
            string sql = "select *,(select name from asm_role  ar where ar.id=ao.qx) roleName from asm_opera ao where ao.companyid="+companyID+" "+sql1;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dd);
            }
           
            return "1";
        }
        [WebMethod]
        public static string getInfo(string operaID,string companyID)
        {
           
            string sql = "select *,(select name from asm_role  ar where ar.id=ao.qx) roleName from asm_opera ao where ao.companyid="+companyID+" and id="+operaID;
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