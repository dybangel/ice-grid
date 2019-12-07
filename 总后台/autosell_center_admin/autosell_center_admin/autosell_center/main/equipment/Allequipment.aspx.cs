using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Web.Services;

namespace autosell_center.main.equipment
{
    public partial class Allequipment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string search(string startTime, string endTime, string bh, string type,string name, string pageCurrentCount)
        {
            string sql = " where 1=1";
            if (startTime != "")
            {
                sql += " and am.validateTime>'" + startTime + "'";
            }
            if (endTime != "")
            {
                sql += " and am.validateTime<'" + endTime + "'";
            }
            if (bh != "")
            {
                sql += " and am.bh='" + bh + "'";
            }
            if (type != "0")
            {
                sql += " and am.zt=" + type;
            }
            if (name!="")
            {
                sql += " and ac.name='" + name+"'";
            }
            string sql1 = "select am.*,ac.name,amt.name mechineType,case am.statu when '0' then '正常' when '1' then '脱机' when '2' then '温度异常'   else '其他' end sta,case am.zt when '1' then '禁用' when '2' then '正常' when '3' then '过期'   else '其他' end t  from asm_mechine am left join asm_company ac on am.companyID=ac.id  left join asm_mechineType amt on am.version=amt.id " + sql;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql1, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {

                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                string ss = Math.Ceiling(d) + "@" + OperUtil.DataTableToJsonWithJsonNet(dt);

                //string ss =OperUtil.DataTableToJsonWithJsonNet(da);
                return ss;
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]
        public static string getCompanyList()
        {
            string sql = "select * from asm_company";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss = OperUtil.DataTableToJsonWithJsonNet(dt);
                return ss;
            }
            else {
                return "1";
            }
        }
        [WebMethod]
        public static string getPath(string id)
        {
            string sql = "select * from asm_mechine where id="+id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string ss =dt.Rows[0]["videoPath"].ToString();
                return ss;
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]
        public static string addPath(string id,string path)
        {
            string sql = "update asm_mechine set videoPath='"+path+"' where id=" + id;
            int a= DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                if (id == "68" || id == "69")
                {
                    string mechineInfo = RedisUtil.getMechine(id);
                    JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                    jay[0]["videoPath"] = path;
                    RedisHelper.SetRedisModel<string>(id + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                }
                return "1";
            }
            else {
                return "2";
            }
        }
    }
}