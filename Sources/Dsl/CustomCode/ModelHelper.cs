using System.Linq;
using Microsoft.VisualStudio.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode
{
    /// <summary>
    /// Provides services for the loading an instance of MyModel.
    /// </summary>
    internal static class ModelHelper
    {
        internal static EntityModel CreateModel()
        {
            Store store = new Store(typeof(DONetEntityModelDesignerDomainModel));
            EntityModel entityModel;
            
            using (store.TransactionManager.BeginTransaction("CreateModel"))
            {
                entityModel = DONetEntityModelDesignerSerializationHelper.Instance.CreateModelHelper(store.DefaultPartition);
                store.TransactionManager.CurrentTransaction.Commit();
            }

            return entityModel;
        }

        internal static EntityModel LoadModel(string modelPath)
        {
            Store store = new Store(typeof(DONetEntityModelDesignerDomainModel));
            EntityModel entityModel;

            using (store.TransactionManager.BeginTransaction("LoadModel"))
            {
                entityModel = DONetEntityModelDesignerSerializationHelper.Instance.LoadModel(store, modelPath, null, null, null);
                store.TransactionManager.CurrentTransaction.Commit();
            }

            return entityModel;
        }

        internal static EntityModel GetEntityModel(this Store store)
        {
            return store.ElementDirectory.FindElements<EntityModel>().Single();
        }
    }
}