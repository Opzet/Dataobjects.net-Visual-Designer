using System.ComponentModel.Design;
using System.Windows.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandImportDBSchema : MenuCommandDesignerBase
    {
        private const int commandId = 0x880;

        public override int CommandId
        {
            get { return commandId; }
        }

        public override void QueryStatus(MenuCommand menuCommand)
        {
            //CurrentModelSelection modelSelection = GetCurrentSelectedPersistentType();
            //bool enabled = modelSelection.GetFromSelection<EntityDiagram>(false).Any();
            menuCommand.Enabled = false;
            menuCommand.Visible = false;
        }

        public override void ExecCommand(MenuCommand menuCommand)
        {
            MessageBox.Show("In Progress...");
//            CurrentModelSelection modelSelection = GetCurrentSelectedPersistentType();
//            ImportDBSchemaUtil dbSchemaUtil = new ImportDBSchemaUtil(modelSelection);
//            if (dbSchemaUtil.DialogShow())
//            {
//                dbSchemaUtil.ImportModels();
//            }
        }
    }
}