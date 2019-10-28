using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace Tests.Common
{
    /// <summary>
    /// Summary description for OrmAttributesTests
    /// </summary>
    [TestClass]
    public class OrmAttributesTests
    {
        #region context

        public OrmAttributesTests()
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
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
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

        #region Test_OrmAttributes_Serialization

        [TestMethod]
        public void Test_OrmAttributes_Serialization()
        {
            TestSerializationForPropetyContraints();

            TestSerializationForOrmAssociationEnd();
            TestSerializationForAssociationInfo();
            TestSerializationForOrmFieldAttribute();
            TestSerializationForOrmHierarchyRootAttribute();
            TestSerializationForOrmIndexFields();
            TestSerializationForOrmKeyAttribute();
            //TestSerializationForOrmTableMappingAttribute();
            TestSerializationForOrmTypeDiscriminatorValueAttribute();
        }
        
        public void TestSerializationForPropetyContraints()
        {
            OrmPropertyConstraints constraints = new OrmPropertyConstraints();

            SetPropertyConstraintsValues(constraints);

            var cloned = constraints.CloneBySerialization();
            Assert.IsNotNull(cloned);
        }

        private static void SetPropertyConstraintsValues(OrmPropertyConstraints constraints)
        {
            foreach (var constraint in constraints.AllConstraints)
            {
                constraint.Used = true;
                constraint.Mode = PropertyConstrainMode.OnSetValue;
                constraint.ErrorMessage = string.Format("error for {0}", constraint.ConstrainType);
            }
            var lengthConstraint = constraints.LengthConstraint;
            lengthConstraint.Min.SetAsDefault();
            lengthConstraint.Max.SetAsCustom(10);
            var rangeConstraint = constraints.RangeConstraint;
            rangeConstraint.Min.Enabled = true;
            rangeConstraint.Min.Value.Value = 120;
            rangeConstraint.Max.Enabled = false;
            var regexConstraint = constraints.RegexConstraint;
            regexConstraint.Pattern = "1234@11";
            regexConstraint.Options = RegexOptions.ECMAScript | RegexOptions.IgnoreCase | RegexOptions.Singleline;
        }

        #region TestSerializationForOrmTypeDiscriminatorValueAttribute

        private void TestSerializationForOrmTypeDiscriminatorValueAttribute()
        {
            const string referentialXml =
                "<content valueType=\"Enabled\"><content default=\"1\"><value useCustomExpression=\"0\"><Value type=\"System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">b4d9f48d-ec7a-4086-aef4-1e8e439f3865</Value></value></content></content>";

            OrmTypeDiscriminatorValueAttribute originalAttribute = CreateOrmTypeDiscriminatorValueAttribute(
                TestAttributeValuesType.Values1);

            string xml = originalAttribute.SerializeToString();
            Assert.IsTrue(Util.StringEqual(xml, referentialXml, true),
                "Serialized xml form of 'OrmTypeDiscriminatorValueAttribute' type differs from referential xml.");

            OrmTypeDiscriminatorValueAttribute clonedAttribute = null;

            Action cloneAttribute =
                () => clonedAttribute = (OrmTypeDiscriminatorValueAttribute)originalAttribute.Clone();
            cloneAttribute();
            Assert.IsTrue(originalAttribute.EqualsTo(clonedAttribute));

            Action testClonedNotEquals = () => Assert.IsFalse(originalAttribute.EqualsTo(clonedAttribute));

            cloneAttribute();
            clonedAttribute.Default = false;
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Enabled = false;
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Value.UseCustomExpression = true;
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Value.CustomExpression = "System.Color";
            testClonedNotEquals();
        }

        #endregion TestSerializationForOrmTypeDiscriminatorValueAttribute

        #region TestSerializationForOrmKeyAttribute

        private void TestSerializationForOrmKeyAttribute()
        {
            const string referentialXml =
                "<key valueType=\"Enabled\"><direction valueType=\"Custom\"><value>Negative</value></direction><position valueType=\"Custom\" value=\"45\" /></key>";

            OrmKeyAttribute originalAttribute = CreateOrmKeyAttribute(TestAttributeValuesType.Values1);

            string xml = originalAttribute.SerializeToString();
            Assert.IsTrue(Util.StringEqual(xml, referentialXml, true),
                "Serialized xml form of 'OrmKeyAttribute' type differs from referential xml.");

            OrmKeyAttribute clonedAttribute = null;

            Action cloneAttribute = () => clonedAttribute = (OrmKeyAttribute)originalAttribute.Clone();
            cloneAttribute();
            Assert.IsTrue(originalAttribute.EqualsTo(clonedAttribute));

            Action testClonedNotEquals = () => Assert.IsFalse(originalAttribute.EqualsTo(clonedAttribute));

            cloneAttribute();
            clonedAttribute.Enabled = false;
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Direction.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Direction.SetAsCustom(KeyDirection.Positive);
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Position.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Position.SetAsCustom(int.MinValue);
            testClonedNotEquals();
        }

        #endregion TestSerializationForOrmKeyAttribute

        #region TestSerializationForOrmIndexFields

        private void TestSerializationForOrmIndexFields()
        {
            const string referentialXml =
                "<fields><keyFields><item value=\"Field3\" image=\"2\" order=\"0\" /><item value=\"Field4\" image=\"1\" order=\"1\" /></keyFields><includedFields><item value=\"Field1\" image=\"1\" order=\"0\" /><item value=\"Field2\" image=\"2\" order=\"1\" /></includedFields></fields>";

            OrmIndexFields originalAttribute = CreateOrmIndexFields(TestAttributeValuesType.Values1);

            string xml = originalAttribute.SerializeToString();
            Assert.IsTrue(Util.StringEqual(xml, referentialXml, true),
                "Serialized xml form of 'OrmIndexFields' type differs from referential xml.");

            OrmIndexFields clonedAttribute = null;

            Action cloneAttribute = () => clonedAttribute = (OrmIndexFields)originalAttribute.Clone();
            cloneAttribute();
            Assert.IsTrue(originalAttribute.EqualsTo(clonedAttribute));

            Action testClonedNotEquals = () => Assert.IsFalse(originalAttribute.EqualsTo(clonedAttribute));

            cloneAttribute();
            clonedAttribute.KeyFields.RemoveAt(0);
            testClonedNotEquals();

            for (int i = 0; i < 2; i++)
            {
                cloneAttribute();
                clonedAttribute.KeyFields[i].DisplayValue = Guid.NewGuid().ToString();
                testClonedNotEquals();

                cloneAttribute();
                clonedAttribute.KeyFields[i].IconIndex = 10;
                testClonedNotEquals();
            }

            cloneAttribute();
            clonedAttribute.IncludedFields.RemoveAt(0);
            testClonedNotEquals();

            for (int i = 0; i < 2; i++)
            {
                cloneAttribute();
                clonedAttribute.IncludedFields[i].DisplayValue = Guid.NewGuid().ToString();
                testClonedNotEquals();

                cloneAttribute();
                clonedAttribute.IncludedFields[i].IconIndex = 99;
                testClonedNotEquals();
            }
        }

        #endregion TestSerializationForOrmIndexFields

        #region TestSerializationForOrmHierarchyRootAttribute

        private void TestSerializationForOrmHierarchyRootAttribute()
        {
            const string referentialXml =
                "<content valueType=\"Enabled\"><value inheritanceSchema=\"ConcreteTable\"><includeTypeId valueType=\"Custom\" value=\"False\" /><mappingName valueType=\"Custom\"><value>tTable1</value></mappingName></value></content>";

            OrmHierarchyRootAttribute originalAttribute = CreateOrmHierarchyRootAttribute(
                TestAttributeValuesType.Values1);

            string xml = originalAttribute.SerializeToString();
            Assert.IsTrue(Util.StringEqual(xml, referentialXml, true),
                "Serialized xml form of 'OrmHierarchyRootAttribute' type differs from referential xml.");

            OrmHierarchyRootAttribute clonedAttribute = null;

            Action cloneAttribute = () => clonedAttribute = (OrmHierarchyRootAttribute)originalAttribute.Clone();
            cloneAttribute();
            Assert.IsTrue(originalAttribute.EqualsTo(clonedAttribute));

            Action testClonedNotEquals = () => Assert.IsFalse(originalAttribute.EqualsTo(clonedAttribute));

            clonedAttribute.IncludeTypeId.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.InheritanceSchema = HierarchyRootInheritanceSchema.ClassTable;
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.MappingName.SetAsCustom("123");
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.MappingName.SetAsDefault();
            testClonedNotEquals();
        }

        #endregion TestSerializationForOrmHierarchyRootAttribute

        #region TestSerializationForOrmFieldAttribute

        private void TestSerializationForOrmFieldAttribute()
        {
            const string referentialXml =
                "<field><mappingName valueType=\"Custom\"><value>tField</value></mappingName><indexed valueType=\"Custom\" value=\"False\" /><lazyLoad valueType=\"Custom\" value=\"True\" /><length valueType=\"Custom\" value=\"123456789\" /><nullable valueType=\"Custom\" value=\"True\" /><nullableOnUpgrade valueType=\"Custom\" value=\"False\" /><precision valueType=\"Custom\" value=\"998877\" /><scale valueType=\"Custom\" value=\"555\" /><typeDiscriminator valueType=\"Custom\" value=\"True\" /><version valueType=\"Custom\"><value>Skip</value></version><defaultValue valueType=\"Enabled\"><content><value useCustomExpression=\"0\"><Value type=\"System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">b4d9f48d-ec7a-4086-aef4-1e8e439f3865</Value></value></content></defaultValue><constraints><constraint type=\"0\" valueType=\"Used\"><content mode=\"1\"><error message=\"error for Email\" /></content></constraint><constraint type=\"1\" valueType=\"Used\"><content mode=\"1\"><error message=\"error for Future\" /></content></constraint><constraint type=\"2\" valueType=\"Used\"><content mode=\"1\" max=\"10\"><error message=\"error for Length\" /></content></constraint><constraint type=\"3\" valueType=\"Used\"><content mode=\"1\"><error message=\"error for NotEmpty\" /></content></constraint><constraint type=\"4\" valueType=\"Used\"><content mode=\"1\"><error message=\"error for NotNull\" /></content></constraint><constraint type=\"5\" valueType=\"Used\"><content mode=\"1\"><error message=\"error for NotNullOrEmpty\" /></content></constraint><constraint type=\"6\" valueType=\"Used\"><content mode=\"1\"><error message=\"error for Past\" /></content></constraint><constraint type=\"7\" valueType=\"Used\"><content mode=\"1\"><error message=\"error for Range\" /><min valueType=\"Enabled\"><content><value useCustomExpression=\"0\"><Value type=\"System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">120</Value></value></content></min><max valueType=\"Disabled\" /></content></constraint><constraint type=\"8\" valueType=\"Used\"><content mode=\"1\" pattern=\"1234@11\" options=\"273\"><error message=\"error for Regex\" /></content></constraint></constraints></field>";

            OrmFieldAttribute originalAttribute = CreateOrmFieldAttribute(TestAttributeValuesType.Values1);
            //SetPropertyConstraintsValues(originalAttribute.Constraints);

            string xml = originalAttribute.SerializeToString();
            Assert.IsTrue(Util.StringEqual(xml, referentialXml, true),
                "Serialized xml form of 'OrmFieldAttribute' type differs from referential xml.");

            OrmFieldAttribute clonedAttribute = null;

            Action cloneAttribute = () => clonedAttribute = originalAttribute.Clone();
            cloneAttribute();
            Assert.IsTrue(originalAttribute.EqualsTo(clonedAttribute));

            Action testClonedNotEquals = () => Assert.IsFalse(originalAttribute.EqualsTo(clonedAttribute));

            cloneAttribute();
            clonedAttribute.DefaultValue.Enabled = false;
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.DefaultValue.Value.UseCustomExpression = true;
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.DefaultValue.Value.CustomExpression = "System.Color";
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Indexed.SetAsCustom(true);
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Indexed.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.LazyLoad.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Length.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Length.SetAsCustom(120);
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.MappingName.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.MappingName.SetAsCustom("field2");
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Nullable.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.NullableOnUpgrade.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Precision.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Scale.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.TypeDiscriminator.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Version.SetAsDefault();
            testClonedNotEquals();
        }

        #endregion TestSerializationForOrmFieldAttribute

        #region TestSerializationForAssociationInfo

        private void TestSerializationForAssociationInfo()
        {
            const string referentialXml = "<end multiplicity=\"Many\" onOwnerRemove=\"Clear\" onTargetRemove=\"Deny\"><pairTo valueType=\"Custom\"><value>Items</value></pairTo></end>";

            AssociationInfo originalAttribute = CreateAssociationInfo(TestAttributeValuesType.Values1);

            string xml = originalAttribute.SerializeToString();
            Assert.IsTrue(Util.StringEqual(xml, referentialXml, true),
                "Serialized xml form of 'AssociationInfo' type differs from referential xml.");

            TestSerializationForOrmAssociationEnd(originalAttribute.ToOrmAssociationEnd());
        }

        #endregion TestSerializationForAssociationInfo

        #region TestSerializationForOrmAssociationEnd

        private void TestSerializationForOrmAssociationEnd(OrmAssociationEnd originalAttribute)
        {
            OrmAssociationEnd clonedAttribute = null;
            Action cloneAttribute = () => clonedAttribute = originalAttribute.Clone();

            cloneAttribute();
            Assert.IsTrue(originalAttribute.EqualsTo(clonedAttribute));

            Action testClonedNotEquals = () => Assert.IsFalse(originalAttribute.EqualsTo(clonedAttribute));

            clonedAttribute.PairTo.SetAsDefault();
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.Multiplicity = MultiplicityKind.One;
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.OnOwnerRemove = AssociationOnRemoveAction.Default;
            testClonedNotEquals();

            cloneAttribute();
            clonedAttribute.OnTargetRemove = AssociationOnRemoveAction.Clear;
            testClonedNotEquals();
        }

        private void TestSerializationForOrmAssociationEnd()
        {
            const string referentialXml =
                "<end multiplicity=\"Many\" onOwnerRemove=\"Clear\" onTargetRemove=\"Deny\"><pairTo valueType=\"Custom\"><value>Items</value></pairTo></end>";

            OrmAssociationEnd originalAttribute = CreateOrmAssociationEnd(TestAttributeValuesType.Values1);

            string xml = originalAttribute.SerializeToString();
            Assert.IsTrue(Util.StringEqual(xml, referentialXml, true),
                "Serialized xml form of 'OrmAssociationEnd' type differs from referential xml.");

            TestSerializationForOrmAssociationEnd(originalAttribute);
        }

        #endregion TestSerializationForOrmAssociationEnd

        #endregion Test_OrmAttributes_Serialization

        #region common methods

        private OrmAssociationEnd CreateOrmAssociationEnd(TestAttributeValuesType valuesType)
        {
            OrmAssociationEnd originalAttribute = new OrmAssociationEnd();
            bool isValues1 = valuesType == TestAttributeValuesType.Values1;

            originalAttribute.Multiplicity = isValues1 ? MultiplicityKind.Many : MultiplicityKind.ZeroOrOne;
            originalAttribute.OnOwnerRemove = isValues1
                                                  ? AssociationOnRemoveAction.Clear
                                                  : AssociationOnRemoveAction.Deny;
            originalAttribute.OnTargetRemove = isValues1
                                                   ? AssociationOnRemoveAction.Deny
                                                   : AssociationOnRemoveAction.Clear;
            originalAttribute.PairTo.SetAsCustom(isValues1 ? "Items" : "RevertedItems");

            return originalAttribute;
        }

        private AssociationInfo CreateAssociationInfo(TestAttributeValuesType valuesType)
        {
            AssociationInfo originalAttribute = new AssociationInfo(CreateOrmAssociationEnd(valuesType));
            return originalAttribute;
        }

        private OrmFieldAttribute CreateOrmFieldAttribute(TestAttributeValuesType valuesType)
        {
            OrmFieldAttribute originalAttribute = new OrmFieldAttribute();
            bool isValues1 = valuesType == TestAttributeValuesType.Values1;

            originalAttribute.DefaultValue.Enabled = isValues1 ? true : false;
            originalAttribute.DefaultValue.Value.UseCustomExpression = isValues1 ? false : true;
            if (isValues1)
            {
                originalAttribute.DefaultValue.Value.Value = new Guid("{B4D9F48D-EC7A-4086-AEF4-1E8E439F3865}");
            }
            else
            {
                originalAttribute.DefaultValue.Value.CustomExpression = "System.Drawing.Color.Black";
            }
            originalAttribute.Indexed.SetAsCustom(isValues1 ? false : true);
            originalAttribute.LazyLoad.SetAsCustom(isValues1 ? true : false);
            originalAttribute.Length.SetAsCustom(isValues1 ? 123456789 : 987654321);
            originalAttribute.MappingName.SetAsCustom(isValues1 ? "tField" : "tRevertedField");
            originalAttribute.Nullable.SetAsCustom(isValues1 ? true : false);
            originalAttribute.NullableOnUpgrade.SetAsCustom(isValues1 ? false : true);
            originalAttribute.Precision.SetAsCustom(isValues1 ? 998877 : 778899);
            originalAttribute.Scale.SetAsCustom(isValues1 ? 555 : 550000);
            originalAttribute.TypeDiscriminator.SetAsCustom(isValues1 ? true : false);
            originalAttribute.Version.SetAsCustom(isValues1 ? VersionMode.Skip : VersionMode.Manual);

            return originalAttribute;
        }

        private OrmHierarchyRootAttribute CreateOrmHierarchyRootAttribute(TestAttributeValuesType valuesType)
        {
            OrmHierarchyRootAttribute originalAttribute = new OrmHierarchyRootAttribute();
            bool isValues1 = valuesType == TestAttributeValuesType.Values1;

            originalAttribute.Enabled = isValues1 ? true : false;
            originalAttribute.IncludeTypeId.SetAsCustom(isValues1 ? false : true);
            originalAttribute.InheritanceSchema = isValues1
                                                      ? HierarchyRootInheritanceSchema.ConcreteTable
                                                      : HierarchyRootInheritanceSchema.SingleTable;
            originalAttribute.MappingName.SetAsCustom(isValues1 ? "tTable1" : "RevertedtTable1");

            return originalAttribute;
        }

        private OrmIndexFields CreateOrmIndexFields(TestAttributeValuesType valuesType)
        {
            OrmIndexFields originalAttribute = new OrmIndexFields();
            bool isValues1 = valuesType == TestAttributeValuesType.Values1;

            originalAttribute.IncludedFields.Add(
                new StringTreeNode
                {
                    DisplayValue = isValues1 ? "Field1" : "RevertedField1",
                    IconIndex = isValues1 ? 1 : 2
                });

            originalAttribute.IncludedFields.Add(
                new StringTreeNode
                {
                    DisplayValue = isValues1 ? "Field2" : "RevertedField2",
                    IconIndex = isValues1 ? 2 : 1
                });

            originalAttribute.KeyFields.Add(
                new StringTreeNode
                {
                    DisplayValue = isValues1 ? "Field3" : "RevertedField3",
                    IconIndex = isValues1 ? 2 : 1
                });

            originalAttribute.KeyFields.Add(
                new StringTreeNode
                {
                    DisplayValue = isValues1 ? "Field4" : "RevertedField4",
                    IconIndex = isValues1 ? 1 : 2
                });

            return originalAttribute;
        }

        private OrmKeyAttribute CreateOrmKeyAttribute(TestAttributeValuesType valuesType)
        {
            OrmKeyAttribute originalAttribute = new OrmKeyAttribute();
            bool isValues1 = valuesType == TestAttributeValuesType.Values1;

            originalAttribute.Enabled = isValues1 ? true : false;
            originalAttribute.Direction.SetAsCustom(isValues1 ? KeyDirection.Negative : KeyDirection.Positive);
            originalAttribute.Position.SetAsCustom(isValues1 ? 45 : 54);
            return originalAttribute;
        }

        private OrmTypeDiscriminatorValueAttribute CreateOrmTypeDiscriminatorValueAttribute(
            TestAttributeValuesType valuesType)
        {
            OrmTypeDiscriminatorValueAttribute originalAttribute = new OrmTypeDiscriminatorValueAttribute();
            bool isValues1 = valuesType == TestAttributeValuesType.Values1;

            originalAttribute.Default = isValues1 ? true : false;
            originalAttribute.Enabled = isValues1 ? true : false;
            originalAttribute.Value.UseCustomExpression = isValues1 ? false : true;
            if (isValues1)
            {
                originalAttribute.Value.Value = new Guid("{B4D9F48D-EC7A-4086-AEF4-1E8E439F3865}");
            }
            else
            {
                originalAttribute.Value.CustomExpression = "System.Color.X";
            }
            return originalAttribute;
        }

        #endregion common methods

        #region Test_OrmAttributes_MergeChanges

        [TestMethod]
        public void Test_OrmAttributes_MergeChanges()
        {
            TestMergeChangesForAssociationInfo();
            TestMergeChangesForOrmFieldAttribute();
            TestMergeChangesForOrmHierarchyRootAttribute();
            TestMergeChangesForOrmKeyAttribute();
            //TestMergeChangesForOrmTableMappingAttribute();
            TestMergeChangesForOrmTypeDiscriminatorValueAttribute();

            Test_OrmFieldAttribute_Constraints();
        }

        private void TestMergeChangesForAssociationInfo()
        {
            var sourceAttribute = CreateAssociationInfo(TestAttributeValuesType.Values1);
            var targetAttribute = CreateAssociationInfo(TestAttributeValuesType.Values2);
            targetAttribute.OnTargetRemove = AssociationOnRemoveAction.Default;

            IOrmAttribute mergedAttribute = sourceAttribute.MergeChanges(targetAttribute, MergeConflictAction.TakeOther);
            string expectedMergedXml =
                "<end multiplicity=\"ZeroOrOne\" onOwnerRemove=\"Deny\" onTargetRemove=\"Deny\"><pairTo valueType=\"Custom\"><value>RevertedItems</value></pairTo></end>";
            string mergedAttributeXml = mergedAttribute.SerializeToString();

            Action testMergedXml =
                () =>
                Assert.IsTrue(Util.StringEqual(expectedMergedXml, mergedAttributeXml, true),
                    "Merged xml and expected xml is not equal!");
            testMergedXml();
        }

        private void TestMergeChangesForOrmTypeDiscriminatorValueAttribute()
        {
            var sourceAttribute = CreateOrmTypeDiscriminatorValueAttribute(TestAttributeValuesType.Values1);
            var targetAttribute = CreateOrmTypeDiscriminatorValueAttribute(TestAttributeValuesType.Values2);
            TestMergeChangesForOrmAttribute(sourceAttribute, targetAttribute);
        }

//        private void TestMergeChangesForOrmTableMappingAttribute()
//        {
//            var sourceAttribute = CreateOrmTableMappingAttribute(TestAttributeValuesType.Values1);
//            var targetAttribute = CreateOrmTableMappingAttribute(TestAttributeValuesType.Values2);
//            TestMergeChangesForOrmAttribute(sourceAttribute, targetAttribute);
//        }

        private void TestMergeChangesForOrmKeyAttribute()
        {
            var sourceAttribute = CreateOrmKeyAttribute(TestAttributeValuesType.Values1);
            var targetAttribute = CreateOrmKeyAttribute(TestAttributeValuesType.Values2);
            TestMergeChangesForOrmAttribute(sourceAttribute, targetAttribute);
        }

        private void TestMergeChangesForOrmHierarchyRootAttribute()
        {
            var sourceAttribute = CreateOrmHierarchyRootAttribute(TestAttributeValuesType.Values1);
            var targetAttribute = CreateOrmHierarchyRootAttribute(TestAttributeValuesType.Values2);
            TestMergeChangesForOrmAttribute(sourceAttribute, targetAttribute);
        }

        private void TestMergeChangesForOrmFieldAttribute()
        {
            var sourceAttribute = CreateOrmFieldAttribute(TestAttributeValuesType.Values1);
            var targetAttribute = CreateOrmFieldAttribute(TestAttributeValuesType.Values2);
            TestMergeChangesForOrmAttribute(sourceAttribute, targetAttribute);
        }

        private void TestMergeChangesForOrmAttribute<T>(T sourceAttribute, T targetAttribute) where T : IOrmAttribute
        {
            var targetGroupItems = targetAttribute.GetAttributeGroupItems(AttributeGroupsListMode.All);
            foreach (var attributeGroupItem in targetGroupItems)
            {
                attributeGroupItem.SetAsDefault();
            }

            bool toXml = false;
            string xmlSource;
            string xmlTarget;
            if (toXml)
            {
                xmlSource = sourceAttribute.SerializeToString();
                xmlTarget = targetAttribute.SerializeToString();
            }

            IOrmAttribute mergedAttribute = sourceAttribute.MergeChanges(targetAttribute, MergeConflictAction.TakeOther);

            foreach (var attributeGroupsListMode in EnumType<AttributeGroupsListMode>.Values)
            {
                var sourceGroupItems = sourceAttribute.GetAttributeGroupItems(attributeGroupsListMode);
                var mergedGroupItems = mergedAttribute.GetAttributeGroupItems(attributeGroupsListMode);

                for (int i = 0; i < mergedGroupItems.Count; i++)
                {
                    var mergedGroupItem = mergedGroupItems[i];
                    var sourceGroupItem = sourceGroupItems[i];

                    Assert.IsTrue(mergedGroupItem.EqualsTo(sourceGroupItem));
                }
            }
        }

        #endregion Test_OrmAttributes_MergeChanges

        #region Test_OrmFieldAttribute_Constraints

        public void Test_OrmFieldAttribute_Constraints()
        {
            OrmPropertyConstraints constraints = new OrmPropertyConstraints();
            foreach (var constraint in constraints.AllConstraints)
            {
                constraint.Used = true;
                constraint.Mode = PropertyConstrainMode.OnSetValue;
                constraint.ErrorMessage = string.Format("error for {0}", constraint.ConstrainType);
            }

            var lengthConstraint = constraints.LengthConstraint;
            lengthConstraint.Min.SetAsDefault();
            lengthConstraint.Max.SetAsCustom(10);

            var rangeConstraint = constraints.RangeConstraint;
            rangeConstraint.Min.Enabled = true;
            rangeConstraint.Min.Value.Value = 120;
            rangeConstraint.Max.Enabled = false;

            var regexConstraint = constraints.RegexConstraint;
            regexConstraint.Pattern = "1234@11";
            regexConstraint.Options = RegexOptions.ECMAScript | RegexOptions.IgnoreCase | RegexOptions.Singleline;

            OrmPropertyConstraints cloned = constraints.Clone();
            Assert.IsNotNull(cloned);

            //var targetAttribute = CreateOrmFieldAttribute(TestAttributeValuesType.Values2);
            //TestMergeChangesForOrmAttribute(originalAttribute, targetAttribute);

//            OrmFieldAttribute clonedAttribute = null;
//
//            Action cloneAttribute = () => clonedAttribute = originalAttribute.Clone();
//            cloneAttribute();
//            Assert.IsTrue(originalAttribute.EqualsTo(clonedAttribute));

            //Action testClonedNotEquals = () => Assert.IsFalse(originalAttribute.EqualsTo(clonedAttribute));

        }

        #endregion Test_OrmFieldAttribute_Constraints

        #region enum TestAttributeValuesType

        public enum TestAttributeValuesType
        {
            Values1,
            Values2
        }

        #endregion enum TestAttributeValuesType
    }
}
