using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TXSoftware.DataObjectsNetEntityModel.Web.Site
{
    public partial class stcc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.CacheControl = "no-cache";

            //Server.r("~/versions/latest/1.2.0.0/debug");
            //string url = "~/versions/latest/1.2.0.0/debug";
            //Response.Redirect(url ,false);
        }

        protected string GetUrlToRedirect()
        {
            string[] segments = Request.Url.Segments;
            StringBuilder sb = new StringBuilder();
            for (int i = 2; i < segments.Count(); i++)
            {
                sb.Append(segments[i]);
            }

            string urlToRedirect = string.Format("http://{0}/{1}", Request.Url.Host, sb.ToString());
            return urlToRedirect;
        }
    }
}