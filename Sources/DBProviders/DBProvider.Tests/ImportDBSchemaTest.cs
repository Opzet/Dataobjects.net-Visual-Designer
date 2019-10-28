using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.Tests
{
    /// <summary>
    /// Summary description for ImportDBSchemaTest
    /// </summary>
    [TestClass]
    public class ImportDBSchemaTest
    {
        #region context 

        public ImportDBSchemaTest()
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

        private static string mock_ModuleFileXtensiveFolder = @"d:\Projects\Private\xp.dev.com\doemd\Sources\DBProviders\DBProvider.Xtensive.Implementation\bin\Debug\";
        //private static string mock_ModuleFileXtensiveFolder = @"c:\Projects\Private\xp-dev.com\doemd\Sources\DBProviders\DBProvider.Xtensive.Implementation\bin\Debug\";
        private static string mock_ModuleFileXtensiveFile = mock_ModuleFileXtensiveFolder + "TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive.dll";

        [TestMethod]
        public void TestMethod1()
        {
            Assembly assemblyFile = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(
                assembly => assembly.GetName().Name == "TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive");

            //Assembly assembly = Assembly.LoadFrom(mock_ModuleFileXtensiveFile);
            if (assemblyFile == null)
            {
                assemblyFile = Assembly.LoadFrom(mock_ModuleFileXtensiveFile);
            }
            Assert.IsNotNull(assemblyFile);

            IDBProviderModule module;
            bool loadResult = DBProviderManager.Instance.LoadModule(assemblyFile, out module);
            Assert.IsTrue(loadResult);
            Assert.IsNotNull(module);
            DBProviderModule dbProviderModule = (DBProviderModule) module;
            Assert.IsNotNull(dbProviderModule);
            
            dbProviderModule.mock_implementationFileFolder = mock_ModuleFileXtensiveFolder;

            DBProviderManager.Instance.InitModules();

            Application.EnableVisualStyles();

            FormImportDBSchema.ResultData resultData;
            bool dialogShow = FormImportDBSchema.DialogShow(out resultData);
        }
    }
}
