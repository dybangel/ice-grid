using autosell_center.cls;
using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.product
{
    public partial class xstjlist : System.Web.UI.Page
    {
        private string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            this._operaID.Value = OperUtil.Get("operaID");
            this.companyId.Value = comID;
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            
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
        public static object getList(string companyID,string mechineID,string keyword,string type,string start,string end, string pageCurrentCount)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(mechineID))
            {
                sql1 += " and a.mechineID in("+mechineID+")";
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sql1 += " and (p.proName like '%"+keyword+ "%' or p.bh like '%"+keyword+"%')";

            }
            if (type!="0")
            {
                sql1 += " and a.type="+type;
            }
            if (!string.IsNullOrEmpty(start))
            {
                sql1 += " and createTime>'" + start+"'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql1 += " and createTime<'" + end+"'";
            }
            string sql = "select a.*,(select bh from asm_mechine where id=a.mechineID)bh,p.proName from  asm_xstj a left join asm_product p on a.productID=p.productID where  a.companyID="+companyID+sql1;
            int startIndex = (int.Parse(pageCurrentCount) - 1) * Config.pageSize + 1;
            int endIndex = int.Parse(pageCurrentCount) * Config.pageSize;

            DataTable dt = Config.getPageDataTable("order by T.id desc", sql, startIndex, endIndex);
            DataTable da = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                double d = double.Parse((da.Rows.Count / double.Parse(Config.pageSize.ToString())).ToString());
                return new { code=200,db=OperUtil.DataTableToJsonWithJsonNet(dt),count= Math.Ceiling(d) };
            }
            return new { code = 500};
        }
        [WebMethod]
        public static object getMechineList(string companyID)
        {
            try
            {
                string sql = "select * from asm_mechine where  mechineName is not null and companyID=" + companyID;
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
        public static object saveActivity(string mechineList, string type, string start, string end, string timeSpan, string productID, string price0, string price1, string price2, string price3, string companyID, string hours, string buyCount)
        {
            if (string.IsNullOrEmpty(mechineList))
            {
                return new { code = 500, msg = "请选择机器" };
            }
            if (string.IsNullOrEmpty(start))
            {
                return new { code = 500, msg = "请选择开始时间" };
            }
            if (string.IsNullOrEmpty(end))
            {
                return new { code = 500, msg = "请选择结束时间" };
            }
            if (type == "1" && string.IsNullOrEmpty(timeSpan))
            {
                return new { code = 500, msg = "请按照要求填写时间段" };
            }

            if (type == "1")//按照周期
            {
                string[] timeArr = timeSpan.Split(',');
                for (int i = 0; i < timeArr.Length; i++)
                {
                    if (timeArr[i].Length != 11)//格式01:30-02:30
                    {
                        return new { code = 500, msg = "时间段格式不正确,请检查是否带空格或多输" };
                    }
                    else
                    {
                        string[] pointArr = timeArr[i].Split('-');
                        if (pointArr.Length != 2)
                        {
                            return new { code = 500, msg = "时间段格式不正确，请检查是否是英文:" };
                        }
                        else
                        {
                            if (pointArr[0].Length != 5 || pointArr[1].Length != 5)
                            {
                                return new { code = 500, msg = "时间段格式不正确，确保每个时间点是5位" };
                            }
                        }
                    }
                }
                //检测价格设置是否正确
                try
                {
                    string[] price0Arr = price0.Split(',');
                    string[] price1Arr = price1.Split(',');
                    string[] price2Arr = price2.Split(',');
                    string[] price3Arr = price3.Split(',');
                    if (price0Arr.Length != timeArr.Length || price1Arr.Length != timeArr.Length || price2Arr.Length != timeArr.Length || price3Arr.Length != timeArr.Length)
                    {
                        return new { code = 500, msg = "价格设置不正确，如果不开启设置0 逗号隔开" };
                    }
                    for (int n = 0; n < timeArr.Length; n++)
                    {
                        try
                        {
                            double.Parse(price0Arr[n]);
                            double.Parse(price1Arr[n]);
                            double.Parse(price2Arr[n]);
                            double.Parse(price3Arr[n]);
                        }
                        catch
                        {
                            return new { code = 500, msg = "输入的价格不正确" };
                        }
                    }


                }
                catch
                {

                }
            }
            else if (type == "2")//按照阶段特价
            {
                if (string.IsNullOrEmpty(price0))
                {
                    return new { code = 500, msg = "零售价不能为空" };
                }
                else
                {
                    try
                    {
                        double.Parse(price0);
                    }
                    catch
                    {
                        return new { code = 500, msg = "零售价设置不正确" };
                    }
                }
                if (string.IsNullOrEmpty(price1))
                {
                    price1 = "0";
                }
                else
                {
                    try
                    {
                        double.Parse(price1);
                    }
                    catch
                    {
                        return new { code = 500, msg = "普通会员价设置不正确" };
                    }
                }
                if (string.IsNullOrEmpty(price2))
                {
                    price2 = "0";
                }
                else
                {
                    try
                    {
                        double.Parse(price2);
                    }
                    catch
                    {
                        return new { code = 500, msg = "白银会员价设置不正确" };
                    }
                }
                if (string.IsNullOrEmpty(price3))
                {
                    price3 = "0";
                }
                else
                {
                    try
                    {
                        double.Parse(price3);
                    }
                    catch
                    {
                        return new { code = 500, msg = "黄金会员价设置不正确" };
                    }
                }
                timeSpan = "";
            }


            string[] mechineArr = mechineList.Split(',');
            if (type == "2")
            {
                for (int k = 0; k < mechineArr.Length; k++)
                {
                    //先删除之前的记录
                    string del = "delete from asm_xstj where productID=" + productID + " and mechineID=" + mechineArr[k];
                    DbHelperSQL.ExecuteSql(del);
                    string insert = "insert into asm_xstj(productID,mechineID,type,startTime,endTime,timeSpan,price0,price1,price2,price3,companyID,createTime,hours,buycount) values(" + productID + "," + mechineArr[k] + "," + type + ",'" + start + "','" + end + "','" + timeSpan + "','" + price0 + "','" + price1 + "','" + price2 + "','" + price3 + "','" + companyID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + hours + "','" + buyCount + "')";
                    DbHelperSQL.ExecuteSql(insert);
                    string sql1 = "select * from asm_xstj where mechineID=" + mechineArr[k] + "  order by timeSpan desc"; ;
                    DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (d1.Rows.Count > 0)
                    {
                        RedisHelper.Remove(mechineArr[k] + "_XSTJList");
                        RedisHelper.SetRedisModel<string>(mechineArr[k] + "_xstj", OperUtil.DataTableToJsonWithJsonNet(d1), new TimeSpan(1, 0, 0));
                    }

                }

            }
            else if (type == "1")
            {
                for (int k = 0; k < mechineArr.Length; k++)
                {
                    //先删除之前的记录
                    string del = "delete from asm_xstj where productID=" + productID + " and mechineID=" + mechineArr[k];
                    DbHelperSQL.ExecuteSql(del);
                    string[] price0Arr = price0.Split(',');
                    string[] price1Arr = price1.Split(',');
                    string[] price2Arr = price2.Split(',');
                    string[] price3Arr = price3.Split(',');
                    for (int m = 0; m < price0Arr.Length; m++)
                    {
                        RedisHelper.SetRedisModel<string>(mechineArr[k] + "_productInfo", "", new TimeSpan(0, 0, 0));
                        string insert = "insert into asm_xstj(productID,mechineID,type,startTime,endTime,timeSpan,price0,price1,price2,price3,companyID,createTime,hours,buycount) values(" + productID + "," + mechineArr[k] + "," + type + ",'" + start + "','" + end + "','" + timeSpan.Split(',')[m] + "','" + price0Arr[m] + "','" + price1Arr[m] + "','" + price2Arr[m] + "','" + price3Arr[m] + "','" + companyID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + hours + "','" + buyCount + "')";
                        DbHelperSQL.ExecuteSql(insert);

                        string sql1 = "select * from asm_xstj where mechineID=" + mechineArr[k] + "  order by timeSpan desc";
                        DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                        if (d1.Rows.Count > 0)
                        {
                            RedisHelper.Remove(mechineArr[k] + "_XSTJList");
                            RedisHelper.SetRedisModel<string>(mechineArr[k] + "_xstj", OperUtil.DataTableToJsonWithJsonNet(d1), new TimeSpan(1, 0, 0));
                        }
                    }

                }
            }
            RedisHelper.SetRedisModel<string>(companyID + "_xstj", "", new TimeSpan(0, 0, 5));
            return new { code = 200, msg = "设置成功" };
        }
        [WebMethod]
        public static object getActivity(string productID,string mechineID)
        {
            string sql = "SELECT STUFF((SELECT ','+convert(varchar,mechineID) FROM  asm_xstj  where productID=" + productID + " for xml path('')),1,1,'') mechineID  ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string sql1 = "select * from asm_xstj where productID="+ productID+" and mechineID="+mechineID;
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                if (d1.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["mechineID"].ToString()))
                {
                    string price0 = "";
                    string price1 = "";
                    string price2 = "";
                    string price3 = "";
                    string timeSpan = "";
                    for (int i = 0; i < d1.Rows.Count; i++)
                    {
                        price0 += d1.Rows[i]["price0"].ToString() + ",";
                        price1 += d1.Rows[i]["price1"].ToString() + ",";
                        price2 += d1.Rows[i]["price2"].ToString() + ",";
                        price3 += d1.Rows[i]["price3"].ToString() + ",";
                        timeSpan += d1.Rows[i]["timeSpan"].ToString() + ",";
                    }
                    return new
                    {
                        code = 200,
                        db = dt.Rows[0]["mechineID"].ToString(),
                        type = d1.Rows[0]["type"].ToString(),
                        start = d1.Rows[0]["startTime"].ToString(),
                        end = d1.Rows[0]["endTime"].ToString(),
                        timeSpan = timeSpan.Substring(0, timeSpan.Length - 1),
                        price0 = price0.Substring(0, price0.Length - 1),
                        price1 = price1.Substring(0, price1.Length - 1),
                        price2 = price2.Substring(0, price2.Length - 1),
                        price3 = price3.Substring(0, price3.Length - 1),
                        hours = d1.Rows[0]["hours"].ToString(),
                        buycount = d1.Rows[0]["buycount"].ToString()
                    };
                }
            }
            return new { code = 500 };
        }
        [WebMethod]
        public static object deleteALL(string id, string companyID)
        {

            string sqlS = "select * from asm_xstj where id in(" + id + ")";
            DataTable ds = DbHelperSQL.Query(sqlS).Tables[0];
            for (int i=0;i<ds.Rows.Count;i++)
            {
                string mechineID = ds.Rows[i]["mechineID"].ToString();
                RedisHelper.Remove(mechineID + "_XSTJList");
                RedisHelper.SetRedisModel<string>(mechineID + "_xstj", "", new TimeSpan(0, 0, 0));
            }
            RedisHelper.SetRedisModel<string>(ds.Rows[0]["mechineID"].ToString() + "_productInfo", "", new TimeSpan(0, 0, 0));
            string sql = "delete from asm_xstj where id in("+id+")";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return new { code = 200, msg = "删除成功" };
            }
            return new { code = 500, msg = "删除失败" };

        }
        [WebMethod]
        public static object delete(string id,string companyID)
        {
            RedisHelper.SetRedisModel<string>(companyID + "_xstj","", new TimeSpan(0, 0, 0));
            string sqlS = "select * from asm_xstj where id="+id;
            DataTable ds = DbHelperSQL.Query(sqlS).Tables[0];
            RedisHelper.Remove(ds.Rows[0]["mechineID"].ToString() + "_XSTJList");
            RedisHelper.SetRedisModel<string>(ds.Rows[0]["mechineID"].ToString() + "_productInfo", "", new TimeSpan(0, 0, 0));
            string sql = "delete from asm_xstj where id="+id;
            int a= DbHelperSQL.ExecuteSql(sql);
            if (a>0)
            {
                return new { code = 200, msg = "删除成功" };
            }
            return new { code = 500, msg = "删除失败" };
        }
    }
}