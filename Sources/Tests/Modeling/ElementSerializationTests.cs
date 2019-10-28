using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Dsl;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode;
using ExtensionMethods = TXSoftware.DataObjectsNetEntityModel.Dsl.ExtensionMethods;

namespace TXSoftware.DataObjectsNetEntityModel.Tests.Modeling
{
    /// <summary>
    /// Summary description for ElementSerializationTests
    /// </summary>
    [TestClass]
    public class ElementSerializationTests
    {
        #region context

        public ElementSerializationTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #endregion context

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        public void Test_Serialization_Persistent_Types()
        {
            EntityModel entityModel = ModelHelper.CreateModel();
            
            TestSerializationInterface(entityModel);
        }

        private void TestSerializationInterface(EntityModel entityModel)
        {
            const string INTERFACE1_NAME = "IEntity";
            const AccessModifier INTERFACE1_ACCESS_MODIFIER = AccessModifier.Internal;
            const string INTERFACE1_DOCUMENTATION = "Documentation 4E6C6256-A5FE-442E-AA37-2B58897D76CB";
            const string INTERFACE1_NAMESPACE = "Namespace 49341C9C-DAC5-4450-BD6F-569FE6FC85A6";

            StringTreeNode index1_keyField_Id = new StringTreeNode { DisplayValue = "Id", IconIndex = 2 };
            StringTreeNode index1_keyField_OId = new StringTreeNode { DisplayValue = "OId", IconIndex = 1 };
            StringTreeNode index1_IncludedField_Age = new StringTreeNode { DisplayValue = "Age", IconIndex = 0 };
            StringTreeNode index1_IncludedField_Weight = new StringTreeNode { DisplayValue = "Weight", IconIndex = 0 };
            Defaultable<double> index1_FillFactor = null;
            Defaultable<string> index1_IndexName = null;

            StringTreeNode index2_keyField_Id2 = new StringTreeNode { DisplayValue = "Id2", IconIndex = 1 };
            StringTreeNode index2_keyField_OId2 = new StringTreeNode { DisplayValue = "OId2", IconIndex = 2 };
            StringTreeNode index2_IncludedField_Age2 = new StringTreeNode { DisplayValue = "Age2", IconIndex = 0 };
            StringTreeNode index2_IncludedField_Weight2 = new StringTreeNode { DisplayValue = "Weight2", IconIndex = 0 };
            Defaultable<double> index2_FillFactor = null;
            Defaultable<string> index2_IndexName = null;

            Interface interface1 = CreateElement(entityModel,
                delegate(Store store)
                {
                    Interface result = new Interface(store);
                    result.Name = INTERFACE1_NAME;
                    result.Access = INTERFACE1_ACCESS_MODIFIER;
                    result.Documentation = INTERFACE1_DOCUMENTATION;
                    result.Namespace = INTERFACE1_NAMESPACE;

                    EntityIndex index1 = new EntityIndex(store);
                    result.Indexes.Add(index1);

                    index1.Fields.KeyFields.Add(index1_keyField_Id);
                    index1.Fields.KeyFields.Add(index1_keyField_OId);
                    index1.Fields.IncludedFields.Add(index1_IncludedField_Age);
                    index1.Fields.IncludedFields.Add(index1_IncludedField_Weight);
                    
                    index1.FillFactor.SetAsCustom(123.45);
                    index1_FillFactor = (Defaultable<double>) index1.FillFactor.Clone();
                    
                    index1.IndexName.SetAsCustom("idxPrimary");
                    index1_IndexName = (Defaultable<string>)index1.IndexName.Clone();

                    EntityIndex index2 = new EntityIndex(store);
                    result.Indexes.Add(index2);

                    index2.Fields.KeyFields.Add(index2_keyField_Id2);
                    index2.Fields.KeyFields.Add(index2_keyField_OId2);
                    index2.Fields.IncludedFields.Add(index2_IncludedField_Age2);
                    index2.Fields.IncludedFields.Add(index2_IncludedField_Weight2);
                    
                    index2.FillFactor.SetAsCustom(45.123);
                    index2_FillFactor = (Defaultable<double>) index2.FillFactor.Clone();

                    index2.IndexName.SetAsCustom("idxSecondary");
                    index2_IndexName = (Defaultable<string>) index2.IndexName.Clone();

                    result.AddScalarProperty("Id", typeof (Int32));

                    return result;
                });

            Assert.AreEqual(entityModel.PersistentTypes.Count, 1);

            Interface clonedInterface1 = TestSerializationByClone(interface1);

            Assert.AreEqual(clonedInterface1.Name, INTERFACE1_NAME);
            Assert.AreEqual(clonedInterface1.Access, INTERFACE1_ACCESS_MODIFIER);
            Assert.AreEqual(clonedInterface1.Documentation, INTERFACE1_DOCUMENTATION);
            Assert.AreEqual(clonedInterface1.Namespace, INTERFACE1_NAMESPACE);

            Assert.AreEqual(clonedInterface1.Indexes.Count, 2);

            EntityIndex clonedIndex1 = clonedInterface1.Indexes[0];
            Assert.AreEqual(clonedIndex1.Fields.KeyFields.Count, 2);
            Assert.AreEqual(clonedIndex1.Fields.IncludedFields.Count, 2);
            Assert.IsTrue(clonedIndex1.FillFactor.EqualsTo(index1_FillFactor));
            Assert.IsTrue(clonedIndex1.IndexName.EqualsTo(index1_IndexName));
            
            StringTreeNode clonedIndex1_keyField_Id = clonedIndex1.Fields.KeyFields[0];
            Assert.AreEqual(clonedIndex1_keyField_Id.DisplayValue, index1_keyField_Id.DisplayValue);
            Assert.AreEqual(clonedIndex1_keyField_Id.IconIndex, index1_keyField_Id.IconIndex);

            StringTreeNode clonedIndex1_keyField_OId = clonedIndex1.Fields.KeyFields[1];
            Assert.AreEqual(clonedIndex1_keyField_OId.DisplayValue, index1_keyField_OId.DisplayValue);
            Assert.AreEqual(clonedIndex1_keyField_OId.IconIndex, index1_keyField_OId.IconIndex);

            StringTreeNode clonedIndex1_IncludedField_Age = clonedIndex1.Fields.IncludedFields[0];
            Assert.AreEqual(clonedIndex1_IncludedField_Age.DisplayValue, index1_IncludedField_Age.DisplayValue);
            Assert.AreEqual(clonedIndex1_IncludedField_Age.IconIndex, index1_IncludedField_Age.IconIndex);

            StringTreeNode clonedIndex1_IncludedField_Weight = clonedIndex1.Fields.IncludedFields[1];
            Assert.AreEqual(clonedIndex1_IncludedField_Weight.DisplayValue, index1_IncludedField_Weight.DisplayValue);
            Assert.AreEqual(clonedIndex1_IncludedField_Weight.IconIndex, index1_IncludedField_Weight.IconIndex);

            EntityIndex clonedIndex2 = clonedInterface1.Indexes[1];
            Assert.AreEqual(clonedIndex2.Fields.KeyFields.Count, 2);
            Assert.AreEqual(clonedIndex2.Fields.IncludedFields.Count, 2);
            Assert.IsTrue(clonedIndex2.FillFactor.EqualsTo(index2_FillFactor));
            Assert.IsTrue(clonedIndex2.IndexName.EqualsTo(index2_IndexName));

            StringTreeNode clonedIndex2_keyField_Id2 = clonedIndex2.Fields.KeyFields[0];
            Assert.AreEqual(clonedIndex2_keyField_Id2.DisplayValue, index2_keyField_Id2.DisplayValue);
            Assert.AreEqual(clonedIndex2_keyField_Id2.IconIndex, index2_keyField_Id2.IconIndex);

            StringTreeNode clonedIndex2_keyField_OId2 = clonedIndex2.Fields.KeyFields[1];
            Assert.AreEqual(clonedIndex2_keyField_OId2.DisplayValue, index2_keyField_OId2.DisplayValue);
            Assert.AreEqual(clonedIndex2_keyField_OId2.IconIndex, index2_keyField_OId2.IconIndex);

            StringTreeNode clonedIndex2_IncludedField_Age2 = clonedIndex2.Fields.IncludedFields[0];
            Assert.AreEqual(clonedIndex2_IncludedField_Age2.DisplayValue, index2_IncludedField_Age2.DisplayValue);
            Assert.AreEqual(clonedIndex2_IncludedField_Age2.IconIndex, index2_IncludedField_Age2.IconIndex);

            StringTreeNode clonedIndex2_IncludedField_Weight2 = clonedIndex2.Fields.IncludedFields[1];
            Assert.AreEqual(clonedIndex2_IncludedField_Weight2.DisplayValue, index2_IncludedField_Weight2.DisplayValue);
            Assert.AreEqual(clonedIndex2_IncludedField_Weight2.IconIndex, index2_IncludedField_Weight2.IconIndex);
        }

        private T TestSerializationByClone<T>(T originalElement) where T: PersistentType
        {
            string originalXml = ExtensionMethods.SerializeToString(originalElement);
            T deserializedElement = ExtensionMethods.DeserializeFromString<T>(originalElement.Store, originalElement.GetDomainClass().Id, originalXml);
            Assert.IsNotNull(deserializedElement);
            string clonedXml = ExtensionMethods.SerializeToString(deserializedElement);
            Assert.IsTrue(Util.StringEqual(originalXml, clonedXml, true));
            return deserializedElement;
        }

        private T CreateElement<T>(EntityModel entityModel, Func<Store, T> createElementFunc) where T: PersistentType
        {
            T result = null;
            entityModel.Store.MakeActionWithinTransaction("Creating element",
                delegate
                {
                    result = createElementFunc(entityModel.Store);
                    entityModel.PersistentTypes.Add(result);
                });

            return result;
        }
    }
}
