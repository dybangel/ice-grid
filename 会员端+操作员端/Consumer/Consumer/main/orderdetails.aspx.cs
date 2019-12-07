using autosell_center.util;
using Consumer.cls;
using DBUtility;
using System;
using System.Data;
using System.Web;
using System.Web.Services;

namespace Consumer.main
{
    public partial class orderdetails : System.Web.UI.Page
    {
        public string pszq = "";//配送周期
        public string qsDate = "";//起订日期
        public string zdDate = "";//止定日期
        public string psStr = "";//配送方式详细
        public string psfs = "";//配送方式
        public string selDate = "";//具体的配送日期
        public string orderNO = "0";
        public string productID = "";
        public string mechineID = "";//机器号
        public string qhDate = "每日早8:30以后";//取货时间
        public string qhAddress = "";
        public string createTime = "";
        public string fkzt = "0";
        public string productName = "";
        public string description = "";
        public string totalMoney = "0";
        public string memberID="";
        public double half_price = 0;
        public string yzsDate = "";
        public string sxDate = "";//失效日期
        public string wcDate = "";//完成日期
        public string dqhDate = "";
        public string yhfs = "";//优惠方式
        public string httpImgUrl = "";
        public string phone = "";//客服电话
        public string syNum = "0";
        public string headURL = "";
        public string companyName = "";
        public string ye = "0";
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
            this._companyID.Value = OperUtil.getCooki("companyID");
            this.member_ID.Value = memberID;
            try
            {
                //获取会员信息
                string sqlM = "select * from asm_member where id="+memberID;
                
                DataTable dm=DbHelperSQL.Query(sqlM).Tables[0];
                headURL = dm.Rows[0]["headurl"].ToString();
                ye = dm.Rows[0]["AvailableMoney"].ToString();
                string sql1 = "select * from asm_company where id=" + this._companyID.Value;
                DataTable d1 = DbHelperSQL.Query(sql1).Tables[0];
                companyName = d1.Rows[0]["name"].ToString();
                //订单生成
                pszq = Request.QueryString["pszq"].ToString();
                qsDate = Request.QueryString["qsDate"].ToString().Replace("/", "-");
                zdDate = Request.QueryString["zdDate"].ToString();
                psStr = Request.QueryString["psStr"].ToString();
                psfs = Request.QueryString["psfs"].ToString();
                selDate = Request.QueryString["selDate"].ToString();
                orderNO = Request.QueryString["orderNO"].ToString();
                productID = Request.QueryString["productID"].ToString();
                mechineID = Request.QueryString["mechineID"].ToString();
                createTime = Request.QueryString["createTime"].ToString();
                yhfs = Request.QueryString["yhfs"].ToString();
                this.mechine_id.Value = mechineID;
                this.product_id.Value = productID;
                this._pszq.Value = pszq;
                this._qsDate.Value = qsDate;
                this._zdDate.Value = zdDate;
                this._psStr.Value = psStr;
                this._psfs.Value = psfs;
                this._selDate.Value = selDate;
                this._orderNO.Value = orderNO;
                this._createTime.Value = createTime;
                this._fkzt.Value = fkzt;
                this._yhfs.Value = yhfs;
                syNum = pszq;
                string trxid = ConvertDateTimeToInt(DateTime.Now).ToString() + mechineID;
                this._trxid.Value = trxid;
                string sql = "select * from asm_mechine where id=" + mechineID;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    qhAddress = dt.Rows[0]["addres"].ToString();
                }
                string sqlkf = "select * from asm_opera where id="+dt.Rows[0]["operaID"].ToString();
                phone = DbHelperSQL.Query(sqlkf).Tables[0].Rows[0]["linkphone"].ToString();
                //初始化商品
                string sql11 = "select * from asm_product where productID=" + productID;
                DataTable dd = DbHelperSQL.Query(sql11).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    productName = dd.Rows[0]["proName"].ToString();
                    this._proName.Value = productName;
                    description = dd.Rows[0]["description"].ToString();
                    half_price = double.Parse(dd.Rows[0]["price2"].ToString())/2;
                    this.halfPrice.Value = half_price.ToString();
                    totalMoney = (double.Parse(dd.Rows[0]["price2"].ToString()) * double.Parse(pszq)).ToString("f2");
                    if (yhfs.IndexOf('折')>-1)
                    {
                        string zk = yhfs.Replace("打","").Replace("折","");
                        totalMoney =(double.Parse(totalMoney) * double.Parse(zk)/10).ToString("f2");
                    }
                    httpImgUrl= dd.Rows[0]["httpImageUrl"].ToString();
                    this._totalMoney.Value = totalMoney;
                     
                }
                initData();
            }
            catch {
                if (OperUtil.getCooki("vshop_openID") != "0")
                {
                    this.member_ID.Value = Util.getMemberID();
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

                orderNO = Request.QueryString["orderNO"].ToString();
                 
                string sql2 = "select * from asm_order where memberID="+ memberID + " and orderNO='"+orderNO+"'";
                DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                if (dt2.Rows.Count>0)
                {
                    syNum = dt2.Rows[0]["syNum"].ToString();
                    pszq = dt2.Rows[0]["zq"].ToString();
                    qsDate = dt2.Rows[0]["qsDate"].ToString();
                    zdDate = dt2.Rows[0]["zdDate"].ToString();
                    psStr = dt2.Rows[0]["psStr"].ToString();
                    psfs = dt2.Rows[0]["psfs"].ToString();
                    orderNO = dt2.Rows[0]["orderNO"].ToString();
                    productID = dt2.Rows[0]["productID"].ToString();
                    mechineID = dt2.Rows[0]["mechineID"].ToString();
                    createTime = dt2.Rows[0]["createTime"].ToString();
                    this.mechine_id.Value = dt2.Rows[0]["mechineID"].ToString();
                    this.product_id.Value = dt2.Rows[0]["productID"].ToString();
                    this._pszq.Value = dt2.Rows[0]["zq"].ToString();
                    this._qsDate.Value = dt2.Rows[0]["qsDate"].ToString();
                    this._zdDate.Value = dt2.Rows[0]["zdDate"].ToString();
                    this._psStr.Value = dt2.Rows[0]["psStr"].ToString();
                    this._psfs.Value = dt2.Rows[0]["psfs"].ToString();
                    this._orderNO.Value = dt2.Rows[0]["orderNO"].ToString();
                    this._createTime.Value = dt2.Rows[0]["createTime"].ToString();
                    this._fkzt.Value = dt2.Rows[0]["fkzt"].ToString();
                   
                    qhAddress = dt2.Rows[0]["qhAddress"].ToString();

                    string sql12 = "select * from asm_mechine where id=" + mechineID;
                    DataTable dt = DbHelperSQL.Query(sql12).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string sqlkf = "select * from asm_opera where id=" + dt.Rows[0]["operaID"].ToString();
                        phone = DbHelperSQL.Query(sqlkf).Tables[0].Rows[0]["linkphone"].ToString();
                        
                    }
                   
                    //初始化商品
                    string sql1 = "select * from asm_product where productID=" + productID;
                    DataTable dd = DbHelperSQL.Query(sql1).Tables[0];
                    if (dd.Rows.Count > 0)
                    {
                        productName = dd.Rows[0]["proName"].ToString();
                        this._proName.Value = productName;
                        description = dd.Rows[0]["description"].ToString();
                        half_price = double.Parse(dd.Rows[0]["price2"].ToString()) / 2;
                        httpImgUrl = dd.Rows[0]["httpImageUrl"].ToString();
                        this.halfPrice.Value = half_price.ToString();
                        totalMoney = dt2.Rows[0]["totalMoney"].ToString();
                        this._totalMoney.Value = dt2.Rows[0]["totalMoney"].ToString();
                    }
                    //查询该订单的详细
                    string sql = "select * from asm_orderDetail where orderNO='"+orderNO+"' and memberID="+ dt2.Rows[0]["memberID"].ToString();
                    DataTable ds = DbHelperSQL.Query(sql).Tables[0];
                    if (ds.Rows.Count>0)
                    {
                        string time = "";
                        for (int i=0;i<ds.Rows.Count;i++)
                        {
                            time += ds.Rows[i]["createTime"].ToString()+",";
                        }
                        time = time.Substring(0,time.Length-1);
                        this._selDate.Value = time;
                        selDate = time;
                    }
                    string sql3 = "select * from asm_orderDetail where zt=3 and orderNO='" + orderNO + "' and memberID=" + dt2.Rows[0]["memberID"].ToString();
                    DataTable ds3 = DbHelperSQL.Query(sql3).Tables[0];
                    if (ds3.Rows.Count > 0)
                    {
                        string time = "";
                        for (int i = 0; i < ds3.Rows.Count; i++)
                        {
                            time += ds3.Rows[i]["createTime"].ToString() + ",";
                        }
                        time = time.Substring(0, time.Length - 1);
                        this._selDate.Value = time;
                        yzsDate = time;
                    }
                    string sql4 = "select * from asm_orderDetail where zt=2 and orderNO='" + orderNO + "' and memberID=" + dt2.Rows[0]["memberID"].ToString();
                    DataTable ds4 = DbHelperSQL.Query(sql4).Tables[0];
                    if (ds4.Rows.Count > 0)
                    {
                        string time = "";
                        for (int i = 0; i < ds4.Rows.Count; i++)
                        {
                            time += ds4.Rows[i]["createTime"].ToString() + ",";
                        }
                        time = time.Substring(0, time.Length - 1);
                        this._sxDate.Value = time;
                        sxDate = time;
                    }
                    string sql5 = "select * from asm_orderDetail where zt=1 and orderNO='" + orderNO + "' and memberID=" + dt2.Rows[0]["memberID"].ToString();
                    DataTable ds5 = DbHelperSQL.Query(sql5).Tables[0];
                    if (ds5.Rows.Count > 0)
                    {
                        string time = "";
                        for (int i = 0; i < ds5.Rows.Count; i++)
                        {
                            time += ds5.Rows[i]["createTime"].ToString() + ",";
                        }
                        time = time.Substring(0, time.Length - 1);
                        this._wcDate.Value = time;
                        wcDate = time;
                    }
                    string sql6 = "select * from asm_orderDetail where zt=4 and orderNO='" + orderNO + "' and memberID=" + dt2.Rows[0]["memberID"].ToString();
                    DataTable ds6 = DbHelperSQL.Query(sql6).Tables[0];
                    if (ds6.Rows.Count > 0)
                    {
                        string time = "";
                        for (int i = 0; i < ds6.Rows.Count; i++)
                        {
                            time += ds6.Rows[i]["createTime"].ToString() + ",";
                        }
                        time = time.Substring(0, time.Length - 1);
                        this._dqhDate.Value = time;
                        dqhDate = time;
                    }
                    initData();
                }
            }
           
           
        }
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        public void initData()
        {
            if (!string.IsNullOrEmpty(orderNO))
            {
                string sql = "select * from asm_order where orderNO='"+orderNO+"'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    fkzt = dt.Rows[0]["fkzt"].ToString();
                }
                else {
                    fkzt = "0";
                }

            }
        }
        [WebMethod]
        public static string okSet(string date,string day,string orderNO)
        {
            string sql = "select * from asm_orderDetail where createTime='"+date+"' and orderNO='"+orderNO+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0&&dt.Rows[0]["zt"].ToString()!="5")
            {
                return "1";//当前状态无法延期
            }
            string sql1 = @"INSERT INTO [dbo].[asm_orderDetail](
                                            [mechineID],
                                            [productID],
                                            [createTime],
                                            [code],
                                            [memberID],
                                            [zt],[ldNO],
                                            [orderNO],
                                        [bz]) 
                        VALUES('"+dt.Rows[0]["mechineID"].ToString()+"','"+dt.Rows[0]["productID"] +"','"+day+"','"+ dt.Rows[0]["code"].ToString() + "','"+dt.Rows[0]["memberID"].ToString()+"',5,'"+dt.Rows[0]["ldNO"].ToString()+"','"+ dt.Rows[0]["orderNO"].ToString() + "','')";
            int a= DbHelperSQL.ExecuteSql(sql1);
            if (a > 0)
            {
                string sql2 = "delete from asm_orderDetail where memberID="+dt.Rows[0]["memberID"].ToString()+" and orderNO='"+ dt.Rows[0]["orderNO"].ToString() + "' and createTime='"+date+"'";
                DbHelperSQL.ExecuteSql(sql2);
                //更新订单的止订日期
                string sql3 = "update asm_order set zdDate = '"+day+"' where orderNO = '"+ dt.Rows[0]["orderNO"].ToString() + "'";
                DbHelperSQL.ExecuteSql(sql3);
                //接着更新会员表的mechineID字段
                return "2";
            }
            return "3";
        }
        [WebMethod]
        public static string isSellOK(string date,string orderNO,string sellPrice)
        {
            //判断今天的商品操作员有没有上货  有上货的话可以转售
            string sql3 = "select * from asm_orderDetail where orderNO='"+orderNO+"' and createTime='"+ DateTime.Now.ToString("yyyy-MM-dd") + "'";
            DataTable dd = DbHelperSQL.Query(sql3).Tables[0];
            if (dd.Rows.Count>0&&string.IsNullOrEmpty(dd.Rows[0]["ldNO"].ToString()))
            {
                return "3";
            }
            string sql4 = "select * from asm_order where orderNO='"+orderNO+"'";
            DataTable d4 = DbHelperSQL.Query(sql4).Tables[0];
            if (d4.Rows.Count > 0 &&d4.Rows[0]["fkzt"].ToString()!="1")
            {
                return "4";
            }
            //判断date的日期和当前的日期是否是今天和明天 如果不是不允许转售
            string sql = "select DATEDIFF(dd,CONVERT(datetime,'"+date+"'),GETDATE()) n";
            DataTable dt=DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0 && int.Parse(dt.Rows[0]["n"].ToString()) <= 2)
            {
                //需要机器重新下载当天的订购数据
                string sql1 = "update asm_orderDetail set zt=3,sellPrice="+sellPrice+ ",sellTime='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' where createTime='" + date+"' and orderNO='"+orderNO+"'";
                DbHelperSQL.ExecuteSql(sql1);
                string sql2 = "update asm_orderDetail set statu=0 where    createTime='"+DateTime.Now.ToString("yyyy-MM-dd")+"' and mechineID='"+dd.Rows[0]["mechineID"].ToString()+"'";
                DbHelperSQL.ExecuteSql(sql2);

                //插入记录
                Util.insertNotice(dd.Rows[0]["memberID"].ToString(), "转售通知", "您将今日的订购产品转售，当前取货码:" + dd.Rows[0]["code"].ToString() + "已作废","");
                
                return "2";
            }
            else {
                return "1";//当前日期不允许转售
            }
        }
        [WebMethod]
        public static string getProduct(string date,string orderNO)
        {
            string sql = "select * from asm_orderDetail where createTime='"+date+"' and orderNO='"+orderNO+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows[0]["zt"].ToString();
        }
        [WebMethod]
        public static string getProductList(string mechineID,string productID)
        {
            string sql = "select * from asm_mechine where id="+mechineID;
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            string sql1 = "select * from asm_product where companyID="+dd.Rows[0]["companyID"].ToString()+" and productID!="+productID;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count>0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dt);
            }
            return "1";
        }
        [WebMethod]
        public static string getActivityList(string mechineID)
        {
            string sql1 = "select * from asm_mechine where id=" + mechineID;
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            string sql = "select * from asm_activity where companyID=" +dt.Rows[0]["companyID"].ToString();
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count > 0)
            {
                return OperUtil.DataTableToJsonWithJsonNet(dd);
            }
            else
            {
                return "1";
            }
        }
        [WebMethod]
        public static string getSYMoney(string orderNO)
        {
            string sql = "select * from asm_order where orderNO='"+orderNO+"'";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            int dayCount = 0;
            if (dd.Rows.Count > 0)
            {
                dayCount = int.Parse(dd.Rows[0]["totalNum"].ToString());
                if (dd.Rows[0]["yhfs"].ToString().IndexOf("赠送") > -1)
                {
                    string day = dd.Rows[0]["yhfs"].ToString().Replace("赠送", "").Replace("天", "");
                    //dayCount = dayCount + int.Parse(day);
                    return ((double.Parse(dd.Rows[0]["totalMoney"].ToString()) / dayCount) * int.Parse(dd.Rows[0]["syNum"].ToString())).ToString("f2");
                }
                else {
                    return ((double.Parse(dd.Rows[0]["totalMoney"].ToString()) / dayCount) * int.Parse(dd.Rows[0]["syNum"].ToString())).ToString("f2");
                }
            }
            return "0";
        }
        [WebMethod]
        public static string getProductPrice2(string productID)
        {
            string sql = "select * from asm_product where productID="+productID;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                return dt.Rows[0]["price2"].ToString();
            }
            return "0";
        }
        [WebMethod]
        public static string dh(string orderNO,string syMoney,string need_money,string zq,string productID,string yhfs)
        {
            //1需要先把旧的的订单的状态更改为已兑换
            //2按照旧的订单的配送方式重新生成新的订单 2 天之后配送
            //3钱款多退少补
            //先判断当前状态不是完成的才可以兑换
            string sql1 = "select * from asm_order where orderNO='" + orderNO + "' and zt in (0,1)";
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count<=0)
            {
                return "2";
            }
            string sql22 = "update asm_order set zt=4 where orderNO='"+orderNO+"'";
            DbHelperSQL.ExecuteSql(sql22);
            string sql = "update asm_orderDetail set zt=7 where orderNO='"+orderNO+"' and zt=5";
            DbHelperSQL.ExecuteSql(sql);
            if (dt.Rows.Count>0)
            {
                string sellDate=insertIntoOrderDetail(dt.Rows[0]["psfs"].ToString(), dt.Rows[0]["psStr"].ToString(),zq);
                string[] sellArr = sellDate.Split(',');
                string order_NO = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
                string fkzt = "1";
                //创建订单
                string sqlInsert = @"INSERT INTO [dbo].[asm_order](
                                                       [mechineID],
                                                       [productID],
                                                       [memberID],
                                                       [totalNum],
                                                       [consumeNum],
                                                       [syNum],
                                                       [createTime],
                                                       [zq],
                                                       [qsDate],
                                                       [zdDate],
                                                       [psStr],
                                                       [psfs],
                                                       [orderNO],
                                                       [fkzt],
                                                       [zt],
                                                       [qhAddress],
                                                       [totalMoney],
                                                       [yhfs])
                        VALUES("+dt.Rows[0]["mechineID"].ToString()+","+productID+","+dt.Rows[0]["memberID"].ToString()+","+zq+",0,"+zq+",'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+zq+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+sellArr[sellArr.Length-1]+"','"+dt.Rows[0]["psStr"].ToString() +"',"+ dt.Rows[0]["psfs"].ToString() + ",'"+ order_NO + "',"+fkzt+",0,'"+ dt.Rows[0]["qhAddress"].ToString() + "',"+need_money+",'"+ yhfs + "')";
                int a=DbHelperSQL.ExecuteSql(sqlInsert);
                if (a>0)
                {
                    //更新商品销售数量
                    string ss = "update asm_product set ljxs=CONVERT(float,ISNULL(ljxs,0))+1 where productID=" + productID;
                    DbHelperSQL.ExecuteSql(ss);
                    string[] selDate = sellArr;
                    if (selDate.Length > 0)
                    {
                        for (int i = 0; i < selDate.Length; i++)
                        {
                            int code = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                            //zt   1-已完成；2-已失效；3-已转售；4-待取货；5-待配送
                            string sql2 = @"INSERT INTO [dbo].[asm_orderDetail](
                                                    [mechineID],
                                                    [productID],
                                                    [createTime],
                                                    [code],
                                                    [memberID],
                                                    [zt],
                                                    [ldNO],
                                                    [orderNO],
                                                    [statu],
                                                    [sellPrice])
                                VALUES('" + dt.Rows[0]["mechineID"].ToString() + "','" + productID + "','" + selDate[i] + "','" + code + "'," + dt.Rows[0]["memberID"].ToString() + ",5,'','" + order_NO + "',0,0)";
                            DbHelperSQL.ExecuteSql(sql2);
                        }
                    }
                    //给会员绑定机器
                    string sql4 = "update asm_member set mechineID=" + dt.Rows[0]["mechineID"].ToString() + ",LastTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',consumeCount=consumeCount+1,sumConsume=sumConsume+" + need_money + " where id=" + dt.Rows[0]["memberID"].ToString();
                    DbHelperSQL.ExecuteSql(sql4);
                    string sql5 = "select * from asm_product where productID=" + productID;
                    DataTable dd5 = DbHelperSQL.Query(sql5).Tables[0];
                    string sql6 = "select * from asm_mechine where id=" + dt.Rows[0]["mechineID"].ToString();
                    DataTable dd6 = DbHelperSQL.Query(sql6).Tables[0];
                    wxHelper wx = new wxHelper(OperUtil.getCooki("companyID"));
                    string data = TemplateMessage.comsume(OperUtil.getCooki("vshop_openID"), "ti4Dkcm1ELNqaskSYsCYMzqL87nPqapNeOgwhvSci_Q", "亲，你的购买的商品信息如下", "" + dd5.Rows[0]["proName"].ToString() + "", need_money, order_NO, dd6.Rows[0]["bh"].ToString(), "欢迎惠顾");
                    TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(OperUtil.getCooki("companyID")), data);
                    //多退少补
                    if (double.Parse(need_money)<double.Parse(syMoney))
                    {
                        //退给会员钱包
                        string sqlUpdate = "update asm_member set AvailableMoney=AvailableMoney+"+(double.Parse(need_money)-double.Parse(syMoney))+" where id="+ dt.Rows[0]["memberID"].ToString();
                        DbHelperSQL.ExecuteSql(sqlUpdate);
                    }
                }
            }
            return "1";
        }
        [WebMethod]
        public static string pdzt(string orderNO)
        {
            string sql1 = "select * from asm_order where orderNO='" + orderNO + "' and zt in (1)";
            DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                return "2";
            }
            return "1";
        }
        public static string insertIntoOrderDetail(string psfs,string psStr,string zq)
        {
            string result = "";
            if (psfs == "1")//按天派送
            {
                if (psStr.IndexOf("每天配送") > -1)
                {
                    result=getDataTimeDay("0", zq);
                }
                else if (psStr.IndexOf("隔一天") > -1)
                {
                    result=getDataTimeDay("1", zq);
                }
                else if (psStr.IndexOf("隔两天") > -1)
                {
                    result=getDataTimeDay("2", zq);
                }
                else if (psStr.IndexOf("隔三天") > -1)
                {
                    result=getDataTimeDay("3", zq);
                }
            }
            else if (psfs == "2")
            {
                //自定义派送
                result=getDataTimeWeek(psStr, int.Parse(zq));
            }
            //创建订单
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="D"></param>
        /// <param name="zq"></param>
        /// <returns></returns>
        public static string getDataTimeDay(string D,string zq)
        {
            string  result = "";
            string day = D;
            int m = 0, n = int.Parse(zq);
            if (day == "1")//隔天派送
            {
                m = n * 2 - 1;
            }
            else if (day == "2")//隔2天派送
            {
                m = n * 3 - 2;
            }
            else if (day == "3")//隔3天派送
            {
                m = n * 4 - 3;
            }
            else
            {
                m = n * 1;//每天派送
            }
            //获取应该配送的日期 应该循环m
            var N = 1;//自增变量
            while (N <= m)
            {
                var t = DateTime.Now.AddDays(N).ToString("yyyy-MM-dd") ;
                if (day == "1")
                {
                    N = N + 2;
                }
                else if (day == "2")
                {
                    N = N + 3;
                }
                else if (day == "3")
                {
                    N = N + 4;
                }
                else if (day == "0")
                {
                    N = N + 1;
                }

                result += t + ",";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
        public static string getDataTimeWeek(string psStr,int zq)
        {
            int count = 0;
            int n = zq;
            string result = "";
            for (int i = 1; i < 1000000; i++)
            {
                var time = getDate(i);
                var week = Week(time);
                if (psStr.IndexOf(week) > -1)
                {
                    result += time + ",";
                    count++;
                }
                if (count == n)
                {
                    break;
                }
            }
            result = result.Substring(0, result.Length - 1);
            return result ;
        }
        public static string getDate(int D)
        {
            return DateTime.Now.AddDays(D).ToString("yyyy-MM-dd");
        }
        public static string Week(string time)
        {
            string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            //string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];
            string week = weekdays[Convert.ToInt32(DateTime.Parse(time).DayOfWeek)];
            return week;
        }
        [WebMethod]
        public static string yzPwd(string memberID, string money, string pwd,string companyID,string trxid)
        {
            //验证密码是否正确
            string sql = "select * from asm_member where id='" + memberID + "' and pwd='" + pwd + "'";
            DataTable dd = DbHelperSQL.Query(sql).Tables[0];
            if (dd.Rows.Count <= 0)
            {
                return "1";//支付密码不正确
            }
            //判断余额
            if (double.Parse(dd.Rows[0]["AvailableMoney"].ToString()) < double.Parse(money))
            {
                return "2";//余额不足
            }
            //更新余额
            string update = "update asm_member set AvailableMoney=AvailableMoney-" + money + " where id='" + memberID + "'";
            DbHelperSQL.ExecuteSql(update);
            string sqlM = "select addres from asm_mechine where id in(select mechineID from asm_member where id="+memberID+")";
            DataTable dM = DbHelperSQL.Query(sqlM).Tables[0];
            string address = "";
            if (dM.Rows.Count>0)
            {
                address = dM.Rows[0]["addres"].ToString();
            }
            //发送消息模板
            wxHelper wx = new wxHelper(companyID);
            string data = TemplateMessage.money_bd(dd.Rows[0]["openID"].ToString(), OperUtil.getMessageID(companyID, "OPENTM403148135"), "余额变动提醒", "购买产品", money, (double.Parse(dd.Rows[0]["AvailableMoney"].ToString())- double.Parse(money)).ToString(), address, "支付时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            TemplateMessage.SendTemplateMsg(wx.IsExistAccess_Token(companyID), data);
            //插入记录
            Util.insertNotice(dd.Rows[0]["id"].ToString(), "余额变动提醒", "您于" + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + "购物消费：" + money + "元；余额：" + (double.Parse(dd.Rows[0]["AvailableMoney"].ToString()) - double.Parse(money)),"");
            Util.moneyChange(dd.Rows[0]["id"].ToString(), money, dd.Rows[0]["AvailableMoney"].ToString(), "会员消费", "2", "");
            string insertSQL = @"insert into asm_pay_info(trxid,acct,statu,type,payType,trxamt,paytime)
                                            values('"+trxid+"','"+ memberID + "',1,2,4,"+(double.Parse(money)*100)+",'"+DateTime.Now.ToString("yyyyMMddHHmmss")+"')";
            DbHelperSQL.ExecuteSql(insertSQL);
            return "3";
        }
    }
}