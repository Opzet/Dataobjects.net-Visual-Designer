using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;
using Debugger = System.Diagnostics.Debugger;

namespace TXSoftware.DataObjectsNetEntityModel.Common.ProjectItem
{
    public class AddModelWizard : IWizard
    {
        private string projectFileName;
        // This method is called before opening any item that 
        // has the OpenInEditor attribute.
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
//            var defaultNS = projectItem.ContainingProject.Properties.Item("DefaultNamespace").Value.ToString();
//            var namespaces = new List<string>();
//            EnvDTE.ProjectItem parent = projectItem.Collection.Parent as EnvDTE.ProjectItem;
//            while (parent != null)
//            {
//                if (parent.Kind != EnvDTE.Constants.vsProjectItemKindPhysicalFile)
//                {
//                    namespaces.Insert(0, parent.Name.Replace(" ", string.Empty));
//                }
//
//                parent = parent.Collection.Parent as EnvDTE.ProjectItem;
//            }
//            namespaces.Insert(0, defaultNS);

            projectFileName = projectItem.FileNames[0];

            projectItem.Properties.Item("CustomTool").Value = "TextTemplatingFileGenerator";
            //projectItem.Properties.Item("CustomToolNamespace").Value = string.Join(".", namespaces.ToArray());
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
        }

        // This method is only called for item templates,
        // not for project templates.
        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
        }

        // This method is called after the project is created.
        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject,
            Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
            var containsKey = replacementsDictionary.ContainsKey("$domFileName$");

            if (!containsKey)
            {
                DTE dte = automationObject as DTE;

                //Debugger.Break();
                string newFileName = replacementsDictionary["$rootname$"];

                string modelFile;
                if (FormEntityModelPicker.DialogShow(dte, newFileName, out modelFile))
                {
                    replacementsDictionary.Add("$domFileName$", modelFile);
                }
            }
        }

        // This method is only called for item templates,
        // not for project templates.
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }

}