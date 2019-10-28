using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
	public class DBProviderManager
	{
		#region singleton

		private static DBProviderManager instance;

		public static DBProviderManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DBProviderManager();
				}
				return instance;
			}
		}

		#endregion singleton

		#region fields

		private readonly List<IDBProviderModule> modules = new List<IDBProviderModule>();
		private readonly object sync = new object();
	    private bool initialized;

		#endregion fields

	    public bool Initialized
	    {
	        get
	        {
                lock(sync)
                {
                    return initialized;
                }
	        }
	    }

	    public void CompleteInitialization()
	    {
            lock (sync)
            {
                if (Initialized)
                {
                    throw new ApplicationException("ProviderManager has already completed initialization.");
                }

                initialized = true;
            }
	    }

	    public IEnumerable<IDBProviderModule> EnumerateModules()
		{
			return modules;
		}

		public IDBProvider FindProvider(Guid providerUniqueID)
		{
		    return (from module in modules
		            let provider = module.Providers.SingleOrDefault(provider => provider.UniqueID.Equals(providerUniqueID))
		            where provider != null
		            select provider).SingleOrDefault();
		}

		public bool LoadModule(string assemblyName)
		{
			IDBProviderModule loadedModule;
			return LoadModule(assemblyName, out loadedModule);
		}

		public bool LoadModule(string assemblyName, out IDBProviderModule loadedModule)
		{
			Assembly assembly = AppDomain.CurrentDomain.Load(assemblyName);
			return LoadModule(assembly, out loadedModule);
		}

		public bool LoadModule(Assembly assembly)
		{
			IDBProviderModule loadedModule;
			return LoadModule(assembly, out loadedModule);
		}

		public bool LoadModule(Assembly assembly, out IDBProviderModule loadedModule)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			var attribute =
				assembly.GetCustomAttributes(typeof (DBProviderModuleRegisterAttribute), false).Cast<DBProviderModuleRegisterAttribute>().SingleOrDefault();

			loadedModule = null;
			if (attribute != null)
			{
				loadedModule = (IDBProviderModule)Activator.CreateInstance(attribute.Type);
			}

			if (loadedModule != null)
			{
				lock (sync)
				{
					this.modules.Add(loadedModule);
				}
			}

			return loadedModule != null;
		}

		public void InitModules()
		{
			IDBProviderModule[] temp;

			lock(sync)
			{
				temp = modules.ToArray();
			}

			foreach (var module in temp)
			{
				try
				{
					module.Initialize();
				}
				catch (Exception e)
				{
					Debug.WriteLine("Error initializing module type: " + module.GetType().AssemblyQualifiedName);
				}
			}
		}

		public void DeInitModules()
		{
			IDBProviderModule[] temp;

			lock (sync)
			{
				temp = modules.ToArray();
			}

			foreach (var module in temp)
			{
				try
				{
					module.Deinitialize();
				}
				catch (Exception e)
				{
					Debug.WriteLine("Error deinitializing module type: " + module.GetType().AssemblyQualifiedName);
				}
			}
		}

		public void ClearModules()
		{
			lock (sync)
			{
				this.modules.Clear();
			}
		}
	}
}