using System;
using System.Collections.Generic;
using Xtensive.Orm;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Web.Model.BLL
{
    public static class VersionManager
    {
        private static TResult Transaction<TResult>(Func<Session, TResult> action)
        {
            TResult result = default(TResult);
            Transaction(session =>
            {
                result = action(session);
            });

            return result;
        }

        private static void Transaction(Action<Session> action)
        {
            var session = Session.Demand();
            using (var transactionScope = session.OpenTransaction())
            {
                action(session);

                transactionScope.Complete();
            }
        }

        public static ProductVersionDto GetProductVersion(string version)
        {
            var result = 
                Transaction(session =>
                        {
                            ProductVersion productVersion =
                                session.Query.All<ProductVersion>().SingleOrDefault(
                                    versionItem => string.Compare(versionItem.Version, version, true) == 0);

                            if (productVersion != null)
                            {
                                return productVersion.TransformToDto<ProductVersionDto>(session);
                            }

                            return null;
                        });

            return result;
        }

        public static IEnumerable<ProductVersionDto> GetProductVersions(bool withPrivate)
        {
            var result =
                Transaction(session =>
                            {
                                var productVersions = session.Query.All<ProductVersion>()
                                    .Where(versionItem => withPrivate || !versionItem.Private).ToList();

                                return productVersions.TransformToDto<ProductVersion, ProductVersionDto>(session);
                            });

            return result;
        }
    }
}