using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive
{
    public class DBProviderModule : IDBProviderModule
    {
        private readonly List<Exception> lastErrors = new List<Exception>();
        // assembly name without version!
        private const string IMPLEMENTATION_ASSEMBLY_NAME =
            "TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive.Implementation, Culture=neutral, PublicKeyToken=801a4a9f826a965d";

        private const string IMPLEMENTATION_ASSEMBLY_FILE_NAME = "TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive.Implementation.dll";

        internal string mock_implementationFileFolder = null;

        private Assembly implementationAssembly;
        private IDBProvider implementationProvider;
        private string installationFolder;
        private string installationPostSharpFolder;

        private const string registryKey1 = "Software\\Wow6432Node\\X-tensive.com\\DataObjects.Net";
        private const string registryKey2 = "Software\\X-tensive.com\\DataObjects.Net";
        private const string registryKeyValue = "LatestInstallLocation";
        
        private const string folderSuffix = "Bin\\Latest";
        private const string folderPostSharpSuffix = "Lib\\PostSharp\\Release";
        private const string assemblyFileNamePrefix = "Xtensive.";
        private const string assemblyFileNamePostSharpPrefix = "PostSharp";

        public ErrorCollection LastErrors
        {
            get { return new ErrorCollection(lastErrors); }
        }

        public IDBProvider[] Providers
        {
            get { return new[] { implementationProvider }; }
        }

        private string ReadInstallationFolderFromRegistry()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(registryKey1);
            if (registryKey == null)
            {
                registryKey = Registry.LocalMachine.OpenSubKey(registryKey2);
            }

            return registryKey != null ? registryKey.GetValue(registryKeyValue) as string : string.Empty;
        }

        public bool Initialize()
        {
            bool result = true;
            try
            {
                installationFolder = Environment.GetEnvironmentVariable("DataObjectsDotNetPath");

                if (string.IsNullOrEmpty(installationFolder))
                {
                    installationFolder = ReadInstallationFolderFromRegistry();
                }

                if (!string.IsNullOrEmpty(installationFolder))
                {
                    installationPostSharpFolder = Path.Combine(installationFolder, folderPostSharpSuffix);
                    installationFolder = Path.Combine(installationFolder, folderSuffix);
                }

                AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
                LoadImplementationAssembly();
            }
            catch (Exception e)
            {
                AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
                result = false;
                UpdateLastError(e);
            }

            return result;
        }

        private void LoadImplementationAssembly()
        {
            string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string implementationFileFullPath = Path.Combine(currentDir, IMPLEMENTATION_ASSEMBLY_FILE_NAME);
            if (!string.IsNullOrEmpty(mock_implementationFileFolder))
            {
                implementationFileFullPath = Path.Combine(mock_implementationFileFolder, IMPLEMENTATION_ASSEMBLY_FILE_NAME);
            }
            //implementationAssembly = AppDomain.CurrentDomain.Load(IMPLEMENTATION_ASSEMBLY_NAME);
            implementationAssembly = Assembly.LoadFrom(implementationFileFullPath);

            if (implementationAssembly == null)
            {
                throw new FileNotFoundException(string.Format("Could not find file for implemetation assembly '{0}'", IMPLEMENTATION_ASSEMBLY_NAME));
            }

            var providerRegisterAttribute =
                this.implementationAssembly.GetCustomAttributes(typeof(DBProviderRegisterAttribute), false).Cast<DBProviderRegisterAttribute>().SingleOrDefault();

            if (providerRegisterAttribute == null)
            {
                throw new TypeLoadException(string.Format("Could not find assembly attribute of type '{0}'!", typeof(DBProviderRegisterAttribute).FullName));
            }

            implementationProvider = (IDBProvider)Activator.CreateInstance(providerRegisterAttribute.Type);
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;

            string fileName = null;
            if (args.Name.StartsWith(assemblyFileNamePrefix))
            {
                string part = args.Name;
                if (args.Name.Contains(","))
                {
                    part = args.Name.Split(',')[0];
                }

                fileName = Path.Combine(installationFolder, string.Format("{0}.dll", part));
            }
            else if (args.Name.StartsWith(assemblyFileNamePostSharpPrefix))
            {
                string part = args.Name;
                if (args.Name.Contains(","))
                {
                    part = args.Name.Split(',')[0];
                }

                fileName = Path.Combine(installationPostSharpFolder, string.Format("{0}.dll", part));
            }

            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                assembly = Assembly.LoadFrom(fileName);
            }

            return assembly;
        }


        public void Deinitialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
        }

        private void UpdateLastError(Exception exception)
        {
            UpdateLastError(exception, false);
        }

        private void UpdateLastError(Exception exception, bool append)
        {
            if (!append)
            {
                this.lastErrors.Clear();
            }

            this.lastErrors.Add(exception);
        }
    }
}
