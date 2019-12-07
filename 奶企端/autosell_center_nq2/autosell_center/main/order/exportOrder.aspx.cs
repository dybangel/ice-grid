using autosell_center.util;
using Consumer.cls;
using DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace autosell_center.main.order
{
    public partial class exportOrder : System.Web.UI.Page
    {
        public string comID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comID = OperUtil.Get("companyID");
            if (string.IsNullOrEmpty(comID))
            {
                Response.Write("<script>alert('您尚未登录或已长时间未进行操作，请重新登录!');top.location.href='../../index.aspx';</script>");
                return;
            }
            this.companyID.Value = comID; 
            this.agentID.Value = OperUtil.Get("operaID");
        }

        protected void export_Click(object sender, EventArgs e)
        {
             
            
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
        public static object exportData(string companyID,string str)
        {
            Random rd = new Random();
            string bh = companyID + DateTime.Now.ToString("yyMMddHHmm")+rd.Next(999);
            JArray jArray = (JArray)JsonConvert.DeserializeObject(str);
            if (jArray.Count > 0)
            {
                List<string> phoneList = new List<string>();
                List<string> list = new List<string>();
                for (int i = 0; i < jArray.Count; i++)
                {
                    int random = rd.Next(100000, 999999);
                    string phone = jArray[i]["phone"].ToString();
                    phoneList.Add(phone);
                    string productCode = jArray[i]["productCode"].ToString();
                    string productPrice = jArray[i]["productPrice"].ToString();
                    string zq = jArray[i]["zq"].ToString();
                    string tjr = jArray[i]["tjr"].ToString();
                    string bz = jArray[i]["bz"].ToString();
                    string orderCode = random.ToString();
                    //检验产品是否存在
                    string sqlp = "select * from  asm_product where bh='"+productCode+"'";
                    DataTable dt= DbHelperSQL.Query(sqlp).Tables[0];
                    if (dt.Rows.Count <= 0)
                    {
                        return new { code = 500, msg = "手机号"+phone+"订购的产品条码不存在" };
                    }
                    string sql = "insert into asm_dgOrder(phone,productCode,productPrice,zq,tjr,orderCode,bz,createTime,bh,companyID)values('" + phone + "','" + productCode + "','" + productPrice + "','" + zq + "','" + tjr + "','" + orderCode + "','" + bz + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + bh + "','"+ companyID + "')";
                    list.Add(sql);

                }
               
                int a = DbHelperSQL.ExecuteSqlTran(list);
                if (a > 0)
                {
                    string sql = "select * from asm_dgOrder where bh='"+bh+"'";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    for (int i=0;i<dt.Rows.Count;i++)
                    {
                        //发送短信
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                //需要异步执行的操作比如发邮件、发短信等
                                string phone = dt.Rows[i]["phone"].ToString();
                                string code = dt.Rows[i]["orderCode"].ToString();
                                OperUtil.sendMessage5(phone,code);
                            }
                            catch
                            {
                                //不做任何处理，防止线程异常导致程序崩溃
                            }
                        }
                       ).Start();
                    }
                   

                    return new { code = 200, msg = "订单导入成功" };
                }
                else
                {
                    return new { code = 500, msg = "订单导入失败" };
                }
            }
            else {
                return new { code = 500, msg = "未读取到excel数据" };
            }
        }
        /// <summary>
        /// Hashtable 方法
        /// </summary> 验证数组元素是否重复
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool IsRepeat(string[] array)
        {
            Hashtable ht = new Hashtable();
            for (int i = 0; i < array.Length; i++)
            {
                if (ht.Contains(array[i]))
                {
                    return true;
                }
                else
                {
                    ht.Add(array[i], array[i]);
                }
            }
            return false;
        }

    }
}