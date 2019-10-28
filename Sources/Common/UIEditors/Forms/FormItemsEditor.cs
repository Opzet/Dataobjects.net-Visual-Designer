using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class FormItemsEditor : Form, IModalDialogForm
    {
        private ItemsEditorOptions editorOptions;
        private ItemsEditorAttribute itemsEditorAttribute;
        private IItemsEditorValidator itemsEditorValidator;
        public bool IsDirty { get; private set; }

        public FormItemsEditor()
        {
            InitializeComponent();
        }

        public void BindData(ITypeDescriptorContext context, object value, object[] attributeArguments)
        {
            PropertyDescriptor propertyDescriptor = context.PropertyDescriptor;
            itemsEditorAttribute = propertyDescriptor.Attributes.OfType<ItemsEditorAttribute>().Single();
            IEnumerable<ITreeNodeData> collection = (IEnumerable<ITreeNodeData>) value;

            itemsEditorValidator = itemsEditorAttribute.CreateValidator(context.Instance);

            Initialize(itemsEditorAttribute.EditorOptions, collection);

            if (!string.IsNullOrEmpty(itemsEditorAttribute.EditorCaption))
            {
                this.Text = itemsEditorAttribute.EditorCaption;
            }

            if (!string.IsNullOrEmpty(itemsEditorAttribute.EditorTitle))
            {
                lbTitle.Text = itemsEditorAttribute.EditorTitle;
            }
            else
            {
                lbTitle.Visible = false;
            }
        }

        public object SaveData()
        {
            return this.CurrentCollection;
        }

        public void Initialize<T>(ItemsEditorOptions editorOptions, IEnumerable<T> collection) where T : ITreeNodeData
        {
            this.editorOptions = editorOptions;
            Type itemType = collection.GetType().GetGenericArguments().Single();
            lvItems.RegisterNodeDataType(itemType);

            lvItems.Nodes.Clear();
            foreach (var treeNodeData in collection)
            {
                lvItems.CreateNode(treeNodeData);
            }

            lvItems.CreateNewItemNode();

            IsDirty = false;
            panelSortIconsHint.Visible = SupportSortingIcons();
        }

        internal IEnumerable<ITreeNodeData> CurrentCollection
        {
            get { return lvItems.AllNodeProxy().Where(proxy => !proxy.IsNewItemNode).Select(proxy => proxy.GetNodeData()).ToList(); }
        }

        private bool SupportSortingIcons()
        {
            return Util.IsFlagSet(ItemsEditorOptions.SortingIcons, this.editorOptions);
        }

        private void lvItems_ValidateLabelEdit(object sender, Components.ValidateLabelEditEventArgs e)
        {
            string newLabel = e.Label.Trim();
            bool isValid = !string.IsNullOrEmpty(newLabel);
            if (isValid)
            {
                if (Util.IsFlagSet(ItemsEditorOptions.UniqueDisplayValues, this.editorOptions))
                {
                    isValid = !lvItems.AllNodeData().Any(data => data.DisplayValue == newLabel);
                }
            }

            e.Cancel = !isValid;
        }

        private void lvItems_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var nodeProxy = e.Node.GetNodeProxy();
            if (string.IsNullOrEmpty(e.Label))
            {
                if (nodeProxy.IsNewItemNode)
                {
                    e.Node.UpdateNodeDataDisplayValue(TreeNodeProxy.NEW_ITEM_NODE_TEXT);
                    e.Node.Text = TreeNodeProxy.NEW_ITEM_NODE_TEXT;
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
            else
            {
                e.Node.UpdateNodeDataDisplayValue(e.Label);
                e.Node.Text = e.Label;
                if (nodeProxy.IsNewItemNode)
                {
                    nodeProxy.IsNewItemNode = false;
                    e.Node.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    int iconIndex = 0;
                    if (SupportSortingIcons())
                    {
                        iconIndex = TreeNodeProxy.ICON_INDEX_SORT_ASC;
                    }

                    UpdateNodeIcon(e.Node, iconIndex);
                    e.Node.GetNodeData().IconIndex = iconIndex;
                }
            }

            if (!lvItems.HasNewItemNode())
            {
                lvItems.CreateNewItemNode();
            }
        }

        private void UpdateNodeIcon(TreeNode node, int iconIndex)
        {
            //int iconIndex = (node.IsNewItemNode() || !SupportSortingIcons()) ? 0 : node.GetNodeData().IconIndex;
            node.ImageIndex = iconIndex;
            node.StateImageIndex = iconIndex;
            node.SelectedImageIndex = iconIndex;
        }

        private void lvItems_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var nodeProxy = e.Node.GetNodeProxy();
            if (nodeProxy.IsNewItemNode)
            {
                e.Node.Text = string.Empty;
                e.Node.UpdateNodeDataDisplayValue(string.Empty);
            }
        }

        private void lvItems_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (lvItems.SelectedNode != null)
                    lvItems.StartLabelEdit();
            }
        }

        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            TreeNode selectedNode = lvItems.SelectedNode;
            TreeNodeProxy selectedNodeProxy = selectedNode.GetNodeProxy();
            if (selectedNodeProxy == null || selectedNodeProxy.IsNewItemNode)
            {
                return;
            }

            if (MessageBox.Show(string.Format("Remove item '{0}' ?", selectedNode.Text), "Remove Item",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                lvItems.Nodes.Remove(selectedNode);
                IsDirty = true;
            }
        }

        private void btnRemoveAll_Click(object sender, System.EventArgs e)
        {
            if (lvItems.Nodes.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("Remove all items?", "Remove all items",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            List<TreeNode> toRemove = new List<TreeNode>();
            foreach (TreeNode treeNode in lvItems.Nodes)
            {
                TreeNodeProxy treeNodeProxy = treeNode.GetNodeProxy();
                if (!treeNodeProxy.IsNewItemNode)
                {
                    toRemove.Add(treeNode);
                }
            }

            foreach (TreeNode treeNode in toRemove)
            {
                lvItems.Nodes.Remove(treeNode);
            }

            if (!lvItems.HasNewItemNode())
            {
                lvItems.CreateNewItemNode();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (itemsEditorValidator != null)
            {
                string error;
                if (!itemsEditorValidator.Validate(CurrentCollection, out error))
                {
                    this.DialogResult = DialogResult.None;
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lvItems_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var hitTest = lvItems.HitTest(e.Location);
            if (hitTest.Location == TreeViewHitTestLocations.Image && hitTest.Node != null && !hitTest.Node.IsNewItemNode() &&
                SupportSortingIcons())
            {
                int iconIndex = hitTest.Node.ImageIndex;
                iconIndex = iconIndex == TreeNodeProxy.ICON_INDEX_SORT_ASC ? TreeNodeProxy.ICON_INDEX_SORT_DESC : TreeNodeProxy.ICON_INDEX_SORT_ASC;
                UpdateNodeIcon(hitTest.Node, iconIndex);
                ITreeNodeData treeNodeData = hitTest.Node.GetNodeData();
                treeNodeData.IconIndex = iconIndex;
            }
        }
    }
}
