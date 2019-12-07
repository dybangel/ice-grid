using autosell_center.cls;
using autosell_center.util;
using Consumer.cls;
using DBUtility;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace autosell_center.api
{
    /// <summary>
    /// ExportExcel 的摘要说明
    /// </summary>
    public class ExportExcel : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "ExportExcel1":  //导出采购单
                    this.ExportExcel1(context);
                    return;
                case "ExportJHD":  //导出拣货单
                    this.exportJHD(context);
                    return;
                case "exportSHD"://导出上货单
                    this.ExportSHD(context);
                    return;
                case "exportProduct"://导出商品列表
                    this.exportProduct(context);
                    return;
                case "ExportActivityList"://导出活动记录
                    this.ExportActivityList(context);
                    return;
                case "exportExcel"://导入excel 订购订单
                    this.exportExcel(context);
                    return;
                case "exportExcelKC"://导出库存
                    this.exportExcelKC(context);
                    return;
                case "exporttotalKC"://导出总库存
                    this.exporttotalKC(context);
                    return;
                case "ExportRecord"://导出流量统计
                    this.ExportRecord(context);
                    return;
            }

        }
        public void ExportRecord(HttpContext context)
        {
            try
            {
                string start = context.Request["start"].ToString();
                string end = context.Request["end"].ToString();
                string mechineList = context.Request["mechineList"].ToString();
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                resp.Charset = "utf-8";
                resp.Clear(); 
                string filename = "流量统计"+DateTime.Now.ToString("yyyyMMddHHmmss");
                resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
                resp.ContentEncoding = System.Text.Encoding.UTF8;

                resp.ContentType = "application/ms-excel";
                string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" +
                    "<style> "
                    + ".table{ font: 9pt Tahoma, Verdana; "
                    + "          color: #000000; "
                    + "          text-align:center;"
                    + "          background-color:#8ECBE8; "
                    + "          font-family:'微软雅黑';font-weight:bold;"
                    + "          font-size:15px;"
                    + "          }"
                    + ".table td{"
                    + "           text-align:center;"
                    + "            height:21px;"
                    + "            background-color:#EFF6FF;"
                    + "          }"
                    + ".table th{ "
                    + "        font: 9pt Tahoma, Verdana;"
                    + "        color: #000000; "
                    + "        font-weight: bold;"
                    + "        background-color: #8ECBEA;"
                    + "        height:25px;  "
                    + "        text-align:center;"
                    + "        padding-left:10px;"
                    + "}"
                    + "</style>";
                resp.Write(style);

                resp.Write("<table class='table'><tr><td colspan='6' style='text-align:left'>导出时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</td></tr> "
                 + "   <tr><th>机器名称</th><th>机器编号</th><th>会员名称/ID</th><th>屏幕唤醒</th><th>扫码出货</th><th>零售支付</th><th>支付完成</th><th>时间</th><th>备注</th></tr>");

                System.Data.DataTable dtSource = new System.Data.DataTable();
                dtSource.TableName = "statistic";
                dtSource.Columns.Add("第一列");
                dtSource.Columns.Add("第二列");
                dtSource.Columns.Add("第三列");
                dtSource.Columns.Add("第四列");
                dtSource.Columns.Add("第五列");
                dtSource.Columns.Add("第六列");
                dtSource.Columns.Add("第七列");
                dtSource.Columns.Add("第八列");
                dtSource.Columns.Add("第九列");

                DataTable dt = getDataRecord(start,end,mechineList);
                System.Data.DataRow row = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        row = dtSource.NewRow();
                        row[0] = dt.Rows[i]["mechineName"].ToString();
                        row[1] = dt.Rows[i]["bh"].ToString();
                        row[2] = dt.Rows[i]["memberStr"].ToString();
                        row[3] = dt.Rows[i]["indexCount"].ToString();
                        row[4] = dt.Rows[i]["smCount"].ToString();
                        row[5] = dt.Rows[i]["productCount"].ToString();
                        row[6] = dt.Rows[i]["endCount"].ToString();
                        row[7] = dt.Rows[i]["timeStr"].ToString();
                        row[8] = dt.Rows[i]["productStr"].ToString();


                        dtSource.Rows.Add(row);
                    }
                    foreach (DataRow tmpRow in dtSource.Rows)
                    {
                        resp.Write("<tr><td style=\"vnd.ms-excel.numberformat:@\">" + tmpRow[0] + "</td>");
                        resp.Write("<td>" + tmpRow[1] + "</td>");
                        resp.Write("<td>" + tmpRow[2] + "</td>");
                        resp.Write("<td>" + tmpRow[3] + "</td>");
                        resp.Write("<td>" + tmpRow[4] + "</td>");
                        resp.Write("<td>" + tmpRow[5] + "</td>");
                        resp.Write("<td>" + tmpRow[6] + "</td>");
                        resp.Write("<td>" + tmpRow[7] + "</td>");
                        resp.Write("<td>" + tmpRow[8] + "</td>");
                        resp.Write("</tr>");
                    }
                    resp.Write("<table>");
                }

                resp.Flush();
                resp.End();

            }
            catch
            {

            }
        }
        public DataTable getDataRecord(string start,string end,string mechineList)
        {
            string sql1 = "";
            if (!string.IsNullOrEmpty(mechineList))
            {
                sql1 += " and r.mechineID in(" + mechineList + ")";
            }
            if (!string.IsNullOrEmpty(start))
            {
                sql1 += " and timeStr>'" + start + "'";
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql1 += " and timeStr<'" + end + "'";
            }
            string sql = "SELECT	r.*, m.name,	mec.mechineName,mec.bh FROM	asm_mechineRecord r LEFT JOIN asm_member m ON r.memberID = m.id LEFT JOIN asm_mechine mec ON r.mechineID = mec.id WHERE	1 = 1 " + sql1;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }
        public void exporttotalKC(HttpContext context)
        {
            string mechineID = context.Request["mechineID"].ToString();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            resp.Charset = "utf-8";
            resp.Clear();
            string filename = "库存明细_" + DateTime.Now.ToString("yyyyMMdd");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
            resp.ContentEncoding = System.Text.Encoding.UTF8;

            resp.ContentType = "application/ms-excel";
            string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" +
                " <style>"
                   + "*{"
                   + "         margin: 0 auto;"
                   + "     }"
                   + " .exTable{"
                   + "         width: 500px;"
                   + "         text-align: center;"
                   + "         border: 1px solid #2db7f5;"
                   + "}"
                   + " .exTable tr:first-child{"
                   + "         width:100%;"
                   + "         font-size:16px;"
                   + "         background:#2db7f5;"
                   + "         line-height:48px;"
                   + "         color:#fff;"
                   + "         font-weight:normal;"
                   + "    }"
                   + " .exTable tr td table{"
                   + "         width: 99.9 %;"
                   + "         float: left;"
                   + "         font-size: 12px;"
                   + "         margin-top: 16px;"
                   + "        border: 1px solid #e8eaec;"
                   + " }"
                   + ".exTable tr td table tr th{"
                   + "         text-align: center;"
                   + "     }"
                   + " .exTable tr td: last-child table{"
                   + "         float: right;"
                   + "     }"
                   + " .exTable tr td table tr th{"
                   + "         font-weight: normal;"
                   + "         font-size: 14px;"
                   + "         line-height: 36px;"
                   + "         background: #f8f8f9;"
                   + "     border-bottom: 1px solid #e8eaec;"
                   + " }"
                   + " .exTable tr td table tr: first -child th{"
                   + "         background: #f8f8f9;"
                   + "         color: #333;"
                   + "         line-height: 36px;"
                   + "         border-bottom: 1px solid #e8eaec;"
                   + " }"
                   + " .exTable tr td table tr td{"
                   + "         height: 36px;"
                   + "         line-height: 36px;"
                   + "         margin: 0 auto;"
                   + "     }"
               + " </ style > ";
            resp.Write(style);
            if (!string.IsNullOrEmpty(mechineID))
            {
                StringBuilder sb = new StringBuilder();
                DataTable dKC = getDataKC(mechineID);
                 
                sb.Append("<table class='exTable' cellpadding='0' cellspacing='0'> "
                      + "  <tr style = 'height:20px;background-color:#2db7f5' ><td colspan = '7'></td></tr>"
                      + "  <tr><td colspan = '7'>"
                      + "      <table cellpadding = '0' cellspacing = '0'>"
                      + "          <thead>"
                      + "          </thead>"
                      + "          <tbody><tr><th>商品条码</th><th>商品名称</th><th>零售库存数量</th><th>预设零售库存</th><th>订购库存数量</th></tr>");
                for (int k = 0; k < dKC.Rows.Count; k++)
                {
                    sb.Append("<tr>"
                     + "<td style = 'vnd.ms-excel.numberformat:@'> " + dKC.Rows[k]["bh"].ToString() + " </td>"
                     + "<td>" + dKC.Rows[k]["proname"].ToString() + "</td>"
                     + "<td>" + dKC.Rows[k]["lsNum"].ToString() + " </td>"
                     + "<td>" + dKC.Rows[k]["totalLsNum"].ToString() + " </td>"
                     + "<td>" + dKC.Rows[k]["dgNum"].ToString() + "</td>"
                     + "</tr>");
                }

                sb.Append("</tbody>"
                      + "</table>"
                      + "</td>"
                      + "</tr>");
                resp.Write(sb);
                resp.Flush();
                resp.End();
            }
            
            

             
        }
        public void exportExcelKC(HttpContext context)
        {
            try
            {
                string mechineID = context.Request["mechineID"].ToString();
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                resp.Charset = "utf-8";
                resp.Clear();
                string sql = "select * from  asm_mechine where id="+mechineID;
                DataTable dM = DbHelperSQL.Query(sql).Tables[0];
                string filename = dM.Rows[0]["mechineName"].ToString()+"_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
                resp.ContentEncoding = System.Text.Encoding.UTF8;

                resp.ContentType = "application/ms-excel";
                string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" +
                    "<style> "
                    + ".table{ font: 9pt Tahoma, Verdana; "
                    + "          color: #000000; "
                    + "          text-align:center;"
                    + "          background-color:#8ECBE8; "
                    + "         font-family:'微软雅黑';font-weight:bold;"
                    + "          font-size:15px;"
                    + "          }"
                    + ".table td{"
                    + "           text-align:center;"
                    + "            height:21px;"
                    + "            background-color:#EFF6FF;"
                    + "          }"
                    + ".table th{ "
                    + "        font: 9pt Tahoma, Verdana;"
                    + "        color: #000000; "
                    + "        font-weight: bold;"
                    + "        background-color: #8ECBEA;"
                    + "        height:25px;  "
                    + "        text-align:center;"
                    + "        padding-left:10px;"
                    + "}"
                    + "</style>";
                resp.Write(style);

                resp.Write("<table class='table'><tr><td colspan='6' style='text-align:left'>导出时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</td></tr> "
                 + "   <tr><th>商品条码</th><th>商品名称</th><th>零售库存数量</th><th>预设零售库存</th><th>订购库存数量</th></tr>");

                System.Data.DataTable dtSource = new System.Data.DataTable();
                dtSource.TableName = "statistic";
                dtSource.Columns.Add("第一列");
                dtSource.Columns.Add("第二列");
                dtSource.Columns.Add("第三列");
                dtSource.Columns.Add("第四列");
                dtSource.Columns.Add("第五列");
              

                DataTable dt = getDataKC(mechineID);
                System.Data.DataRow row = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        row = dtSource.NewRow();
                        row[0] = dt.Rows[i]["bh"].ToString();
                        row[1] = dt.Rows[i]["proname"].ToString();
                        row[2] = dt.Rows[i]["lsNum"].ToString();
                        row[3] = dt.Rows[i]["totalLsNum"].ToString();
                        row[4] = dt.Rows[i]["dgNum"].ToString();
                      

                        dtSource.Rows.Add(row);
                    }
                    foreach (DataRow tmpRow in dtSource.Rows)
                    {
                        resp.Write("<tr><td style=\"vnd.ms-excel.numberformat:@\">" + tmpRow[0] + "</td>");
                        resp.Write("<td>" + tmpRow[1] + "</td>");
                        resp.Write("<td>" + tmpRow[2] + "</td>");
                        resp.Write("<td>" + tmpRow[3] + "</td>");
                        resp.Write("<td>" + tmpRow[4] + "</td>");
                      
                        resp.Write("</tr>");
                    }
                    resp.Write("<table>");
                }

                resp.Flush();
                resp.End();

            }
            catch
            {

            }
        }
        public DataTable getDataKC(string mechineID)
        {
            //string sql = "select * from  (SELECT *,(select proname from asm_product p where p.productID=t.productID) proname,(select bh from asm_product p where p.productID=t.productID) bh"
            //        +" FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY productID ORDER BY Id DESC) Num FROM asm_kcDetail k where mechineID = "+mechineID+") t WHERE t.Num = 1) A where A.proname is not null ";
            //DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            //return dt;
            if (!string.IsNullOrEmpty(mechineID))
            {
                string[] mechineIDArr = mechineID.Split(',');
                string sql = "";
                for (int i=0; i<mechineIDArr.Length;i++)
                {
                    sql += " SELECT * FROM asm_kcDetail WHERE(id IN(SELECT MAX([id])   FROM asm_kcDetail where mechineID="+ mechineIDArr[i] + " and  asm_kcDetail.productID  in(select productID from  asm_ldInfo where mechineID=" + mechineIDArr[i] + " and productID!='')  GROUP BY productID)) union";
                }
                string sql1 = "select * from (select A.productID,(select bh from asm_product where productID=A.productID)bh,"
                    +" (select proname from asm_product where productID = A.productID)proname,SUM(dgNum)dgNum,SUM(lsNum)lsNum,SUM(totalLsNum)totalLsNum,SUM(imbalance)imbalance,SUM(totalDgNum)totalDgNum from ("+sql.Substring(0,sql.Length-5)+ ") A group by productID) C where proname is not null";
                DataTable dt=DbHelperSQL.Query(sql1).Tables[0];
                return dt;
            }
            return null;
        }
        public void exportExcel(HttpContext context)
        {
            try
            {
                HttpFileCollection files = context.Request.Files;
                if (files.Count > 0)
                {
                    HttpPostedFile file = files[0];
                    if (file.ContentLength > 0)
                    {
                        string fileName = file.FileName;//文件名  
                        int fileSize = file.ContentLength;//文件大小 
                        string ex = fileName.Substring(fileName.Length - 4, 4).ToLower();
                        if (ex== "xls" ||ex == "xlsx")
                        {
                            string date = DateTime.Now.ToString("yyyyMMddHHMMss");
                            string path = "../UploadedExcel/"+date+ex;
                            string fName = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UploadedExcel"), Path.GetFileName(fileName));
                            files[0].SaveAs(fName);
                            DataTable dtData = ExcelHelper.ExcelToDataTable(fName);
                            if (dtData.Rows.Count>=1)
                            {
                                context.Response.Write("{\"code\":\"200\",\"db\":"+OperUtil.DataTableToJsonWithJsonNet(dtData)+"}");
                                return;
                            }
                            context.Response.Write("{\"code\":\"300\",\"msg\":\"没有记录\"}");
                            return;
                        }
                        else
                        {
                            context.Response.Write("{\"code\":\"500\",\"msg\":\"文件格式不正确\"}");
                            return;
                        }
                    }
                }
                else {
                    context.Response.Write("{\"code\":\"500\",\"msg\":\"请先选择对应的文件\"}");
                    return;
                }
            }
            catch {
                context.Response.Write("{\"code\":\"500\",\"msg\":\"文件上传异常\"}");
                return;
            }
             
          
        }
        public void ExportActivityList(HttpContext context)
        {
            try
            {
                string start = context.Request["start"].ToString();
                string end = context.Request["end"].ToString();
                string keyword = context.Request["keyword"].ToString();
                string activityType = context.Request["activityType"].ToString();
                string deltype = context.Request["deltype"].ToString();
                string companyID = context.Request["companyID"].ToString();
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                resp.Charset = "utf-8";
                resp.Clear();
                string filename = "活动记录_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
                resp.ContentEncoding = System.Text.Encoding.UTF8;

                resp.ContentType = "application/ms-excel";
                string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" +
                    "<style> "
                    + ".table{ font: 9pt Tahoma, Verdana; "
                    + "          color: #000000; "
                    + "          text-align:center;"
                    + "          background-color:#8ECBE8; "
                    + "         font-family:'微软雅黑';font-weight:bold;"
                    + "          font-size:15px;"
                    + "          }"
                    + ".table td{"
                    + "           text-align:center;"
                    + "            height:21px;"
                    + "            background-color:#EFF6FF;"
                    + "          }"
                    + ".table th{ "
                    + "        font: 9pt Tahoma, Verdana;"
                    + "        color: #000000; "
                    + "        font-weight: bold;"
                    + "        background-color: #8ECBEA;"
                    + "        height:25px;  "
                    + "        text-align:center;"
                    + "        padding-left:10px;"
                    + "}"
                    + "</style>";
                resp.Write(style);

                resp.Write("<table class='table'><tr><td colspan='6' style='text-align:left'>导出时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</td></tr> "
                 + "   <tr><th>活动类别</th><th>购买内容</th><th>会员名称</th><th>手机号</th><th>订单金额</th><th>参与时间</th><th>活动奖励内容</th><th>处理时间</th><th>备注</th><th>是否处理</th></tr>");

                System.Data.DataTable dtSource = new System.Data.DataTable();
                dtSource.TableName = "statistic";
                dtSource.Columns.Add("第一列");
                dtSource.Columns.Add("第二列");
                dtSource.Columns.Add("第三列");
                dtSource.Columns.Add("第四列");
                dtSource.Columns.Add("第五列");
                dtSource.Columns.Add("第六列");
                dtSource.Columns.Add("第七列");
                dtSource.Columns.Add("第八列");
                dtSource.Columns.Add("第九列");
                dtSource.Columns.Add("第十列");
                
                DataTable dt = getDatat(start,end,keyword,activityType,deltype,companyID);
                System.Data.DataRow row = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                         
                        row = dtSource.NewRow();
                        row[0] = dt.Rows[i]["type"].ToString()=="1"?"订购":"充值";
                        row[1] = dt.Rows[i]["activityName"].ToString();
                        row[2] = dt.Rows[i]["nickname"].ToString();
                        row[3] = dt.Rows[i]["phone"].ToString();
                        row[4] = dt.Rows[i]["totalMoney"].ToString();
                        row[5] = dt.Rows[i]["partTime"].ToString();
                        row[6] = dt.Rows[i]["activityContent"].ToString();
                        row[7] = dt.Rows[i]["delTime"].ToString();
                        row[8] = dt.Rows[i]["bz"].ToString();
                        row[9] = dt.Rows[i]["status"].ToString()=="1"?"已处理":"未处理";
                       
                        dtSource.Rows.Add(row);
                    }
                    foreach (DataRow tmpRow in dtSource.Rows)
                    {
                        resp.Write("<tr><td>" + tmpRow[0] + "</td>");
                        resp.Write("<td>" + tmpRow[1] + "</td>");
                        resp.Write("<td>" + tmpRow[2] + "</td>");
                        resp.Write("<td>" + tmpRow[3] + "</td>");
                        resp.Write("<td>" + tmpRow[4] + "</td>");
                        resp.Write("<td>" + tmpRow[5] + "</td>");
                        resp.Write("<td>" + tmpRow[6] + "</td>");
                        resp.Write("<td>" + tmpRow[7] + "</td>");
                        resp.Write("<td>" + tmpRow[8] + "</td>");
                        resp.Write("<td>" + tmpRow[9] + "</td>");
                        resp.Write("</tr>");
                    }
                    resp.Write("<table>");
                }

                resp.Flush();
                resp.End();

            }
            catch
            {

            }
        }
        public DataTable getDatat(string start,string end,string phone,string type,string deltype,string companyID)
        {
            try
            {
                string sql1 = "";
                if (!string.IsNullOrEmpty(start))
                {
                    sql1 += " and partTime>='" + start + "' ";
                }
                if (!string.IsNullOrEmpty(end))
                {
                    sql1 += " and partTime<='" + end + "' ";
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    sql1 += " and m.phone like '%" + phone + "%'";
                }
                if (type != "0")
                {
                    sql1 += " and type=" + type;
                }
                if (deltype != "-1")
                {
                    sql1 += " and status=" + deltype;
                }
                string sql = "select p.*,m.nickname,m.phone from asm_partActivity p left join asm_member m on p.memberID=m.id where p.companyID=" + companyID + sql1;
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count>0)
                {
                    return dt;
                }
            }
            catch {

            }
            return null;
        }
        public void exportProduct(HttpContext context)
        {
            try
            {
                string keyword = context.Request["keyword"].ToString();
                string qy = context.Request["qy"].ToString();
                string code = context.Request["code"].ToString();
                string brandID = context.Request["brandID"].ToString();
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                resp.Charset = "utf-8";
                resp.Clear();
                string filename = "产品列表_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
                resp.ContentEncoding = System.Text.Encoding.UTF8;

                resp.ContentType = "application/ms-excel";
                string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" +
                    "<style> "
                    + ".table{ font: 9pt Tahoma, Verdana; "
                    + "          color: #000000; "
                    + "          text-align:center;"
                    + "          background-color:#8ECBE8; "
                    + "          font-family:'微软雅黑';font-weight:bold;"
                    + "          font-size:15px;"
                    + "          }"
                    + ".table td{"
                    + "           text-align:center;"
                    + "            height:21px;"
                    + "            background-color:#EFF6FF;"
                    + "          }"
                    + ".table th{ "
                    + "        font: 9pt Tahoma, Verdana;"
                    + "        color: #000000; "
                    + "        font-weight: bold;"
                    + "        background-color: #8ECBEA;"
                    + "        height:25px;  "
                    + "        text-align:center;"
                    + "        padding-left:10px;"
                    + "}"
                    + "</style>";
                resp.Write(style);

                resp.Write("<table class='table'><tr><td colspan='6' style='text-align:left'>导出时间:"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"</td></tr> "
                 + "   <tr><th>产品名称</th><th>品牌</th><th>分类</th><th>条码</th><th>简称</th><th>售卖类型</th><th>零售价</th><th>普通价</th><th>白银价</th><th>黄金价</th><th>规格</th><th>包装</th><th>保质期</th><th>标签</th><th>权重</th></tr>");

                System.Data.DataTable dtSource = new System.Data.DataTable();
                dtSource.TableName = "statistic";
                dtSource.Columns.Add("第一列");
                dtSource.Columns.Add("第二列");
                dtSource.Columns.Add("第三列");
                dtSource.Columns.Add("第四列");
                dtSource.Columns.Add("第五列");
                dtSource.Columns.Add("第六列");
                dtSource.Columns.Add("第七列");
                dtSource.Columns.Add("第八列");
                dtSource.Columns.Add("第九列");
                dtSource.Columns.Add("第十列");
                dtSource.Columns.Add("第十一列");
                dtSource.Columns.Add("第十二列");
                dtSource.Columns.Add("第十三列");
                dtSource.Columns.Add("第十四列");
                dtSource.Columns.Add("第十五列");
                DataTable dt = getProductList(keyword,qy,code,brandID);
                System.Data.DataRow row = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        //1盒2袋3杯4瓶5个6包
                        string bz = dt.Rows[i]["sluid"].ToString();
                        row = dtSource.NewRow();
                        row[0] = dt.Rows[i]["proName"].ToString();
                        row[1] = dt.Rows[i]["brandName"].ToString();
                        row[2] = dt.Rows[i]["typeName"].ToString();
                        row[3] = dt.Rows[i]["bh"].ToString();
                        row[4] = dt.Rows[i]["shortName"].ToString();
                        row[5] = dt.Rows[i]["dstype"].ToString();
                        row[6] = dt.Rows[i]["price0"].ToString();
                        row[7] = dt.Rows[i]["price1"].ToString()==""?"暂无设置": dt.Rows[i]["price1"].ToString();
                        row[8] = dt.Rows[i]["price2"].ToString()==""?"暂无设置":dt.Rows[i]["price2"].ToString();
                        row[9] = dt.Rows[i]["price3"].ToString()==""?"暂无设置": dt.Rows[i]["price3"].ToString();
                        row[10] = dt.Rows[i]["progg"].ToString();
                        row[11] = (bz=="1"?"盒":(bz=="2"?"袋":(bz=="3"?"杯":(bz=="4"?"瓶":(bz=="5"?"个":(bz=="6"?"包":""))))));
                        row[12] = dt.Rows[i]["bzq"].ToString()+"天";
                        row[13] = dt.Rows[i]["tag"].ToString();
                        row[14] = dt.Rows[i]["weight"].ToString();
                        dtSource.Rows.Add(row);
                    }
                    foreach (DataRow tmpRow in dtSource.Rows)
                    {
                        resp.Write("<tr><td>" + tmpRow[0] + "</td>");
                        resp.Write("<td>" + tmpRow[1] + "</td>");
                        resp.Write("<td>" + tmpRow[2] + "</td>");
                        resp.Write("<td>" + tmpRow[3] + "</td>");
                        resp.Write("<td>" + tmpRow[4] + "</td>");
                        resp.Write("<td>" + tmpRow[5] + "</td>");
                        resp.Write("<td>" + tmpRow[6] + "</td>");
                        resp.Write("<td>" + tmpRow[7] + "</td>");
                        resp.Write("<td>" + tmpRow[8] + "</td>");
                        resp.Write("<td>" + tmpRow[9] + "</td>");
                        resp.Write("<td>" + tmpRow[10] + "</td>");
                        resp.Write("<td>" + tmpRow[11] + "</td>");
                        resp.Write("<td>" + tmpRow[12] + "</td>");
                        resp.Write("<td>" + tmpRow[13] + "</td>");
                        resp.Write("<td>" + tmpRow[14] + "</td>");
                        resp.Write("</tr>");
                    }
                    resp.Write("<table>");
                }

                resp.Flush();
                resp.End();

            }
            catch {

            }
        }
        public void exportJHD(HttpContext context)
        {
            string operaID = context.Request["operaID"].ToString();
            string companyID = context.Request["companyID"].ToString();
            string lsTime = context.Request["lsTime"].ToString();
            string dgStart = context.Request["dgStart"].ToString();
            string dgEnd = context.Request["dgEnd"].ToString();
            string productName = context.Request["productName"].ToString();
            string loginID = context.Request["loginID"].ToString();
            string brandList = context.Request["brandList"].ToString();
            string mechineList = context.Request["mechineIDList"].ToString();
            string sql = "select * from asm_opera where id=" + operaID;
            DataTable dtOpera = DbHelperSQL.Query(sql).Tables[0];
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            resp.Charset = "utf-8";
            resp.Clear();
            string filename = dtOpera.Rows[0]["name"].ToString() + "_拣货单_" + DateTime.Now.ToString("yyyyMMdd");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
            resp.ContentEncoding = System.Text.Encoding.UTF8;

            resp.ContentType = "application/ms-excel";
            string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" +
                "<style> "
                + ".table{ font: 9pt Tahoma, Verdana; "
                + "          color: #000000; "
                + "          text-align:center;"
                + "          background-color:#8ECBE8; "
                + "         font-family:'微软雅黑';font-weight:bold;"
                + "          font-size:15px;"
                + "          }"
                + ".table td{"
                + "           text-align:center;"
                + "            height:21px;"
                + "            background-color:#EFF6FF;"
                + "          }"
                + ".table th{ "
                + "        font: 9pt Tahoma, Verdana;"
                + "        color: #000000; "
                + "        font-weight: bold;"
                + "        background-color: #8ECBEA;"
                + "        height:25px;  "
                + "        text-align:center;"
                + "        padding-left:10px;"
                + "}"
                + "</style>";
            resp.Write(style);
            string sqlM = "SELECT STUFF((SELECT ','+mechineName FROM  asm_mechine where id in ("+mechineList+") for xml path('')),1,1,'') 	name";
            DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
            string mechineStr = "";
            if (dm.Rows.Count>0)
            {
                mechineStr = dm.Rows[0]["name"].ToString();
            }
          
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class='table'><tr><td colspan='6' style='text-align:left'>配送员:" + dtOpera.Rows[0]["name"].ToString() + "</td></tr><tr><td colspan='6' style='text-align:left'>导出时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</td></tr><tr><td colspan='6' style='text-align:left'>零售时间:" + lsTime + "</td></tr><tr><td  colspan='6' style='text-align:left'>订购时间:" + dgStart + "~" + dgEnd + "</td></tr><tr><td colspan='6' style='text-align:left'>"+mechineStr+"</td></tr> "
                + "<tr><th>商品条码</th><th>品牌</th><th>商品分类</th><th>商品名称</th><th>包装</th><th>规格</th><th>订购</th><th>总数量</th></tr>");
            
            System.Data.DataTable dtSource = new System.Data.DataTable();
            dtSource.TableName = "statistic";
            dtSource.Columns.Add("第一列");
            dtSource.Columns.Add("第二列");
            dtSource.Columns.Add("第三列");
            dtSource.Columns.Add("第四列");
            dtSource.Columns.Add("第五列");
            dtSource.Columns.Add("第六列");
            dtSource.Columns.Add("第七列");
            dtSource.Columns.Add("第八列");
            if (string.IsNullOrEmpty(lsTime))
            {
                
                return ;
            }
            if (string.IsNullOrEmpty(dgStart))
            {
                return ;
            }
            if (string.IsNullOrEmpty(dgEnd))
            {
                return ;
            }
            if (string.IsNullOrEmpty(brandList))
            {
                return ;
            }
            string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                     + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + companyID + " and brandID in(" + brandList + ") and is_del=0 GROUP BY brandID; ";
            string productID = "";
            DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
            //检索机器ID
            if (string.IsNullOrEmpty(mechineList))//如果没选机器默认查询该操作员下所有
            {
                string sqlMechine = "SELECT  operaID ,value = ( STUFF(( SELECT    ',' + convert(varchar,id) FROM asm_mechine"
                   + " WHERE operaID = Test.operaID  FOR XML PATH('') ), 1, 1, '') )FROM asm_mechine  AS Test where companyID = " + companyID + " and operaID=" + operaID + " GROUP BY operaID";
                mechineList = DbHelperSQL.Query(sqlMechine).Tables[0].Rows[0]["value"].ToString();
            }
            if (brandDt.Rows.Count > 0)
            {
                for (int i = 0; i < brandDt.Rows.Count; i++)
                {
                    productID += brandDt.Rows[i]["value"].ToString() + ",";
                }
                productID = productID.Substring(0, productID.Length - 1);
                //此处目的是为了去掉历史记录里之前有的产品而现在把该产品给下架的产品ID
                if (!string.IsNullOrEmpty(mechineList))
                {
                    string sqlLd = "SELECT STUFF((SELECT ','+productID FROM  asm_ldinfo where mechineID in (" + mechineList + ")"
                         + " and productID  in(" + productID + ")  for xml path('')),1,1,'') productID";
                    DataTable dd = DbHelperSQL.Query(sqlLd).Tables[0];
                    if (dd.Rows.Count > 0)
                    {
                        productID = dd.Rows[0]["productID"].ToString();
                    }
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    string sqlp = "select * from  asm_product where proname like '%" + productName + "%' or bh like '%" + productName + "%' or shortName like '%" + productName + "%'";
                    DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                    if (dp.Rows.Count > 0)
                    {
                        for (int k = 0; k < dp.Rows.Count; k++)
                        {
                            productID = productID + ",";
                        }
                        productID = productID.Substring(0, productID.Length - 1);
                    }
                }
            }
            productID = String.Join(",", productID.Split(',').Distinct<string>());

            DataTable dt = OperUtil.getJHD(mechineList,productID, dgStart, dgEnd, lsTime);


            System.Data.DataRow row = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                int totalNum = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //typeName
                    row = dtSource.NewRow();
                    string bz = dt.Rows[i]["sluid"].ToString();//1盒2袋3杯4瓶5个6包
                    bz = (bz == "1" ? "盒" : (bz == "2" ? "袋" : (bz == "3" ? "杯" : (bz == "4" ? "瓶" : (bz == "5" ? "个" : (bz == "6" ? "包" : ""))))));
                    row[0] = dt.Rows[i]["bh"].ToString(); 
                    row[1] = dt.Rows[i]["brandName"].ToString();
                    row[2] = dt.Rows[i]["typeName"].ToString();
                    row[3] = dt.Rows[i]["proname"].ToString();
                    row[4] = bz;
                    row[5] = dt.Rows[i]["progg"].ToString();
                    row[6] = int.Parse(dt.Rows[i]["totalDg"].ToString());
                    row[7] = int.Parse(dt.Rows[i]["imbalance"].ToString())+int.Parse(dt.Rows[i]["totalDG"].ToString());
                    totalNum += int.Parse(row[7].ToString());
                    dtSource.Rows.Add(row);
                }
                foreach (DataRow tmpRow in dtSource.Rows)
                {
                    if (int.Parse(tmpRow[7].ToString())>0)
                    {
                        sb.Append("<tr><td style=\"vnd.ms-excel.numberformat:@\">" + tmpRow[0] + "</td>");
                        sb.Append("<td>" + tmpRow[1] + "</td>");
                        sb.Append("<td>" + tmpRow[2] + "</td>");
                        sb.Append("<td>" + tmpRow[3] + "</td>");
                        sb.Append("<td>" + tmpRow[4] + "</td>");
                        sb.Append("<td>" + tmpRow[5] + "</td>");
                        sb.Append("<td>" + tmpRow[6] + "</td>");
                        sb.Append("<td>" + tmpRow[7] + "</td></tr>");
                    }
                }
                sb.Append("<tr><td colspan='6'>总计:" + totalNum + "</td></tr>");
                sb.Append("<tr><td colspan='3'></td><td>操作员:</td><td colspan='2'>" + dtOpera.Rows[0]["name"].ToString() + "</td></tr>");
                sb.Append("<table>");
            }
            resp.Write(sb.ToString());
           
            var fileContents = Encoding.Default.GetBytes(sb.ToString());
            //设置excel保存到服务器的路径
            var filePath = context.Server.MapPath("~/excel/" + filename + ".xls");
            //保存excel到指定路径
            WriteBuffToFile(fileContents, filePath);

            //插入 到处历史记录
            string url = "http://"+HttpContext.Current.Request.Url.Host + "/excel/" + filename + ".xls";
            string insert = "insert into asm_excellist (companyID,downUrl,createTime,excelName,mechineID,operaID,type,loginID)values('" + companyID + "','" + url + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + filename + "','','"+ operaID + "',2,'"+loginID+"')";
            DbHelperSQL.ExecuteSql(insert);
            resp.Flush();
            resp.End();
        }
        public void ExportSHD(HttpContext context)
        {
            string operaID = context.Request["operaID"].ToString();
            string companyID = context.Request["companyID"].ToString();
            string lsTime = context.Request["lsTime"].ToString();
            string dgStart = context.Request["dgStart"].ToString();
            string dgEnd = context.Request["dgEnd"].ToString();
            string productName = context.Request["productName"].ToString();
            string loginID = context.Request["loginID"].ToString();
            string brandList = context.Request["brandList"].ToString();
            string mechineList = context.Request["mechineIDList"].ToString();
            string sql = "select * from asm_opera where id="+operaID;
            DataTable dtOpera = DbHelperSQL.Query(sql).Tables[0];
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            resp.Charset = "utf-8";
            resp.Clear();
            string filename = dtOpera.Rows[0]["name"].ToString()+"_上货单_" + DateTime.Now.ToString("yyyyMMdd");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
            resp.ContentEncoding = System.Text.Encoding.UTF8;

            resp.ContentType = "application/ms-excel";
            string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" +
                " <style>"
                   + "*{"
                   +"         margin: 0 auto;"
                   +"     }"
                   +" .exTable{"
                   +"         width: 500px;"
                   +"         text-align: center;"
                   +"         border: 1px solid #2db7f5;"
                   +"}"
                   +" .exTable tr:first-child{"
                   +"         width:100%;"
                   +"         font-size:16px;"
                   +"         background:#2db7f5;"
                   +"         line-height:48px;"
                   +"         color:#fff;"
                   +"         font-weight:normal;"
                   +"    }"
                   +" .exTable tr td table{"
                   +"         width: 99.9 %;"
                   +"         float: left;"
                   +"         font-size: 12px;"
                   +"         margin-top: 16px;"
                   +"        border: 1px solid #e8eaec;"
                   +" }"
                   +".exTable tr td table tr th{"
                   +"         text-align: center;"
                   +"     }"
                   +" .exTable tr td: last-child table{"
                   +"         float: right;"
                   +"     }"
                   +" .exTable tr td table tr th{"
                   +"         font-weight: normal;"
                   +"         font-size: 14px;"
                   +"         line-height: 36px;"
                   +"         background: #f8f8f9;"
                   +"     border-bottom: 1px solid #e8eaec;"
                   +" }"
                   +" .exTable tr td table tr: first -child th{"
                   +"         background: #f8f8f9;"
                   +"         color: #333;"
                   +"         line-height: 36px;"
                   +"         border-bottom: 1px solid #e8eaec;"
                   +" }"
                   +" .exTable tr td table tr td{"
                   +"         height: 36px;"
                   +"         line-height: 36px;"
                   +"         margin: 0 auto;"
                   +"     }"
               +" </ style > ";
            resp.Write(style);
            if (string.IsNullOrEmpty(lsTime))
            {
                return;
            }
            if (string.IsNullOrEmpty(dgStart))
            {
                return;
            }
            if (string.IsNullOrEmpty(dgEnd))
            {
                return;
            }
            if (string.IsNullOrEmpty(brandList))
            {
                return;
            }
            string sqlM = "select * from asm_mechine where id in ("+mechineList+")";
            DataTable dm = DbHelperSQL.Query(sqlM).Tables[0];
            string sqlDls = "select * from  asm_opera where id='"+dm.Rows[0]["dls"].ToString()+"'";
            DataTable ddls = DbHelperSQL.Query(sqlDls).Tables[0];
            string str1 = "<table class='exTable' cellpadding='0' cellspacing='0'>"
                       + " <tr><th colspan = '8'> 操作员:" + dtOpera.Rows[0]["name"].ToString() + ";手机号："+ dtOpera.Rows[0]["linkphone"].ToString() + ";"+DateTime.Now.ToString("yyyy-MM-dd") + "上货单 </th></tr>";

             
            
            string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                     + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + companyID + " and brandID in(" + brandList + ") and is_del=0 GROUP BY brandID; ";
            string productID = "";
            DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
            //检索机器ID
            if (!string.IsNullOrEmpty(mechineList))
            {
                string sqlMechine = "SELECT  operaID ,value = ( STUFF(( SELECT    ',' + convert(varchar,id) FROM asm_mechine"
                   + " WHERE operaID = Test.operaID  FOR XML PATH('') ), 1, 1, '') )FROM asm_mechine  AS Test where companyID = " + companyID + " and operaID=" + operaID + " GROUP BY operaID";
                mechineList = DbHelperSQL.Query(sqlMechine).Tables[0].Rows[0]["value"].ToString();
            }
            if (brandDt.Rows.Count > 0)
            {
                for (int i = 0; i < brandDt.Rows.Count; i++)
                {
                    productID += brandDt.Rows[i]["value"].ToString() + ",";
                }
                productID = productID.Substring(0, productID.Length - 1);
                //此处目的是为了去掉历史记录里之前有的产品而现在把该产品给下架的产品ID
                if (!string.IsNullOrEmpty(mechineList))
                {
                    string sqlLd = "SELECT STUFF((SELECT ','+productID FROM  asm_ldinfo where mechineID in (" + mechineList + ")"
                         + " and productID  in(" + productID + ")  for xml path('')),1,1,'') productID";
                    DataTable dd = DbHelperSQL.Query(sqlLd).Tables[0];
                    if (dd.Rows.Count > 0)
                    {
                        productID = dd.Rows[0]["productID"].ToString();
                    }
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    string sqlp = "select * from  asm_product where proname like '%" + productName + "%' or bh like '%" + productName + "%' or shortName like '%" + productName + "%'";
                    DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                    if (dp.Rows.Count > 0)
                    {
                        for (int k = 0; k < dp.Rows.Count; k++)
                        {
                            productID = productID + ",";
                        }
                        productID = productID.Substring(0, productID.Length - 1);
                    }
                }
            }
            productID = String.Join(",", productID.Split(',').Distinct<string>());



            string str3 = "";
            if (dm.Rows.Count > 0)
            {
                for (int i=0;i<dm.Rows.Count;i++)
                {
                    string strList = getList(dm.Rows[i]["id"].ToString(), productID, lsTime, dgStart, dgEnd);
                    
                    if (strList!="")
                    {
                        str3 += "<tr style='height:20px;background-color:#2db7f5'><td colspan='7'></td></tr><tr><td colspan='7'><table cellpadding = '0' cellspacing = '0'>"
                    + "             <thead>"
                    + "                   <tr><th colspan = '9'>管理员：" + ddls.Rows[0]["name"].ToString() + "</th></tr>"
                    + "                 <tr>"
                    + "                     <th colspan = '4'> 机器名称：" + dm.Rows[i]["mechineName"].ToString() + " </th><th colspan = '5'> 地址：" + dm.Rows[i]["addres"].ToString() + " </th>"
                    + "                 </tr>"
                    + "             </thead>"
                    + "             <tbody>"
                    + "                 <tr>"
                    + "                     <th> 商品条码 </th><th> 品牌 </th><th> 商品名称 </th><th> 商品分类 </th><th> 包装 </th><th> 规格 </th><th> 订购补货量 </th><th> 总数量 </th><th> 料道编号 </th>"
                    + "                 </tr>"
                    + strList
                    + "             </tbody>"
                    + "         </table>" + "</td></tr> ";
                    }

                  
                }
            }
             

            string str2= "</table>";

            string str = str1 + str3 + str2;
            resp.Write(str);
           
          
            var fileContents = Encoding.Default.GetBytes(str);
            //设置excel保存到服务器的路径

            var filePath = context.Server.MapPath("~/excel/" + filename + ".xls");
            //保存excel到指定路径
            WriteBuffToFile(fileContents, filePath);

            //插入 到处历史记录
            string url = "http://"+HttpContext.Current.Request.Url.Host + "/excel/" + filename + ".xls";
            string insert = "insert into asm_excellist (companyID,downUrl,createTime,excelName,mechineID,operaID,type,loginID)values('" + companyID + "','" + url + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + filename + "','','" + operaID + "',3,'"+ loginID + "')";
            DbHelperSQL.ExecuteSql(insert);
            resp.Flush();
            resp.End();
        }
        public string getList(string mechineID,string productID, string lsTime,string dgStart,string dgEnd)
        {
            DataTable dt = OperUtil.getSHD(mechineID, productID, dgStart, dgEnd, lsTime);
            string list = "";
            string list2 = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                int count = 0;
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    if (int.Parse(dt.Rows[k]["imbalance"].ToString())+int.Parse(dt.Rows[k]["totalDG"].ToString()) >0)
                    {
                        string bz = dt.Rows[k]["sluid"].ToString();//1盒2袋3杯4瓶5个6包
                        bz = (bz == "1" ? "盒" : (bz == "2" ? "袋" : (bz == "3" ? "杯" : (bz == "4" ? "瓶" : (bz == "5" ? "个" : (bz == "6" ? "包" : ""))))));
                        if (int.Parse(dt.Rows[k]["totalDG"].ToString())>0)
                        {
                            list += "<tr style='font-weight:bold;font-size:16px'><td style=\"vnd.ms-excel.numberformat:@\"> " + dt.Rows[k]["bh"].ToString() + " </td><td>" + dt.Rows[k]["brandName"].ToString() + "</td><td> " + dt.Rows[k]["proname"].ToString() + " </td><td>" + dt.Rows[k]["typeName"].ToString() + "</td><td> " + bz + " </td><td> " + dt.Rows[k]["progg"].ToString() + " </td><td>" + dt.Rows[k]["totalDG"].ToString() + "</td><td> " + (int.Parse(dt.Rows[k]["imbalance"].ToString()) + int.Parse(dt.Rows[k]["totalDG"].ToString())) + " </td><td> " + dt.Rows[k]["ldNO"].ToString() + " </td></tr>";
                        }
                        else {
                            list += "<tr><td style=\"vnd.ms-excel.numberformat:@\"> " + dt.Rows[k]["bh"].ToString() + " </td><td>" + dt.Rows[k]["brandName"].ToString() + "</td><td> " + dt.Rows[k]["proname"].ToString() + " </td><td>" + dt.Rows[k]["typeName"].ToString() + "</td><td> " + bz + " </td><td> " + dt.Rows[k]["progg"].ToString() + " </td><td>" + dt.Rows[k]["totalDG"].ToString() + "</td><td> " + (int.Parse(dt.Rows[k]["imbalance"].ToString()) + int.Parse(dt.Rows[k]["totalDG"].ToString())) + " </td><td> " + dt.Rows[k]["ldNO"].ToString() + " </td></tr>";
                        }
                        
                        count += (int.Parse(dt.Rows[k]["imbalance"].ToString()) + int.Parse(dt.Rows[k]["totalDG"].ToString()));
                        list2 = "<tr colspan='8'>合计" + count + "</tr>";
                    }
                }
               
            }
            return list;
        }
        public DataTable getSHD(string mechineID,string companyID, string lsTime, string dgStart, string dgEnd,string productName)
        {
            try
            {
                string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                        + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + companyID + " and is_del=0 GROUP BY brandID";
                DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
                if (brandDt.Rows.Count > 0)
                {
                    //string lsTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //string dgStart = DateTime.Now.ToString("yyyy-MM-dd");
                    //string dgEnd = DateTime.Now.ToString("yyyy-MM-dd");
                    string sqlBrand = "SELECT  companyID ,value = ( STUFF(( SELECT    ',' + convert(varchar,id) FROM asm_brand"
                      + "  WHERE companyID = Test.companyID  FOR XML PATH('') ), 1, 1, '') )FROM asm_brand  AS Test where companyID = " + companyID + "  GROUP BY companyID";
                    string brandList = DbHelperSQL.Query(sqlBrand).Tables[0].Rows[0]["value"].ToString();
                   
                    string mechineList = mechineID;
                    string sqlP = "select * from  asm_product where brandID in (" + brandList + ")";
                    DataTable dp = DbHelperSQL.Query(sqlP).Tables[0];
                    string productID = "";
                    if (dp.Rows.Count > 0)
                    {
                        for (int i = 0; i < dp.Rows.Count; i++)
                        {
                            productID += dp.Rows[i]["productID"].ToString() + ",";
                        }
                        productID = productID.Substring(0, productID.Length - 1);

                        string sqlLd = "SELECT STUFF((SELECT ','+productID FROM  asm_ldinfo where mechineID in (" + mechineList + ")"
                            + " and productID  in(" + productID + ")  for xml path('')),1,1,'') productID";
                        DataTable dd = DbHelperSQL.Query(sqlLd).Tables[0];
                        if (dd.Rows.Count > 0)
                        {
                            productID = dd.Rows[0]["productID"].ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(mechineList))
                    {
                        string sqlPP = "";
                        if (!string.IsNullOrEmpty(productName))
                        {
                            sqlPP = " and (p.proname like '%" + productName + "%' or p.bh like '%" + productName + "%' or p.shortName like '%" + productName + "%')";
                        }
                        string sql = "select productID,bh,proname,progg,case when sluid=1 then '盒' when sluid=2 then '袋' when sluid=3 then '杯' when sluid=4 then '瓶' when sluid=5 then '个' when sluid=6 then '包' else '其他' end sluid,"
                     + " (select brandName from asm_brand b where b.id=p.brandID) brandName,(select typeName from asm_protype t where t.productTypeID=p.protype) typeName,"
                     + "  isnull((SELECT dgNum FROM asm_kcDetail WHERE(id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   dgNum,"
                     + "  isnull((SELECT lsNum FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   lsNum,"
                     + "  isnull((SELECT totalLsNum FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   totalLsNum,"
                     + "  isnull((SELECT imbalance FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   imbalance,"
                     + "  isnull((select COUNT(*) totalDgNum from asm_orderlistDetail o where zt<7 and mechineID in (" + mechineList + ") and o.productID = p.productID and createTime>= '" + dgStart + "'and createTime<= '" + dgEnd + "' and mechineID in (" + mechineList + ") group by productID),0) totalDgNum,"
                     + "  isnull((SELECT imbalance FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)+ "
                     + "  isnull((select COUNT(*) totalDgNum from asm_orderlistDetail o where zt<7 and mechineID in (" + mechineList + ") and o.productID = p.productID and createTime>= '" + dgStart + "'and createTime<= '" + dgEnd + "' and mechineID in (" + mechineList + ") group by productID),0) total,"
                     + "  (SELECT STUFF((SELECT ','+ldNO FROM  asm_ldInfo  WHERE mechineID in("+mechineList+") and productID = p.productID for xml  path('') ),1,1,'')) ldNO"
                     + "  from asm_product p where companyID = " + companyID + " and brandID in (" + brandList + ") and productID in(" + productID + ") "+sqlPP;
                        Util.Debuglog("sql="+sql, "上货单sql.txt");
                        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            return dt;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                return null;
            }
        }
        public DataTable getJHD(string operaID,string companyID,string lsTime,string dgStart,string dgEnd,string productName)
        {
            try
            {
                string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                        + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + companyID + " and is_del=0  GROUP BY brandID";
                DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
                if (brandDt.Rows.Count > 0)
                { 
                    string sqlOpera = "select * from asm_opera where id=" + operaID;
                    DataTable dtOpera = DbHelperSQL.Query(sqlOpera).Tables[0];
                    string sqlBrand = "SELECT  companyID ,value = ( STUFF(( SELECT    ',' + convert(varchar,id) FROM asm_brand"
                      + "  WHERE companyID = Test.companyID  FOR XML PATH('') ), 1, 1, '') )FROM asm_brand  AS Test where companyID = " + companyID + "  GROUP BY companyID";
                    string brandList = DbHelperSQL.Query(sqlBrand).Tables[0].Rows[0]["value"].ToString();
                    string sqlMechine = "SELECT  operaID ,value = ( STUFF(( SELECT    ',' + convert(varchar,id) FROM asm_mechine"
                       + " WHERE operaID = Test.operaID  FOR XML PATH('') ), 1, 1, '') )FROM asm_mechine  AS Test where companyID = " + companyID + " and operaID=" + operaID + " GROUP BY operaID";
                    string mechineList = DbHelperSQL.Query(sqlMechine).Tables[0].Rows[0]["value"].ToString();
                    string sqlP = "select * from  asm_product where brandID in (" + brandList + ")";
                    DataTable dp = DbHelperSQL.Query(sqlP).Tables[0];
                    string productID = "";
                    if (dp.Rows.Count > 0)
                    {
                        for (int i = 0; i < dp.Rows.Count; i++)
                        {
                            productID += dp.Rows[i]["productID"].ToString() + ",";
                        }
                        productID = productID.Substring(0, productID.Length - 1);

                        string sqlLd = "SELECT STUFF((SELECT ','+productID FROM  asm_ldinfo where mechineID in (" + mechineList + ")"
                            + " and productID  in(" + productID + ")  for xml path('')),1,1,'') productID";
                        DataTable dd = DbHelperSQL.Query(sqlLd).Tables[0];
                        if (dd.Rows.Count > 0)
                        {
                            productID = dd.Rows[0]["productID"].ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(mechineList))
                    {
                        string sqlPP = "";
                        if (!string.IsNullOrEmpty(productName))
                        {
                            sqlPP = " and (p.proname like '%" + productName + "%' or p.bh like '%" + productName + "%' or p.shortName like '%" + productName + "%')";
                        }
                        string sql = "select productID,bh,proname,progg,(select typeName from asm_protype t where t.productTypeID=p.protype) typeName,"
                            + " case when sluid=1 then '盒' when sluid=2 then '袋' when sluid=3 then '杯' when sluid=4 then '瓶' when sluid=5 then '个' when sluid=6 then '包' else '其他' end sluid,"
                     + " (select brandName from asm_brand b where b.id=p.brandID) brandName,"
                     + "  isnull((SELECT dgNum FROM asm_kcDetail WHERE(id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   dgNum,"
                     + "  isnull((SELECT lsNum FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   lsNum,"
                     + "  isnull((SELECT totalLsNum FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   totalLsNum,"
                     + "  isnull((SELECT imbalance FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   imbalance,"
                     + "  isnull((select COUNT(*) totalDgNum from asm_orderlistDetail o where zt<7 and mechineID in (" + mechineList + ") and o.productID = p.productID and createTime>= '" + dgStart + "'and createTime<= '" + dgEnd + "' and mechineID in (" + mechineList + ") group by productID),0) totalDgNum,"
                     + "  isnull((SELECT imbalance FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)+ "
                     + "  isnull((select COUNT(*) totalDgNum from asm_orderlistDetail o where zt<7 and mechineID in (" + mechineList + ") and o.productID = p.productID and createTime>= '" + dgStart + "'and createTime<= '" + dgEnd + "' and mechineID in (" + mechineList + ") group by productID),0) total"
                     + "  from asm_product p where companyID = " + companyID + " and brandID in (" + brandList + ") and productID in(" + productID + ") "+sqlPP;
                        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            return dt;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                return null;
            }
        }
        public DataTable getCGD(string lsTime, string dgStart, string dgEnd, string brandList, string operaList, string mechineList, string companyID,string productName)
        {
            try
            {
                if (string.IsNullOrEmpty(brandList))
                {

                    return null;
                }
                if (string.IsNullOrEmpty(operaList))
                {
                    return null;
                }
                string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                         + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + companyID + " and is_del=0 and brandID in(" + brandList + ") GROUP BY brandID; ";
                DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
                if (brandDt.Rows.Count > 0)
                {
                    string productID = "";
                    for (int i = 0; i < brandDt.Rows.Count; i++)
                    {
                        productID += brandDt.Rows[i]["value"].ToString() + ",";
                    }
                    productID = productID.Substring(0, productID.Length - 1);

                    string sqlLd = "SELECT STUFF((SELECT ','+productID FROM  asm_ldinfo where mechineID in (" + mechineList + ")"
                            + " and productID  in(" + productID + ")  for xml path('')),1,1,'') productID";
                    DataTable dd = DbHelperSQL.Query(sqlLd).Tables[0];
                    if (dd.Rows.Count > 0)
                    {
                        productID = dd.Rows[0]["productID"].ToString();
                    }

                    if (string.IsNullOrEmpty(mechineList))
                    {
                        return null;
                    }
                    string sqlP = "";
                    if (!string.IsNullOrEmpty(productName))
                    {
                        sqlP = " and (p.proname like '%" + productName + "%' or p.bh like '%" + productName + "%' or p.shortName like '%" + productName + "%')";
                    }
                    string sql = "select productID,bh,proname,progg,case when sluid=1 then '盒' when sluid=2 then '袋' when sluid=3 then '杯' when sluid=4 then '瓶' when sluid=5 then '个' when sluid=6 then '包' else '其他' end sluid,"
                         + " (select brandName from asm_brand b where b.id=p.brandID) brandName,"
                         + "  isnull((SELECT dgNum FROM asm_kcDetail WHERE(id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   dgNum,"
                         + "  isnull((SELECT lsNum FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   lsNum,"
                         + "  isnull((SELECT totalLsNum FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   totalLsNum,"
                         + "  isnull((SELECT imbalance FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)   imbalance,"
                         + "  isnull((select COUNT(*) totalDgNum from asm_orderlistDetail o where zt<7 and mechineID in (" + mechineList + ") and o.productID = p.productID and createTime>= '" + dgStart + "'and createTime<= '" + dgEnd + "' and mechineID in (" + mechineList + ") group by productID),0) totalDgNum,"
                         + "  isnull((SELECT imbalance FROM asm_kcDetail WHERE (id IN(SELECT MAX([id])   FROM asm_kcDetail where asm_kcDetail.productID = p.productID  GROUP BY productID)) and dateTime< '" + lsTime + "' and mechineID in (" + mechineList + ")),0)+ "
                         + "  isnull((select COUNT(*) totalDgNum from asm_orderlistDetail o where zt<7 and mechineID in (" + mechineList + ") and o.productID = p.productID and createTime>= '" + dgStart + "'and createTime<= '" + dgEnd + "' and mechineID in (" + mechineList + ") group by productID),0) total"
                         + "  from asm_product p where companyID = " + companyID + " and brandID in (" + brandList + ") and productID in(" + productID + ") " + sqlP;
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                }
                else
                {
                    return null;

                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        public void ExportExcel1(HttpContext context)
        {
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            StringBuilder sb = new StringBuilder();
            resp.Charset = "utf-8";
            resp.Clear();
            string filename = "采购单_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
            resp.ContentEncoding = System.Text.Encoding.UTF8;

            resp.ContentType = "application/ms-excel";
            string style = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" + 
                "<style> "
                +".table{ font: 9pt Tahoma, Verdana; "
                +"          color: #000000; "
                +"          text-align:center;" 
                +"          background-color:#8ECBE8; "
                + "         font-family:'微软雅黑';font-weight:bold;"
                + "          font-size:15px;"
                + "          }"
                +".table td{"
                +"           text-align:center;"
                +"            height:21px;"
                +"            background-color:#EFF6FF;"
                +"          }"
                +".table th{ "
                +"        font: 9pt Tahoma, Verdana;" 
                +"        color: #000000; "
                +"        font-weight: bold;" 
                +"        background-color: #8ECBEA;" 
                +"        height:25px;  "
                +"        text-align:center;" 
                +"        padding-left:10px;"
                +"}"
                +"</style>";
            resp.Write(style);
            string lsTime = context.Request["lsTime"].ToString();
            string dgStart = context.Request["dgStart"].ToString();
            string dgEnd = context.Request["dgEnd"].ToString();
            string brandList = context.Request["brandList"].ToString();
            string operaList = context.Request["operaList"].ToString();
            string mechineList = context.Request["mechineList"].ToString();
            string companyID = context.Request["companyID"].ToString();
            string productName = context.Request["productName"].ToString();
            string loginID = context.Request["loginID"].ToString();
            sb.Append("<table class='table'><tr><td colspan='6' style='text-align:left'>导出时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</td></tr><tr><td colspan='6' style='text-align:left'>零售时间:" + lsTime + "</td></tr><tr><td  colspan='6' style='text-align:left'>订购时间:" + dgStart + "~" + dgEnd + "</td></tr> "
                +"<tr><th>商品条码</th><th>品牌</th><th>商品名称</th><th>包装</th><th>规格</th><th>总数量</th></tr>");
            System.Data.DataTable dtSource = new System.Data.DataTable();
            dtSource.TableName = "statistic";
            dtSource.Columns.Add("第一列");
            dtSource.Columns.Add("第二列");
            dtSource.Columns.Add("第三列");
            dtSource.Columns.Add("第四列");
            dtSource.Columns.Add("第五列");
            dtSource.Columns.Add("第六列");

            string brandSql = "SELECT  brandID ,value = ( STUFF(( SELECT    ',' + convert(varchar,productID) FROM asm_product"
                       + " WHERE brandID = Test.brandID  FOR XML PATH('') ), 1, 1, '') )FROM asm_product  AS Test where companyID = " + companyID + " and brandID in(" + brandList + ") and is_del=0 GROUP BY brandID; ";
            string productID = "";
            DataTable brandDt = DbHelperSQL.Query(brandSql).Tables[0];
            if (brandDt.Rows.Count > 0)
            {
                for (int i = 0; i < brandDt.Rows.Count; i++)
                {
                    productID += brandDt.Rows[i]["value"].ToString() + ",";
                }
                productID = productID.Substring(0, productID.Length - 1);
                //此处目的是为了去掉历史记录里之前有的产品而现在把该产品给下架的产品ID
                string sqlLd = "SELECT STUFF((SELECT ','+productID FROM  asm_ldinfo where mechineID in (" + mechineList + ")"
                          + " and productID  in(" + productID + ")  for xml path('')),1,1,'') productID";
                DataTable dd = DbHelperSQL.Query(sqlLd).Tables[0];
                if (dd.Rows.Count > 0)
                {
                    productID = dd.Rows[0]["productID"].ToString();
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    string sqlp = "select * from  asm_product where proname like '%" + productName + "%' or bh like '%" + productName + "%' or shortName like '%" + productName + "%'";
                    DataTable dp = DbHelperSQL.Query(sqlp).Tables[0];
                    if (dp.Rows.Count > 0)
                    {
                        for (int k = 0; k < dp.Rows.Count; k++)
                        {
                            productID += dd.Rows[0]["productID"].ToString() + ",";
                        }
                        productID = productID.Substring(0, productID.Length - 1);
                    }
                }
            }
            productID = String.Join(",", productID.Split(',').Distinct<string>());




            DataTable dt = OperUtil.getCGD(mechineList,productID,dgStart,dgEnd,lsTime);
            System.Data.DataRow row = null;
            if (dt!=null&&dt.Rows.Count>0)
            {
                int totalNum = 0;
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    row = dtSource.NewRow();
                    row[0] = dt.Rows[i]["bh"].ToString();
                    row[1] = dt.Rows[i]["brandName"].ToString();
                    row[2] = dt.Rows[i]["proname"].ToString();
                    row[3] =dt.Rows[i]["sluid"].ToString();
                    row[4] = dt.Rows[i]["progg"].ToString();
                    row[5] = int.Parse(dt.Rows[i]["imbalance"].ToString())+int.Parse(dt.Rows[i]["totalDG"].ToString());
                    totalNum += int.Parse(row[5].ToString());
                    dtSource.Rows.Add(row);
                }
                foreach (DataRow tmpRow in dtSource.Rows)
                {
                    if (int.Parse(tmpRow[5].ToString()) >0)
                    {
                        sb.Append("<tr><td style=\"vnd.ms-excel.numberformat:@\">" + tmpRow[0] + "</td>");
                        sb.Append("<td>" + tmpRow[1] + "</td>");
                        sb.Append("<td>" + tmpRow[2] + "</td>");
                        sb.Append("<td>" + tmpRow[3] + "</td>");
                        sb.Append("<td>" + tmpRow[4] + "</td>");
                        sb.Append("<td>" + tmpRow[5] + "</td>");
                        sb.Append("</tr>");
                    }
                }
                sb.Append("<tr><td colspan='6'>总计:" + totalNum + "</td></tr>");
                sb.Append("<tr><td colspan='3'></td><td>操作员签名:</td><td colspan='2'></td></tr>");
                sb.Append("<table>");
                resp.Write(sb.ToString());


                context.Response.Charset = "utf-8";
                context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.ContentType = "application/ms-excel";
                context.Response.Write(style);
                var fileContents = Encoding.Default.GetBytes(sb.ToString());
                //设置excel保存到服务器的路径
                
                var filePath = context.Server.MapPath("~/excel/" + filename + ".xls");
                //保存excel到指定路径
                WriteBuffToFile(fileContents, filePath);
                //插入 到处历史记录
                string url = "http://"+HttpContext.Current.Request.Url.Host + "/excel/"+ filename+".xls";
                string insert = "insert into asm_excellist (companyID,downUrl,createTime,excelName,mechineID,operaID,type,loginID)values('" + companyID + "','"+url+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+filename+"','','',1,'"+loginID+"')";
                DbHelperSQL.ExecuteSql(insert);
                
            }
            resp.Flush();
            resp.End();
        }
        public static DataTable getProductList(string keyword, string qy, string code, string brandID)
        {
            string sql1 = " and 1=1";
            if (keyword.Trim() != "")
            {
                sql1 += " and proName like '%" + keyword + "%'";
            }
            if (qy != "0")
            {
                sql1 += " and A.companyID=" + qy;
            }
            if (!string.IsNullOrEmpty(code))
            {
                sql1 += " and A.bh='" + code + "'";
            }
            if (brandID != "0")
            {
                sql1 += " and A.brandID=" + brandID;
            }
            string sql = "select A.*,b.typeName,(select brandName from asm_brand where id=A.brandID) brandName from (select ap.*,ac.name from asm_product ap left join asm_company ac on ap.companyID=ac.id) A left join asm_protype B  on A.protype=B.productTypeID where A.is_del=0   " + sql1;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt;
               
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 写字节数组到文件
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="filePath"></param>
        public static void WriteBuffToFile(byte[] buff, string filePath)
        {
            WriteBuffToFile(buff, 0, buff.Length, filePath);
        }
        /// <summary>
        /// 写字节数组到文件
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset">开始位置</param>
        /// <param name="len"></param>
        /// <param name="filePath"></param>
        public static void WriteBuffToFile(byte[] buff, int offset, int len, string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            FileStream output = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(buff, offset, len);
            writer.Flush();
            writer.Close();
            output.Close();
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