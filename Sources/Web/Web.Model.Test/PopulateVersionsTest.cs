using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TXSoftware.DataObjectsNetEntityModel;
using TXSoftware.DataObjectsNetEntityModel.Web.Model;
using TXSoftware.DataObjectsNetEntityModel.Web.Shared;
using Xtensive.Orm;

namespace Web.Model.Test
{
    public class PopulateVersionsTest : AutoBuildTest
    {
        [Test]
        public void Test()
        {
            // Creating entity
            using (var session = Domain.OpenSession())
            {
                using (var transactionScope = session.OpenTransaction())
                {
                    //AddVersion_1_0_0_0(session);
                    AddVersion_1_0_5_0(session);

                    transactionScope.Complete();
                }
            }

            // Checking the result entity
            using (var session = Domain.OpenSession())
            {
                using (var transactionScope = session.OpenTransaction())
                {
                    var productVersions = session.Query.All<ProductVersion>().ToList();
                    var productVersionDtos = productVersions.TransformToDto<ProductVersion, ProductVersionDto>(session);
                    ProductVersionsDto versionsBag = new ProductVersionsDto(productVersionDtos);
                    string xml = SerializeUtils.SerializeDataContract(versionsBag);
                    File.WriteAllText(@"c:\doemd-versions.xml", xml);

                    var dataContract = SerializeUtils.DeserializeDataContract<ProductVersionsDto>(xml);

                    transactionScope.Complete();
                }
            }

            // Writing message to log
            Log.Info("Test passed.");
        }

        private static void AddVersion_1_0_0_0(Session session)
        {
            ProductVersion productVersion = new ProductVersion(session, "1.0.0.0")
                {
                    Private = false,
                    IsLatest = false,
                    Published = new DateTime(2011, 03, 24),
                    PublishedOffset = TimeZoneInfo.Local.BaseUtcOffset,
                    Url = "http://doemd.codeplex.com/releases/view/63159",
                    Description = "Alpha version 1.0.0.0 of DataObjects.Net Entity Model Designer"
                };

            var productVersionChange = new ProductVersionChange(session)
                {
                    Order = 0,
                    Type = ProductVersionChangeType.Feature,
                    Title = "DSL Desginer",
                    Content = "DSL Designer with basic modeling of http://www.DataObjects.Net persistent types: Interface, Entity, Structure, Typed EntitySet"
                };

            productVersion.Changes.Add(productVersionChange);
            productVersionChange = new ProductVersionChange(session)
                {
                    Order = 1,
                    Type = ProductVersionChangeType.Feature,
                    Title = "T4 Templates",
                    Content = "T4 template generator for generate http://www.DataObjects.Net persistent classes from model file(s)"
                };
            productVersion.Changes.Add(productVersionChange);
        }
        
