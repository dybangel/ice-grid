using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;

namespace autosell_center.util
{
    //redis读写的部分通用方法
    public class RedisUtil
    {

        public static string getMechine(string mechineID)
        {
            string mechineInfo = null;
            try
            {
                mechineInfo = RedisHelper.GetRedisModel<string>(mechineID + "_mechineInfoSet");
            }
            catch
            {
            }

            if (string.IsNullOrEmpty(mechineInfo))
            {
                string sql = "select am.*,ac.p1,ac.p2,ac.p3,ac.p4,ac.p5,ac.p6,ac.p7,ac.p8,ac.p9,ac.p10,am.setTem,'' videoListNo,'' productTypeNo,'' androidProductNo,'' priceSwitch from asm_mechine am left join asm_company ac on am.companyID=ac.id where am.id='" + mechineID + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {

                    RedisHelper.SetRedisModel<string>(mechineID + "_mechineInfoSet", OperUtil.DataTableToJsonWithJsonNet(dt));
                    mechineInfo = OperUtil.DataTableToJsonWithJsonNet(dt);
                }
            }

            return mechineInfo;
        }

        public static JArray DeserializeObject(string mechineInfo)
        {
            if (!String.IsNullOrEmpty(mechineInfo))
            {
                JArray jArray = (JArray)JsonConvert.DeserializeObject(mechineInfo);
                return jArray;
            }
            return null;
        }
       
      
    }
}