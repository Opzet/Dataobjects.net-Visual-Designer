using System;
using System.Text;

namespace TXSoftware.DataObjectsNetEntityModel.Web.Site
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
/*
            lbTime.Text = DateTime.Now.ToString();
            StringBuilder sb = new StringBuilder();
            foreach (var key in this.Request.ServerVariables.AllKeys)
            {
                sb.AppendFormat("{0}={1}", key, this.Request.ServerVariables[key]);
                sb.Append("<br/>");
            }

            

            lbTime.Text = sb.ToString();

            bool canRebuildDB = false;

            var iddqd = Request.QueryString["iddqd"];
            if (iddqd != null && iddqd == "true")
            {
                canRebuildDB = true;
            }
            
            if (canRebuildDB)
            {
                DomainBuilder.Build(true);

                lbTime.Text = "Rebuild DB done.";
            }
*/
        }
    }
}