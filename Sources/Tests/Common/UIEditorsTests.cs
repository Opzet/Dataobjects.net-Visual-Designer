using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;

namespace Tests.Common
{
    [TestClass]
    public class UIEditorsTests
    {
        // Use TestInitialize to run code before running each test 
         [TestInitialize()]
         public void MyTestInitialize()
         {
             Application.EnableVisualStyles();
         }

        [TestMethod]
        public void Test_AddAssocation_New()
        {
            PersistentTypeItem[] existingTypes =
                new PersistentTypeItem[]
                {
                    new PersistentTypeItem("User", EntityKind.Entity, new []{"Id", "Name"}), 
                    new PersistentTypeItem("Car", EntityKind.Entity, new []{"Id", "Name", "Brand"}), 
                    new PersistentTypeItem("Address", EntityKind.Structure, new []{"City", "Street"}), 
                };
            string[] existingAssociations = new[]
                                            {
                                                "User1_Association"
                                            };

            FormAddAssociation.ResultData resultData;
            bool dialogShow = FormAddAssociation.DialogShow(existingTypes, existingAssociations, null, out resultData);
        }
    }
}