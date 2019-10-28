using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TXSoftware.DataObjectsNetEntityModel.DBProvider;
using TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive;

namespace DBProvider.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class CommonDBProvidersTests
    {
        public CommonDBProvidersTests()
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
        public void Test_Load_Xtensive_DBProvider()
        {
            TestLogger logger = new TestLogger("Test_Load_Xtensive_DBProvider");
            DBProviderModule xtensiveModule = new DBProviderModule();
            //xtensiveModule.mock_implementationFileFolder = @"c:\Projects\Private\xp-dev.com\doemd\Sources\DBProviders\DBProvider.Xtensive.Implementation\bin\Debug\";
            xtensiveModule.mock_implementationFileFolder = @"d:\Projects\Private\xp.dev.com\doemd\Sources\DBProviders\DBProvider.Xtensive.Implementation\bin\Debug\";
            bool result = xtensiveModule.Initialize();
            try
            {
                Assert.IsTrue(result, "Initialization of Xtensive's 'DBProviderModule' failed. Last errors: " +
                                      string.Join(Environment.NewLine, xtensiveModule.LastErrors.Select(ex => ex.Message)));

                Assert.IsNotNull(xtensiveModule.Providers, "Xtensive's 'DBProviderModule' returns null list of providers.");
                Assert.AreNotEqual(xtensiveModule.Providers.Length, 0, "Xtensive's 'DBProviderModule' returns zero providers.");

                IDBProvider dbProvider = xtensiveModule.Providers.First();

                var connectionInfo = dbProvider.ConnectionInfoProvider.CreateInfo(new StorageEngine("sqlserver"), "sa", "halo", "localhost", "TheConsole");
                try
                {
                    bool connected = dbProvider.Connect(connectionInfo);
                    Assert.IsTrue(connected, "Connection to sql server failed.");

                    ServerCollection servers = dbProvider.GetAllServers(LoadingMode.TopLevel);
                    Assert.IsNotNull(servers, "dbProvider.GetAllServers returns null");
                    Assert.AreNotEqual(servers.Count, 0, "dbProvider.GetAllServers returns empty list of servers.");

                    logger.Log("Provider '{0}' retrieved {1} servers", dbProvider.Name, servers.Count);
                    foreach (var server in servers)
                    {
                        logger.Log("\tserver '{0}'", server.Name);
                    }

                    Server serverLocalhost = servers["localhost"];
                    bool refreshResult = dbProvider.Refresh(serverLocalhost, LoadingMode.TopLevel);
                    Assert.IsTrue(refreshResult, "Refreshing details about server 'localhost' failed");

                    logger.Log("\tProvider '{0}' refreshed {1} databases for server '{2}'", dbProvider.Name, serverLocalhost.Databases.Count, serverLocalhost.Name);
                    foreach (var database in serverLocalhost.Databases)
                    {
                        logger.Log("\t\tdatabase '{0}'", database.Name);
                        refreshResult = dbProvider.Refresh(database, LoadingMode.TopLevel);
                        Assert.IsTrue(refreshResult, string.Format("Refreshing details about database '{0}' failed", database.Name));

                        logger.Log("\t\tProvider '{0}' refreshed {1} schemas for database '{2}'", dbProvider.Name, database.Schemas.Count, database.Name);
                        foreach (var schema in database.Schemas)
                        {
                            logger.Log("\t\t\tschema '{0}'", schema.Name);
                        }
                    }
                }
                finally
                {
                    dbProvider.Disconnect();
                    Assert.IsFalse(dbProvider.IsConnected, "DbProvider is still connected after 'Disconnect' method called.");
                }
            }
            finally
            {
                xtensiveModule.Deinitialize();
            }
        }

        /*public bool LoadProviders()
        {
            UnloadProviders();

            bool result = Directory.Exists(providersDirectory);
            if (result)
            {
                FileInfo[] providerAssemblyFiles = new DirectoryInfo(providersDirectory).GetFiles("*.dll", SearchOption.TopDirectoryOnly);
                foreach (FileInfo fileInfo in providerAssemblyFiles)
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(fileInfo.FullName);
                        bool loadResult = ProviderManager.Instance.LoadModule(assembly);
                    }
                    catch (Exception e)
                    {
                        //TODO: Log error
                        Debug.WriteLine(e.Message);
                    }
                }

                ProviderManager.Instance.InitModules();
            }

            return result;
        }

        private static void UnloadProviders()
        {
            ProviderManager.Instance.DeInitModules();
            ProviderManager.Instance.ClearModules();
        }*/
    }
}
