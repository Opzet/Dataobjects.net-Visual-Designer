using System;
using System.IO;
using System.Windows.Forms;
using EnvDTE;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class FormEntityModelPicker : Form
    {
        private DTE dte;
        //private List<EnvDTE.ProjectItem> modelsList;
        private string projectPath = string.Empty;
        private TreeNode rootNode;
        private string EntityModelFile;

        private const int IMAGE_ROOT = 0;
        private const int IMAGE_FOLDER = 1;
        private const int IMAGE_FOLDER_OPEN = 2;
        private const int IMAGE_FILE = 3;

        public FormEntityModelPicker()
        {
            InitializeComponent();
        }

        public static bool DialogShow(DTE dte, string newFileName, out string modelFile)
        {
            bool result = false;
            modelFile = null;

            try
            {
                using (FormEntityModelPicker form = new FormEntityModelPicker())
                {
                    form.Initialize(dte);
                    result = form.ShowDialog() == DialogResult.OK;
                    if (result)
                    {
                        modelFile = form.EntityModelFile;
                        newFileName = Path.Combine(form.projectPath, newFileName);

                        modelFile = Util.MakeRelativePath(newFileName, modelFile);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
            }
            return result;
        }

        TreeNode firstFileNode = null;

        private void Initialize(DTE dte)
        {
            this.dte = dte;
            //modelsList = new List<EnvDTE.ProjectItem>();

            var activeProjects = (Array)dte.ActiveSolutionProjects;
            var project = (Project)activeProjects.GetValue(0);
            SelectedItem selectedItem = dte.SelectedItems.Item(1);

            if (selectedItem != null && selectedItem.ProjectItem != null)
            {
                string fileName = selectedItem.ProjectItem.FileNames[0];
                //MessageBox.Show("selectedItem.ProjectItem.FileNames[0] = " + fileName);
                projectPath = Path.GetDirectoryName(fileName);
            }
            else
            {
                projectPath = Path.GetDirectoryName(project.FullName);
                //MessageBox.Show("project.FullName = " + project.FullName);
            }

            rootNode = AddNode(null, string.Format("{0} [Project]", project.Name), IMAGE_ROOT, project.ParentProjectItem);
            foreach (EnvDTE.ProjectItem projectItem in project.ProjectItems)
            {
                PopulateModels(rootNode, projectItem);
            }
            rootNode.Expand();
            if (firstFileNode != null)
            {
                SelectNodeAndExpandToParentRoots(firstFileNode);
            }

            UpdateButtons();
        }

        private void SelectNodeAndExpandToParentRoots(TreeNode treeNode)
        {
            TreeNode parent = treeNode.Parent;
            while (parent != null)
            {
                parent.Expand();
                parent = parent.Parent;
            }
            treeModels.SelectedNode = treeNode;
            treeNode.EnsureVisible();
        }

        private TreeNode AddNode(TreeNode parent, string text, int imageIndex, EnvDTE.ProjectItem projectItem)
        {
            var nodes = parent == null ? treeModels.Nodes : parent.Nodes;
            var treeNode = nodes.Add(Guid.NewGuid().ToString(), text, imageIndex, imageIndex);

            bool isVirtualFolder = IsVirtualFolder(projectItem.Kind);

            string fullPath = projectItem == null || isVirtualFolder ? string.Empty : Path.Combine(projectPath, projectItem.FileNames[0]);
            treeNode.Tag = fullPath;
            return treeNode;
        }

        private void PopulateModels(TreeNode parentNode, EnvDTE.ProjectItem item)
        {
            var projectItemKind = DecodeProjectItemKind(item.Kind);
            bool canProcess = projectItemKind != ProjectItemKind.Other;
            if (canProcess && projectItemKind == ProjectItemKind.File)
            {
                canProcess = item.Name.ToLower().EndsWith(".dom");
            }

            if (canProcess)
            {
                int imageIndex = projectItemKind == ProjectItemKind.File ? IMAGE_FILE : IMAGE_FOLDER;
                var treeNode = AddNode(parentNode, item.Name, imageIndex, item);

                if (firstFileNode == null && projectItemKind == ProjectItemKind.File)
                {
                    firstFileNode = treeNode;
                }

                foreach (EnvDTE.ProjectItem pi in item.ProjectItems)
                {
                    PopulateModels(treeNode, pi);
                }
            }
        }

        #region enum ProjectItemKind

        public enum ProjectItemKind
        {
            Folder,
            File,
            Other
        }

        #endregion enum ProjectItemKind

        private bool IsVirtualFolder(string sProjectItemKind)
        {
            return sProjectItemKind.In(Constants.vsProjectItemKindSolutionItems,
                Constants.vsProjectItemKindVirtualFolder);
        }

        private ProjectItemKind DecodeProjectItemKind(string sProjectItemKind)
        {
            ProjectItemKind result;

            switch (sProjectItemKind)
            {
                case Constants.vsProjectItemKindMisc:
                case Constants.vsProjectItemKindPhysicalFolder:
                case Constants.vsProjectItemKindSolutionItems:
                case Constants.vsProjectItemKindVirtualFolder:
                {
                    result = ProjectItemKind.Folder;
                    break;
                }
                case Constants.vsProjectItemKindPhysicalFile:
                {
                    result = ProjectItemKind.File;
                    break;
                }
                default:
                {
                    result = ProjectItemKind.Other;
                    break;
                }
            }

            return result;
        }

        private void UpdateButtons()
        {
            btnOk.Enabled = treeModels.SelectedNode != null && treeModels.SelectedNode.ImageIndex == IMAGE_FILE;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Make sure there is an item selected
            var selectedNode = treeModels.SelectedNode;
            if (selectedNode == null || selectedNode.ImageIndex != IMAGE_FILE)
            {
                DialogResult = DialogResult.None;
                return;
            }

            EntityModelFile = (string) selectedNode.Tag;
        }

        private void treeModels_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateButtons();
        }

        private void treeModels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnOk_Click(null, null);
        }

        private void treeModels_AfterExpand(object sender, TreeViewEventArgs e)
        {
            ChangeFolderIcon(e.Node, true);
        }

        private void treeModels_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            ChangeFolderIcon(e.Node, false);
        }

        private void ChangeFolderIcon(TreeNode node, bool openIcon)
        {
            if (node.ImageIndex.In(1, 2))
            {
                int imageIndex = openIcon ? IMAGE_FOLDER_OPEN : IMAGE_FOLDER;
                node.ImageIndex = imageIndex;
                node.SelectedImageIndex = imageIndex;
            }
        }
    }
}
