using System;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class DONetEntityModelDesignerEditorFactory : IVsEditorFactoryNotify
    {
        #region IVsEditorFactoryNotify Members

        public int NotifyDependentItemSaved(IVsHierarchy pHier, uint itemidParent, string pszMkDocumentParent, 
            uint itemidDpendent, string pszMkDocumentDependent)
        {
            // Ignored
            return 0;
        }

        public int NotifyItemAdded(uint grfEFN, IVsHierarchy pHier, uint itemid, string pszMkDocument)
        {
            try
            {
                int hr = SafeNotifyItemAdded(grfEFN, pHier, itemid, pszMkDocument);
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int SafeNotifyItemAdded(uint grfEFN, IVsHierarchy pHier, uint itemid, string pszMkDocument)
        {
            object itemObject;
            int hr = pHier.GetProperty(itemid, (int)__VSHPROPID.VSHPROPID_ExtObject, out itemObject);

            ProjectItem item = itemObject as ProjectItem;
            if (item == null)
            {
                return -1;
            }

            // Set the name of the custom tool on .dom files
            item.Properties.Item("CustomTool").Value = Const.FILEGENERATOR_IDENT;

            return 0;
        }

        public int NotifyItemRenamed(IVsHierarchy pHier, uint itemid, string pszMkDocumentOld, string pszMkDocumentNew)
        {
            // Ignored
            return 0;
        }

        #endregion
    }
}