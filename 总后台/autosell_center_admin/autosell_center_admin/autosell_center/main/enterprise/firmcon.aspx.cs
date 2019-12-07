using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.enterprise
{
    public partial class firmcon : System.Web.UI.Page
    {
        public string company_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            company_id = Request.QueryString["companyID"].ToString();
            this.companyID.Value = Request.QueryString["companyID"].ToString();
        }
        [WebMethod]
        public static string search()
        {
            string sql = "select am.*,amt.id mechineType ,amt.name from asm_mechine am left join asm_mechineType amt on am.[version]=amt.id where companyID is null";
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
        public static string getMechineByType(string type)
        {
            string sql = "select * from asm_mechine where companyID is null and [version]=" + type;
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
        public static string okbtn(string id,string companyID)
        {
            if (id.Trim()!="")
            {
                string sql = "update asm_mechine set companyID="+companyID+ ",statu=2 where id in (" + id+") ";
                int a= DbHelperSQL.ExecuteSql(sql);
                if (a > 0)
                {
                    if (id == "68" || id == "69")
                    {
                        string mechineInfo = RedisUtil.getMechine(id);
                        JArray jay = RedisUtil.DeserializeObject(mechineInfo);

                        jay[0]["companyID"] = companyID;
                        jay[0]["statu"] =2;
                        RedisHelper.SetRedisModel<string>(id + "_mechineInfoSet", JsonConvert.SerializeObject(jay));

                    }
                    return "1";
                }
                else {
                    return "2";
                }
            }
           
            return "2";
        }
    }
}