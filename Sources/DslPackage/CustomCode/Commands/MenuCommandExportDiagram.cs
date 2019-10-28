using System.ComponentModel.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandExportDiagram: MenuCommandDesignerBase
    {
        private const int commandId = 0x840;

        public override int CommandId
        {
            get { return commandId; }
        }

        public override void QueryStatus(MenuCommand menuCommand)
        {
            CurrentModelSelection modelSelection = GetCurrentSelectedPersistentType();
            bool enabled = modelSelection.GetFromSelection<EntityDiagram>().Any();
            menuCommand.Enabled = enabled;
            menuCommand.Visible = enabled;
        }

        public override void ExecCommand(MenuCommand menuCommand)
        {
            var modelSelection = GetCurrentSelectedPersistentType();
            var diagramDocView = modelSelection.DiagramDocView;

            var dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.DefaultExt = "png";
            dlg.Filter = "PNG image (*.png)|*.png|Bitmap image (*.bmp)|*.bmp|JPEG image (*.jpg)|*.jpg|GIF image (*.gif)|*.gif|TIF image (*.tif)|*.tif";
            dlg.AddExtension = true;
            dlg.RestoreDirectory = true;
            dlg.FileName = "DOEntityModel";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var diagram = diagramDocView.CurrentDiagram;
                var bitmap = diagram.CreateBitmap(diagram.NestedChildShapes,
                                                  Microsoft.VisualStudio.Modeling.Diagrams.Diagram.
                                                      CreateBitmapPreference.FavorClarityOverSmallSize);
                ImageFormat format = ImageFormat.Png;

                switch (Path.GetExtension(dlg.FileName).ToLower())
                {
                    case ".bmp":
                    format = ImageFormat.Bmp;
                    break;
                    case ".jpg":
                    format = ImageFormat.Jpeg;
                    break;
                    case ".gif":
                    format = ImageFormat.Gif;
                    break;
                    case ".tif":
                    format = ImageFormat.Tiff;
                    break;
                    case ".png":
                    default:
                    format = ImageFormat.Png;
                    break;
                }
                bitmap.Save(dlg.FileName, format);
            }
        }
    }
}