        private static void AddVersion_1_0_5_0(Session session)
        {
            ProductVersion productVersion = new ProductVersion(session, "1.0.5.0")
                {
                    Private = false,
                    IsLatest = true,
                    Published = DateTime.Now,
                    PublishedOffset = TimeZoneInfo.Local.BaseUtcOffset,
                    Url = "http://doemd.codeplex.com/releases/view/63777",
                    Description = "Alpha version 1.0.5.0 of DataObjects.Net Entity Model Designer"
                };

            var productVersionChange = new ProductVersionChange(session)
            {
                Order = 0,
                Type = ProductVersionChangeType.Issue,
                Title = "Unable to add boolean as type for scalar property",
                WorkItemId = "8139",
                Url = "http://doemd.codeplex.com/workitem/8139",
                Content = "Definition of scalar property type is redesigned, now is able to define boolean, timespan or any other external type."
            };
            productVersion.Changes.Add(productVersionChange);


            productVersionChange = new ProductVersionChange(session)
            {
                Order = 1,
                Type = ProductVersionChangeType.Issue,
                Title = "Create 'ToSelf' association creates duplicate properties",
                WorkItemId = "8169",
                Url = "http://doemd.codeplex.com/workitem/8169",
                Content = "When creating association (from dialog) and pointing properties to same type member 'ToSelf', then it generate 2 properties with same name and type in entity."
            };
            productVersion.Changes.Add(productVersionChange);


            productVersionChange = new ProductVersionChange(session)
            {
                Order = 2,
                Type = ProductVersionChangeType.Issue,
                Title = "Allow define Key settings on navigation property",
                WorkItemId = "8221",
                Url = "http://doemd.codeplex.com/workitem/8221",
                Content = "Add possibility to define Key settings on navigation properties."
            };
            productVersion.Changes.Add(productVersionChange);


            productVersionChange = new ProductVersionChange(session)
            {
                Order = 3,
                Type = ProductVersionChangeType.Issue,
                Title = "Allow to set access modifier on get/set of property",
                WorkItemId = "8222",
                Url = "http://doemd.codeplex.com/workitem/8222",
                Content = "Allow to set access modifier on get/set of property."
            };
            productVersion.Changes.Add(productVersionChange);


            productVersionChange = new ProductVersionChange(session)
            {
                Order = 4,
                Type = ProductVersionChangeType.Issue,
                Title = "Allow source of association to be Structure",
                WorkItemId = "8228",
                Url = "http://doemd.codeplex.com/workitem/8228",
                Content = "In 'Add Association' dialog you can now specify Structure type in source of association."
            };
            productVersion.Changes.Add(productVersionChange);





            productVersionChange = new ProductVersionChange(session)
            {
                Order = 0,
                Type = ProductVersionChangeType.Feature,
                Title = "Update T4 templates to generate also POCO classes",
                WorkItemId = "8140",
                Url = "http://doemd.codeplex.com/workitem/8140",
                Content = "T4 template (only C# project item template) was updated to generate DTO classes as an mirror to generated 'real' entity classes defined in entity model"
            };
            productVersion.Changes.Add(productVersionChange);


            productVersionChange = new ProductVersionChange(session)
            {
                Order = 1,
                Type = ProductVersionChangeType.Feature,
                Title = "Update T4 templates to decorate classes with DataContract and DataMember attributes",
                WorkItemId = "8142",
                Url = "http://doemd.codeplex.com/workitem/8142",
                Content = "T4 template (only C# project item template) was updated to decorate generated entity or DTO classes with [DataContract] and [DataMember] attributes required for WCF."
            };
            productVersion.Changes.Add(productVersionChange);


            productVersionChange = new ProductVersionChange(session)
            {
                Order = 2,
                Type = ProductVersionChangeType.Feature,
                Title = "Introduce simple 'external type' definition for scalar properties",
                WorkItemId = "8181",
                Url = "http://doemd.codeplex.com/workitem/8181",
                Content = "User should define external type (simple - by defining namespace name and type name, like DSL designer does) which can be used as type for scalar properties."
            };
            productVersion.Changes.Add(productVersionChange);


            productVersionChange = new ProductVersionChange(session)
            {
                Order = 3,
                Type = ProductVersionChangeType.Feature,
                Title = "Introduce DataContract and DataMember settings in Designer",
                WorkItemId = "8292",
                Url = "http://doemd.codeplex.com/workitem/8292",
                Content = "Introduce [DataContract] settings(like IsReference and others) for persistent types and [DataMember] settings for propertiers."
            };
            productVersion.Changes.Add(productVersionChange);


            productVersionChange = new ProductVersionChange(session)
            {
                Order = 4,
                Type = ProductVersionChangeType.Feature,
                Title = "Redesign Add Association dialog to be simpler",
                WorkItemId = "8325",
                Url = "http://doemd.codeplex.com/workitem/8325",
                Content = "Redesign Add Association dialog to be simpler, there will be 2 modes of defining new association: Simple and Advanced."
            };
            productVersion.Changes.Add(productVersionChange);

            productVersionChange = new ProductVersionChange(session)
            {
                Order = 5,
                Type = ProductVersionChangeType.Feature,
                Title = "New version check on opening designer",
                WorkItemId = "8084",
                Url = "http://doemd.codeplex.com/workitem/8084",
                Content = "New version check on opening designer."
            };
            productVersion.Changes.Add(productVersionChange);
        }
    }
}