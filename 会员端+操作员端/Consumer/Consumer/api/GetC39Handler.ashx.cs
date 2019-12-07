using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace tm.cls
{
    /// <summary>
    /// GetC39Handler 的摘要说明
    /// </summary>
    public class GetC39Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string OrderNo = context.Request.Params["code"];
            Code39 _Code39 = new Code39();
            _Code39.Height = 100;
            _Code39.Magnify = 2;
            _Code39.ViewFont = new Font("Arial", 12);
            System.Drawing.Image _CodeImage = _Code39.GetCodeImage(OrderNo, Code39.Code39Model.Code39Normal, true);
            System.IO.MemoryStream _Stream = new System.IO.MemoryStream();
            _CodeImage.Save(_Stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            context.Response.ContentType = "image/tiff";
            context.Response.Clear();
            context.Response.BufferOutput = true;
            context.Response.BinaryWrite(_Stream.GetBuffer());
            context.Response.Flush();
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