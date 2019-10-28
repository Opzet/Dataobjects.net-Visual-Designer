using System;
using System.IO;
using System.Net;
using System.Web;
using TXSoftware.DataObjectsNetEntityModel.Web.Shared;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Web.Site.Properties;

namespace TXSoftware.DataObjectsNetEntityModel.Web.Site.App_Data
{
    /// <summary>
    /// Summary description for Versions
    /// </summary>
    public class VersionsHandler : IHttpHandler
    {
        private ProductVersionsDto GetAllProductVersions(string fileName)
        {
            ProductVersionsDto result = HttpContext.Current.Application.Get("AllProductVersions") as ProductVersionsDto;
            if (result == null)
            {
                try
                {
                    string content = File.ReadAllText(fileName);
                    result = SerializeUtils.DeserializeDataContract<ProductVersionsDto>(content);
                }
                catch
                {}

                if (result == null)
                {
                    result = new ProductVersionsDto();
                }

                HttpContext.Current.Application.Set("AllProductVersions", result);
            }


            return result;
        }

        public void ProcessRequest(HttpContext context)
        {
            var routeValues = context.Request.RequestContext.RouteData.Values;
            string mode = routeValues["mode"] as string;
            string currentversion = routeValues["currentversion"] as string;
            string isDebugStr = routeValues["isDebug"] as string;
            bool isDebug = !string.IsNullOrEmpty(isDebugStr) && isDebugStr.ToLower() == "debug";

            WriteCounterScript(context);

            if (mode.ToLower() == "latest")
            {
                GetLatestVersion(context, currentversion, isDebug);
            }

            context.Response.Flush();
        }

        private void WriteCounterScript(HttpContext context)
        {
            //string urlToStatCounter = string.Format("http://{0}/stcc.aspx", context.Request.Url.Host);
//            string urlToStatCounter = "http://doemd.aspone.cz/stcc.aspx";
//            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(urlToStatCounter);
//            request.BeginGetResponse(ar => request.EndGetResponse(ar), null);
        }

        private void GetLatestVersion(HttpContext context, string currentversion, bool isDebug)
        {
            context.Response.ContentType = "text/plain";
            string result = string.Empty;

            if (string.IsNullOrEmpty(currentversion))
            {
                result = isDebug ? "Unknown current version" : string.Empty;
            }
            else
            {
                string localFile = context.Server.MapPath("~/Versions/doemd-versions.xml");
                if (File.Exists(localFile))
                {
                    try
                    {
                        VersionNumber currentVersionNumber = new VersionNumber(currentversion);

                        ProductVersionsDto allProductVersions = GetAllProductVersions(localFile);
                        ProductVersionDto foundProductVersion =
                            (from productVersion in allProductVersions
                             let versionNumber = new VersionNumber(productVersion.Version)
                             where versionNumber.CompareTo(currentVersionNumber) == 1
                             orderby versionNumber descending 
                             select productVersion).FirstOrDefault();

                        if (foundProductVersion == null)
                        {
                            result = isDebug ? "Current version is actual latest version." : string.Empty;
                        }
                        else
                        {
                            result = foundProductVersion.ToXml();
                            context.Response.ContentType = "text/xml";
                        }
                    }
                    catch (Exception e)
                    {
                        result = isDebug ? string.Format("Error: {0}", e.Message) : string.Empty;
                    }
                }
                else
                {
                    result = isDebug ? "Error processing request #003" : string.Empty;
                }
            }

            context.Response.Write(result);
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