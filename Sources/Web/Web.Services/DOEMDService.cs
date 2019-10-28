/*
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using TXSoftware.DataObjectsNetEntityModel.Web.Model;
using TXSoftware.DataObjectsNetEntityModel.Web.Shared.Service;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Web.Services
{
    //[ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true)]
    public class ManagementService : IManagementService
    {
        public ProductVersionDto GetProductVersion(string version)
        {
            ProductVersionDto productVersion = VersionManager.GetProductVersion(version);

            #region temporary, remove in final 

            if (productVersion == null)
            {
                productVersion = new ProductVersionDto("0.0.0.0");
            }

            #endregion temporary, remove in final

            return productVersion;
        }

        public IEnumerable<ProductVersionDto> GetProductVersions(bool withPrivate)
        {
            var productVersions = VersionManager.GetProductVersions(withPrivate);

            #region temporary, remove in final

            if (productVersions.Count() == 0)
            {
                List<ProductVersionDto> list = new List<ProductVersionDto>();
                list.Add(new ProductVersionDto("0.0.0.0"));
                list.Add(new ProductVersionDto("10.5.3.1"));
                productVersions = list;
            }

            #endregion temporary, remove in final

            return productVersions;
        }
    }
}
*/
