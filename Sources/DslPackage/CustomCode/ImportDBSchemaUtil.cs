/*using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TXSoftware.DataObjectsNetEntityModel.DBProvider;
using TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public class ImportDBSchemaUtil
    {
        private CurrentModelSelection modelSelection;
        private FormImportDBSchema.ResultData resultData;
        private EntityDiagram entityDiagram;
        private EntityModel entityModel;

        internal ImportDBSchemaUtil(CurrentModelSelection modelSelection)
        {
            this.modelSelection = modelSelection;
            entityDiagram = modelSelection.GetFromSelection<EntityDiagram>(false).Single();
            entityModel = entityDiagram.Store.ElementDirectory.FindElements<EntityModel>().Single();
        }

        public bool DialogShow()
        {
            CheckDBProvidersManager();
            return FormImportDBSchema.DialogShow(out resultData);
        }

        private void CheckDBProvidersManager()
        {
            if (!DBProviderManager.Instance.Initialized)
            {
                string dbModulesPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string[] dbModuleFiles = Directory.GetFiles(dbModulesPath, "*.dll", SearchOption.TopDirectoryOnly);
                foreach (string dbModuleFile in dbModuleFiles)
                {
                    if (File.Exists(dbModuleFile))
                    {
                        Assembly moduleAssembly = null;
                        try
                        {
                            moduleAssembly = Assembly.LoadFrom(dbModuleFile);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }

                        if (moduleAssembly != null)
                        {
                            bool loadModuleResult = DBProviderManager.Instance.LoadModule(moduleAssembly);
                            if (!loadModuleResult)
                            {
                                Debug.WriteLine(string.Format("Error loading assembly file '{0}' (does not contain db module class?)", dbModuleFile));
                            }
                        }
                    }
                }

                DBProviderManager.Instance.InitModules();
                DBProviderManager.Instance.CompleteInitialization();
            }
        }

        public void ImportModels()
        {
            //modelSelection.MakeActionWithinTransaction()
        }
    }
}*/