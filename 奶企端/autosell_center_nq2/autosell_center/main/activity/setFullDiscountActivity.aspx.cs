using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZTreeDemo;

namespace autosell_center.main.activity
{
    public partial class setFullDiscountActivity : System.Web.UI.Page
    {
        public string comID = "";
        public string activityID = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            comID = OperUtil.Get("companyID");

            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
          
            this._activityID.Value = Request.QueryString["activityID"].ToString();
            this.companyID.Value = comID;
            //获取选中的列表
            string sql = "select * from asm_activity_fulldiscount_detail  where activityID='" + this._activityID.Value+"' and companyID='"+companyID.Value+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            
            if (dt.Rows.Count>0)
            {
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    this.menu_id.Value += dt.Rows[i]["productTypeID"].ToString() + "_" + dt.Rows[i]["productID"].ToString()+"-";
                }
                this.menu_id.Value = this.menu_id.Value.Substring(0,this.menu_id.Value.Length-1);
            }
        }
        [WebMethod]
        public static string qxChoose(string id,string companyID,string activityID)
        {
            string[] idArr = id.Split(',');
            if (idArr.Length>0)
            {
                //首先删除该企业下该活动的参加的活动
                string sqlD = "delete from asm_activity_fulldiscount_detail where companyID='" + companyID+"' and activityID='"+activityID+"'";
                DbHelperSQL.ExecuteSql(sqlD);
                for (int i=0;i<idArr.Length-1;i++)
                {
                    string productTypeID = idArr[i].Split('_')[0];
                    string productID = idArr[i].Split('_')[1];
                    string sql = "insert into asm_activity_fulldiscount_detail(companyID,productTypeID,productID,type,activityID) values('" + companyID+"','"+ productTypeID + "','"+productID+"','1','"+activityID+"')";
                    
                    DbHelperSQL.ExecuteSql(sql);
                }
            }
          
            return "1";
        }
        //获取零售商品列表
        [WebMethod]
        public static object getProdctList( string companyID)
        {
            #region 生成数据
            List<Node> nodes = new List<Node>();

            string sql = "select * from asm_protype where  typeName is not null";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];

            string sqlp = "select * from asm_product where companyID='"+companyID+ "' and dstype in(2,3) and is_del=0";
            DataTable dd = DbHelperSQL.Query(sqlp).Tables[0];
            if (dt.Rows.Count>0)
            {
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    Node pN = new Node();
                    pN.id = dt.Rows[i]["productTypeID"].ToString();
                    pN.pId = "0";
                    pN.name = dt.Rows[i]["typeName"].ToString();
                    pN.open = "false";
                    pN.level = 1;
                    nodes.Add(pN);
                    //接着循环加商品
                    if (dd.Rows.Count>0)
                    {
                        for (int j=0;j<dd.Rows.Count;j++)
                        {
                            if (dt.Rows[i]["productTypeID"].ToString().Equals(dd.Rows[j]["protype"].ToString())) {
                                Node pN2 = new Node();
                                pN2.id = dt.Rows[i]["productTypeID"].ToString() + "_" + dd.Rows[j]["productID"].ToString();
                                pN2.pId = dt.Rows[i]["productTypeID"].ToString();
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
              
            }


            return nodes;
           

            #endregion

            //用于异步加载的数据




        }
    }
}