using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace TXSoftware.DataObjectsNetEntityModel.Web.Shared.Service
{
    //[ServiceContract(Name = "ManagementService")]
    [ServiceContract]
    public interface IManagementService
    {
        /// <summary>
        /// Gets the detailed information about product version specified by version string.
        /// </summary>
        /// <param name="version">The version string in format Major.Minor.Build.Rev , e.g.: 1.0.1.0</param>
        /// <returns>
        /// Detailed information about product version as <see cref="ProductVersionDto"/> object or <c>null</c> if version does not exist.
        /// </returns>
        /// <remarks>
        /// To get latest product version, parameter <paramref name="version"/> must be null or "" (empty string).
        /// </remarks>
        [WebInvoke(UriTemplate = "productversion/{version}", Method = "GET", RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        //[OperationContract(Name = "GetProductVersion")]
        ProductVersionDto GetProductVersion(string version);

        /// <summary>
        /// Gets the all product versions.
        /// </summary>
        /// <param name="withPrivate"></param>
        /// <returns>
        /// Detailed information about all product versions as collection of <see cref="ProductVersionDto"/> objects.
        /// </returns>
        [WebInvoke(UriTemplate = "productversions?withPrivate={withPrivate}", Method = "GET", RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        //[OperationContract(Name = "GetProductVersions")]
        IEnumerable<ProductVersionDto> GetProductVersions(bool withPrivate);
    }
}