using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using ZTreeDemo;

namespace ZTreeDemo
{
    /// <summary>
    /// GetDatesHandler 的摘要说明
    /// </summary>
    public class GetDatesHandler : IHttpHandler
    {

        int? pId = null;
        public string companyID;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "Text/plain";
            if (context.Request["id"] != null && !string.IsNullOrEmpty(context.Request["id"].ToString()))
                pId = Convert.ToInt32(context.Request["id"]);
            if (context.Request["companyID"] != null && !string.IsNullOrEmpty(context.Request["companyID"].ToString()))
            {
                companyID = context.Request["companyID"].ToString();
            }
            context.Response.Write(GetDate(companyID));
            context.Response.End();
        }

        public  string GetDate(string companyID)
        {
            #region 生成数据
          
            List<Node> nodes = new List<Node>();

            string sql = "select * from asm_mechine where companyID='"+companyID+ "' and mechineName is not null";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];

            string sqlp = "select * from asm_product where companyID='"+companyID+ "' and dstype in(1,3) and is_del=0";
            DataTable dd = DbHelperSQL.Query(sqlp).Tables[0];
            if (dt.Rows.Count>0)
            {
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    Node pN = new Node();
                    pN.id = dt.Rows[i]["id"].ToString();
                    pN.pId = "0";
                    pN.name = dt.Rows[i]["mechineName"].ToString();
                    pN.open = "false";
                    pN.level = 1;
                    nodes.Add(pN);
                    //接着循环加商品
                    if (dd.Rows.Count>0)
                    {
                        for (int j=0;j<dd.Rows.Count;j++)
                        {
                            Node pN2 = new Node();
                            pN2.id = dt.Rows[i]["id"].ToString()+"_"+dd.Rows[j]["productID"].ToString();
                            pN2.pId =dt.Rows[i]["id"].ToString();
                            pN2.name = dd.Rows[j]["proName"].ToString();
                            pN2.open = "false";
                            pN2.url = "";//跳转路径
                            pN2.target = "_self";
                            pN2.level = 2;
                            pN2.type = "get";
                            nodes.Add(pN2);
                        }
                    }
                }
              
            }
           
             
            #endregion

            //用于异步加载的数据
           

            JavaScriptSerializer jsSer = new JavaScriptSerializer();
            jsSer.MaxJsonLength = Int32.MaxValue;
            if (pId == null)
            {
                return jsSer.Serialize(nodes);
            }
            else {
                return "";
            }
                
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}