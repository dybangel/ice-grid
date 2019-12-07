using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Consumer.main
{
    public partial class placeorderok : System.Web.UI.Page
    {
        public string pszq = "";//配送周期
        public string qsDate = "";//起订日期
        public string zdDate = "";//止定日期
        public string psStr = "";//配送方式详细
        public string psfs = "";//配送方式
        public string selDate = "";//具体的配送日期
        public string orderNO = "";
        public string productID = "";
        public string mechineID = "";
        public string createTime = "";//订单创建时间
        public string yhfs = "";//优惠方式
        public string memberID = "";
        public string totalMoney = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OperUtil.getCooki("vshop_openID") != "0")
            {
                memberID = Util.getMemberID();

            }
            else
            {
                string userAgent = Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    Response.Redirect("WXCallback.aspx?companyID=" + OperUtil.getCooki("companyID"));
                    return;
                }
            }

            pszq = Request.QueryString["pszq"].ToString();
            qsDate= Request.QueryString["qsDate"].ToString().Replace("/","-") ;
            zdDate= Request.QueryString["zdDate"].ToString() ;
            psStr= Request.QueryString["psStr"].ToString() ;
            psfs= Request.QueryString["psfs"].ToString(); 
            selDate= Request.QueryString["selDate"].ToString();
            orderNO = Request.QueryString["orderNO"].ToString(); ;
            productID = Request.QueryString["productID"].ToString();
            mechineID = Request.QueryString["mechineID"].ToString();
            createTime = Request.QueryString["createTime"].ToString();
            yhfs = Request.QueryString["yhfs"].ToString();

            //验证单号是否存在没有添加
            if (!IsPostBack)
            {
                string sql = "select * from asm_order where orderNO='"+orderNO+"'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count<=0)
                {
                    string sqlMechine = "select * from asm_mechine where id='"+mechineID+"'";
                    DataTable dm = DbHelperSQL.Query(sqlMechine).Tables[0];
                    if (dm.Rows.Count>0)
                    {
                        //计算总金额
                        string sql11 = "select * from asm_product where productID=" + productID;
                        DataTable dd = DbHelperSQL.Query(sql11).Tables[0];
                        if (dd.Rows.Count > 0)
                        {
                            totalMoney = (double.Parse(dd.Rows[0]["price2"].ToString()) * double.Parse(pszq)).ToString("f2");
                            if (yhfs.IndexOf('折') > -1)
                            {
                                string zk = yhfs.Replace("打", "").Replace("折", "");
                                totalMoney = (double.Parse(totalMoney) * double.Parse(zk) / 10).ToString("f2");
                            }
                        }

                        //添加订单
                        string sql1 = "insert into asm_order(mechineID,productID,memberID,totalNum,consumeNum,syNum,createTime,zq,qsDate,zdDate,psStr,psfs,orderNO,fkzt,zt,qhAddress,totalMoney,yhfs)"
                            + "values('" + mechineID + "','" + productID + "','" + memberID + "'," + pszq + ",0," + pszq + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + pszq + ",'" + qsDate + "','" + zdDate + "','" + psStr + "','" + psfs + "','" + orderNO + "',0,0,'"+dm.Rows[0]["addres"].ToString() +"',"+totalMoney+",'"+yhfs+"')";
                        int a= DbHelperSQL.ExecuteSql(sql1);
                        if(a>0)
                        {
                            //订单添加成功  此处没有往订单明细表生成记录
                        }
                    }
                  
                }
            }
        }
    }
}