using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.equipment
{
    public partial class equipmentlist : System.Web.UI.Page
    {
        private string comID = "";
        public string operaID = "";
        public string mapKey = ConfigurationManager.AppSettings["mapKey"];
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            operaID = OperUtil.Get("operaID");
            this.opera_id.Value = operaID;
            this.agentID.Value = operaID;
            this.companyId.Value = comID;
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            if (operaID != "0")
            {
                string sql = "select * from asm_opera where   appQX in (2) and companyID="+comID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                this.sel1.DataTextField = "name";
                this.sel1.DataValueField = "id";
                this.sel1.DataSource = dt;
                this.sel1.DataBind();

                string sql0 = "select * from asm_opera where companyID=" + comID + " and qx in (select roleID from asm_qx where menuID='szdls') and appQX in (1,2)";
                DataTable dt1 = DbHelperSQL.Query(sql0).Tables[0];
                this.DropDownList1.DataTextField = "name";
                this.DropDownList1.DataValueField = "id";
                this.DropDownList1.DataSource = dt1;
                this.DropDownList1.DataBind();
            }
            else {
                string sql = "select * from asm_opera where companyID=" + comID + " and appQX in (1,2)";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                this.sel1.DataTextField = "name";
                this.sel1.DataValueField = "id";
                this.sel1.DataSource = dt;
                this.sel1.DataBind();
                string sql0 = "select * from asm_opera where companyID=" + comID + " and qx in (select roleID from asm_qx where menuID='szdls') and appQX in (1,2)";
                DataTable dt1 = DbHelperSQL.Query(sql0).Tables[0];
                this.DropDownList1.DataTextField = "name";
                this.DropDownList1.DataValueField = "id";
                this.DropDownList1.DataSource = dt1;
                this.DropDownList1.DataBind();
            }
             
            string sql1 = "select * from asm_zfbhb where companyID=" + comID;
            DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
            this.hblist.DataTextField = "des";
            this.hblist.DataValueField = "id";
            this.hblist.DataSource = dd;
            this.hblist.DataBind();
            this.hblist.Items.Insert(0, new ListItem("暂不设置", "0")); //添加项
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
        public static object pause(string id)
        {
            string sql = "select * from asm_mechine where id="+id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                int a = 0;
                if (dt.Rows[0]["openStatus"].ToString() == "0")
                {
                    string update = "update asm_mechine set openStatus=1 where id="+id;
                    a=DbHelperSQL.ExecuteSql(update);
                    if (id == "68"|| id == "69")
                    {
                        string mechineInfo = RedisUtil.getMechine(id);
                        JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                        jay[0]["openStatus"] = 1;
                        
                        RedisHelper.SetRedisModel<string>(id + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                    }
                } else if (dt.Rows[0]["openStatus"].ToString() == "1")
                {
                    string update = "update asm_mechine set openStatus=0 where id=" + id;
                    a=DbHelperSQL.ExecuteSql(update);
                    if (id == "68" || id == "69")
                    {
                        string mechineInfo = RedisUtil.getMechine(id);
                        JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                        jay[0]["openStatus"] = 0;

                        RedisHelper.SetRedisModel<string>(id + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                    }
                }
                if (a > 0)
                {
                    Util.ClearRedisMechineInfoByMechineID(id);
                    return new { code = 200, msg = "设置成功" };
                }
                else {
                    return new { code = 500, msg = "设置失败" };
                }
            }
            return new { code = 500, msg = "设置失败" };
        }

        [WebMethod]
        public static string search(string mechineID, string opera,string companyID,string agentID)
        {
            string sql = " where 1=1 and A.companyID="+companyID;
            if (mechineID != "")
            {
                sql += " and A.id in(" + mechineID + ")";
            }
            if (opera != "")
            {
                sql += " and B.name='" + opera + "'";
            }
            //if (agentID != "0")
            //{
            //    sql += " and dls='" + agentID + "'";
            //}
            string sql1 = "select A.*,B.name operaName,(select name from asm_opera where asm_opera.id=dls) dlsName from (select am.*,ac.name,amt.name mechineType, case am.statu when '0' then '正常' when '1' then '脱机' when '2' then '温度异常'   else '其他' end sta,case am.zt when '1' then '禁用' when '2' then '正常' when '3' then '过期'   else '其他' end t,(select des from asm_zfbhb where id=am.hbid) hbdes from asm_mechine am left join asm_company ac on am.companyID=ac.id  left join asm_mechineType amt on am.version=amt.id)   A left join asm_opera B on A.operaID=B.id" + sql;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
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
        [WebMethod]
        public static string setAdmin(string mechineID,string operaID)
        {
            string sql = "update  asm_mechine set operaID="+operaID+" where id="+mechineID;
            int a=DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                if (mechineID == "68" || mechineID == "69")
                {
                    string mechineInfo = RedisUtil.getMechine(mechineID);
                    JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                    jay[0]["operaID"] = operaID;
                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                }
                Util.ClearRedisMechineInfoByMechineID(mechineID);
                return "1";
            }
            else {
                return "2";
            }
        }
        [WebMethod]
        public static string setLocation(string mechineID, string jd, string wd, string address, string name,string province, string city,string distinct,string addre)
        {
            string sql = "update asm_mechine set mechineName='"+name+"',zdX='"+jd+"',zdY='"+wd+"',addres='"+ address + "',province='"+province+ "',city='"+city+ "',country='"+distinct+"' where id=" + mechineID;
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                if (mechineID == "68" || mechineID == "69")
                {
                    string mechineInfo = RedisUtil.getMechine(mechineID);
                    JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                    jay[0]["mechineName"] = name;
                    jay[0]["zdX"] = jd;
                    jay[0]["zdY"] = wd;
                    jay[0]["addres"] = address;
                    jay[0]["province"] = province;
                    jay[0]["city"] = city;
                    jay[0]["country"] = distinct;
                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                }
                Util.ClearRedisMechineInfoByMechineID(mechineID);
                return "1";
            }
            else
            {
                return "2";
            }
            
        }
        [WebMethod]
        public static string searchJWD(string mechineID)
        {
            string sql = "select * from asm_mechine where id="+mechineID;
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
        [WebMethod]
        public static object getMechineList(string companyID)
        {
            try
            {
                string sql = "select * from asm_mechine where  mechineName is not null and companyID=" + companyID ;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
                }
                return new { code = 200, db = OperUtil.DataTableToJsonWithJsonNet(dt) };
            }
            catch
            {
                return new { code = 500, msg = "系统异常" };
            }
        }
        [WebMethod]
        public static string getPath(string id)
        {
           
            string sql = "select * from asm_mechine where id=" + id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = dt.Rows[0]["videoPath"].ToString()+"||"+ dt.Rows[0]["setTem"].ToString()+"||"+ dt.Rows[0]["hbID"].ToString();
                return ss;
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]
        public static string getSoftInfo(string id)
        {

            string sql = "select * from asm_mechine where id=" + id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = dt.Rows[0]["newsoftversion"].ToString() + "||" + dt.Rows[0]["newVerCode"].ToString() + "||" + dt.Rows[0]["downUrl"].ToString() + "||" + dt.Rows[0]["updateTime"].ToString(); 
                return ss;
            }
            else
            {
                return "1";
            }
        }
        
        [WebMethod]
        public static string addPath(string id, string path,string setTem,string hbid)
        {
           
            string sql = "update asm_mechine set videoPath='" + path + "',setTem='"+ setTem + "',hbid="+hbid+" where id=" + id;
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                Util.ClearRedisMechineInfoByMechineID(id);
                if (id == "68" || id == "69")
                {
                    string mechineInfo = RedisUtil.getMechine(id);
                    JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                    jay[0]["videoPath"] = path;
                    jay[0]["setTem"] = setTem;
                    jay[0]["hbid"] = hbid;
                    RedisHelper.SetRedisModel<string>(id + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                }
                return "1";
            }
            else
            {
                return "2";
            }
        }
        [WebMethod]
        public static string addSoftUpdateBtn(string mechineID, string newsoftversion, string newVerCode, string downUrl, string updateTime)
        {

            string sql = "update asm_mechine set newsoftversion='" + newsoftversion + "',newVerCode=" + newVerCode + ",downUrl='" + downUrl + "',updateTime='" + updateTime + "' where id in(" + mechineID + ")";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                string[] idList = mechineID.Split(',');
                for (int i = 0; i < idList.Length; i++)
                {
                    if (idList[i] == "68" || idList[i] == "69")
                    {
                        string mechineInfo = RedisUtil.getMechine(idList[i]);
                        JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                        jay[0]["newsoftversion"] = newsoftversion;
                        jay[0]["newVerCode"] = newVerCode;
                        jay[0]["downUrl"] = downUrl;
                        jay[0]["updateTime"] = updateTime;
                        RedisHelper.SetRedisModel<string>(idList[i] + "_mechineInfoSet", JsonConvert.SerializeObject(jay));
                    }
                    else
                    {
                       
                    }

                }
                
                return "1";
            }
            else
            {
                return "2";
            }
        }
        
        [WebMethod]
        public static string okDls(string mechineID,string operaID)
        {
            string sql = "update  asm_mechine set dls=" + operaID + " where id=" + mechineID;
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                Util.ClearRedisMechineInfoByMechineID(mechineID);
                if (mechineID == "68" || mechineID == "69")
                {
                    string mechineInfo = RedisUtil.getMechine(mechineID);
                    JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                    jay[0]["dls"] = operaID;
                    
                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                }
                return "1";
            }
            else
            {
                return "2";
            }
        }
        [WebMethod]
        public static string yz(string operaID)
        {
            string sql = "select * from asm_opera where id="+operaID+"";
            DataTable dt= DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return "2";
            }
            else {
                return "2";
            }
        }
    }
}