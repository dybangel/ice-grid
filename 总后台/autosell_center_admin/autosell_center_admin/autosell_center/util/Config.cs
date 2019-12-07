using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace autosell_center.util
{
    public class Config
    {
        public static int pageSize = 50;

        public static DataTable getPageDataTable(string sort, string sql1, int startIndex, int endIndex)
        {
            string sql = "select * from(select Row_Number() over(" + sort + ") as Row,T.* from (" + sql1 + ")T ) TT where TT.Row between " + startIndex + "  and " + endIndex + "   ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }
    }
}