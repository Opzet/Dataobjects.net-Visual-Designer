using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace Tests.Common
{
    /// <summary>
    /// Summary description for Serialization
    /// </summary>
    [TestClass]
    public class DefaultableTests
    {
        #region context 

        public DefaultableTests()
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

        #region Test_Defaultable_Merge

        [TestMethod]
        public void Test_Defaultable_Merge()
        {
            TestDefaultableMerge("Source Value", "Target Value");
            TestDefaultableMerge(DateTime.Now, DateTime.Now.ToUniversalTime());
            TestDefaultableMerge(TimeSpan.MinValue, TimeSpan.MaxValue);
            TestDefaultableMerge(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
            TestDefaultableMerge(short.MinValue, short.MaxValue);
            TestDefaultableMerge(UInt16.MinValue, UInt16.MaxValue);
            TestDefaultableMerge(Int16.MinValue, Int16.MaxValue);
            TestDefaultableMerge(uint.MinValue, uint.MaxValue);
            TestDefaultableMerge(int.MinValue, int.MaxValue);
            TestDefaultableMerge(UInt32.MinValue, UInt32.MaxValue);
            TestDefaultableMerge(long.MinValue, long.MaxValue);
            TestDefaultableMerge(UInt64.MinValue, UInt64.MaxValue);
            TestDefaultableMerge(decimal.MinValue, decimal.MaxValue);
            TestDefaultableMerge(double.MinValue, double.MaxValue);
            TestDefaultableMerge(SByte.MinValue, SByte.MaxValue);
            TestDefaultableMerge(Single.MinValue, Single.MaxValue);
            TestDefaultableMerge(Guid.NewGuid(), Guid.NewGuid());
            TestDefaultableMerge(new byte[] { 255, 0, 128, 4 }, new byte[] { 255, 0, 128, 4, 111 });
        }

        #endregion Test_Defaultable_Merge

        #region Test_Defaultable_Serialization

        [TestMethod]
        public void Test_Defaultable_Serialization()
        {
            TestSerializeDefaultableGeneric("Loren Ipsum Value");
            TestSerializeDefaultableGeneric(DateTime.Now);
            TestSerializeDefaultableGeneric(DateTime.Now.ToUniversalTime());
            TestSerializeDefaultableGeneric(TimeSpan.MinValue);
            TestSerializeDefaultableGeneric(TimeSpan.MaxValue);
            TestSerializeDefaultableGeneric(DateTimeOffset.MinValue);
            TestSerializeDefaultableGeneric(DateTimeOffset.MaxValue);
            TestSerializeDefaultableGeneric(short.MinValue);
            TestSerializeDefaultableGeneric(short.MaxValue);
            TestSerializeDefaultableGeneric(UInt16.MinValue);
            TestSerializeDefaultableGeneric(UInt16.MaxValue);
            TestSerializeDefaultableGeneric(Int16.MinValue);
            TestSerializeDefaultableGeneric(Int16.MaxValue);
            TestSerializeDefaultableGeneric(uint.MinValue);
            TestSerializeDefaultableGeneric(uint.MaxValue);
            TestSerializeDefaultableGeneric(int.MinValue);
            TestSerializeDefaultableGeneric(int.MaxValue);
            TestSerializeDefaultableGeneric(UInt32.MinValue);
            TestSerializeDefaultableGeneric(UInt32.MaxValue);
            TestSerializeDefaultableGeneric(long.MinValue);
            TestSerializeDefaultableGeneric(long.MaxValue);
            TestSerializeDefaultableGeneric(UInt64.MinValue);
            TestSerializeDefaultableGeneric(UInt64.MaxValue);
            TestSerializeDefaultableGeneric(decimal.MinValue);
            TestSerializeDefaultableGeneric(decimal.MaxValue);
            TestSerializeDefaultableGeneric(double.MinValue);
            TestSerializeDefaultableGeneric(double.MaxValue);
            TestSerializeDefaultableGeneric(SByte.MinValue);
            TestSerializeDefaultableGeneric(SByte.MaxValue);
            TestSerializeDefaultableGeneric(Single.MinValue);
            TestSerializeDefaultableGeneric(Single.MaxValue);
            TestSerializeDefaultableGeneric(Guid.NewGuid());
            TestSerializeDefaultableGeneric(new byte[]{255, 0, 128, 4});
        }

        #endregion Test_Defaultable_Serialization

        #region support test methods

        private void TestDefaultableMerge<T>(T currentValue, T otherValue)
        {
            TestDefaultableMerge(currentValue, otherValue, MergeConflictAction.TakeCurrent);
            TestDefaultableMerge(currentValue, otherValue, MergeConflictAction.TakeOther);
        }

        private void TestDefaultableMerge<T>(T currentValue, T otherValue, MergeConflictAction mergeConflictAction)
        {
            Defaultable<T> current = new Defaultable<T>();
            current.SetAsCustom(currentValue);

            Defaultable<T> other = new Defaultable<T>();
            other.SetAsCustom(otherValue);

            Defaultable<T> merged = current.Merge(other, mergeConflictAction);
            string errorMessage = string.Format("Merging '{0}' values", currentValue.GetType().Name);
            if (mergeConflictAction == MergeConflictAction.TakeCurrent)
            {
                Assert.IsTrue(current.EqualsTo(merged), errorMessage);
            }
            else if (mergeConflictAction == MergeConflictAction.TakeOther)
            {
                Assert.IsTrue(other.EqualsTo(merged), errorMessage);
            }
        }

        private void TestSerializeDefaultableGeneric<T>(T testValue)
        {
            Defaultable<T> value = new Defaultable<T>();
            TestDefaultableAsDefault(value);

            value.SetAsCustom(testValue);
            Defaultable<T> cloned = value.CloneBySerialization();
            Assert.IsTrue(value.EqualsTo(cloned), string.Format("Serializing '{0}' value", testValue.GetType().Name));
        }

        private void TestDefaultableAsDefault(Defaultable source)
        {
            Defaultable cloned = source.CloneBySerialization();
            Assert.IsTrue(source.EqualsTo(cloned));
        }

        #endregion support test methods
    }
}