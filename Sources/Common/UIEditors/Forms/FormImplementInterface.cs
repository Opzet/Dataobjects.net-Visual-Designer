using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common.Properties;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class FormImplementInterface : FormInputBox
    {
        public const int VALUE_ENTITY = 0;
        public const int VALUE_STRUCTURE = 1;
        public new const int DEFAULT_WIDTH = 441;
        private bool cancelExpand = false;
        ManualResetEvent treeExpandedEvent = new ManualResetEvent(false);
        Regex regex = new Regex(@"(^[a-zA-Z][a-zA-Z0-9_]*)|(^[_][a-zA-Z0-9_]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private bool manuallCheckingNodes = false;
        private bool internalPopulatingTree;

        private InterfaceTreeItem sourceInterface;

        public FormImplementInterface()
        {
            InitializeComponent();
        }

        private void PopulateForm(InterfaceTreeItem sourceInterface)
        {
            this.sourceInterface = sourceInterface;
            PopulateTypes();
            PopulateTree();
        }

        private void PopulateTypes()
        {
            cmbTypes.Items.Clear();
            cmbTypes.Items.Add(new IconListEntry("Entity", Resources.Entity));
            cmbTypes.Items.Add(new IconListEntry("Structure", Resources.Structure));

            cmbTypes.SelectedIndex = 0;
        }

        private void PopulateRootInterfaces()
        {
            InterfaceTreeItem selectedRootInterface = SelectedRootInterface;
            cmbRootInterface.Items.Clear();
            List<InterfaceTreeItem> selectedInterfaces = GetSelectedInterfaces();
            foreach (var @interface in selectedInterfaces)
            {
                cmbRootInterface.Items.Add(new IconListEntry(@interface.Name, @interface, Resources.Interface));
            }

            SelectedRootInterface = selectedRootInterface;
        }

        private void SelectFirstInterfaceAsRoot()
        {
            InterfaceTreeItem selectedRootInterface = SelectedRootInterface;

            if (selectedRootInterface == null)
            {
                List<InterfaceTreeItem> selectedInterfaces = GetSelectedInterfaces();
                if (selectedInterfaces.Count > 0)
                {
                    SelectedRootInterface = selectedInterfaces[0];
                }
            }
        }

        protected InterfaceTreeItem SelectedRootInterface
        {
            get
            {
                if (cmbRootInterface.SelectedItem != null)
                {
                    IconListEntry entry = (IconListEntry) cmbRootInterface.SelectedItem;
                    return (InterfaceTreeItem) entry.Value;
                }

                return null;
            }
            set 
            {
                foreach (IconListEntry entry in cmbRootInterface.Items)
                {
                    InterfaceTreeItem item = (InterfaceTreeItem)entry.Value;
                    if (item == value)
                    {
                        cmbRootInterface.SelectedItem = entry;
                        break;
                    }
                }
            }
        }

        private void PopulateTree()
        {
            internalPopulatingTree = true;
            try
            {
                tvTree.Nodes.Clear();
                PopulateNode(tvTree.Nodes, sourceInterface);
                tvTree.ExpandAll();
            }
            finally
            {
                internalPopulatingTree = false;
            }
        }

        private void RecursivePopulateTree(TreeNodeCollection nodes, IEnumerable<InterfaceTreeItem> interfaces)
        {
            foreach (var @interface in interfaces)
            {
                PopulateNode(nodes, @interface);
            }
        }

        private void PopulateNode(TreeNodeCollection nodes, InterfaceTreeItem @interface)
        {
            TreeNode treeNode = nodes.Add(@interface.Name);
            treeNode.Tag = @interface;
            tvTree.SetNodeCheckState(treeNode, CheckState.Unchecked);
            if (@interface.Childs.Count > 0)
            {
                RecursivePopulateTree(treeNode.Nodes, @interface.Childs);
            }
        }

        private InterfaceTreeItem GetInterfaceItem(TreeNode node)
        {
            return (InterfaceTreeItem) node.Tag;
        }

        private bool EqualsInterfaceItems(InterfaceTreeItem item1, InterfaceTreeItem item2)
        {
            if (item1.Data is IEqualityComparer || item2.Data is IEqualityComparer)
            {
                IEqualityComparer comparer = item1.Data as IEqualityComparer;
                if (comparer == null)
                {
                    comparer = item2.Data as IEqualityComparer;
                }

                return comparer.Equals(item1.Data, item2.Data);
            }

            return item1.Data == item2.Data;
        }

        private bool EqualsInterfaceItems(TreeNode node1, TreeNode node2)
        {
            InterfaceTreeItem item1 = GetInterfaceItem(node1);
            InterfaceTreeItem item2 = GetInterfaceItem(node2);

            return EqualsInterfaceItems(item1, item2);
        }

        public static bool DialogShow(string caption, string message, InterfaceTreeItem sourceInterface, out ImplementData resultData)
        {
            return DialogShow(caption, message, sourceInterface, DEFAULT_WIDTH, out resultData);
        }

        public static bool DialogShow(string caption, string message, InterfaceTreeItem sourceInterface, int width, out ImplementData resultData)
        {
            resultData = null;
            FormImplementInterface form;
            string entityName = string.Empty;
            bool result = InternalDialogShow(width, caption, message, obj => obj.PopulateForm(sourceInterface), 
                ref entityName, out form);
            if (result)
            {
                resultData = form.GetResult(entityName);
            }

            return result;
        }

        private bool ValidateName(out string error)
        {
            error = string.Empty;
            string name = edValue.Text;
            bool isValid = !string.IsNullOrEmpty(name);
            if (isValid)
            {
//                Match match = regex.Match(name);
//                isValid = match != null && Util.StringEqual(match.Value, name, true);
                if (!regex.IsMatch(name))
                {
                    error = "Type name contains invalid characters which are not supported.";
                }
            }
            else
            {
                error = "Type name must be specified.";
            }

            return isValid;
        }

        #region Overrides of FormInputBox

        protected override bool Validate(out string error)
        {
            error = string.Empty;
            bool isValid = false;
            RecursiveIterateNodes(tvTree.Nodes, delegate(TreeNode node, InterfaceTreeItem item)
                                                {
                                                    CheckState nodeCheckState = tvTree.GetNodeCheckState(node);
                                                    if (nodeCheckState == CheckState.Checked)
                                                    {
                                                        isValid = true;
                                                        return true;
                                                    }

                                                    return false;
                                                });




            if (!isValid)
            {
                error = "At least one interface must be selected.";
            }
            else
            {
                isValid = this.ValidateName(out error);
            }

            return isValid;
        }

        #endregion

        private ImplementData GetResult(string entityName)
        {
            ImplementData result =
                new ImplementData(cmbTypes.SelectedIndex == VALUE_ENTITY ? EntityKind.Entity : EntityKind.Structure,
                                  entityName);

            result.Selection = GetSelectedInterfaces();
            result.Root = SelectedRootInterface;
            return result;
        }

        private List<InterfaceTreeItem> GetSelectedInterfaces()
        {
            List<InterfaceTreeItem> selectedInterfaces = new List<InterfaceTreeItem>();

            RecursiveIterateNodes(tvTree.Nodes,
                                  delegate(TreeNode node, InterfaceTreeItem item)
                                  {
                                      CheckState nodeCheckState = tvTree.GetNodeCheckState(node);
                                      if (nodeCheckState != CheckState.Unchecked)
                                      {
                                          if (!selectedInterfaces.Any(treeItem => EqualsInterfaceItems(treeItem, item)))
                                          {
                                              selectedInterfaces.Add(item);
                                          }
                                      }
                                  });

            return selectedInterfaces;
        }

        private void RecursiveIterateNodes(TreeNodeCollection nodes, Action<TreeNode, InterfaceTreeItem> predicate)
        {
            RecursiveIterateNodes(nodes, (node, item) =>
                                             {
                                                 predicate(node, item);
                                                 return false;
                                             });
        }

        private bool RecursiveIterateNodes(TreeNodeCollection nodes, Func<TreeNode, InterfaceTreeItem, bool> predicate)
        {
            foreach (TreeNode node in nodes)
            {
                InterfaceTreeItem item = (InterfaceTreeItem) node.Tag;
                bool cancel = predicate(node, item);

                if (cancel)
                {
                    // true - cancel iteration
                    return true;
                }

                if (node.Nodes.Count > 0)
                {
                    cancel = RecursiveIterateNodes(node.Nodes, predicate);
                    if (cancel)
                    {
                        return true;
                    }
                }
            }

            // false - continue iteration
            return false;
        }

        private void tvTree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private TreeNode GetRootNode()
        {
            return tvTree.Nodes[0];
        }

        private bool HasAnyParentNodeChecked(TreeNode node)
        {
            TreeNode parent = node.Parent;;
            while (parent != null)
            {
                CheckState parentState = tvTree.GetNodeCheckState(parent);
                if (parentState != CheckState.Unchecked)
                {
                    return true;
                }

                parent = parent.Parent;
            }

            return false;
        }

        private void tvTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (manuallCheckingNodes || internalPopulatingTree)
            {
                return;
            }

            CheckState nodeState = tvTree.GetNodeCheckState(e.Node);

            manuallCheckingNodes = true;
            try
            {
                Func<TreeNodeCollection, TreeNode[]> getIndeterminateNodes = collection =>
                                                                                        collection.OfType<TreeNode>().
                                                                                            Where(
                                                                                                node =>
                                                                                                tvTree.GetNodeCheckState
                                                                                                    (node) ==
                                                                                                CheckState.Indeterminate).ToArray();

                var indeterminateNodesBefore = getIndeterminateNodes(e.Node.Nodes);

                CheckState newNodesState = nodeState == CheckState.Checked
                                               ? CheckState.Indeterminate
                                               : CheckState.Unchecked;
                tvTree.DeepSetCheckState(e.Node.Nodes, newNodesState);

                var indeterminateNodesAfter = getIndeterminateNodes(e.Node.Nodes);

                TreeNode[] uncheckedDelta = null;
                if (newNodesState == CheckState.Unchecked)
                {
                    uncheckedDelta = indeterminateNodesBefore.Except(indeterminateNodesAfter).ToArray();
                }

                RecursiveIterateNodes(GetRootNode().Nodes,
                                      delegate(TreeNode node, InterfaceTreeItem item)
                                      {
                                          //var nodeInterface = GetInterfaceItem(node);

                                          if (newNodesState == CheckState.Indeterminate)
                                          {
                                              if ((from indetNode in indeterminateNodesAfter
                                                       let indetInterface = GetInterfaceItem(indetNode)
                                                        where EqualsInterfaceItems(indetNode, node) && indetNode != node
                                                       select indetNode).Any())
                                              {
                                                  tvTree.SetNodeCheckState(node, CheckState.Indeterminate);
                                              }
                                          }
                                          else
                                          {
                                              if (uncheckedDelta != null)
                                              {
                                                  if ((from indetNode in uncheckedDelta
                                                       let indetInterface = GetInterfaceItem(indetNode)
                                                       where EqualsInterfaceItems(indetNode, node) && indetNode != node
                                                       select indetNode).Any() && !(HasAnyParentNodeChecked(node)))
                                                  {
                                                      tvTree.SetNodeCheckState(node, CheckState.Unchecked);
                                                  }
                                              }
                                          }
                                      });
            }
            finally
            {
                manuallCheckingNodes = false;

                PopulateRootInterfaces();
                SelectFirstInterfaceAsRoot();
            }
        }

        private void tvTree_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (manuallCheckingNodes)
            {
                return;
            }

            CheckState nodeState = tvTree.GetNodeCheckState(e.Node);
            e.Cancel = nodeState == CheckState.Indeterminate;
        }

        #region class InterfaceTreeItem

        public class InterfaceTreeItem
        {
            public InterfaceTreeItem Parent { get; set; }

            public object Data { get; set; }

            public string Name { get; set; }

            public List<InterfaceTreeItem> Childs { get; private set; }

            public InterfaceTreeItem()
                : this(null, null)
            { }

            public InterfaceTreeItem(string name, object data)
            {
                this.Name = name;
                this.Data = data;
                this.Childs = new List<InterfaceTreeItem>();
            }
        }

        #endregion class InterfaceTreeItem

        #region class ImplementData

        public class ImplementData
        {
            public EntityKind EntityKind { get; private set; }
            public string Name { get; private set; }
            public List<InterfaceTreeItem> Selection { get; set; }
            public InterfaceTreeItem Root { get; set; }

            public ImplementData(EntityKind entityKind, string name)
            {
                EntityKind = entityKind;
                Name = name;
            }
        }

        #endregion class ImplementData
    }

    public enum EntityKind
    {
        Entity,
        Structure,
        Interface
    }
}
